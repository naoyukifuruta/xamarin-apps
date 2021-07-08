using System;
using System.Diagnostics;
using Prism;
using Prism.Navigation;
using Prism.Mvvm;
using System.Collections.Generic;
using SimpleTimecard.Models;

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

        public HistoryPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "History";
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            var hoge = new List<Timecard>();
            hoge.Add(new Timecard
            {
                StartTime = DateTime.Now
            });
            hoge.Add(new Timecard
            {
                StartTime = DateTime.Now
            });
            hoge.Add(new Timecard
            {
                StartTime = DateTime.Now
            });
            hoge.Add(new Timecard
            {
                StartTime = DateTime.Now
            });

            TimecardHistories = hoge;
        }
    }
}
