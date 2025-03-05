using Grand.Infrastructure.ModelBinding;
using Grand.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Widgets.TicketSystem.Models
{
    public class ContactFormModel : BaseEntityModel
    {
        public ContactFormModel()
        {
            AvailableWidgetZones = new List<SelectListItem>();
            AvailableStores = new List<SelectListItem>();
            FormFields = new List<ContactFormFieldModel>();
        }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Fields.Name")]
        [Required]
        public string Name { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Fields.Description")]
        public string Description { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Fields.WidgetZone")]
        [Required]
        public string WidgetZone { get; set; }
        public IList<SelectListItem> AvailableWidgetZones { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Fields.Active")]
        public bool Active { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }
        public IList<string> SelectedStoreIds { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Fields.CreatedOn")]
        public DateTime CreatedOnUtc { get; set; }
        public string CreatedOn { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Fields.UpdatedOn")]
        public DateTime? UpdatedOnUtc { get; set; }
        public string UpdatedOn { get; set; }
        
        // Form alanları
        public IList<ContactFormFieldModel> FormFields { get; set; }
    }
    
    public class ContactFormFieldModel : BaseEntityModel
    {
        public ContactFormFieldModel()
        {
            AvailableFieldTypes = new List<SelectListItem>();
            FieldOptions = new List<string>();
        }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Fields.Field.Name")]
        [Required]
        public string Name { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Fields.Field.FieldType")]
        [Required]
        public string FieldType { get; set; }
        public IList<SelectListItem> AvailableFieldTypes { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Fields.Field.IsRequired")]
        public bool IsRequired { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Fields.Field.DefaultValue")]
        public string DefaultValue { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Fields.Field.FieldOptions")]
        public IList<string> FieldOptions { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Fields.Field.DisplayOrder")]
        public int DisplayOrder { get; set; }
    }
    
    public class ContactFormListModel : BaseModel
    {
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.List.SearchName")]
        public string SearchName { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.List.SearchActive")]
        public bool? SearchActive { get; set; }
           
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
    
    public class ContactFormSubmissionModel : BaseEntityModel
    {
        public ContactFormSubmissionModel()
        {
            FormValues = new Dictionary<string, string>();
        }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Submission.Fields.ContactFormId")]
        public string ContactFormId { get; set; }
        public string ContactFormName { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Submission.Fields.CustomerId")]
        public string CustomerId { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Submission.Fields.CustomerEmail")]
        public string CustomerEmail { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Submission.Fields.CustomerFullName")]
        public string CustomerFullName { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Submission.Fields.FormValues")]
        public IDictionary<string, string> FormValues { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Submission.Fields.CreatedOn")]
        public DateTime CreatedOnUtc { get; set; }
        public string CreatedOn { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Submission.Fields.ConvertedToTicket")]
        public bool ConvertedToTicket { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Submission.Fields.TicketId")]
        public string TicketId { get; set; }
    }
    
    public class ContactFormSubmissionListModel : BaseModel
    {
        public ContactFormSubmissionListModel()
        {
            AvailableForms = new List<SelectListItem>();
            AvailableStores = new List<SelectListItem>();
        }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Submission.List.SearchFormId")]
        public string SearchFormId { get; set; }
        public IList<SelectListItem> AvailableForms { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Submission.List.SearchStoreId")]
        public string SearchStoreId { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Submission.List.SearchConvertedToTicket")]
        public bool? SearchConvertedToTicket { get; set; }
           
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
} 