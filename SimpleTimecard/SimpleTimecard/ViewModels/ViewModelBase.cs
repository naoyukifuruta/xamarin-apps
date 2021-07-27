using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;

namespace SimpleTimecard.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible, IPageLifecycleAware, IApplicationLifecycleAware
    {
        // Navigation
        protected INavigationService NavigationService { get; private set; }

        // ページタイトル
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ViewModelBase() { }

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public virtual void Initialize(INavigationParameters parameters) { }

        public virtual void OnNavigatedFrom(INavigationParameters parameters) { }

        public virtual void OnNavigatedTo(INavigationParameters parameters) { }

        public virtual void OnNavigatingTo(INavigationParameters parameters) { }

        public virtual void Destroy() { }

        public virtual void OnAppearing() { }

        public virtual void OnDisappearing() { }

        public virtual void OnResume() { }

        public virtual void OnSleep() { }
    }
}
