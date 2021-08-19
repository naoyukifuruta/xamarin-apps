using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Navigation;
using Reactive.Bindings;
using Realms;
using SimpleTimecard.Interfaces;
using SimpleTimecard.Models;
using SimpleTimecard.Views;
using Xamarin.Forms;

namespace SimpleTimecard.ViewModels
{
    public class HistoryPageViewModel : ViewModelBase
    {
        public ReactiveProperty<List<Timecard>> TimecardHistories { get; } = new ReactiveProperty<List<Timecard>>();

        public AsyncReactiveCommand NavigateToAddPageCommand { get; } = new AsyncReactiveCommand();
        public AsyncReactiveCommand<Timecard> NavigateToEditPageCommand { get; } = new AsyncReactiveCommand<Timecard>();

        public HistoryPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "History";

            NavigateToAddPageCommand.Subscribe(_ => NavigateToAddPage());
            NavigateToEditPageCommand.Subscribe( timecard => NavigateToEditPage(timecard));
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            var realm = Realm.GetInstance();
            var allTimecards = realm.All<Timecard>().ToList();

            TimecardHistories.Value = allTimecards.OrderBy(x => x.EntryDate).ToList();
        }

        // 追加画面へ遷移
        private async Task NavigateToAddPage()
        {
            await NavigationService.NavigateAsync(nameof(NavigationPage) + "/" + nameof(AddPage), useModalNavigation: true);
        }

        // 編集画面へ遷移
        private async Task NavigateToEditPage(Timecard timecard)
        {
            var param = new NavigationParameters();
            param.Add(nameof(Timecard), timecard);
            await NavigationService.NavigateAsync(nameof(EditPage), param);
        }

        // 削除
        public void DeleteTimecard(string timecardId)
        {
            var realm = Realm.GetInstance();
            var target = realm.Find<Timecard>(timecardId);
            realm.Write(() => {
                realm.Remove(target);
            });

            var allTimecards = realm.All<Timecard>().ToList();
            TimecardHistories.Value = allTimecards;

            DependencyService.Get<IToast>().Show("削除しました。");
        }
    }
}
