using Prism;
using Prism.Ioc;
using SimpleTimecard.Common;
using SimpleTimecard.ViewModels;
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
            containerRegistry.RegisterForNavigation<MainTabbedPage, MainTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<TodayPage, TodayPageViewModel>();
            containerRegistry.RegisterForNavigation<HistoryPage, HistoryPageViewModel>();
        }

        /// <summary>
        /// アプリ起動時
        /// </summary>
        protected override void OnStart()
        {
            Logger.Debug(this.GetType().Name + ":" + System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// アプリがフォアグラウンドになった時
        /// </summary>
        protected override void OnResume()
        {
            Logger.Debug(this.GetType().Name + ":" + System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// アプリがバックグラウンドになった時
        /// </summary>
        protected override void OnSleep()
        {
            Logger.Debug(this.GetType().Name + ":" + System.Reflection.MethodBase.GetCurrentMethod().Name);
        }
    }
}