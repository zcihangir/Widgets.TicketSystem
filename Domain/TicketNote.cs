using Grand.Domain;

namespace Widgets.TicketSystem.Domain
{
    /// <summary>
    /// Ticket notu entity
    /// </summary>
    public class TicketNote : SubBaseEntity
    {
        public TicketNote()
        {
            CreatedOnUtc = DateTime.UtcNow;
        }
        
        /// <summary>
        /// Not içeriği
        /// </summary>
        public string Note { get; set; }
        
        /// <summary>
        /// Notu ekleyen kişi ID'si
        /// </summary>
        public string CreatedById { get; set; }
        
        /// <summary>
        /// Notu ekleyen kişi adı
        /// </summary>
        public string CreatedByName { get; set; }
        
        /// <summary>
        /// Notun müşteriye gösterilip gösterilmeyeceği
        /// </summary>
        public bool DisplayToCustomer { get; set; }
        
        /// <summary>
        /// Notun oluşturulma tarihi
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }
    }
} 