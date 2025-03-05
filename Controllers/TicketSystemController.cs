using Grand.Business.Core.Interfaces.Common.Localization;
using Grand.Business.Core.Interfaces.Common.Security;
using Grand.Domain.Customers;
using Grand.Infrastructure;
using Grand.Web.Common.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Widgets.TicketSystem.Domain;
using Widgets.TicketSystem.Models;
using Widgets.TicketSystem.Services;

namespace Widgets.TicketSystem.Controllers
{
    public class TicketSystemController : BasePluginController
    {
        #region Fields
        private readonly ITicketService _ticketService;
        private readonly IContactFormService _contactFormService;
        private readonly IDepartmentService _departmentService;
        private readonly ITranslationService _translationService;
        private readonly IPermissionService _permissionService;
        private readonly IContextAccessor _workContext;
        private readonly TicketSystemSettings _ticketSystemSettings;
        #endregion

        #region Ctor
        public TicketSystemController(
            ITicketService ticketService,
            IContactFormService contactFormService,
            IDepartmentService departmentService,
            ITranslationService translationService,
            IPermissionService permissionService,
            IContextAccessor workContext,
            TicketSystemSettings ticketSystemSettings)
        {
            _ticketService = ticketService;
            _contactFormService = contactFormService;
            _departmentService = departmentService;
            _translationService = translationService;
            _permissionService = permissionService;
            _workContext = workContext;
            _ticketSystemSettings = ticketSystemSettings;
        }
        #endregion

        #region Utilities
        protected async Task<bool> IsCustomerAllowed()
        {
            if (!_ticketSystemSettings.Enable)
                return false;

            // Mağaza kontrolü
            if (_ticketSystemSettings.LimitedToStores.Any() && 
                !_ticketSystemSettings.LimitedToStores.Contains(_workContext.StoreContext.CurrentStore.Id))
                return false;

            // Müşteri grubu kontrolü
            if (_ticketSystemSettings.LimitedToGroups.Any())
            {
                var customerGroups = _workContext.WorkContext.CurrentCustomer.Groups;
                if (!customerGroups.Any(x => _ticketSystemSettings.LimitedToGroups.Contains(x)))
                    return false;
            }

            return true;
        }
        
        protected string GetCustomerFullName(Customer customer)
        {
            if (customer == null)
                return string.Empty;
                
            return $"{customer.Email}";
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IActionResult> CustomerTickets()
        {
            if (!await IsCustomerAllowed())
                return RedirectToRoute("HomePage");

            if (!_ticketSystemSettings.ShowTicketListInCustomerAccount)
                return RedirectToRoute("CustomerInfo");

            var model = new CustomerTicketsModel();
            
            var tickets = await _ticketService.GetAllTickets(
                customerId: _workContext.WorkContext.CurrentCustomer.Id,
                pageIndex: 0,
                pageSize: 20);

            foreach (var ticket in tickets)
            {
                model.Tickets.Add(new CustomerTicketModel
                {
                    Id = ticket.Id,
                    Subject = ticket.Subject,
                    Department = ticket.Department,
                    Status = ticket.Status,
                    Priority = ticket.Priority,
                    CreatedOn = ticket.CreatedOnUtc.ToString("g")
                });
            }

            ViewBag.SelectedTabId = (int)Widgets.TicketSystem.Models.AccountNavigationEnum.Tickets;
            return View(model);
        }

        public async Task<IActionResult> TicketDetails(string id)
        {
            if (!await IsCustomerAllowed())
                return RedirectToRoute("HomePage");

            var ticket = await _ticketService.GetTicketById(id);
            if (ticket == null || ticket.CustomerId != _workContext.WorkContext.CurrentCustomer.Id)
                return RedirectToAction("CustomerTickets");

            var model = new CustomerTicketDetailsModel
            {
                Id = ticket.Id,
                Subject = ticket.Subject,
                Message = ticket.Message,
                Department = ticket.Department,
                Status = ticket.Status,
                Priority = ticket.Priority,
                CreatedOn = ticket.CreatedOnUtc.ToString("g")
            };

            // Ticket notlarını doldur
            foreach (var note in ticket.TicketNotes.Where(x => x.DisplayToCustomer).OrderByDescending(x => x.CreatedOnUtc))
            {
                model.Notes.Add(new CustomerTicketNoteModel
                {
                    Note = note.Note,
                    CreatedByName = note.CreatedByName,
                    CreatedOn = note.CreatedOnUtc.ToString("g")
                });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddTicketReply(CustomerTicketReplyModel model)
        {
            if (!await IsCustomerAllowed())
                return RedirectToRoute("HomePage");

            var ticket = await _ticketService.GetTicketById(model.TicketId);
            if (ticket == null || ticket.CustomerId != _workContext.WorkContext.CurrentCustomer.Id)
                return RedirectToAction("CustomerTickets");

            if (ModelState.IsValid)
            {
                var note = new TicketNote
                {
                    Note = model.Message,
                    CreatedById = _workContext.WorkContext.CurrentCustomer.Id,
                    CreatedByName = GetCustomerFullName(_workContext.WorkContext.CurrentCustomer),
                    DisplayToCustomer = true,
                    CreatedOnUtc = DateTime.UtcNow
                };

                await _ticketService.InsertTicketNote(note, ticket);

                // Ticket durumunu güncelle
                if (ticket.Status == TicketSystemDefaults.StatusClosed)
                {
                    ticket.Status = TicketSystemDefaults.StatusOpen;
                    await _ticketService.UpdateTicket(ticket);
                }

                return RedirectToAction("TicketDetails", new { id = ticket.Id });
            }

            return RedirectToAction("TicketDetails", new { id = model.TicketId });
        }

        public async Task<IActionResult> CreateTicket(string productId = "", string orderId = "")
        {
            if (!await IsCustomerAllowed())
                return RedirectToRoute("HomePage");

            var model = new CreateTicketModel
            {
                ProductId = productId,
                OrderId = orderId
            };

            // Departman seçeneklerini doldur
            var departments = await _departmentService.GetAllDepartments(active: true);
            model.AvailableDepartments = departments.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(CreateTicketModel model)
        {
            if (!await IsCustomerAllowed())
                return RedirectToRoute("HomePage");

            if (ModelState.IsValid)
            {
                var ticket = new Ticket
                {
                    Subject = model.Subject,
                    Message = model.Message,
                    Department = model.Department,
                    Status = TicketSystemDefaults.StatusOpen,
                    Priority = TicketSystemDefaults.PriorityNormal,
                    CustomerId = _workContext.WorkContext.CurrentCustomer.Id,
                    CustomerEmail = _workContext.WorkContext.CurrentCustomer.Email,
                    CustomerFullName = GetCustomerFullName(_workContext.WorkContext.CurrentCustomer),
                    OrderId = model.OrderId,
                    ProductId = model.ProductId,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow
                };

                await _ticketService.InsertTicket(ticket);

                return RedirectToAction("TicketCreated");
            }

            // Departman seçeneklerini doldur
            var departments = await _departmentService.GetAllDepartments(active: true);
            model.AvailableDepartments = departments.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = x.Name,
                Value = x.Name
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> TicketCreated()
        {
            if (!await IsCustomerAllowed())
                return RedirectToRoute("HomePage");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContactForm(string contactFormId, IFormCollection form)
        {
            if (!await IsCustomerAllowed())
                return RedirectToRoute("HomePage");

            var contactForm = await _contactFormService.GetContactFormById(contactFormId);
            if (contactForm == null)
                return RedirectToRoute("HomePage");

            var formValues = new Dictionary<string, string>();
            
            // Form değerlerini topla
            foreach (var field in contactForm.FormFields)
            {
                var key = $"field_{field.Id}";
                if (form.ContainsKey(key))
                {
                    var value = form[key].ToString();
                    formValues.Add(field.Name, value);
                }
            }

            // Form gönderimini kaydet
            var submission = new ContactFormSubmission
            {
                ContactFormId = contactFormId,
                CustomerId = _workContext.WorkContext.CurrentCustomer.Id,
                CustomerEmail = _workContext.WorkContext.CurrentCustomer.Email,
                CustomerFullName = GetCustomerFullName(_workContext.WorkContext.CurrentCustomer),
                FormValues = formValues,
                CreatedOnUtc = DateTime.UtcNow
            };

            await _contactFormService.InsertContactFormSubmission(submission);

            return RedirectToAction("ContactFormSubmitted");
        }

        public async Task<IActionResult> ContactFormSubmitted()
        {
            if (!await IsCustomerAllowed())
                return RedirectToRoute("HomePage");

            return View();
        }
        #endregion
    }
} 