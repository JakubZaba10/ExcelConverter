using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;

namespace ExcelDBConverter
{
    public class FolderDialogBrowser : BrowserDialog
    {
        private FolderBrowserDialog folderBrowserDialog;
        public FolderDialogBrowser() => folderBrowserDialog = new FolderBrowserDialog();

        protected override void DialogBehaviour(ObservableCollection<DataFile> dataFiles)
        {
            string[] files = Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.xlsx");
            SetListItemsSource(files, dataFiles);
        }

        protected override DialogResult OpenDialog() => folderBrowserDialog.ShowDialog();
    }
}
