using Prism.Navigation;
using Prism.Services;
using Realms;
using SimpleTimecard.Common;
using SimpleTimecard.Interfaces;
using SimpleTimecard.Models;
using Xamarin.Forms;

namespace SimpleTimecard.ViewModels
{
    public class SettingPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _pageDialogService;

        public SettingPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            Title = "設定";
            _pageDialogService = pageDialogService;
        }

        public Command OnClickInitializedDatabase => new Command(async () =>
        {
            Logger.Trace();

            var result = await _pageDialogService.DisplayAlertAsync("確認", "データベースを初期化しますか？", "OK", "キャンセル");
            if (!result)
            {
                return;
            }

            var realm = Realm.GetInstance();
            using (var tran = realm.BeginWrite())
            {
                realm.RemoveAll<Timecard>();
                tran.Commit();

                // トランザクション張る意味ないけど書き方を試す目的で
            }

            DependencyService.Get<IToast>().Show("データを全削除しました。");
        });

        public Command OnClickClose => new Command(async () =>
        {
            await base.NavigationService.GoBackAsync(useModalNavigation: true);
        });
    }
}
