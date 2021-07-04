using System;
using System.Threading.Tasks;
using AiForms.Dialogs;
using AiForms.Dialogs.Abstractions;
using Prism.Navigation;
using Xamarin.Forms;

namespace CopyPasteSnippet.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private string _labelTime = string.Empty;
        public string LabelTime
        {
            get { return _labelTime; }
            set
            {
                SetProperty(ref _labelTime, value);
            }
        }

        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Main Page"; // 仮

            // 時計
            new Task(async () =>
            {
                while (true)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        LabelTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
                    );
                    await Task.Delay(1000);
                }
            }).Start();
        }

        public override async void OnAppearing()
        {
            base.OnAppearing();

            // Default Loading Dialog 用の設定が可能です。
            Configurations.LoadingConfig = new LoadingConfig
            {
                IndicatorColor = Color.White,
                OverlayColor = Color.Black,
                Opacity = 0.4,
                DefaultMessage = "Loading...",
            };

            await Loading.Instance.StartAsync(async progress => {
                await Task.Delay(50);
                progress.Report((1) * 0.01d);
            });
        }
    }
}
