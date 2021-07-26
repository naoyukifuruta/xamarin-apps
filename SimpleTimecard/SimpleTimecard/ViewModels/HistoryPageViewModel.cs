using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;
using Realms;
using SimpleTimecard.Models;
using SimpleTimecard.Views;
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

        public DelegateCommand AddButtonCommand { get; set; }
        public ICommand CellTappedCommand { get; set; }

        public HistoryPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "History";

            AddButtonCommand = new DelegateCommand(TransitionSettingPage);

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

        private async void TransitionSettingPage()
        {
            await NavigationService.NavigateAsync("AddPage");
        }

        /// <summary>
        /// 編集画面へ遷移
        /// </summary>
        /// <param name="timecard"></param>
        private async void TappedListCell(Timecard timecard)
        {
            var param = new NavigationParameters();
            param.Add(nameof(Timecard), timecard);

            await NavigationService.NavigateAsync(nameof(EditPage), param);
        }

        public void DeleteTimecard(string timecardId)
        {
            // 削除
            var realm = Realm.GetInstance();
            var target = realm.Find<Timecard>(timecardId);
            realm.Write(() => {
                realm.Remove(target);
            });

            // 再取得
            var allTimecards = realm.All<Timecard>().ToList();
            TimecardHistories = allTimecards;
        }
    }
}
