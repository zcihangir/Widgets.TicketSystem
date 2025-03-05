using Grand.Infrastructure.ModelBinding;
using Grand.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace Widgets.TicketSystem.Models
{
    public class TicketStatusModel : BaseEntityModel
    {
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.TicketStatus.Fields.Name")]
        [Required]
        public string Name { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.TicketStatus.Fields.Color")]
        [Required]
        public string Color { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.TicketStatus.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.TicketStatus.Fields.IsDefault")]
        public bool IsDefault { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.TicketStatus.Fields.IsClosed")]
        public bool IsClosed { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.TicketStatus.Fields.CreatedOn")]
        public DateTime CreatedOnUtc { get; set; }
        public string CreatedOn { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.TicketStatus.Fields.UpdatedOn")]
        public DateTime UpdatedOnUtc { get; set; }
        public string UpdatedOn { get; set; }
    }
} 