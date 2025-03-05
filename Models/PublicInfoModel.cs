using System.Collections.Generic;

namespace Widgets.TicketSystem.Models
{
    public class PublicInfoModel
    {
        public PublicInfoModel()
        {
            ContactFormFields = new List<ContactFormFieldModel>();
        }
        
        // Ticket oluşturma için
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Department { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        
        // Contact form için
        public string ContactFormId { get; set; }
        public string ContactFormName { get; set; }
        public string ContactFormDescription { get; set; }
        public IList<ContactFormFieldModel> ContactFormFields { get; set; }
        
        public class ContactFormFieldModel
        {
            public ContactFormFieldModel()
            {
                FieldOptions = new List<string>();
            }
            
            public string Id { get; set; }
            public string Name { get; set; }
            public string FieldType { get; set; }
            public bool IsRequired { get; set; }
            public string DefaultValue { get; set; }
            public IList<string> FieldOptions { get; set; }
            public int DisplayOrder { get; set; }
        }
    }
} 