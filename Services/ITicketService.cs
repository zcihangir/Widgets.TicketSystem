using Grand.Domain;
using Widgets.TicketSystem.Domain;

namespace Widgets.TicketSystem.Services
{
    /// <summary>
    /// Ticket servisi arayüzü
    /// </summary>
    public interface ITicketService
    {
        /// <summary>
        /// Tüm ticket'ları getirir
        /// </summary>
        Task<IPagedList<Ticket>> GetAllTickets(
            string storeId = "",
            string customerId = "",
            string staffId = "",
            string department = "",
            string status = "",
            string priority = "",
            string orderId = "",
            string productId = "",
            string contactFormId = "",
            int pageIndex = 0,
            int pageSize = int.MaxValue
        );
        
        /// <summary>
        /// ID'ye göre ticket getirir
        /// </summary>
        Task<Ticket> GetTicketById(string id);
        
        /// <summary>
        /// Yeni ticket ekler
        /// </summary>
        Task InsertTicket(Ticket ticket);
        
        /// <summary>
        /// Ticket günceller
        /// </summary>
        Task UpdateTicket(Ticket ticket);
        
        /// <summary>
        /// Ticket siler
        /// </summary>
        Task DeleteTicket(Ticket ticket);
        
        /// <summary>
        /// Ticket'a not ekler
        /// </summary>
        Task InsertTicketNote(TicketNote ticketNote, Ticket ticket);
        
        /// <summary>
        /// Ticket notunu günceller
        /// </summary>
        Task UpdateTicketNote(TicketNote ticketNote, Ticket ticket);
        
        /// <summary>
        /// Ticket notunu siler
        /// </summary>
        Task DeleteTicketNote(TicketNote ticketNote, Ticket ticket);
        
        /// <summary>
        /// Ticket ID'sine göre notları getirir
        /// </summary>
        Task<IList<TicketNote>> GetTicketNotesByTicketId(string ticketId);
    }
} 