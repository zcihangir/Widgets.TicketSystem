using Grand.Domain;
using Grand.Domain.Stores;

namespace Widgets.TicketSystem.Domain
{
    /// <summary>
    /// İletişim formu entity
    /// </summary>
    public class ContactForm : BaseEntity
    {
        public ContactForm()
        {
            FormFields = new List<ContactFormField>();
            StoreIds = new List<string>();
            CreatedOnUtc = DateTime.UtcNow;
            UpdatedOnUtc = DateTime.UtcNow;
        }
        
        /// <summary>
        /// Form adı
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Form açıklaması
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Form widget bölgesi
        /// </summary>
        public string WidgetZone { get; set; }
        
        /// <summary>
        /// Form aktif mi?
        /// </summary>
        public bool Active { get; set; }
        
        /// <summary>
        /// Form görüntülenme sırası
        /// </summary>
        public int DisplayOrder { get; set; }
        
        /// <summary>
        /// Mağazalara özel mi?
        /// </summary>
        public bool LimitedToStores { get; set; }
        
        /// <summary>
        /// Mağaza ID'leri
        /// </summary>
        public IList<string> StoreIds { get; set; }
        
        /// <summary>
        /// Form alanları
        /// </summary>
        public IList<ContactFormField> FormFields { get; set; }
    }
} 