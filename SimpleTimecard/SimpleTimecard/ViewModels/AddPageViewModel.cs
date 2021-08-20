using System;
using System.Threading.Tasks;
using Prism.Navigation;
using Reactive.Bindings;
using Realms;
using SimpleTimecard.Models;

namespace SimpleTimecard.ViewModels
{
    public class AddPageViewModel : ViewModelBase
    {
        public ReactiveProperty<DateTime> SelectedEntryDate { get; } = new ReactiveProperty<DateTime>();
        public ReactiveProperty<TimeSpan> SelectedStartTime { get; } = new ReactiveProperty<TimeSpan>();
        public ReactiveProperty<TimeSpan> SelectedEndTime { get; } = new ReactiveProperty<TimeSpan>();

        public AsyncReactiveCommand RegisterCommand { get; } = new AsyncReactiveCommand();

        public AddPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "追加";

            SelectedEntryDate.Value = DateTime.Now;
            SelectedStartTime.Value = new TimeSpan();
            SelectedEndTime.Value = new TimeSpan();

            RegisterCommand.Subscribe(_ => Register());
        }

        public async Task Register()
        {
            var realm = Realm.GetInstance();
            realm.Write(() =>
            {
                var addedTimecard = realm.Add<Timecard>(new Timecard()
                {
                    EntryDate = SelectedEntryDate.Value,
                    StartTimeString = string.Format($"{SelectedStartTime.Value.Hours.ToString("D2")}:{SelectedStartTime.Value.Minutes.ToString("D2")}"),
                    EndTimeString = string.Format($"{SelectedEndTime.Value.Hours.ToString("D2")}:{SelectedEndTime.Value.Minutes.ToString("D2")}"),
                });
            });

            await base.NavigationService.GoBackAsync(useModalNavigation: true);
        }
    }
}
