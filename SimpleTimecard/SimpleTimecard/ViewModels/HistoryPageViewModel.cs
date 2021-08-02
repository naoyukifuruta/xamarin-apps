using System.Collections.Generic;
using System.Linq;
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
            set { SetProperty(ref this._timecardHistories, value); }
        }

        public HistoryPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "History";
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            var realm = Realm.GetInstance();
            var allTimecards = realm.All<Timecard>().ToList();

            TimecardHistories = allTimecards;
        }

        public Command OnClickAdd => new Command(async () =>
        {
            await NavigationService.NavigateAsync(nameof(AddPage));
            //await NavigationService.NavigateAsync(nameof(AddPage), useModalNavigation: true);
        });

        public Command OnClickListViewCell => new Command<Timecard>(async (timecard) =>
        {
            var param = new NavigationParameters();
            param.Add(nameof(Timecard), timecard);

            await NavigationService.NavigateAsync(nameof(EditPage), param);
        });

        // TODO: bindできるかも？あとで試す
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
