using Grand.Infrastructure.ModelBinding;
using Grand.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace Widgets.TicketSystem.Models
{
    public class ConfigurationModel : BaseModel
    {
        public ConfigurationModel()
        {
            SelectedCustomerGroupIds = new List<string>();
        }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Fields.Enable")]
        public bool Enable { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }
 
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Fields.LimitedToGroups")]
        public bool LimitedToGroups { get; set; }
        
        [UIHint("CustomerGroups")]
        public IList<string> SelectedCustomerGroupIds { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Fields.SendEmailNotification")]
        public bool SendEmailNotification { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Fields.NotificationEmail")]
        [EmailAddress]
        public string NotificationEmail { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Fields.ShowTicketListInCustomerAccount")]
        public bool ShowTicketListInCustomerAccount { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Fields.ShowTicketOptionOnProductPage")]
        public bool ShowTicketOptionOnProductPage { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Fields.ShowTicketOptionOnOrderDetailsPage")]
        public bool ShowTicketOptionOnOrderDetailsPage { get; set; }
    }
} 