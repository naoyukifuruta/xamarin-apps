using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Prism;
using Prism.Navigation;
using Xamarin.Forms;

namespace SimpleTimecard.ViewModels
{
    public class TodayPageViewModel : ViewModelBase, IActiveAware
    {
        private bool _isActive;
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            set
            {
                if (value)
                {
                    Debug.WriteLine($"{typeof(TodayPageViewModel).Name} is Active!");
                }
                SetProperty(ref this._isActive, value);
            }
        }
        public event EventHandler IsActiveChanged;

        public TodayPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Today";
        }
    }
}
