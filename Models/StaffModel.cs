using Grand.Infrastructure.ModelBinding;
using Grand.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Widgets.TicketSystem.Models
{
    public class StaffModel : BaseEntityModel
    {
        public StaffModel()
        {
            
            AvailableDepartments = new List<SelectListItem>();
            SelectedDepartmentIds = new List<string>();
        }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Staff.Fields.Name")]
        [Required]
        public string Name { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Staff.Fields.Email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Staff.Fields.CustomerId")]
        public string CustomerId { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Staff.Fields.CustomerEmail")]
        public string CustomerEmail { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Staff.Fields.CustomerFullName")]
        public string CustomerFullName { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Staff.Fields.Active")]
        public bool Active { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Staff.Fields.IsAdmin")]
        public bool IsAdmin { get; set; }

        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Staff.Fields.Departments")]
        public IList<SelectListItem> AvailableDepartments { get; set; }
        public IList<string> SelectedDepartmentIds { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Staff.Fields.CreatedOn")]
        public DateTime CreatedOnUtc { get; set; }
        public string CreatedOn { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Staff.Fields.UpdatedOn")]
        public DateTime UpdatedOnUtc { get; set; }
        public string UpdatedOn { get; set; }
    }
    
    public class StaffListModel : BaseModel
    {
        public StaffListModel()
        {
            AvailableDepartments = new List<SelectListItem>();
        }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Staff.List.SearchName")]
        public string SearchName { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Staff.List.SearchEmail")]
        public string SearchEmail { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Staff.List.SearchActive")]
        public bool? SearchActive { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Staff.List.SearchDepartmentId")]
        public string SearchDepartmentId { get; set; }
        public IList<SelectListItem> AvailableDepartments { get; set; }
     
        
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
} 