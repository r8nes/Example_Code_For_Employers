using UnityEngine;
using VContainer;
using VContainer.Unity;
using Core.Services;
using UI.Windows.Splash;
using UI.Windows.MainMenu;
using UI.Windows.DailyQuests;
using UI.Windows.Shop;
using UI.Windows.DailyLogin;
using UI.Windows.Settings;
using UI.Windows.Achievements;
using UI.Windows.Profile;
using UI.Windows.Leaderboard;
using UI.Windows.Guide;
using UI.Popups.Updates;
using UI.Windows.CharacterInfo;
using UI.Windows.CharacterCreate;

namespace Core.DI
{
    public class ProjectLifetimeScope : LifetimeScope
    {
        [Header("Ref")]
        [SerializeField] private Transform _uiRoot;
        [SerializeField] private WindowsInstaller _windowsInstaller;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<SaveService>(Lifetime.Singleton);
            builder.Register<AuthService>(Lifetime.Singleton);
            builder.Register<DataService>(Lifetime.Singleton);
            builder.Register<CurrencyService>(Lifetime.Singleton);
            builder.Register<QuestService>(Lifetime.Singleton);
            builder.Register<AchievementService>(Lifetime.Singleton);
            builder.Register<ShopService>(Lifetime.Singleton);
            builder.Register<CharacterService>(Lifetime.Singleton);  

            builder.Register<LocalizationService>(Lifetime.Singleton);
            builder.Register<AudioService>(Lifetime.Singleton);
            builder.Register<AnalyticsService>(Lifetime.Singleton);
            builder.Register<EventsService>(Lifetime.Singleton);
            builder.Register<SeasonalService>(Lifetime.Singleton);

            builder.Register<UIService>(Lifetime.Singleton).WithParameter(_uiRoot);
            builder.RegisterInstance(_windowsInstaller);

            RegisterPresenters(builder);

            builder.RegisterEntryPoint<GameInitializer>(Lifetime.Singleton);
        }

        private void RegisterPresenters(IContainerBuilder builder)
        {
            builder.Register<SplashPresenter>(Lifetime.Transient);
            builder.Register<MainMenuPresenter>(Lifetime.Transient);
            builder.Register<DailyQuestsPresenter>(Lifetime.Transient);
            builder.Register<ShopPresenter>(Lifetime.Transient);
            builder.Register<DailyLoginPresenter>(Lifetime.Transient);
            builder.Register<SettingsPresenter>(Lifetime.Transient);
            builder.Register<AchievementsPresenter>(Lifetime.Transient);
            builder.Register<ProfilePresenter>(Lifetime.Transient);
            builder.Register<LeaderboardPresenter>(Lifetime.Transient);
            builder.Register<GuidePresenter>(Lifetime.Transient);
            builder.Register<UpdatesPresenter>(Lifetime.Transient);
            builder.Register<CharacterInfoPresenter>(Lifetime.Transient); 
            builder.Register<CharacterEditPresenter>(Lifetime.Transient);  
            builder.Register<CharacterCreatePresenter>(Lifetime.Transient); 
        }
    }
}