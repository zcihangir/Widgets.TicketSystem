using Grand.Infrastructure.ModelBinding;
using Grand.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Widgets.TicketSystem.Models
{
    public class TicketModel : BaseEntityModel
    {
        public TicketModel()
        {
   
            AvailableDepartments = new List<SelectListItem>();
            AvailableStatuses = new List<SelectListItem>();
            AvailablePriorities = new List<SelectListItem>();
            AvailableStaff = new List<SelectListItem>();
            TicketNotes = new List<TicketNoteModel>();
        }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.Subject")]
        [Required]
        public string Subject { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.Message")]
        [Required]
        public string Message { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.Department")]
        [Required]
        public string Department { get; set; }
        public IList<SelectListItem> AvailableDepartments { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.Status")]
        [Required]
        public string Status { get; set; }
        public IList<SelectListItem> AvailableStatuses { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.Priority")]
        [Required]
        public string Priority { get; set; }
        public IList<SelectListItem> AvailablePriorities { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.CustomerId")]
        public string CustomerId { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.CustomerEmail")]
        [EmailAddress]
        public string CustomerEmail { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.CustomerFullName")]
        public string CustomerFullName { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.StaffId")]
        public string StaffId { get; set; }
        public IList<SelectListItem> AvailableStaff { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.StaffName")]
        public string StaffName { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.OrderId")]
        public string OrderId { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.ProductId")]
        public string ProductId { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.ContactFormId")]
        public string ContactFormId { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.CreatedOn")]
        public DateTime CreatedOnUtc { get; set; }
        public string CreatedOn { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.UpdatedOn")]
        public DateTime? UpdatedOnUtc { get; set; }
        public string UpdatedOn { get; set; }
        
        // Ticket notları
        public IList<TicketNoteModel> TicketNotes { get; set; }
        
        // Yeni not ekleme
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.AddNote")]
        public string AddNote { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.DisplayToCustomer")]
        public bool DisplayToCustomer { get; set; } = true;
    }
    
    public class TicketNoteModel : BaseEntityModel
    {
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.Note")]
        public string Note { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.CreatedById")]
        public string CreatedById { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.CreatedByName")]
        public string CreatedByName { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.DisplayToCustomer")]
        public bool DisplayToCustomer { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.Fields.CreatedOn")]
        public DateTime CreatedOnUtc { get; set; }
        public string CreatedOn { get; set; }
    }
    
    public class TicketListModel : BaseModel
    {
        public TicketListModel()
        {
            AvailableDepartments = new List<SelectListItem>();
            AvailableStatuses = new List<SelectListItem>();
            AvailablePriorities = new List<SelectListItem>();
            AvailablePageSizes = "10, 20, 50, 100";
        }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.List.SearchCustomerEmail")]
        public string SearchCustomerEmail { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.List.SearchDepartment")]
        public string SearchDepartment { get; set; }
        public IList<SelectListItem> AvailableDepartments { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.List.SearchStatus")]
        public string SearchStatus { get; set; }
        public IList<SelectListItem> AvailableStatuses { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Ticket.List.SearchPriority")]
        public string SearchPriority { get; set; }
        public IList<SelectListItem> AvailablePriorities { get; set; }
        
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string AvailablePageSizes { get; set; }
    }
} 