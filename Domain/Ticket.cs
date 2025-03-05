using Grand.Domain;
using Grand.Domain.Stores;

namespace Widgets.TicketSystem.Domain
{
    /// <summary>
    /// Ticket entity
    /// </summary>
    public class Ticket : BaseEntity
    {
        public Ticket()
        {
            TicketNotes = new List<TicketNote>();
            CreatedOnUtc = DateTime.UtcNow;
            UpdatedOnUtc = DateTime.UtcNow;
        }
        
        /// <summary>
        /// Ticket başlığı
        /// </summary>
        public string Subject { get; set; }
        
        /// <summary>
        /// Ticket içeriği
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Ticket departmanı
        /// </summary>
        public string Department { get; set; }
        
        /// <summary>
        /// Ticket durumu (Open, Processing, Closed)
        /// </summary>
        public string Status { get; set; }
        
        /// <summary>
        /// Ticket önceliği (Low, Normal, High)
        /// </summary>
        public string Priority { get; set; }
        
        /// <summary>
        /// Ticket'ı oluşturan müşteri ID'si
        /// </summary>
        public string CustomerId { get; set; }
        
        /// <summary>
        /// Ticket'ı oluşturan müşteri e-posta adresi
        /// </summary>
        public string CustomerEmail { get; set; }
        
        /// <summary>
        /// Ticket'ı oluşturan müşteri adı
        /// </summary>
        public string CustomerFullName { get; set; }
        
        /// <summary>
        /// Ticket'a atanan personel ID'si
        /// </summary>
        public string StaffId { get; set; }
        
        /// <summary>
        /// Ticket'a atanan personel adı
        /// </summary>
        public string StaffName { get; set; }
        
        /// <summary>
        /// Ticket'ın ilişkili olduğu sipariş ID'si
        /// </summary>
        public string OrderId { get; set; }
        
        /// <summary>
        /// Ticket'ın ilişkili olduğu ürün ID'si
        /// </summary>
        public string ProductId { get; set; }
        
        /// <summary>
        /// Ticket'ın ilişkili olduğu form ID'si
        /// </summary>
        public string ContactFormId { get; set; }
        
        /// <summary>
        /// Ticket notları
        /// </summary>
        public IList<TicketNote> TicketNotes { get; set; }
        
      
    }
} 