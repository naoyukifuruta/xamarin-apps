using System;
using System.Diagnostics;
using Prism;
using Prism.Navigation;
using Prism.Mvvm;
using System.Collections.Generic;
using SimpleTimecard.Models;
using Realms;
using System.Linq;
using Prism.Commands;
using System.Windows.Input;
using Xamarin.Forms;

namespace SimpleTimecard.ViewModels
{
    public class HistoryPageViewModel : ViewModelBase, IActiveAware
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
                    Debug.WriteLine($"{typeof(HistoryPageViewModel).Name} is Active!");
                }
                SetProperty(ref this._isActive, value);
            }
        }
        public event EventHandler IsActiveChanged;

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
