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
    public class HistoryPageViewModel : ViewModelBase
    {
        private List<Timecard> _timecardHistories;
        public List<Timecard> TimecardHistories
        {
            get { return _timecardHistories; }
            set
            {
                SetProperty(ref this._timecardHistories, value);
            }
        }

        public ICommand CellTappedCommand { get; set; }

        public HistoryPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "History";

            CellTappedCommand = new Command<Timecard>((timecard) =>
            {
                TappedListCell(timecard);
            });
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            var realm = Realm.GetInstance();
            var allTimecards = realm.All<Timecard>().ToList();

            TimecardHistories = allTimecards;
        }

        public void TappedListCell(Timecard timecard)
        {
            Debug.WriteLine($"{timecard.TimecardId}");
            Debug.WriteLine($"{timecard.StartTime}");
            Debug.WriteLine($"{timecard.EndTime}");
        }
    }
}
