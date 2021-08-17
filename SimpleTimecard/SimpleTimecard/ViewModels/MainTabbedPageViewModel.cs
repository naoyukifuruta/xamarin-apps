using Prism.Navigation;
using SimpleTimecard.Common;
using SimpleTimecard.Views;
using Xamarin.Forms;

namespace SimpleTimecard.ViewModels
{
    public class MainTabbedPageViewModel : ViewModelBase
    {
        public MainTabbedPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Logger.Trace();
        }

        public Command OnClickSetting => new Command(async () =>
        {
            await NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + nameof(SettingPage), useModalNavigation: true);
        });
    }
}
