using Grand.Infrastructure.ModelBinding;
using Grand.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Widgets.TicketSystem.Models
{
    public class ContactFormFieldValueModel : BaseModel
    {
        public ContactFormFieldValueModel()
        {
            AvailableOptions = new List<SelectListItem>();
        }
        
        public string Id { get; set; }
        
        public string ContactFormId { get; set; }
        
        public string FieldId { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Field.Value.Name")]
        public string Name { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Field.Value.FieldType")]
        public string FieldType { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Field.Value.IsRequired")]
        public bool IsRequired { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.ContactForm.Field.Value.Value")]
        public string Value { get; set; }
        
        public IList<SelectListItem> AvailableOptions { get; set; }
        
        public bool IsText => FieldType == "Text";
        public bool IsTextArea => FieldType == "TextArea";
        public bool IsNumber => FieldType == "Number";
        public bool IsEmail => FieldType == "Email";
        public bool IsDate => FieldType == "Date";
        public bool IsCheckbox => FieldType == "Checkbox";
        public bool IsRadio => FieldType == "Radio";
        public bool IsDropdown => FieldType == "Dropdown";
        public bool IsMultiSelect => FieldType == "MultiSelect";
        public bool IsFile => FieldType == "File";
        
        public int DisplayOrder { get; set; }
    }
    
    public class ContactFormFieldTypeModel
    {
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public bool SupportsOptions { get; set; }
        public bool SupportsDefaultValue { get; set; }
    }
} 