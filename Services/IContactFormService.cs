using Grand.Domain;
using Widgets.TicketSystem.Domain;

namespace Widgets.TicketSystem.Services
{
    /// <summary>
    /// İletişim formu servisi arayüzü
    /// </summary>
    public interface IContactFormService
    {
        /// <summary>
        /// Tüm formları getirir
        /// </summary>
        Task<IPagedList<ContactForm>> GetAllContactForms(
            string storeId = "",
            bool? active = null,
            string widgetZone = "",
            int pageIndex = 0,
            int pageSize = int.MaxValue
        );
        
        /// <summary>
        /// ID'ye göre form getirir
        /// </summary>
        Task<ContactForm> GetContactFormById(string id);
        
        /// <summary>
        /// Yeni form ekler
        /// </summary>
        Task InsertContactForm(ContactForm contactForm);
        
        /// <summary>
        /// Form günceller
        /// </summary>
        Task UpdateContactForm(ContactForm contactForm);
        
        /// <summary>
        /// Form siler
        /// </summary>
        Task DeleteContactForm(ContactForm contactForm);
        
        /// <summary>
        /// Tüm form gönderimlerini getirir
        /// </summary>
        Task<IPagedList<ContactFormSubmission>> GetAllContactFormSubmissions(
            string storeId = "",
            string contactFormId = "",
            string customerId = "",
            bool? convertedToTicket = null,
            int pageIndex = 0,
            int pageSize = int.MaxValue
        );
        
        /// <summary>
        /// ID'ye göre form gönderimi getirir
        /// </summary>
        Task<ContactFormSubmission> GetContactFormSubmissionById(string id);
        
        /// <summary>
        /// Yeni form gönderimi ekler
        /// </summary>
        Task InsertContactFormSubmission(ContactFormSubmission submission);
        
        /// <summary>
        /// Form gönderimi günceller
        /// </summary>
        Task UpdateContactFormSubmission(ContactFormSubmission submission);
        
        /// <summary>
        /// Form gönderimi siler
        /// </summary>
        Task DeleteContactFormSubmission(ContactFormSubmission submission);
        
        /// <summary>
        /// Form gönderimini ticket'a dönüştürür
        /// </summary>
        Task<Ticket> ConvertSubmissionToTicket(ContactFormSubmission submission);
    }
} 