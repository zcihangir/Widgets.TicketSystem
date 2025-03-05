using Grand.Business.Core.Interfaces.Cms;
using Grand.Business.Core.Interfaces.Common.Localization;
using Grand.Infrastructure.Caching;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace Widgets.TicketSystem
{
    public class TicketSystemWidgetProvider : IWidgetProvider
    {
        private readonly TicketSystemSettings _ticketSystemSettings;
        private readonly ITranslationService _translationService;
        private readonly ICacheBase _cacheBase;
        private readonly ILogger<TicketSystemWidgetProvider> _logger;

        public TicketSystemWidgetProvider(
            TicketSystemSettings ticketSystemSettings,
            ITranslationService translationService,
            ICacheBase cacheBase,
            ILogger<TicketSystemWidgetProvider> logger)
        {
            _ticketSystemSettings = ticketSystemSettings;
            _translationService = translationService;
            _cacheBase = cacheBase;
            _logger = logger;
        }

        /// <summary>
        /// Konfigürasyon sayfasının URL'si
        /// </summary>
        public string ConfigurationUrl => TicketSystemDefaults.ConfigurationUrl;

        /// <summary>
        /// Plugin'in sistem adı
        /// </summary>
        public string SystemName => TicketSystemDefaults.ProviderSystemName;

        /// <summary>
        /// Plugin'in kullanıcı dostu adı
        /// </summary>
        public string FriendlyName => _translationService.GetResource(TicketSystemDefaults.FriendlyName);

        /// <summary>
        /// Plugin'in görüntülenme önceliği
        /// </summary>
        public int Priority => _ticketSystemSettings.DisplayOrder;

        /// <summary>
        /// Plugin'in hangi mağazalarda gösterileceği
        /// </summary>
        public IList<string> LimitedToStores => _ticketSystemSettings.LimitedToStores;

        /// <summary>
        /// Plugin'in hangi müşteri gruplarına gösterileceği
        /// </summary>
        public IList<string> LimitedToGroups => _ticketSystemSettings.LimitedToGroups;

        /// <summary>
        /// Widget'ın gösterileceği bölgeleri döndürür
        /// </summary>
        public async Task<IList<string>> GetWidgetZones()
        {
            if (!_ticketSystemSettings.Enable)
                return new List<string>();

            try
            {
                var widgetZonePath = TicketSystemDefaults.SupportWidgetZoneXml;
                var xdoc = XDocument.Load(Path.Combine(Directory.GetCurrentDirectory(), widgetZonePath));
                return await Task.FromResult(xdoc.Root.Elements("WidgetZones").Select(e => e.Value).ToList());
            }
            catch (Exception)
            {
                return await Task.FromResult(new List<string>
                {

                        "account_navigation_after",

                    });
            }
        }

        /// <summary>
        /// Widget'ın kullanacağı ViewComponent adını döndürür
        /// </summary>
        public async Task<string> GetPublicViewComponentName(string widgetZone)
        {
            return await Task.FromResult("WidgetTicketSystem");
        }
    }
}