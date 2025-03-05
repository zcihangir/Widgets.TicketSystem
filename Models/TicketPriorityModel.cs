using Grand.Infrastructure.ModelBinding;
using Grand.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace Widgets.TicketSystem.Models
{
    public class TicketPriorityModel : BaseEntityModel
    {
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.TicketPriority.Fields.Name")]
        [Required]
        public string Name { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.TicketPriority.Fields.Color")]
        [Required]
        public string Color { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.TicketPriority.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.TicketPriority.Fields.IsDefault")]
        public bool IsDefault { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.TicketPriority.Fields.CreatedOn")]
        public DateTime CreatedOnUtc { get; set; }
        public string CreatedOn { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.TicketPriority.Fields.UpdatedOn")]
        public DateTime UpdatedOnUtc { get; set; }
        public string UpdatedOn { get; set; }
    }
} 