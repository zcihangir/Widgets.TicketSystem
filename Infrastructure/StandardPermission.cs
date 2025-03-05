using Grand.Domain.Permissions;

namespace Widgets.TicketSystem.Infrastructure
{
    public static class StandardPermission
    {
        private static string TicketSystem => "TicketSystem";

        public static readonly Permission ManageTickets = new() 
        {
            Name = "Admin area. Manage Tickets",
            SystemName = TicketSystemDefaults.ProviderSystemName,
            Area = "Admin area",
            Category = TicketSystem,
            Actions = new List<string> { 
                PermissionActionName.List, 
                PermissionActionName.Create, 
                PermissionActionName.Edit, 
                PermissionActionName.Delete,
                PermissionActionName.Preview,
                PermissionActionName.Export,
                PermissionActionName.Import
            }
        };
        
        public static readonly Permission ManageContactForms = new() 
        {
            Name = "Admin area. Manage Contact Forms",
            SystemName = "Widgets.TicketSystem.ContactForms",
            Area = "Admin area",
            Category = TicketSystem,
            Actions = new List<string> { 
                PermissionActionName.List, 
                PermissionActionName.Create, 
                PermissionActionName.Edit, 
                PermissionActionName.Delete,
                PermissionActionName.Preview
            }
        };
    }
} 