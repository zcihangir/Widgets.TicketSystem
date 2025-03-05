using Grand.Domain.Configuration;

namespace Widgets.TicketSystem
{
    public class TicketSystemSettings : ISettings
    {
        public TicketSystemSettings()
        {
            LimitedToStores = new List<string>();
            LimitedToGroups = new List<string>();
        }
        
        /// <summary>
        /// Eklentinin aktif olup olmadığını belirler
        /// </summary>
        public bool Enable { get; set; }
        
        /// <summary>
        /// Eklentinin görüntülenme sırası
        /// </summary>
        public int DisplayOrder { get; set; }
        
        /// <summary>
        /// Eklentinin hangi mağazalarda gösterileceği
        /// </summary>
        public IList<string> LimitedToStores { get; set; }
        
        /// <summary>
        /// Eklentinin hangi müşteri gruplarına gösterileceği
        /// </summary>
        public IList<string> LimitedToGroups { get; set; }
        
        /// <summary>
        /// Yeni ticket oluşturulduğunda bildirim e-postası gönderilecek mi?
        /// </summary>
        public bool SendEmailNotification { get; set; }
        
        /// <summary>
        /// Bildirim e-postası gönderilecek adres
        /// </summary>
        public string NotificationEmail { get; set; }
        
        /// <summary>
        /// Müşteri hesabında ticket listesi gösterilsin mi?
        /// </summary>
        public bool ShowTicketListInCustomerAccount { get; set; }
        
        /// <summary>
        /// Ürün sayfasında ticket oluşturma seçeneği gösterilsin mi?
        /// </summary>
        public bool ShowTicketOptionOnProductPage { get; set; }
        
        /// <summary>
        /// Sipariş detay sayfasında ticket oluşturma seçeneği gösterilsin mi?
        /// </summary>
        public bool ShowTicketOptionOnOrderDetailsPage { get; set; }
    }
} 