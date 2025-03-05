using Grand.Business.Core.Interfaces.Common.Configuration;
using Grand.Business.Core.Interfaces.Common.Directory;
using Grand.Business.Core.Interfaces.Common.Localization;
using Grand.Business.Core.Interfaces.Common.Security;
using Grand.Data;
using Grand.Domain.Customers;
using Grand.Domain.Localization;
using Grand.Domain.Permissions;
using Grand.Infrastructure.Plugins;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;
using Widgets.TicketSystem.Domain;
using Widgets.TicketSystem.Infrastructure;
using StandardPermission = Widgets.TicketSystem.Infrastructure.StandardPermission;

namespace Widgets.TicketSystem
{
    public class TicketSystemPlugin : BasePlugin, IPlugin
    {
        #region Fields
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<ContactForm> _contactFormRepository;
        private readonly IRepository<ContactFormSubmission> _submissionRepository;
        private readonly IDatabaseContext _databaseContext;
        private readonly ITranslationService _translationService;
        private readonly ILanguageService _languageService;
        private readonly ISettingService _settingService;
        private readonly IPermissionService _permissionService;
        private readonly IGroupService _groupService;
        private readonly IPluginTranslateResource _pluginTranslateResource;
        private readonly ILogger<TicketSystemPlugin> _logger;
        #endregion

        #region Ctor
        public TicketSystemPlugin(
            IRepository<Ticket> ticketRepository,
            IRepository<ContactForm> contactFormRepository,
            IRepository<ContactFormSubmission> submissionRepository,
            IDatabaseContext databaseContext,
            ITranslationService translationService,
            ILanguageService languageService,
            ISettingService settingService,
            IPermissionService permissionService,
            IGroupService groupService,
            IPluginTranslateResource pluginTranslateResource,
            ILogger<TicketSystemPlugin> logger)
        {
            _ticketRepository = ticketRepository;
            _contactFormRepository = contactFormRepository;
            _submissionRepository = submissionRepository;
            _databaseContext = databaseContext;
            _translationService = translationService;
            _languageService = languageService;
            _settingService = settingService;
            _permissionService = permissionService;
            _groupService = groupService;
            _pluginTranslateResource = pluginTranslateResource;
            _logger = logger;
        }
        #endregion

        #region Utilities
        protected virtual async Task InstallLocaleResources()
        {
            // Dil kaynaklarını yükleme
            var languages = await _languageService.GetAllLanguages(true);
            foreach (var language in languages)
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), 
                    $"Plugins/{TicketSystemDefaults.ProviderSystemName}/Resources/DefaultLanguage.xml");
                if (File.Exists(filePath))
                {
                    var localesXml = File.ReadAllText(filePath);
                    await _translationService.ImportResourcesFromXmlInstall(language, localesXml);
                }
            }
        }

        protected virtual async Task UninstallLocalResources()
        {
            // Dil kaynaklarını kaldırma
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), 
                $"Plugins/{TicketSystemDefaults.ProviderSystemName}/Resources/DefaultLanguage.xml");
            if (File.Exists(filePath))
            {
                var resources = from name in XDocument.Load(filePath).Document.Descendants("Resource")
                               select name.Attribute("Name").Value;
                foreach (var resource in resources)
                {
                    await _pluginTranslateResource.DeletePluginTranslationResource(resource);
                }
            }
        }

        protected virtual async Task InstallPermissions()
        {
            // Ticket izinlerini yükleme
            var ticketPermission = await _permissionService.GetPermissionBySystemName(TicketSystemDefaults.ProviderSystemName);
            if (ticketPermission == null)
            {
                ticketPermission = StandardPermission.ManageTickets;
                ticketPermission.CustomerGroups.Add(
                    (await _groupService.GetCustomerGroupBySystemName(SystemCustomerGroupNames.Administrators)).Id);
                
                await _permissionService.InsertPermission(ticketPermission);
                await SaveTranslationPermissionName(ticketPermission);
            }
            
            // Contact Form izinlerini yükleme
            var contactFormPermission = await _permissionService.GetPermissionBySystemName("Widgets.TicketSystem.ContactForms");
            if (contactFormPermission == null)
            {
                contactFormPermission = StandardPermission.ManageContactForms;
                contactFormPermission.CustomerGroups.Add(
                    (await _groupService.GetCustomerGroupBySystemName(SystemCustomerGroupNames.Administrators)).Id);
                
                await _permissionService.InsertPermission(contactFormPermission);
                await SaveTranslationPermissionName(contactFormPermission);
            }
        }

        private async Task SaveTranslationPermissionName(Permission permissionRecord)
        {
            var name = $"Permission.{permissionRecord.SystemName}";
            var value = permissionRecord.Name;
            foreach (var lang in await _languageService.GetAllLanguages(true))
            {
                var lsr = new TranslationResource {
                    LanguageId = lang.Id,
                    Name = name,
                    Value = value
                };
                await _translationService.InsertTranslateResource(lsr);
            }
        }

        protected virtual async Task UninstallPermissions()
        {
            // Ticket izinlerini kaldırma
            var ticketPermission = await _permissionService.GetPermissionBySystemName(TicketSystemDefaults.ProviderSystemName);
            if (ticketPermission != null)
            {
                await _permissionService.DeletePermission(ticketPermission);
                await DeleteTranslationPermissionName(ticketPermission);
            }
            
            // Contact Form izinlerini kaldırma
            var contactFormPermission = await _permissionService.GetPermissionBySystemName("Widgets.TicketSystem.ContactForms");
            if (contactFormPermission != null)
            {
                await _permissionService.DeletePermission(contactFormPermission);
                await DeleteTranslationPermissionName(contactFormPermission);
            }
        }

        private async Task DeleteTranslationPermissionName(Permission permissionRecord)
        {
            var name = $"Permission.{permissionRecord.SystemName}";
            foreach (var lang in await _languageService.GetAllLanguages(true))
            {
                var lsr = await _translationService.GetTranslateResourceByName(name, lang.Id);
                if (lsr != null)
                    await _translationService.DeleteTranslateResource(lsr);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Plugin kurulumu
        /// </summary>
        public override async Task Install()
        {
            try
            {
                // Dil kaynaklarını yükle
                await InstallLocaleResources();
                
                // Veritabanı tablolarını ve indekslerini oluştur
                await _databaseContext.CreateIndex(_ticketRepository, 
                    OrderBuilder<Ticket>.Create().Ascending(x => x.Id), 
                    "TicketSystem_Ticket");
                    
                await _databaseContext.CreateIndex(_contactFormRepository, 
                    OrderBuilder<ContactForm>.Create().Ascending(x => x.Id), 
                    "TicketSystem_ContactForm");
                    
                await _databaseContext.CreateIndex(_submissionRepository, 
                    OrderBuilder<ContactFormSubmission>.Create().Ascending(x => x.Id), 
                    "TicketSystem_ContactFormSubmission");
                
                // İzinleri yükle
                await InstallPermissions();
                
                // Ayarları kaydet
                await _settingService.SaveSetting(new TicketSystemSettings
                {
                    Enable = true,
                    DisplayOrder = 1,
                    SendEmailNotification = true,
                    ShowTicketListInCustomerAccount = true,
                    ShowTicketOptionOnProductPage = true,
                    ShowTicketOptionOnOrderDetailsPage = true
                });
                
                await base.Install();
                
                _logger.LogInformation("Plugin installed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during plugin installation");
                throw;
            }
        }

        /// <summary>
        /// Plugin kaldırma
        /// </summary>
        public override async Task Uninstall()
        {
            try
            {
                // Dil kaynaklarını kaldır
                await UninstallLocalResources();
                
                // İzinleri kaldır
                await UninstallPermissions();
                
                // Ayarları kaldır
                await _settingService.DeleteSetting<TicketSystemSettings>();
                
                await base.Uninstall();
                
                _logger.LogInformation("Plugin uninstalled successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during plugin uninstallation");
                throw;
            }
        }

        /// <summary>
        /// Konfigürasyon URL'si
        /// </summary>
        public override string ConfigurationUrl()
        {
            return TicketSystemDefaults.ConfigurationUrl;
        }
        #endregion
    }
} 