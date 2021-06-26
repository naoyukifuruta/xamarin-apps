using System;
using System.Threading.Tasks;
using Prism.Navigation;

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
    }
}
