using System;
using System.Threading.Tasks;
using Prism.Navigation;
using Xamarin.Forms;

namespace SimpleTimecard.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        /// <summary>
        /// 現在時刻ラベルテキスト
        /// </summary>
        private string _labelCurrentTimeText = string.Empty;
        public string LabelCurrentTimeText
        {
            get
            {
                return _labelCurrentTimeText;
            }
            set
            {
                SetProperty(ref _labelCurrentTimeText, value);
            }
        }

        public MainPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Top";

            new Task(async () =>
            {
                while (true)
                {
                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                        LabelCurrentTimeText = DateTime.Now.ToString("yyyy/MM/dd ddd HH:mm:ss")
                    );
                    await Task.Delay(1000);
                }
            }).Start();
        }
    }
}
