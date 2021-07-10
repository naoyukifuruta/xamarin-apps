using System.Windows.Input;
using Xamarin.Forms;

namespace SimpleTimecard.Behaviors
{
    internal class ListViewItemTappedBehavior : Behavior<ListView>
    {
        public static BindableProperty CommandProperty = BindableProperty.Create(
            "Command", typeof(ICommand), typeof(ListViewItemTappedBehavior), null
        );

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);

            bindable.BindingContextChanged += (sender, e) =>
            {
                this.BindingContext = ((ListView)sender).BindingContext;
            };

            bindable.ItemTapped += (sender, e) =>
            {
                ((ListView)sender).SelectedItem = null;
                if (this.Command == null) return;
                this.Command.Execute(e.Item);
            };
        }
    }
}
