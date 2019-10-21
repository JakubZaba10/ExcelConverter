using System.Collections.ObjectModel;

namespace ExcelDBConverter
{
    public class DialogManager
    {
        private BrowserDialog browserDialog;

        public void CreateDialogTask(System.Windows.Controls.ListView listView,
                                     ObservableCollection<DataFile> dataFiles)
        {
            browserDialog.PerformDialogTask(dataFiles);
        }

        public void SetBrowserDialog(BrowserDialog _browserDialog) => browserDialog = _browserDialog;
    }
}
