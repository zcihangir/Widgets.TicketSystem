using Grand.Domain;

namespace Widgets.TicketSystem.Domain
{
    /// <summary>
    /// İletişim formu alanı entity
    /// </summary>
    public class ContactFormField : SubBaseEntity
    {
        public ContactFormField()
        {
            FieldOptions = new List<string>();
        }
        
        /// <summary>
        /// Alan adı
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Alan tipi (Text, TextArea, Email, Number, Date, Select, Checkbox, Radio)
        /// </summary>
        public string FieldType { get; set; }
        
        /// <summary>
        /// Alan zorunlu mu?
        /// </summary>
        public bool IsRequired { get; set; }
        
        /// <summary>
        /// Alan varsayılan değeri
        /// </summary>
        public string DefaultValue { get; set; }
        
        /// <summary>
        /// Alan seçenekleri (Select, Checkbox, Radio için)
        /// </summary>
        public IList<string> FieldOptions { get; set; }
        
        /// <summary>
        /// Alan görüntülenme sırası
        /// </summary>
        public int DisplayOrder { get; set; }
    }
} 