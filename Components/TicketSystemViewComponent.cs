using Grand.Business.Core.Interfaces.Common.Localization;
using Grand.Infrastructure;
using Grand.Infrastructure.Caching;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Widgets.TicketSystem.Domain;
using Widgets.TicketSystem.Models;
using Widgets.TicketSystem.Services;

namespace Widgets.TicketSystem.Components
{
    [ViewComponent(Name = "WidgetTicketSystem")]
    public class TicketSystemViewComponent : ViewComponent
    {
        #region Fields
        private readonly ITicketService _ticketService;
        private readonly IContactFormService _contactFormService;
        private readonly IDepartmentService _departmentService;
        private readonly IContextAccessor _contextAccessor;
        private readonly ICacheBase _cacheBase;
        private readonly TicketSystemSettings _ticketSystemSettings;
        private readonly ITranslationService _translationService;
        private readonly ILogger<TicketSystemViewComponent> _logger;
        #endregion

        #region Ctor
        public TicketSystemViewComponent(
            ITicketService ticketService,
            IContactFormService contactFormService,
            IDepartmentService departmentService,
            IContextAccessor contextAccessor,
            ICacheBase cacheBase,
            TicketSystemSettings ticketSystemSettings,
            ITranslationService translationService,
            ILogger<TicketSystemViewComponent> logger)
        {
            _ticketService = ticketService;
            _contactFormService = contactFormService;
            _departmentService = departmentService;
            _contextAccessor = contextAccessor;
            _cacheBase = cacheBase;
            _ticketSystemSettings = ticketSystemSettings;
            _translationService = translationService;
            _logger = logger;
        }
        #endregion

        #region Methods
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData = null)
        {
            if (!_ticketSystemSettings.Enable)
                return Content("");

            try
            {
                // Kullanıcı paneli navigasyon sekmesi
                if (widgetZone == "account_navigation_after" && _ticketSystemSettings.ShowTicketListInCustomerAccount)
                {
                    // Gelen model Grand.Web.Models.Customer.CustomerNavigationModel tipinde
                    // Bunu kendi modelimize dönüştürelim
                    var customerNavigationModel = new CustomerNavigationModel();
                    
                    // Eğer additionalData null değilse ve doğru tipteyse
                    if (additionalData != null)
                    {
                        // Reflection kullanarak özellikleri kopyalayalım
                        var properties = additionalData.GetType().GetProperties();
                        foreach (var property in properties)
                        {
                            var targetProperty = typeof(CustomerNavigationModel).GetProperty(property.Name);
                            if (targetProperty != null && targetProperty.CanWrite)
                            {
                                var value = property.GetValue(additionalData);
                                targetProperty.SetValue(customerNavigationModel, value);
                            }
                        }
                    }
                    
                    // HideTickets özelliğini false olarak ayarlayalım
                    customerNavigationModel.HideTickets = false;
                    
                    return View("NavigationTab", customerNavigationModel);
                }

                var infoModel = new PublicInfoModel();

                // Ürün sayfası widget'ı
                if (widgetZone == "productdetails_inside_overview_buttons_after" && _ticketSystemSettings.ShowTicketOptionOnProductPage)
                {
                    var productId = additionalData as string;
                    if (!string.IsNullOrEmpty(productId))
                    {
                        infoModel.ProductId = productId;
                        return View(infoModel);
                    }
                }

                // Sipariş detay sayfası widget'ı
                if (widgetZone == "orderdetails_page_overview" && _ticketSystemSettings.ShowTicketOptionOnOrderDetailsPage)
                {
                    var orderId = additionalData as string;
                    if (!string.IsNullOrEmpty(orderId))
                    {
                        infoModel.OrderId = orderId;
                        return View(infoModel);
                    }
                }

                // İletişim formu widget'ı
                var contactForm = await GetContactFormByWidgetZone(widgetZone);
                if (contactForm != null)
                {
                    infoModel.ContactFormId = contactForm.Id;
                    infoModel.ContactFormName = contactForm.Name;
                    infoModel.ContactFormDescription = contactForm.Description;

                    // Form alanlarını doldur
                    foreach (var field in contactForm.FormFields.OrderBy(x => x.DisplayOrder))
                    {
                        infoModel.ContactFormFields.Add(new PublicInfoModel.ContactFormFieldModel {
                            Id = field.Id,
                            Name = field.Name,
                            FieldType = field.FieldType,
                            IsRequired = field.IsRequired,
                            DefaultValue = field.DefaultValue,
                            FieldOptions = field.FieldOptions,
                            DisplayOrder = field.DisplayOrder
                        });
                    }

                    return View(infoModel);
                }

                return Content("");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TicketSystemViewComponent");
                return Content("");
            }
        }

        private async Task<ContactForm> GetContactFormByWidgetZone(string widgetZone)
        {
            if (string.IsNullOrEmpty(widgetZone))
                return null;

            var cacheKey = $"{TicketSystemDefaults.CachePrefix}:contactform:{widgetZone}";
            return await _cacheBase.GetAsync(cacheKey, async () =>
            {
                var contactForms = await _contactFormService.GetAllContactForms(
                    storeId: _contextAccessor.StoreContext.CurrentStore.Id,
                    active: true,
                    widgetZone: widgetZone);

                return contactForms.FirstOrDefault();
            });
        }
        #endregion
    }
}