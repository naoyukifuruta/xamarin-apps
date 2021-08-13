using System;
using System.Threading.Tasks;
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

            // ナビゲーションタイトルに現在時刻を表示
            new Task(async () =>
            {
                while (true)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        Title = DateTime.Now.ToString("yyyy/MM/dd ddd HH:mm:ss")
                    );
                    await Task.Delay(1000);
                }
            }).Start();
        }

        public Command OnClickSetting => new Command(async () =>
        {
            await NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + nameof(SettingPage), useModalNavigation: true);
        });
    }
}
