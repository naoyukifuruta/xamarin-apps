using System;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Navigation;

namespace SimpleTimecard.ViewModels
{
    public class MainTabbedPageViewModel : ViewModelBase
    {
        public DelegateCommand SettingButtonCommand { get; set; }

        public MainTabbedPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            // ナビゲーションバーの設定ボタン押下時のコマンドを設定
            SettingButtonCommand = new DelegateCommand(async () =>
            {
                await NavigationService.NavigateAsync("SettingPage");
            });

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
    }
}
