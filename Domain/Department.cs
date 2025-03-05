using Grand.Domain;
using Grand.Domain.Common;

namespace Widgets.TicketSystem.Domain
{
    public class Department : BaseEntity
    {
        
        /// <summary>
        /// Departman adı
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Departman açıklaması
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Departmanın aktif olup olmadığı
        /// </summary>
        public bool Active { get; set; }
        
        /// <summary>
        /// Görüntülenme sırası
        /// </summary>
        public int DisplayOrder { get; set; }
        
        /// <summary>
        /// Mağazalara özel mi
        /// </summary>
        public bool LimitedToStores { get; set; }
        

    }
} 