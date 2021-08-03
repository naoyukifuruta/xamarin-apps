using System;
using Prism.Commands;
using Prism.Navigation;
using Realms;
using SimpleTimecard.Models;
using Xamarin.Forms;

namespace SimpleTimecard.ViewModels
{
    public class AddPageViewModel : ViewModelBase
    {
        private DateTime _selectedEntryDate;
        public DateTime SelectedEntryDate
        {
            get { return _selectedEntryDate; }
            set { SetProperty(ref _selectedEntryDate, value); }
        }

        private TimeSpan _selectedStartTime;
        public TimeSpan SelectedStartTime
        {
            get { return _selectedStartTime; }
            set { SetProperty(ref _selectedStartTime, value); }
        }

        private TimeSpan _selectedEndTime;
        public TimeSpan SelectedEndTime
        {
            get { return _selectedEndTime; }
            set { SetProperty(ref _selectedEndTime, value); }
        }

        public AddPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "追加";

            SelectedEntryDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);
            SelectedStartTime = new TimeSpan();
            SelectedEndTime = new TimeSpan();
        }

        public Command OnClickEntry => new Command(async () =>
        {
            var realm = Realm.GetInstance();
            realm.Write(() =>
            {
                var addedTimecard = realm.Add<Timecard>(new Timecard()
                {
                    EntryDate = DateTime.SpecifyKind(SelectedEntryDate, DateTimeKind.Local),
                    StartTimeString = string.Format($"{SelectedStartTime.Hours.ToString("D2")}:{SelectedStartTime.Minutes.ToString("D2")}"),
                    EndTimeString = string.Format($"{SelectedEndTime.Hours.ToString("D2")}:{SelectedEndTime.Minutes.ToString("D2")}"),
                });
            });

            await base.NavigationService.GoBackAsync();
        });

        public Command OnClickCancel => new Command(async () =>
        {
            await base.NavigationService.GoBackAsync();
        });
    }
}
