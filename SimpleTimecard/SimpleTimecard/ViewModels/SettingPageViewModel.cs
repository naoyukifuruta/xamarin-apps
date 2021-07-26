using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Realms;
using SimpleTimecard.Interfaces;
using SimpleTimecard.Models;
using Xamarin.Forms;

namespace SimpleTimecard.ViewModels
{
    public class SettingPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _pageDialogService;

        public DelegateCommand InitializedDatabaseCommand { get; set; }

        public SettingPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService) : base(navigationService)
        {
            Title = "設定";

            _pageDialogService = pageDialogService;

            // DB初期化コマンドの定義
            InitializedDatabaseCommand = new DelegateCommand(async () =>
            {
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
        }
    }
}
