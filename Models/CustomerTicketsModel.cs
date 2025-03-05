using Grand.Infrastructure.Models;

namespace Widgets.TicketSystem.Models
{
    public class CustomerTicketsModel : BaseModel
    {
        public CustomerTicketsModel()
        {
            Tickets = new List<CustomerTicketModel>();
        }
        
        public IList<CustomerTicketModel> Tickets { get; set; }
    }
    
    public class CustomerTicketModel : BaseEntityModel
    {
        public string Subject { get; set; }
        public string Department { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string CreatedOn { get; set; }
    }
    
    public class CustomerTicketDetailsModel : BaseEntityModel
    {
        public CustomerTicketDetailsModel()
        {
            Notes = new List<CustomerTicketNoteModel>();
        }
        
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Department { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public string CreatedOn { get; set; }
        
        public IList<CustomerTicketNoteModel> Notes { get; set; }
    }
    
    public class CustomerTicketNoteModel
    {
        public string Note { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedOn { get; set; }
        public bool IsStaffNote { get; set; }
    }
    
    public class CustomerTicketReplyModel
    {
        public string TicketId { get; set; }
        public string Message { get; set; }
    }
    
    public class CreateTicketModel : BaseModel
    {
        public CreateTicketModel()
        {
            AvailableDepartments = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
        }
        
        [System.ComponentModel.DataAnnotations.Required]
        public string Subject { get; set; }
        
        [System.ComponentModel.DataAnnotations.Required]
        public string Message { get; set; }
        
        [System.ComponentModel.DataAnnotations.Required]
        public string Department { get; set; }
        public IList<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> AvailableDepartments { get; set; }
        
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
    }
} 