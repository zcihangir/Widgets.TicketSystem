using Grand.Business.Core.Interfaces.Common.Localization;
using Grand.Business.Core.Interfaces.Common.Security;
using Grand.Domain.Permissions;
using Grand.Web.Common.Controllers;
using Grand.Web.Common.Filters;
using Grand.Web.Common.Security.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Widgets.TicketSystem.Domain;
using Widgets.TicketSystem.Models;
using Widgets.TicketSystem.Services;
using StandardPermission = Widgets.TicketSystem.Infrastructure.StandardPermission;
using Grand.Business.Core.Interfaces.Common.Configuration;
using Grand.Business.Core.Interfaces.Common.Stores;
using Grand.Business.Core.Interfaces.Common.Directory;

namespace Widgets.TicketSystem.Areas.Admin.Controllers
{

    [PermissionAuthorize(PermissionSystemName.Widgets)]
    public class TicketSystemController : BaseAdminPluginController
    {
        #region Fields
        private readonly ITicketService _ticketService;
        private readonly IContactFormService _contactFormService;
        private readonly IDepartmentService _departmentService;
        private readonly ITranslationService _translationService;
        private readonly IPermissionService _permissionService;
        private readonly IStoreService _storeService;
        private readonly IGroupService _customerGroupService;
        private readonly TicketSystemSettings _ticketSystemSettings;
        private readonly ISettingService _settingService;
        #endregion

        #region Ctor
        public TicketSystemController(
            ITicketService ticketService,
            IContactFormService contactFormService,
            IDepartmentService departmentService,
            ITranslationService translationService,
            IPermissionService permissionService,
            IStoreService storeService,
            IGroupService customerGroupService,
            TicketSystemSettings ticketSystemSettings,
            ISettingService settingService)
        {
            _ticketService = ticketService;
            _contactFormService = contactFormService;
            _departmentService = departmentService;
            _translationService = translationService;
            _permissionService = permissionService;
            _storeService = storeService;
            _customerGroupService = customerGroupService;
            _ticketSystemSettings = ticketSystemSettings;
            _settingService = settingService;
        }
        #endregion

        #region Utilities
        protected async Task<bool> HasAccessToTicket()
        {
            return await _permissionService.Authorize(StandardPermission.ManageTickets);
        }

        protected async Task<bool> HasAccessToContactForms()
        {
            return await _permissionService.Authorize(StandardPermission.ManageContactForms);
        }
        #endregion

        #region Configuration
        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.Authorize(StandardPermission.ManageTickets))
                return AccessDeniedView();

            var model = new ConfigurationModel
            {
                Enable = _ticketSystemSettings.Enable,
                DisplayOrder = _ticketSystemSettings.DisplayOrder,
                LimitedToGroups = _ticketSystemSettings.LimitedToGroups.Any(),
                SendEmailNotification = _ticketSystemSettings.SendEmailNotification,
                NotificationEmail = _ticketSystemSettings.NotificationEmail,
                ShowTicketListInCustomerAccount = _ticketSystemSettings.ShowTicketListInCustomerAccount,
                ShowTicketOptionOnProductPage = _ticketSystemSettings.ShowTicketOptionOnProductPage,
                ShowTicketOptionOnOrderDetailsPage = _ticketSystemSettings.ShowTicketOptionOnOrderDetailsPage,
                SelectedCustomerGroupIds = _ticketSystemSettings.LimitedToGroups.ToList()
            };

            // Müşteri gruplarını yükle
            var customerGroups = await _customerGroupService.GetAllCustomerGroups();
            ViewBag.AvailableCustomerGroups = customerGroups.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.Authorize(StandardPermission.ManageTickets))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return View(model);

            // Ayarları güncelle
            _ticketSystemSettings.Enable = model.Enable;
            _ticketSystemSettings.DisplayOrder = model.DisplayOrder;
            _ticketSystemSettings.LimitedToGroups = model.LimitedToGroups ? model.SelectedCustomerGroupIds : new List<string>();
            _ticketSystemSettings.SendEmailNotification = model.SendEmailNotification;
            _ticketSystemSettings.NotificationEmail = model.NotificationEmail;
            _ticketSystemSettings.ShowTicketListInCustomerAccount = model.ShowTicketListInCustomerAccount;
            _ticketSystemSettings.ShowTicketOptionOnProductPage = model.ShowTicketOptionOnProductPage;
            _ticketSystemSettings.ShowTicketOptionOnOrderDetailsPage = model.ShowTicketOptionOnOrderDetailsPage;

            // Ayarları kaydet
            await _settingService.SaveSetting(_ticketSystemSettings);

            Success(_translationService.GetResource("Plugins.Widgets.TicketSystem.Configuration.Updated"));
            return RedirectToAction("Configure");
        }
        #endregion

        #region Tickets
        public async Task<IActionResult> List()
        {
            if (!await HasAccessToTicket())
                return AccessDeniedView();

            var model = new TicketListModel();
            
            // Durum ve öncelik seçeneklerini doldur
            model.AvailableStatuses = new List<SelectListItem>
            {
                new SelectListItem { Text = _translationService.GetResource("Admin.Common.All"), Value = "" },
                new SelectListItem { Text = TicketSystemDefaults.StatusOpen, Value = TicketSystemDefaults.StatusOpen },
                new SelectListItem { Text = TicketSystemDefaults.StatusProcessing, Value = TicketSystemDefaults.StatusProcessing },
                new SelectListItem { Text = TicketSystemDefaults.StatusClosed, Value = TicketSystemDefaults.StatusClosed }
            };
            
            model.AvailablePriorities = new List<SelectListItem>
            {
                new SelectListItem { Text = _translationService.GetResource("Admin.Common.All"), Value = "" },
                new SelectListItem { Text = TicketSystemDefaults.PriorityLow, Value = TicketSystemDefaults.PriorityLow },
                new SelectListItem { Text = TicketSystemDefaults.PriorityNormal, Value = TicketSystemDefaults.PriorityNormal },
                new SelectListItem { Text = TicketSystemDefaults.PriorityHigh, Value = TicketSystemDefaults.PriorityHigh }
            };
            
            // Departman seçeneklerini doldur
            var departments = await _departmentService.GetAllDepartments(active: true);
            model.AvailableDepartments = new List<SelectListItem>
            {
                new SelectListItem { Text = _translationService.GetResource("Admin.Common.All"), Value = "" }
            };
            
            foreach (var department in departments)
            {
                model.AvailableDepartments.Add(new SelectListItem
                {
                    Text = department.Name,
                    Value = department.Name
                });
            }
            

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> TicketList(TicketListModel model)
        {
            if (!await HasAccessToTicket())
                return AccessDeniedView();

            var tickets = await _ticketService.GetAllTickets(
                customerId: null,
                staffId: null,
                department: model.SearchDepartment,
                status: model.SearchStatus,
                priority: model.SearchPriority,
                pageIndex: model.PageIndex,
                pageSize: model.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = tickets.Select(x => new
                {
                    Id = x.Id,
                    Subject = x.Subject,
                    Department = x.Department,
                    Status = x.Status,
                    Priority = x.Priority,
                    CustomerEmail = x.CustomerEmail,
                    CustomerFullName = x.CustomerFullName,
                    StaffName = x.StaffName,
                    CreatedOn = x.CreatedOnUtc.ToString("g")
                }),
                Total = tickets.TotalCount
            };

            return Json(gridModel);
        }

        public async Task<IActionResult> Create()
        {
            if (!await HasAccessToTicket())
                return AccessDeniedView();

            var model = new TicketModel
            {
                Status = TicketSystemDefaults.StatusOpen,
                Priority = TicketSystemDefaults.PriorityNormal,
                CreatedOnUtc = DateTime.UtcNow
            };

            // Durum seçeneklerini doldur
            model.AvailableStatuses = new List<SelectListItem>
            {
                new SelectListItem { Text = TicketSystemDefaults.StatusOpen, Value = TicketSystemDefaults.StatusOpen, Selected = model.Status == TicketSystemDefaults.StatusOpen },
                new SelectListItem { Text = TicketSystemDefaults.StatusProcessing, Value = TicketSystemDefaults.StatusProcessing, Selected = model.Status == TicketSystemDefaults.StatusProcessing },
                new SelectListItem { Text = TicketSystemDefaults.StatusClosed, Value = TicketSystemDefaults.StatusClosed, Selected = model.Status == TicketSystemDefaults.StatusClosed }
            };

            // Öncelik seçeneklerini doldur
            model.AvailablePriorities = new List<SelectListItem>
            {
                new SelectListItem { Text = TicketSystemDefaults.PriorityLow, Value = TicketSystemDefaults.PriorityLow, Selected = model.Priority == TicketSystemDefaults.PriorityLow },
                new SelectListItem { Text = TicketSystemDefaults.PriorityNormal, Value = TicketSystemDefaults.PriorityNormal, Selected = model.Priority == TicketSystemDefaults.PriorityNormal },
                new SelectListItem { Text = TicketSystemDefaults.PriorityHigh, Value = TicketSystemDefaults.PriorityHigh, Selected = model.Priority == TicketSystemDefaults.PriorityHigh }
            };

            // Departman seçeneklerini doldur
            var departments = await _departmentService.GetAllDepartments(active: true);
            model.AvailableDepartments = new List<SelectListItem>();
            
            foreach (var department in departments)
            {
                model.AvailableDepartments.Add(new SelectListItem
                {
                    Text = department.Name,
                    Value = department.Name
                });
            }

            // Personel seçeneklerini doldur
            model.AvailableStaff = new List<SelectListItem>
            {
                new SelectListItem { Text = _translationService.GetResource("Admin.Common.None"), Value = "" }
            };
            // TODO: Personel listesini getir

            return View(model);
        }

        [HttpPost, ArgumentNameFilter(KeyName = "save-continue", Argument = "continueEditing")]
        public async Task<IActionResult> Create(TicketModel model, bool continueEditing)
        {
            if (!await HasAccessToTicket())
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var ticket = new Ticket
                {
                    Subject = model.Subject,
                    Message = model.Message,
                    Department = model.Department,
                    Status = model.Status,
                    Priority = model.Priority,
                    CustomerId = model.CustomerId,
                    CustomerEmail = model.CustomerEmail,
                    CustomerFullName = model.CustomerFullName,
                    StaffId = model.StaffId,
                    StaffName = model.StaffName,
                    OrderId = model.OrderId,
                    ProductId = model.ProductId,
                    ContactFormId = model.ContactFormId,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow
                };

                await _ticketService.InsertTicket(ticket);

                Success(_translationService.GetResource("Plugins.Widgets.TicketSystem.Ticket.Added"));

                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = ticket.Id });
                }
                return RedirectToAction("List");
            }

            // Durum, öncelik, departman, personel ve mağaza seçeneklerini doldur
            // TODO: Durum, öncelik, departman, personel ve mağaza seçeneklerini getir

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (!await HasAccessToTicket())
                return AccessDeniedView();

            var ticket = await _ticketService.GetTicketById(id);
            if (ticket == null)
                return RedirectToAction("List");

            var model = new TicketModel
            {
                Id = ticket.Id,
                Subject = ticket.Subject,
                Message = ticket.Message,
                Department = ticket.Department,
                Status = ticket.Status,
                Priority = ticket.Priority,
                CustomerId = ticket.CustomerId,
                CustomerEmail = ticket.CustomerEmail,
                CustomerFullName = ticket.CustomerFullName,
                StaffId = ticket.StaffId,
                StaffName = ticket.StaffName,
                OrderId = ticket.OrderId,
                ProductId = ticket.ProductId,
                ContactFormId = ticket.ContactFormId,
                CreatedOnUtc = ticket.CreatedOnUtc,
                CreatedOn = ticket.CreatedOnUtc.ToString("g"),
                UpdatedOnUtc = ticket.UpdatedOnUtc,
                UpdatedOn = ticket.UpdatedOnUtc.HasValue ? ticket.UpdatedOnUtc.Value.ToString("g") : ""
            };

            // Durum seçeneklerini doldur
            model.AvailableStatuses = new List<SelectListItem>
            {
                new SelectListItem { Text = TicketSystemDefaults.StatusOpen, Value = TicketSystemDefaults.StatusOpen, Selected = model.Status == TicketSystemDefaults.StatusOpen },
                new SelectListItem { Text = TicketSystemDefaults.StatusProcessing, Value = TicketSystemDefaults.StatusProcessing, Selected = model.Status == TicketSystemDefaults.StatusProcessing },
                new SelectListItem { Text = TicketSystemDefaults.StatusClosed, Value = TicketSystemDefaults.StatusClosed, Selected = model.Status == TicketSystemDefaults.StatusClosed }
            };

            // Öncelik seçeneklerini doldur
            model.AvailablePriorities = new List<SelectListItem>
            {
                new SelectListItem { Text = TicketSystemDefaults.PriorityLow, Value = TicketSystemDefaults.PriorityLow, Selected = model.Priority == TicketSystemDefaults.PriorityLow },
                new SelectListItem { Text = TicketSystemDefaults.PriorityNormal, Value = TicketSystemDefaults.PriorityNormal, Selected = model.Priority == TicketSystemDefaults.PriorityNormal },
                new SelectListItem { Text = TicketSystemDefaults.PriorityHigh, Value = TicketSystemDefaults.PriorityHigh, Selected = model.Priority == TicketSystemDefaults.PriorityHigh }
            };

            // Departman seçeneklerini doldur
            var departments = await _departmentService.GetAllDepartments(active: true);
            model.AvailableDepartments = new List<SelectListItem>();
            
            foreach (var department in departments)
            {
                model.AvailableDepartments.Add(new SelectListItem
                {
                    Text = department.Name,
                    Value = department.Name,
                    Selected = model.Department == department.Name
                });
            }


            // Personel seçeneklerini doldur
            model.AvailableStaff = new List<SelectListItem>
            {
                new SelectListItem { Text = _translationService.GetResource("Admin.Common.None"), Value = "", Selected = string.IsNullOrEmpty(model.StaffId) }
            };
            // TODO: Personel listesini getir

            // Ticket notlarını getir
            var ticketNotes = await _ticketService.GetTicketNotesByTicketId(id);
            model.TicketNotes = ticketNotes.Select(tn => new TicketNoteModel
            {
                Id = tn.Id,
                Note = tn.Note,
                CreatedById = tn.CreatedById,
                CreatedByName = tn.CreatedByName,
                DisplayToCustomer = tn.DisplayToCustomer,
                CreatedOnUtc = tn.CreatedOnUtc,
                CreatedOn = tn.CreatedOnUtc.ToString("g")
            }).ToList();

            return View(model);
        }

        [HttpPost, ArgumentNameFilter(KeyName = "save-continue", Argument = "continueEditing")]
        public async Task<IActionResult> Edit(TicketModel model, bool continueEditing)
        {
            if (!await HasAccessToTicket())
                return AccessDeniedView();

            var ticket = await _ticketService.GetTicketById(model.Id);
            if (ticket == null)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                ticket.Subject = model.Subject;
                ticket.Message = model.Message;
                ticket.Department = model.Department;
                ticket.Status = model.Status;
                ticket.Priority = model.Priority;
                ticket.CustomerId = model.CustomerId;
                ticket.CustomerEmail = model.CustomerEmail;
                ticket.CustomerFullName = model.CustomerFullName;
                ticket.StaffId = model.StaffId;
                ticket.StaffName = model.StaffName;
                ticket.OrderId = model.OrderId;
                ticket.ProductId = model.ProductId;
                ticket.ContactFormId = model.ContactFormId;
                ticket.UpdatedOnUtc = DateTime.UtcNow;

                await _ticketService.UpdateTicket(ticket);

                // Yeni not ekle
                if (!string.IsNullOrEmpty(model.AddNote))
                {
                    var note = new TicketNote
                    {
                        Note = model.AddNote,
                        CreatedById = "admin", // TODO: Gerçek admin ID'sini al
                        CreatedByName = "Admin", // TODO: Gerçek admin adını al
                        DisplayToCustomer = model.DisplayToCustomer,
                        CreatedOnUtc = DateTime.UtcNow
                    };

                    await _ticketService.InsertTicketNote(note, ticket);
                }

                Success(_translationService.GetResource("Plugins.Widgets.TicketSystem.Ticket.Updated"));

                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = ticket.Id });
                }
                return RedirectToAction("List");
            }

            // Durum, öncelik, departman, personel ve mağaza seçeneklerini doldur
            // TODO: Durum, öncelik, departman, personel ve mağaza seçeneklerini getir

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            if (!await HasAccessToTicket())
                return AccessDeniedView();

            var ticket = await _ticketService.GetTicketById(id);
            if (ticket == null)
                return RedirectToAction("List");

            await _ticketService.DeleteTicket(ticket);

            Success(_translationService.GetResource("Plugins.Widgets.TicketSystem.Ticket.Deleted"));

            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNote(string id, string ticketId)
        {
            if (!await HasAccessToTicket())
                return AccessDeniedView();

            var ticket = await _ticketService.GetTicketById(ticketId);
            if (ticket == null)
                return RedirectToAction("List");

            var note = ticket.TicketNotes.FirstOrDefault(x => x.Id == id);
            if (note == null)
                return RedirectToAction("Edit", new { id = ticketId });

            await _ticketService.DeleteTicketNote(note, ticket);

            Success(_translationService.GetResource("Plugins.Widgets.TicketSystem.Ticket.Note.Deleted"));

            return RedirectToAction("Edit", new { id = ticketId });
        }
        #endregion

        #region Contact Forms
        public async Task<IActionResult> ContactFormList()
        {
            if (!await HasAccessToContactForms())
                return AccessDeniedView();

            var model = new ContactFormListModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ContactFormList(ContactFormListModel model)
        {
            if (!await HasAccessToContactForms())
                return AccessDeniedView();

            var forms = await _contactFormService.GetAllContactForms(
                active: model.SearchActive,
                pageIndex: model.PageIndex,
                pageSize: model.PageSize);

            var gridModel = new DataSourceResult
            {
                Data = forms.Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                    WidgetZone = x.WidgetZone,
                    Active = x.Active,
                    DisplayOrder = x.DisplayOrder,
                    CreatedOn = x.CreatedOnUtc.ToString("g")
                }),
                Total = forms.TotalCount
            };

            return Json(gridModel);
        }

        public async Task<IActionResult> CreateContactForm()
        {
            if (!await HasAccessToContactForms())
                return AccessDeniedView();

            var model = new ContactFormModel
            {
                Active = true,
                DisplayOrder = 1
            };
            
            // Widget bölgelerini doldur
            model.AvailableWidgetZones = new List<SelectListItem>
            {
                new SelectListItem { Text = "contactus_top", Value = "contactus_top" },
                new SelectListItem { Text = "contactus_bottom", Value = "contactus_bottom" },
                new SelectListItem { Text = "home_page_top", Value = "home_page_top" },
                new SelectListItem { Text = "home_page_bottom", Value = "home_page_bottom" },
                new SelectListItem { Text = "footer_before", Value = "footer_before" },
                new SelectListItem { Text = "footer_after", Value = "footer_after" }
            };
            
            // Mağaza seçeneklerini doldur
            var stores = await _storeService.GetAllStores();
            model.AvailableStores.Add(new SelectListItem { Text = _translationService.GetResource("Admin.Common.All"), Value = "" });
            foreach (var store in stores)
            {
                model.AvailableStores.Add(new SelectListItem
                {
                    Text = store.Name,
                    Value = store.Id
                });
            }

            return View(model);
        }

        [HttpPost, ArgumentNameFilter(KeyName = "save-continue", Argument = "continueEditing")]
        public async Task<IActionResult> CreateContactForm(ContactFormModel model, bool continueEditing)
        {
            if (!await HasAccessToContactForms())
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var contactForm = new ContactForm
                {
                    Name = model.Name,
                    Description = model.Description,
                    WidgetZone = model.WidgetZone,
                    Active = model.Active,
                    DisplayOrder = model.DisplayOrder,
                    LimitedToStores = model.LimitedToStores,
                    CreatedOnUtc = DateTime.UtcNow,
                    UpdatedOnUtc = DateTime.UtcNow
                };
                
                // Mağaza ID'lerini ekle
                if (model.SelectedStoreIds != null && model.SelectedStoreIds.Any())
                {
                    foreach (var storeId in model.SelectedStoreIds)
                    {
                        contactForm.StoreIds.Add(storeId);
                    }
                }

                // Form alanlarını ekle
                foreach (var field in model.FormFields)
                {
                    contactForm.FormFields.Add(new ContactFormField
                    {
                        Name = field.Name,
                        FieldType = field.FieldType,
                        IsRequired = field.IsRequired,
                        DefaultValue = field.DefaultValue,
                        FieldOptions = field.FieldOptions,
                        DisplayOrder = field.DisplayOrder
                    });
                }

                await _contactFormService.InsertContactForm(contactForm);

                Success(_translationService.GetResource("Plugins.Widgets.TicketSystem.ContactForm.Added"));

                if (continueEditing)
                {
                    return RedirectToAction("EditContactForm", new { id = contactForm.Id });
                }
                return RedirectToAction("ContactFormList");
            }

            // Widget bölgeleri ve mağaza seçeneklerini doldur
            model.AvailableWidgetZones = new List<SelectListItem>
            {
                new SelectListItem { Text = "contactus_top", Value = "contactus_top" },
                new SelectListItem { Text = "contactus_bottom", Value = "contactus_bottom" },
                new SelectListItem { Text = "home_page_top", Value = "home_page_top" },
                new SelectListItem { Text = "home_page_bottom", Value = "home_page_bottom" },
                new SelectListItem { Text = "footer_before", Value = "footer_before" },
                new SelectListItem { Text = "footer_after", Value = "footer_after" }
            };
            
            // Mağaza seçeneklerini doldur
            var stores = await _storeService.GetAllStores();
            model.AvailableStores.Add(new SelectListItem { Text = _translationService.GetResource("Admin.Common.All"), Value = "" });
            foreach (var store in stores)
            {
                model.AvailableStores.Add(new SelectListItem
                {
                    Text = store.Name,
                    Value = store.Id
                });
            }

            return View(model);
        }

        public async Task<IActionResult> EditContactForm(string id)
        {
            if (!await HasAccessToContactForms())
                return AccessDeniedView();

            var contactForm = await _contactFormService.GetContactFormById(id);
            if (contactForm == null)
                return RedirectToAction("ContactFormList");

            var model = new ContactFormModel
            {
                Id = contactForm.Id,
                Name = contactForm.Name,
                Description = contactForm.Description,
                WidgetZone = contactForm.WidgetZone,
                Active = contactForm.Active,
                DisplayOrder = contactForm.DisplayOrder,
                LimitedToStores = contactForm.LimitedToStores,
                SelectedStoreIds = contactForm.StoreIds.ToList(),
                CreatedOnUtc = contactForm.CreatedOnUtc,
                CreatedOn = contactForm.CreatedOnUtc.ToString("g"),
                UpdatedOnUtc = contactForm.UpdatedOnUtc,
                UpdatedOn = contactForm.UpdatedOnUtc?.ToString("g")
            };

            // Form alanlarını doldur
            foreach (var field in contactForm.FormFields.OrderBy(x => x.DisplayOrder))
            {
                model.FormFields.Add(new ContactFormFieldModel
                {
                    Id = field.Id,
                    Name = field.Name,
                    FieldType = field.FieldType,
                    IsRequired = field.IsRequired,
                    DefaultValue = field.DefaultValue,
                    FieldOptions = field.FieldOptions?.ToList() ?? new List<string>(),
                    DisplayOrder = field.DisplayOrder
                });
            }

            // Widget bölgeleri ve mağaza seçeneklerini doldur
            // Widget bölgelerini doldur
            model.AvailableWidgetZones = new List<SelectListItem>
            {
                new SelectListItem { Text = "contactus_top", Value = "contactus_top" },
                new SelectListItem { Text = "contactus_bottom", Value = "contactus_bottom" },
                new SelectListItem { Text = "home_page_top", Value = "home_page_top" },
                new SelectListItem { Text = "home_page_bottom", Value = "home_page_bottom" },
                new SelectListItem { Text = "footer_before", Value = "footer_before" },
                new SelectListItem { Text = "footer_after", Value = "footer_after" }
            };
            
            // Özel widget bölgesi varsa ekle
            if (!string.IsNullOrEmpty(contactForm.WidgetZone) && 
                !model.AvailableWidgetZones.Any(x => x.Value == contactForm.WidgetZone))
            {
                model.AvailableWidgetZones.Add(new SelectListItem 
                { 
                    Text = contactForm.WidgetZone, 
                    Value = contactForm.WidgetZone,
                    Selected = true
                });
            }
            
            // Mağaza seçeneklerini doldur
            var stores = await _storeService.GetAllStores();
            model.AvailableStores.Add(new SelectListItem { Text = _translationService.GetResource("Admin.Common.All"), Value = "" });
            foreach (var store in stores)
            {
                model.AvailableStores.Add(new SelectListItem
                {
                    Text = store.Name,
                    Value = store.Id,
                    Selected = model.SelectedStoreIds.Contains(store.Id)
                });
            }

            return View(model);
        }

        [HttpPost, ArgumentNameFilter(KeyName = "save-continue", Argument = "continueEditing")]
        public async Task<IActionResult> EditContactForm(ContactFormModel model, bool continueEditing)
        {
            if (!await HasAccessToContactForms())
                return AccessDeniedView();

            var contactForm = await _contactFormService.GetContactFormById(model.Id);
            if (contactForm == null)
                return RedirectToAction("ContactFormList");

            if (ModelState.IsValid)
            {
                contactForm.Name = model.Name;
                contactForm.Description = model.Description;
                contactForm.WidgetZone = model.WidgetZone;
                contactForm.Active = model.Active;
                contactForm.DisplayOrder = model.DisplayOrder;
                contactForm.LimitedToStores = model.LimitedToStores;
                contactForm.UpdatedOnUtc = DateTime.UtcNow;
                
                // Mağaza ID'lerini güncelle
                contactForm.StoreIds.Clear();
                if (model.SelectedStoreIds != null && model.SelectedStoreIds.Any())
                {
                    foreach (var storeId in model.SelectedStoreIds)
                    {
                        contactForm.StoreIds.Add(storeId);
                    }
                }

                // Form alanlarını güncelle
                contactForm.FormFields.Clear();
                foreach (var field in model.FormFields)
                {
                    contactForm.FormFields.Add(new ContactFormField
                    {
                        Name = field.Name,
                        FieldType = field.FieldType,
                        IsRequired = field.IsRequired,
                        DefaultValue = field.DefaultValue,
                        FieldOptions = field.FieldOptions,
                        DisplayOrder = field.DisplayOrder
                    });
                }

                await _contactFormService.UpdateContactForm(contactForm);

                Success(_translationService.GetResource("Plugins.Widgets.TicketSystem.ContactForm.Updated"));

                if (continueEditing)
                {
                    return RedirectToAction("EditContactForm", new { id = contactForm.Id });
                }
                return RedirectToAction("ContactFormList");
            }

            // Widget bölgeleri ve mağaza seçeneklerini doldur
            // TODO: Widget bölgeleri ve mağaza seçeneklerini getir

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteContactForm(string id)
        {
            if (!await HasAccessToContactForms())
                return AccessDeniedView();

            var contactForm = await _contactFormService.GetContactFormById(id);
            if (contactForm == null)
                return RedirectToAction("ContactFormList");

            await _contactFormService.DeleteContactForm(contactForm);

            Success(_translationService.GetResource("Plugins.Widgets.TicketSystem.ContactForm.Deleted"));

            return RedirectToAction("ContactFormList");
        }
        #endregion

        #region Contact Form Submissions
        public async Task<IActionResult> SubmissionList()
        {
            if (!await HasAccessToContactForms())
                return AccessDeniedView();

            var model = new ContactFormSubmissionListModel();
            
            // Form ve mağaza seçeneklerini doldur
            var forms = await _contactFormService.GetAllContactForms(active: true);
            model.AvailableForms = new List<SelectListItem>
            {
                new SelectListItem { Text = _translationService.GetResource("Admin.Common.All"), Value = "" }
            };
            
            foreach (var form in forms)
            {
                model.AvailableForms.Add(new SelectListItem
                {
                    Text = form.Name,
                    Value = form.Id
                });
            }
            
            // Mağaza seçeneklerini doldur
            var stores = await _storeService.GetAllStores();
            model.AvailableStores = new List<SelectListItem>
            {
                new SelectListItem { Text = _translationService.GetResource("Admin.Common.All"), Value = "" }
            };
            
            foreach (var store in stores)
            {
                model.AvailableStores.Add(new SelectListItem
                {
                    Text = store.Name,
                    Value = store.Id
                });
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmissionList(ContactFormSubmissionListModel model)
        {
            if (!await HasAccessToContactForms())
                return AccessDeniedView();

            var submissions = await _contactFormService.GetAllContactFormSubmissions(
                storeId: model.SearchStoreId,
                contactFormId: model.SearchFormId,
                convertedToTicket: model.SearchConvertedToTicket,
                pageIndex: model.PageIndex,
                pageSize: model.PageSize);

            // Form adlarını bir sözlükte toplayalım
            var formIds = submissions.Select(x => x.ContactFormId).Distinct().ToList();
            var formNames = new Dictionary<string, string>();
            
            foreach (var formId in formIds)
            {
                if (!string.IsNullOrEmpty(formId))
                {
                    var form = await _contactFormService.GetContactFormById(formId);
                    if (form != null)
                    {
                        formNames[formId] = form.Name;
                    }
                }
            }

            var gridModel = new DataSourceResult
            {
                Data = submissions.Select(x => new
                {
                    Id = x.Id,
                    ContactFormId = x.ContactFormId,
                    ContactFormName = formNames.ContainsKey(x.ContactFormId) ? formNames[x.ContactFormId] : "",
                    CustomerEmail = x.CustomerEmail,
                    CustomerFullName = x.CustomerFullName,
                    ConvertedToTicket = x.ConvertedToTicket,
                    TicketId = x.TicketId,
                    CreatedOn = x.CreatedOnUtc.ToString("g")
                }),
                Total = submissions.TotalCount
            };

            return Json(gridModel);
        }

        public async Task<IActionResult> ViewSubmission(string id)
        {
            if (!await HasAccessToContactForms())
                return AccessDeniedView();

            var submission = await _contactFormService.GetContactFormSubmissionById(id);
            if (submission == null)
                return RedirectToAction("SubmissionList");

            var contactForm = await _contactFormService.GetContactFormById(submission.ContactFormId);

            var model = new ContactFormSubmissionModel
            {
                Id = submission.Id,
                ContactFormId = submission.ContactFormId,
                ContactFormName = contactForm?.Name,
                CustomerId = submission.CustomerId,
                CustomerEmail = submission.CustomerEmail,
                CustomerFullName = submission.CustomerFullName,
                FormValues = submission.FormValues,
                CreatedOnUtc = submission.CreatedOnUtc,
                CreatedOn = submission.CreatedOnUtc.ToString("g"),
                ConvertedToTicket = submission.ConvertedToTicket,
                TicketId = submission.TicketId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConvertToTicket(string id)
        {
            if (!await HasAccessToContactForms() || !await HasAccessToTicket())
                return AccessDeniedView();

            var submission = await _contactFormService.GetContactFormSubmissionById(id);
            if (submission == null)
                return RedirectToAction("SubmissionList");

            if (submission.ConvertedToTicket)
            {
                Warning(_translationService.GetResource("Plugins.Widgets.TicketSystem.ContactForm.Submission.AlreadyConverted"));
                return RedirectToAction("ViewSubmission", new { id = submission.Id });
            }

            var ticket = await _contactFormService.ConvertSubmissionToTicket(submission);

            Success(_translationService.GetResource("Plugins.Widgets.TicketSystem.ContactForm.Submission.ConvertedToTicket"));

            return RedirectToAction("Edit", new { id = ticket.Id });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSubmission(string id)
        {
            if (!await HasAccessToContactForms())
                return AccessDeniedView();

            var submission = await _contactFormService.GetContactFormSubmissionById(id);
            if (submission == null)
                return RedirectToAction("SubmissionList");

            await _contactFormService.DeleteContactFormSubmission(submission);

            Success(_translationService.GetResource("Plugins.Widgets.TicketSystem.ContactForm.Submission.Deleted"));

            return RedirectToAction("SubmissionList");
        }
        #endregion

        #region Utilities
        public async Task<IActionResult> AddFormField(int index)
        {
            if (!await HasAccessToContactForms())
                return AccessDeniedView();
                
            ViewData["Index"] = index;
            return PartialView("_FormFieldItem", new ContactFormFieldModel());
        }
        #endregion
    }

    public class DataSourceResult
    {
        public object Data { get; set; }
        public int Total { get; set; }
    }
} 