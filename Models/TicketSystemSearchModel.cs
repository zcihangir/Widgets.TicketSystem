using Grand.Infrastructure.ModelBinding;
using Grand.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Widgets.TicketSystem.Models
{
    public class TicketSystemSearchModel : BaseModel
    {
        public TicketSystemSearchModel()
        {
            AvailableDepartments = new List<SelectListItem>();
            AvailableStatuses = new List<SelectListItem>();
            AvailablePriorities = new List<SelectListItem>();
            AvailableStaff = new List<SelectListItem>();
            AvailableDateRanges = new List<SelectListItem>();
        }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Search.Keyword")]
        public string Keyword { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Search.TicketNumber")]
        public string TicketNumber { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Search.CustomerEmail")]
        public string CustomerEmail { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Search.DepartmentId")]
        public string DepartmentId { get; set; }
        public IList<SelectListItem> AvailableDepartments { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Search.StatusId")]
        public string StatusId { get; set; }
        public IList<SelectListItem> AvailableStatuses { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Search.PriorityId")]
        public string PriorityId { get; set; }
        public IList<SelectListItem> AvailablePriorities { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Search.StaffId")]
        public string StaffId { get; set; }
        public IList<SelectListItem> AvailableStaff { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Search.DateRange")]
        public string DateRangeId { get; set; }
        public IList<SelectListItem> AvailableDateRanges { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Search.StartDate")]
        public DateTime? StartDate { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Search.EndDate")]
        public DateTime? EndDate { get; set; }
        
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
} 