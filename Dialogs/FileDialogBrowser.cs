using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace ExcelDBConverter
{
    public class FileDialogBrowser : BrowserDialog
    {
        OpenFileDialog openFileDialog;

        public FileDialogBrowser()
        {
            openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Multiselect = true,
                Filter = "Excel Files|*.xlsx"
            };
        }

        protected override void DialogBehaviour(ObservableCollection<DataFile> dataFiles)
        {
            var pathManager = new PathManager(openFileDialog.FileNames[0]);
            openFileDialog.InitialDirectory = pathManager.ReturnFilePath();
            SetListItemsSource(openFileDialog.FileNames, dataFiles);
        }

        protected override DialogResult OpenDialog() => openFileDialog.ShowDialog();
    }
}
