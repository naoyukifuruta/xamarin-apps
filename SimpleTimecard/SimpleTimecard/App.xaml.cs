using Prism;
using Prism.Ioc;
using SimpleTimecard.ViewModels;
using SimpleTimecard.Views;
using Xamarin.Forms;

namespace SimpleTimecard
{
    public partial class App
    {
        public App() : this(null) { }

        public App(IPlatformInitializer platformInitializer) : base(platformInitializer)
        {

        }

        protected override void OnInitialized()
        {
            InitializeComponent();

            NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
        }
    }
}
