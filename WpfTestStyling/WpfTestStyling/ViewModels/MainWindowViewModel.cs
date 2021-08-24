using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static WpfTestStyling.Items;

namespace WpfTestStyling.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private string _testText;
        public string TestText
        {
            get => _testText;
            set
            {
                SetProperty(ref _testText, value);
            }
        }

        private bool _textBoxReadonly;
        public bool TextBoxReadOnly
        {
            get => _textBoxReadonly;
            set
            {
                SetProperty(ref _textBoxReadonly, value);
            }
        }

        private bool _textBoxEnabled;
        public bool TextBoxEnabled
        {
            get => _textBoxEnabled;
            set
            {
                SetProperty(ref _textBoxEnabled, value);
            }
        }

        private string _textBoxText;
        public string TextBoxText
        {
            get => _textBoxText;
            set
            {
                SetProperty(ref _textBoxText, value);
            }
        }

        private ObservableCollection<RectItem> _rectItems;
        public ObservableCollection<RectItem> RectItems
        {
            get => _rectItems;
            set
            {
                SetProperty(ref _rectItems, value);
            }
        }


        public ICommand ClickGetOvertimeCommand { get; }
        public ICommand ClickReadOnlyCommand { get; }
        public ICommand ClickEnabledCommand { get; }

        public MainWindowViewModel()
        {
            ClickGetOvertimeCommand = new DelegateCommand(OnExecuteClickGetOvertime);
            ClickReadOnlyCommand = new DelegateCommand(OnExecuteClickReadOnly);
            ClickEnabledCommand = new DelegateCommand(OnExecuteClickEnabled);
            TextBoxEnabled = true;
            TextBoxReadOnly = false;
        }

        private async void OnExecuteClickGetOvertime()
        {
            string uri = @"http://masch212.de/TimeSheetJens/api/overtime";
            TextBoxText = await GetProductAsync(uri);
        }
        private void OnExecuteClickReadOnly()
        {
            TextBoxReadOnly = TextBoxReadOnly ? false : true;
            RectItems = new ObservableCollection<RectItem>();
            RectItems.Add(new RectItem()
            {
                Height = 100,
                Width = 100,
                X = 20,
                Y = 20
            });
        }
        private void OnExecuteClickEnabled()
        {
            TextBoxEnabled = TextBoxEnabled ? false : true;
        }

        async Task<string> GetProductAsync(string path)
        {
            HttpClient client = new HttpClient();
            string result = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            return result;
        }
    }
}
