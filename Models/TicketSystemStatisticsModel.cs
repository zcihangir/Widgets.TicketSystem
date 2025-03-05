using Grand.Infrastructure.ModelBinding;
using Grand.Infrastructure.Models;

namespace Widgets.TicketSystem.Models
{
    public class TicketSystemStatisticsModel : BaseModel
    {
        public TicketSystemStatisticsModel()
        {
            StatusStatistics = new List<StatusStatisticsModel>();
            PriorityStatistics = new List<PriorityStatisticsModel>();
            DepartmentStatistics = new List<DepartmentStatisticsModel>();
            StaffStatistics = new List<StaffStatisticsModel>();
            MonthlyStatistics = new List<MonthlyStatisticsModel>();
        }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Statistics.TotalTickets")]
        public int TotalTickets { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Statistics.OpenTickets")]
        public int OpenTickets { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Statistics.ClosedTickets")]
        public int ClosedTickets { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Statistics.TicketsCreatedToday")]
        public int TicketsCreatedToday { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Statistics.TicketsClosedToday")]
        public int TicketsClosedToday { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Statistics.AverageResponseTime")]
        public string AverageResponseTime { get; set; }
        
        [GrandResourceDisplayName("Plugins.Widgets.TicketSystem.Statistics.AverageResolutionTime")]
        public string AverageResolutionTime { get; set; }
        
        public IList<StatusStatisticsModel> StatusStatistics { get; set; }
        public IList<PriorityStatisticsModel> PriorityStatistics { get; set; }
        public IList<DepartmentStatisticsModel> DepartmentStatistics { get; set; }
        public IList<StaffStatisticsModel> StaffStatistics { get; set; }
        public IList<MonthlyStatisticsModel> MonthlyStatistics { get; set; }
    }
    
    public class StatusStatisticsModel
    {
        public string StatusId { get; set; }
        public string StatusName { get; set; }
        public string StatusColor { get; set; }
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }
    
    public class PriorityStatisticsModel
    {
        public string PriorityId { get; set; }
        public string PriorityName { get; set; }
        public string PriorityColor { get; set; }
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }
    
    public class DepartmentStatisticsModel
    {
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }
    
    public class StaffStatisticsModel
    {
        public string StaffId { get; set; }
        public string StaffName { get; set; }
        public int AssignedTickets { get; set; }
        public int ClosedTickets { get; set; }
        public decimal ClosedPercentage { get; set; }
        public string AverageResponseTime { get; set; }
    }
    
    public class MonthlyStatisticsModel
    {
        public string Month { get; set; }
        public int Year { get; set; }
        public int Created { get; set; }
        public int Closed { get; set; }
    }
} 