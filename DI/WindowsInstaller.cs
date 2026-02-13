using UnityEngine;
using VContainer;
using Core.Services;
using UI.Windows.Splash;
using UI.Windows.MainMenu;
using UI.Windows.DailyQuests;
using UI.Windows.DailyLogin;
using UI.Windows.Settings;
using UI.Windows.Achievements;
using UI.Windows.Profile;
using UI.Windows.Leaderboard;
using UI.Windows.Guide;
using UI.Popups.Updates;
using UI.Windows.CharacterInfo;
using UI.Windows.CharacterCreate;
using UI.Panels;

namespace Core.DI
{
    public class WindowsInstaller : MonoBehaviour
    {
        [Header("Persistent Panels")]
        [SerializeField] private TopCurrencyPanel _topCurrencyPanel; 

        [Header("Screens")]
        [SerializeField] private SplashWindow _splashWindowPrefab;
        [SerializeField] private MainMenuWindow _mainMenuWindowPrefab;
        [SerializeField] private CharacterCreateWindow _characterCreateWindowPrefab; 

        [Header("Overlay Windows")]
        [SerializeField] private CharacterInfoWindow _characterInfoWindowPrefab;
        [SerializeField] private CharacterEditWindow _characterEditWindowPrefab; 
        [SerializeField] private DailyQuestsWindow _dailyQuestsWindowPrefab;
        [SerializeField] private ShopWindow _shopWindowPrefab;
        [SerializeField] private SettingsWindow _settingsWindowPrefab;
        [SerializeField] private AchievementsWindow _achievementsWindowPrefab;
        [SerializeField] private ProfileWindow _profileWindowPrefab;
        [SerializeField] private LeaderboardWindow _leaderboardWindowPrefab;
        [SerializeField] private GuideWindow _guideWindowPrefab;

        [Header("Popups")]
        [SerializeField] private DailyLoginPopup _dailyLoginPopupPrefab;
        [SerializeField] private UpdatesPopup _updatesPopupPrefab;

        private UIService _uiService;
        private IObjectResolver _container;

        public void Initialize(UIService uiService, IObjectResolver container)
        {
            _uiService = uiService;
            _container = container;

            if (_topCurrencyPanel != null)
                _topCurrencyPanel.Initialize(container.Resolve<CurrencyService>(), uiService);
            else
                Debug.LogWarning("TopCurrencyPanel not assigned!!!");

            RegisterAllWindows();
        }

        private void RegisterAllWindows()
        {
            RegisterWindow<SplashWindow, SplashPresenter>(_splashWindowPrefab);
            RegisterWindow<MainMenuWindow, MainMenuPresenter>(_mainMenuWindowPrefab);
            RegisterWindow<CharacterCreateWindow, CharacterCreatePresenter>(_characterCreateWindowPrefab);

            RegisterWindow<CharacterInfoWindow, CharacterInfoPresenter>(_characterInfoWindowPrefab);
            RegisterWindow<CharacterEditWindow, CharacterEditPresenter>(_characterEditWindowPrefab);
            RegisterWindow<DailyQuestsWindow, DailyQuestsPresenter>(_dailyQuestsWindowPrefab);
            RegisterWindow<ShopWindow, ShopPresenter>(_shopWindowPrefab);
            RegisterWindow<SettingsWindow, SettingsPresenter>(_settingsWindowPrefab);
            RegisterWindow<AchievementsWindow, AchievementsPresenter>(_achievementsWindowPrefab);
            RegisterWindow<ProfileWindow, ProfilePresenter>(_profileWindowPrefab);
            RegisterWindow<LeaderboardWindow, LeaderboardPresenter>(_leaderboardWindowPrefab);
            RegisterWindow<GuideWindow, GuidePresenter>(_guideWindowPrefab);

            RegisterWindow<DailyLoginPopup, DailyLoginPresenter>(_dailyLoginPopupPrefab);
            RegisterWindow<UpdatesPopup, UpdatesPresenter>(_updatesPopupPrefab);
        }

        private void RegisterWindow<TWindow, TPresenter>(TWindow prefab)
            where TWindow : Window
            where TPresenter : class
        {
            if (prefab == null)
            {
                Debug.LogWarning($"Prefab emprty: {typeof(TWindow).Name}");
                return;
            }

            var window = Instantiate(prefab, transform);
            var presenter = _container.Resolve<TPresenter>();

            window.gameObject.SetActive(false);

            if (window is IConstructable<TPresenter> constructable)
                constructable.Construct(presenter);
            else
                Debug.LogWarning($"{typeof(TWindow).Name} missing IConstructable<{typeof(TPresenter).Name}>");

            _uiService.RegisterWindow(window);
            Debug.Log($"{typeof(TWindow).Name}");
        }
    }

    public interface IConstructable<TPresenter>
    {
        void Construct(TPresenter presenter);
    }
}