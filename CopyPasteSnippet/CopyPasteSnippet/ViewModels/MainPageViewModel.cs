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
        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Main Page";
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
