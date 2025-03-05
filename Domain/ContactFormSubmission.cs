using Grand.Domain;
using Grand.Domain.Stores;

namespace Widgets.TicketSystem.Domain
{
    /// <summary>
    /// İletişim formu gönderimi entity
    /// </summary>
    public class ContactFormSubmission : BaseEntity
    {
        public ContactFormSubmission()
        {
            FormValues = new Dictionary<string, string>();
            CreatedOnUtc = DateTime.UtcNow;
        }
        
        /// <summary>
        /// İlişkili form ID'si
        /// </summary>
        public string ContactFormId { get; set; }
        
        /// <summary>
        /// Gönderen müşteri ID'si
        /// </summary>
        public string CustomerId { get; set; }
        
        /// <summary>
        /// Gönderen müşteri e-posta adresi
        /// </summary>
        public string CustomerEmail { get; set; }
        
        /// <summary>
        /// Gönderen müşteri adı
        /// </summary>
        public string CustomerFullName { get; set; }
        
        /// <summary>
        /// Form değerleri (alan adı, değer)
        /// </summary>
        public IDictionary<string, string> FormValues { get; set; }
        
        
        /// <summary>
        /// Gönderim ticket'a dönüştürüldü mü?
        /// </summary>
        public bool ConvertedToTicket { get; set; }
        
        /// <summary>
        /// İlişkili ticket ID'si
        /// </summary>
        public string TicketId { get; set; }
        
    }
} 