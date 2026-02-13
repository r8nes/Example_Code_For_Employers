using UnityEngine;
using VContainer;
using VContainer.Unity;
using Core.Services;
using UI.Windows.Splash;

namespace Core.DI
{
    public class GameInitializer : IStartable
    {
        private readonly SaveService _saveService;
        private readonly AuthService _authService;
        private readonly DataService _dataService;
        private readonly CurrencyService _currencyService;
        private readonly QuestService _questService;
        private readonly AchievementService _achievementService;
        private readonly ShopService _shopService;
        private readonly CharacterService _characterService; 
        private readonly LocalizationService _localizationService;
        private readonly AudioService _audioService;
        private readonly AnalyticsService _analyticsService;
        private readonly EventsService _eventsService;
        private readonly SeasonalService _seasonalService;
        private readonly UIService _uiService;
        private readonly WindowsInstaller _windowsInstaller;
        private readonly IObjectResolver _container;

        public GameInitializer(
            SaveService saveService,
            AuthService authService,
            DataService dataService,
            CurrencyService currencyService,
            QuestService questService,
            AchievementService achievementService,
            ShopService shopService,
            CharacterService characterService,
            LocalizationService localizationService,
            AudioService audioService,
            AnalyticsService analyticsService,
            EventsService eventsService,
            SeasonalService seasonalService,
            UIService uiService,
            WindowsInstaller windowsInstaller,
            IObjectResolver container)
        {
            _saveService = saveService;
            _authService = authService;
            _dataService = dataService;
            _currencyService = currencyService;
            _questService = questService;
            _achievementService = achievementService;
            _shopService = shopService;
            _characterService = characterService;
            _localizationService = localizationService;
            _audioService = audioService;
            _analyticsService = analyticsService;
            _eventsService = eventsService;
            _seasonalService = seasonalService;
            _uiService = uiService;
            _windowsInstaller = windowsInstaller;
            _container = container;
        }

        public void Start()
        {
            Debug.Log("[Game] Initializing...");

            _saveService.Initialize();

            _authService.Initialize();
            _dataService.Initialize();

            _currencyService.Initialize();
            _questService.Initialize();
            _achievementService.Initialize();
            _shopService.Initialize();
            _characterService.Initialize(); 

            _localizationService.Initialize();
            _audioService.Initialize();
            _analyticsService.Initialize(_authService.PlayerId);
            _seasonalService.Initialize();
            _eventsService.Initialize();

            _analyticsService.TrackSessionStart();

            // TODO: заменить на загрузку из ресурсов
            _audioService.PlayMusic(AudioClipNames.MENU_MUSIC, fadeIn: true);

            _windowsInstaller.Initialize(_uiService, _container);

            _uiService.Show<SplashWindow>();
        }
    }
}