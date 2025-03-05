using Grand.Data;
using Grand.Domain;
using Grand.Infrastructure.Caching;
using Microsoft.Extensions.Logging;
using Widgets.TicketSystem.Domain;

namespace Widgets.TicketSystem.Services
{
    /// <summary>
    /// İletişim formu servisi implementasyonu
    /// </summary>
    public class ContactFormService : IContactFormService
    {
        #region Fields
        private readonly IRepository<ContactForm> _contactFormRepository;
        private readonly IRepository<ContactFormSubmission> _submissionRepository;
        private readonly ITicketService _ticketService;
        private readonly ICacheBase _cacheBase;
        private readonly ILogger<ContactFormService> _logger;
        #endregion

        #region Ctor
        public ContactFormService(
            IRepository<ContactForm> contactFormRepository,
            IRepository<ContactFormSubmission> submissionRepository,
            ITicketService ticketService,
            ICacheBase cacheBase,
            ILogger<ContactFormService> logger)
        {
            _contactFormRepository = contactFormRepository;
            _submissionRepository = submissionRepository;
            _ticketService = ticketService;
            _cacheBase = cacheBase;
            _logger = logger;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Tüm formları getirir
        /// </summary>
        public async Task<IPagedList<ContactForm>> GetAllContactForms(
            string storeId = "",
            bool? active = null,
            string widgetZone = "",
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            try
            {
                var query = _contactFormRepository.Table;

                // Mağaza filtresi
                if (!string.IsNullOrEmpty(storeId))
                {
                    query = query.Where(x => x.LimitedToStores == false || x.StoreIds.Contains(storeId));
                }

                // Aktiflik filtresi
                if (active.HasValue)
                    query = query.Where(x => x.Active == active.Value);

                // Widget bölgesi filtresi
                if (!string.IsNullOrEmpty(widgetZone))
                    query = query.Where(x => x.WidgetZone == widgetZone);

                // Sıralama
                query = query.OrderBy(x => x.DisplayOrder).ThenBy(x => x.Name);

                return await PagedList<ContactForm>.Create(query, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllContactForms");
                throw;
            }
        }

        /// <summary>
        /// ID'ye göre form getirir
        /// </summary>
        public async Task<ContactForm> GetContactFormById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
                
            return await _contactFormRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Yeni form ekler
        /// </summary>
        public async Task InsertContactForm(ContactForm contactForm)
        {
            if (contactForm == null)
                throw new ArgumentNullException(nameof(contactForm));
                
            contactForm.CreatedOnUtc = DateTime.UtcNow;
            contactForm.UpdatedOnUtc = DateTime.UtcNow;
            
            await _contactFormRepository.InsertAsync(contactForm);
            
            // Cache'i temizle
            await _cacheBase.RemoveByPrefix(TicketSystemDefaults.CachePrefix);
            
            _logger.LogInformation("New contact form added. ID: {0}", contactForm.Id);
        }

        /// <summary>
        /// Form günceller
        /// </summary>
        public async Task UpdateContactForm(ContactForm contactForm)
        {
            if (contactForm == null)
                throw new ArgumentNullException(nameof(contactForm));
                
            contactForm.UpdatedOnUtc = DateTime.UtcNow;
            
            await _contactFormRepository.UpdateAsync(contactForm);
            
            // Cache'i temizle
            await _cacheBase.RemoveByPrefix(TicketSystemDefaults.CachePrefix);
            
            _logger.LogInformation("Contact form updated. ID: {0}", contactForm.Id);
        }

        /// <summary>
        /// Form siler
        /// </summary>
        public async Task DeleteContactForm(ContactForm contactForm)
        {
            if (contactForm == null)
                throw new ArgumentNullException(nameof(contactForm));
                
            await _contactFormRepository.DeleteAsync(contactForm);
            
            // Cache'i temizle
            await _cacheBase.RemoveByPrefix(TicketSystemDefaults.CachePrefix);
            
            _logger.LogInformation("Contact form deleted. ID: {0}", contactForm.Id);
        }

        /// <summary>
        /// Tüm form gönderimlerini getirir
        /// </summary>
        public async Task<IPagedList<ContactFormSubmission>> GetAllContactFormSubmissions(
            string storeId = "",
            string contactFormId = "",
            string customerId = "",
            bool? convertedToTicket = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue)
        {
            try
            {
                var query = _submissionRepository.Table;

                // Mağaza filtresi


                // Form filtresi
                if (!string.IsNullOrEmpty(contactFormId))
                    query = query.Where(x => x.ContactFormId == contactFormId);

                // Müşteri filtresi
                if (!string.IsNullOrEmpty(customerId))
                    query = query.Where(x => x.CustomerId == customerId);

                // Ticket'a dönüştürülme filtresi
                if (convertedToTicket.HasValue)
                    query = query.Where(x => x.ConvertedToTicket == convertedToTicket.Value);

                // Sıralama
                query = query.OrderByDescending(x => x.CreatedOnUtc);

                return await PagedList<ContactFormSubmission>.Create(query, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllContactFormSubmissions");
                throw;
            }
        }

        /// <summary>
        /// ID'ye göre form gönderimi getirir
        /// </summary>
        public async Task<ContactFormSubmission> GetContactFormSubmissionById(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
                
            return await _submissionRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Yeni form gönderimi ekler
        /// </summary>
        public async Task InsertContactFormSubmission(ContactFormSubmission submission)
        {
            if (submission == null)
                throw new ArgumentNullException(nameof(submission));
                
            submission.CreatedOnUtc = DateTime.UtcNow;
            
            await _submissionRepository.InsertAsync(submission);
            
            _logger.LogInformation("New contact form submission added. ID: {0}", submission.Id);
        }

        /// <summary>
        /// Form gönderimi günceller
        /// </summary>
        public async Task UpdateContactFormSubmission(ContactFormSubmission submission)
        {
            if (submission == null)
                throw new ArgumentNullException(nameof(submission));
                
            await _submissionRepository.UpdateAsync(submission);
            
            _logger.LogInformation("Contact form submission updated. ID: {0}", submission.Id);
        }

        /// <summary>
        /// Form gönderimi siler
        /// </summary>
        public async Task DeleteContactFormSubmission(ContactFormSubmission submission)
        {
            if (submission == null)
                throw new ArgumentNullException(nameof(submission));
                
            await _submissionRepository.DeleteAsync(submission);
            
            _logger.LogInformation("Contact form submission deleted. ID: {0}", submission.Id);
        }

        /// <summary>
        /// Form gönderimini ticket'a dönüştürür
        /// </summary>
        public async Task<Ticket> ConvertSubmissionToTicket(ContactFormSubmission submission)
        {
            if (submission == null)
                throw new ArgumentNullException(nameof(submission));
                
            if (submission.ConvertedToTicket)
                throw new InvalidOperationException("Submission already converted to ticket");
                
            try
            {
                // Form bilgilerini al
                var contactForm = await GetContactFormById(submission.ContactFormId);
                if (contactForm == null)
                    throw new InvalidOperationException("Contact form not found");
                
                // Form değerlerini daha okunabilir bir formatta hazırla
                var messageBuilder = new System.Text.StringBuilder();
                messageBuilder.AppendLine($"Form: {contactForm.Name}");
                messageBuilder.AppendLine("-----------------------------------");
                
                // Form alanlarını ve değerlerini ekle
                foreach (var field in contactForm.FormFields.OrderBy(f => f.DisplayOrder))
                {
                    if (submission.FormValues.TryGetValue(field.Name, out var value))
                    {
                        messageBuilder.AppendLine($"{field.Name}: {value}");
                    }
                }
                
                // Eğer form alanları dışında değerler varsa onları da ekle
                var extraFields = submission.FormValues.Keys.Except(contactForm.FormFields.Select(f => f.Name));
                if (extraFields.Any())
                {
                    messageBuilder.AppendLine();
                    messageBuilder.AppendLine("Diğer Bilgiler:");
                    messageBuilder.AppendLine("-----------------------------------");
                    
                    foreach (var key in extraFields)
                    {
                        messageBuilder.AppendLine($"{key}: {submission.FormValues[key]}");
                    }
                }
                
                // Ticket oluştur
                var ticket = new Ticket
                {
                    Subject = $"İletişim Formu: {contactForm.Name}",
                    Message = messageBuilder.ToString(),
                    Department = "Support", // Varsayılan departman
                    Status = TicketSystemDefaults.StatusOpen,
                    Priority = TicketSystemDefaults.PriorityNormal,
                    CustomerId = submission.CustomerId,
                    CustomerEmail = submission.CustomerEmail,
                    CustomerFullName = submission.CustomerFullName,
                    ContactFormId = submission.ContactFormId,
                };
                
                await _ticketService.InsertTicket(ticket);
                
                // Submission'ı güncelle
                submission.ConvertedToTicket = true;
                submission.TicketId = ticket.Id;
                await UpdateContactFormSubmission(submission);
                
                _logger.LogInformation("Contact form submission converted to ticket. Submission ID: {0}, Ticket ID: {1}", submission.Id, ticket.Id);
                
                return ticket;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error converting submission to ticket. Submission ID: {0}", submission.Id);
                throw;
            }
        }
        #endregion
    }
} 