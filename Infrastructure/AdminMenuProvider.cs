using Grand.Business.Core.Interfaces.Common.Localization;
using Grand.Business.Core.Interfaces.Common.Security;
using Grand.Web.Common.Menu;

namespace Widgets.TicketSystem.Infrastructure
{
    public class AdminMenuProvider : IAdminMenuProvider
    {
        #region Fields
        private readonly ITranslationService _translationService;
        private readonly IPermissionService _permissionService;
        private readonly TicketSystemSettings _ticketSystemSettings;
        #endregion

        #region Ctor
        public AdminMenuProvider(
            ITranslationService translationService,
            IPermissionService permissionService,
            TicketSystemSettings ticketSystemSettings)
        {
            _translationService = translationService;
            _permissionService = permissionService;
            _ticketSystemSettings = ticketSystemSettings;
        }
        #endregion

        #region Properties
        public string ConfigurationUrl => TicketSystemDefaults.ConfigurationUrl;

        public string SystemName => TicketSystemDefaults.ProviderSystemName;

        public string FriendlyName => _translationService.GetResource(TicketSystemDefaults.FriendlyName);

        public int Priority => 10;

        public IList<string> LimitedToStores => _ticketSystemSettings.LimitedToStores;

        public IList<string> LimitedToGroups => _ticketSystemSettings.LimitedToGroups;

        public async Task<bool> Authorize(IPermissionProvider provider)
        {
            return await _permissionService.Authorize(StandardPermission.ManageTickets);
        }



        public Task ManageSiteMap(SiteMapNode rootNode)
        {
            if (!_ticketSystemSettings.Enable)
                return Task.CompletedTask;

            var menuModel = new SiteMapNode {
                SystemName = "TicketSystem",
                ResourceName = "Plugins.Widgets.TicketSystem",
                IconClass = "icon-share",
                PermissionNames = new List<string> {
               TicketSystemDefaults.ProviderSystemName
            },
                Visible = true
            };

            // Tickets menüsü
            var ticketsMenuItem = new SiteMapNode {
                SystemName = "Tickets",
                ResourceName = "Plugins.Widgets.TicketSystem.Tickets",
                ControllerName = "TicketSystem",
                ActionName = "List",
                IconClass = "fa fa-ticket-alt",
                PermissionNames = new List<string> {
                    TicketSystemDefaults.ProviderSystemName
                   },
                Visible = true
            };

            // Departmanlar menüsü
            var departmentsMenuItem = new SiteMapNode {
                SystemName = "Departments",
                ResourceName = "Plugins.Widgets.TicketSystem.Department.List",
                ControllerName = "Department",
                ActionName = "List",
                IconClass = "fa fa-building",
                  PermissionNames = new List<string> {
                    TicketSystemDefaults.ProviderSystemName
                   },
                Visible = true
            };

            // Contact Forms menüsü
            var contactFormsMenuItem = new SiteMapNode {
                SystemName = "ContactForms",
                ResourceName = "Plugins.Widgets.TicketSystem.ContactForms",
                ControllerName = "TicketSystem",
                ActionName = "ContactFormList",
                IconClass = "fa fa-envelope",
                PermissionNames = new List<string> {
                    TicketSystemDefaults.ProviderSystemName
                   },
                Visible = true
            };

            // Contact Form Submissions menüsü
            var submissionsMenuItem = new SiteMapNode {
                SystemName = "ContactFormSubmissions",
                ResourceName = "Plugins.Widgets.TicketSystem.ContactFormSubmissions",
                ControllerName = "TicketSystem",
                ActionName = "SubmissionList",
                IconClass = "fa fa-paper-plane",
                PermissionNames = new List<string> {
                    TicketSystemDefaults.ProviderSystemName
                   },
                Visible = true
            };

            // Konfigürasyon menüsü
            var configMenuItem = new SiteMapNode {
                SystemName = "Configuration",
                ResourceName = "Plugins.Widgets.TicketSystem.Configuration",
                ControllerName = "TicketSystem",
                ActionName = "Configure",
                IconClass = "fa fa-cog",
                PermissionNames = new List<string> {
                    TicketSystemDefaults.ProviderSystemName
                   },
                Visible = true
            };

            menuModel.ChildNodes.Add(ticketsMenuItem);
            menuModel.ChildNodes.Add(departmentsMenuItem);
            menuModel.ChildNodes.Add(contactFormsMenuItem);
            menuModel.ChildNodes.Add(submissionsMenuItem);
            menuModel.ChildNodes.Add(configMenuItem);

            rootNode.ChildNodes.Add(menuModel);

            return Task.CompletedTask;
        }
        #endregion
    }
}