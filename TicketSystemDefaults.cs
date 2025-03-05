namespace Widgets.TicketSystem
{
    public static class TicketSystemDefaults
    {
        public const string ProviderSystemName = "Widgets.TicketSystem";
        public const string FriendlyName = "Widgets.TicketSystem.FriendlyName";
        public const string ConfigurationUrl = "../TicketSystem/Configure";
        
        // Cache anahtarları
        public const string WidgetZonesCacheKey = "Widgets.TicketSystem.Zones";
        public const string CachePrefix = "Widgets.TicketSystem.";
        public const string DepartmentsPrefixCacheKey = "Widgets.TicketSystem.Departments.";
        
        // Widget bölgeleri
        public const string SupportWidgetZoneXml = "Plugins/Widgets.TicketSystem/SupportWidgetZone.xml";
        
        // Ticket durumları
        public const string StatusOpen = "Open";
        public const string StatusProcessing = "Processing";
        public const string StatusClosed = "Closed";
        
        // Ticket öncelikleri
        public const string PriorityLow = "Low";
        public const string PriorityNormal = "Normal";
        public const string PriorityHigh = "High";
    }
} 