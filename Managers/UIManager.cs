using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace ExcelDBConverter
{
    public class UIManager
    {
        private DialogManager dialogManager;
        public static ComboBox DeleteTypeCbx { get; private set; }
        public static ComboBox DialogTypeCbx { get; set; }        
        public static Button SearchButton { get; private set; }
        public static Button DeleteButton { get; private set; }
        public static Button ConvertButton { get; private set; }
        public static ListView ListView { get; private set; }        

        public UIManager(ComboBox _deletionTypeCbx,
                         ComboBox _dialogTypeCbx,
                         ListView _listView,
                         Button searchDialogBtn,
                         Button deleteBtn,
                         Button convertBtn)
        {
            DeleteTypeCbx = _deletionTypeCbx;
            DialogTypeCbx = _dialogTypeCbx;
            ListView = _listView;
            SearchButton = searchDialogBtn;
            DeleteButton = deleteBtn;
            ConvertButton = convertBtn;
            dialogManager = new DialogManager();
            DeleteTypeCbx.ItemsSource = DeletionConditionDict.GetTypeList();
            DialogTypeCbx.ItemsSource = BrowserDialogDict.GetTypeList();
            SetBrowserDialog();
        }

        public void SetBrowserDialog()
        {
            var dialogType = BrowserDialogDict.ReturnDialogType(DialogTypeCbx.SelectedValue.ToString());
            dialogManager.SetBrowserDialog(dialogType);
        }

        public void CreateDialogTask(DataManager dataManager, ObservableCollection<DataFile> dataFiles)
        {
            dialogManager.CreateDialogTask(ListView, dataFiles);
            dataManager.ManageDirectoryObservators();
        }

        public void SetProgressItemsSource(ProgressUIComponent progressUIComponent,
                                           Label fileName,
                                           ProgressBar convertProgressBar)
        {
            fileName.DataContext = progressUIComponent;
            convertProgressBar.DataContext = progressUIComponent;
        }

        public static void SetButtonsAvailability(bool isAvailable)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                DeleteButton.IsEnabled = isAvailable;
                ConvertButton.IsEnabled = isAvailable;
                SearchButton.IsEnabled = isAvailable;
            });            
        }
    }
}