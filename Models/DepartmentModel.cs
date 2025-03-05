using Grand.Infrastructure.ModelBinding;
using Grand.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Widgets.TicketSystem.Models
{
    public class DepartmentModel : BaseEntityModel
    {
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Department.Fields.Name")]
        [Required]
        public string Name { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Department.Fields.Description")]
        public string Description { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Department.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Department.Fields.Active")]
        public bool Active { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Department.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Department.Fields.CreatedOn")]
        public string CreatedOn { get; set; }
        public DateTime CreatedOnUtc { get; set; }
    }
    
    public class DepartmentListModel : BaseModel
    {
        public DepartmentListModel()
        {
            AvailableStores = new List<SelectListItem>();
        }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Department.List.SearchName")]
        public string SearchName { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Department.List.SearchStoreId")]
        public string SearchStoreId { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Department.List.SearchActive")]
        public bool? SearchActive { get; set; }
        
        public IList<SelectListItem> AvailableStores { get; set; }
        
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
} 