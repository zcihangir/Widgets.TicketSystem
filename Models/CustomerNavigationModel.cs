using Grand.Infrastructure.Models;

namespace Widgets.TicketSystem.Models
{
    public class CustomerNavigationModel : BaseModel
    {
        public bool HideInfo { get; set; }
        public bool HideAddresses { get; set; }
        public bool HideOrders { get; set; }
        public bool HideOutOfStockSubscriptions { get; set; }
        public bool HideMerchandiseReturns { get; set; }
        public bool HideDownloadableProducts { get; set; }
        public bool HideLoyaltyPoints { get; set; }
        public bool HideChangePassword { get; set; }
        public bool HideDeleteAccount { get; set; }
        public bool HideAuctions { get; set; }
        public bool HideNotes { get; set; }
        public bool HideDocuments { get; set; }
        public bool HideReviews { get; set; }
        public bool HideCourses { get; set; }
        public bool HideSubAccounts { get; set; }
        public bool HideTickets { get; set; }
        public AccountNavigationEnum SelectedTab { get; set; }
    }
} 