using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Prism.Navigation;
using Realms;
using SimpleTimecard.Models;
using Xamarin.Forms;

namespace SimpleTimecard.ViewModels
{
    public class AddPageViewModel : ViewModelBase
    {
        public AddPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "追加";
        }
    }
}
