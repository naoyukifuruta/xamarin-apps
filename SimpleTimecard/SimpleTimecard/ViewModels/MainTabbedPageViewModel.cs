using Prism.Navigation;
using SimpleTimecard.Common;

namespace SimpleTimecard.ViewModels
{
    public class MainTabbedPageViewModel : ViewModelBase
    {
        public MainTabbedPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Logger.Trace();
            Title = string.Empty;
        }
    }
}
