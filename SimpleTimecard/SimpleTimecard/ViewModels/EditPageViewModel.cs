using System;
using Prism.Navigation;
using SimpleTimecard.Common;
using SimpleTimecard.Models;

namespace SimpleTimecard.ViewModels
{
    public class EditPageViewModel : ViewModelBase
    {
        public EditPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "編集";
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            var val = parameters[nameof(Timecard)] as Timecard;
            if (val == null)
            {
                throw new ArgumentException(nameof(Timecard));
            }

            Logger.Debug(val.TimecardId);
        }
    }
}
