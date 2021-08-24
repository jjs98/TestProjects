using JeSch.Presentation.Contracts.Observable;
using JeSch.Presentation.Wpf.Components.Commands;
using System.Windows.Input;

namespace WpfApp1.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private string _myText;
        
        public string MyText
        {
            get => _myText;
            set
            {
                SetProperty(ref _myText, value);
                (ClickButtenCommand as DelegateCommand)?.NotifyCanExecuteChanged();
            }
        }

        public ICommand ClickButtenCommand { get; }

        public MainWindowViewModel()
        {
            ClickButtenCommand = new DelegateCommand(OnExecuteClickButton, () => MyText != "Hello");
        }

        private void OnExecuteClickButton()
        {
        }
    }
}
