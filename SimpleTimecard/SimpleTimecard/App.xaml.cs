using Prism;
using Prism.Ioc;
using SimpleTimecard.Common;
using SimpleTimecard.Views;
using Xamarin.Forms;

namespace SimpleTimecard
{
    public partial class App
    {
        public App() : this(null) { }

        public App(IPlatformInitializer platformInitializer) : base(platformInitializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync("NavigationPage/MainTabbedPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainTabbedPage>();
            containerRegistry.RegisterForNavigation<TodayPage>();
            containerRegistry.RegisterForNavigation<HistoryPage>();
            containerRegistry.RegisterForNavigation<SettingPage>();
            containerRegistry.RegisterForNavigation<AddPage>();
            containerRegistry.RegisterForNavigation<EditPage>();
        }

        protected override void OnStart()
        {
            Logger.Trace();
        }

        protected override void OnResume()
        {
            Logger.Trace();
        }

        protected override void OnSleep()
        {
            Logger.Trace();
        }
    }
}