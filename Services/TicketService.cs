using Grand.Data;
using Grand.Domain;
using Grand.Infrastructure.Caching;
using Microsoft.Extensions.Logging;
using Widgets.TicketSystem.Domain;

namespace Widgets.TicketSystem.Services
{
    /// <summary>
    /// Ticket servisi implementasyonu
    /// </summary>
    public class TicketService : ITicketService
    {
        #region Fields
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly ICacheBase _cacheBase;
        private readonly ILogger<TicketService> _logger;
        #endregion

        #region Ctor
        public TicketService(
            IRepository<Ticket> ticketRepository,
            ICacheBase cacheBase,
            ILogger<TicketService> logger)
        {
            _ticketRepository = ticketRepository;
            _cacheBase = cacheBase;
            _logger = logger;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Tüm ticket'ları getirir
        /// </summary>
        public async Task<IPagedList<Ticket>> GetAllTickets(
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
            int pageSize = int.MaxValue)
        {
            try
            {
                var query = _ticketRepository.Table;

                // Mağaza filtresi


                // Müşteri filtresi
                if (!string.IsNullOrEmpty(customerId))
                    query = query.Where(x => x.CustomerId == customerId);

                // Personel filtresi
                if (!string.IsNullOrEmpty(staffId))
                    query = query.Where(x => x.StaffId == staffId);

                // Departman filtresi
                if (!string.IsNullOrEmpty(department))
                    query = query.Where(x => x.Department == department);

                // Durum filtresi
                if (!string.IsNullOrEmpty(status))
                    query = query.Where(x => x.Status == status);

                // Öncelik filtresi
                if (!string.IsNullOrEmpty(priority))
                    query = query.Where(x => x.Priority == priority);

                // Sipariş filtresi
                if (!string.IsNullOrEmpty(orderId))
                    query = query.Where(x => x.OrderId == orderId);

                // Ürün filtresi
                if (!string.IsNullOrEmpty(productId))
                    query = query.Where(x => x.ProductId == productId);

                // Form filtresi
                if (!string.IsNullOrEmpty(contactFormId))
                    query = query.Where(x => x.ContactFormId == contactFormId);

                // Sıralama
                query = query.OrderByDescending(x => x.CreatedOnUtc);

                return await PagedList<Ticket>.Create(query, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllTickets");
                throw;
            }
        }

        /// <summary>
        /// ID'ye göre ticket getirir
        /// </summary>
        public async Task<Ticket> GetTicketById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
                
            return await _ticketRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Yeni ticket ekler
        /// </summary>
        public async Task InsertTicket(Ticket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));
                
            ticket.CreatedOnUtc = DateTime.UtcNow;
            ticket.UpdatedOnUtc = DateTime.UtcNow;
            
            await _ticketRepository.InsertAsync(ticket);
            
            // Cache'i temizle
            await _cacheBase.RemoveByPrefix(TicketSystemDefaults.CachePrefix);
            
            _logger.LogInformation("New ticket added. ID: {0}", ticket.Id);
        }

        /// <summary>
        /// Ticket günceller
        /// </summary>
        public async Task UpdateTicket(Ticket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));
                
            ticket.UpdatedOnUtc = DateTime.UtcNow;
            
            await _ticketRepository.UpdateAsync(ticket);
            
            // Cache'i temizle
            await _cacheBase.RemoveByPrefix(TicketSystemDefaults.CachePrefix);
            
            _logger.LogInformation("Ticket updated. ID: {0}", ticket.Id);
        }

        /// <summary>
        /// Ticket siler
        /// </summary>
        public async Task DeleteTicket(Ticket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));
                
            await _ticketRepository.DeleteAsync(ticket);
            
            // Cache'i temizle
            await _cacheBase.RemoveByPrefix(TicketSystemDefaults.CachePrefix);
            
            _logger.LogInformation("Ticket deleted. ID: {0}", ticket.Id);
        }

        /// <summary>
        /// Ticket'a not ekler
        /// </summary>
        public async Task InsertTicketNote(TicketNote ticketNote, Ticket ticket)
        {
            if (ticketNote == null)
                throw new ArgumentNullException(nameof(ticketNote));
                
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));
                
            ticketNote.CreatedOnUtc = DateTime.UtcNow;
            ticket.TicketNotes.Add(ticketNote);
            
            await UpdateTicket(ticket);
            
            _logger.LogInformation("New ticket note added. Ticket ID: {0}, Note ID: {1}", ticket.Id, ticketNote.Id);
        }

        /// <summary>
        /// Ticket notunu günceller
        /// </summary>
        public async Task UpdateTicketNote(TicketNote ticketNote, Ticket ticket)
        {
            if (ticketNote == null)
                throw new ArgumentNullException(nameof(ticketNote));
                
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));
                
            var existingNote = ticket.TicketNotes.FirstOrDefault(x => x.Id == ticketNote.Id);
            if (existingNote == null)
                throw new ArgumentException("Note not found", nameof(ticketNote));
                
            existingNote.Note = ticketNote.Note;
            existingNote.DisplayToCustomer = ticketNote.DisplayToCustomer;
            
            await UpdateTicket(ticket);
            
            _logger.LogInformation("Ticket note updated. Ticket ID: {0}, Note ID: {1}", ticket.Id, ticketNote.Id);
        }

        /// <summary>
        /// Ticket notunu siler
        /// </summary>
        public async Task DeleteTicketNote(TicketNote ticketNote, Ticket ticket)
        {
            if (ticketNote == null)
                throw new ArgumentNullException(nameof(ticketNote));
                
            if (ticket == null)
                throw new ArgumentNullException(nameof(ticket));
                
            var existingNote = ticket.TicketNotes.FirstOrDefault(x => x.Id == ticketNote.Id);
            if (existingNote == null)
                throw new ArgumentException("Note not found", nameof(ticketNote));
                
            ticket.TicketNotes.Remove(existingNote);
            
            await UpdateTicket(ticket);
            
            _logger.LogInformation("Ticket note deleted. Ticket ID: {0}, Note ID: {1}", ticket.Id, ticketNote.Id);
        }
        
        /// <summary>
        /// Ticket ID'sine göre notları getirir
        /// </summary>
        public async Task<IList<TicketNote>> GetTicketNotesByTicketId(string ticketId)
        {
            if (string.IsNullOrEmpty(ticketId))
                return new List<TicketNote>();
                
            var ticket = await GetTicketById(ticketId);
            if (ticket == null)
                return new List<TicketNote>();
                
            return ticket.TicketNotes.OrderByDescending(x => x.CreatedOnUtc).ToList();
        }
        #endregion
    }
} 