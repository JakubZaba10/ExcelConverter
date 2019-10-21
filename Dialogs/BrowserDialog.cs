using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExcelDBConverter
{
    public abstract class BrowserDialog
    {
        protected abstract DialogResult OpenDialog();
        protected abstract void DialogBehaviour(ObservableCollection<DataFile> dataFiles);

        private void CheckIfConvertedVersionExists(DataFile file)
        {
            var ResultPath = file.FilePath + "\\" + "Result";
            if (Directory.Exists(ResultPath) &&
                File.Exists(ResultPath + "\\" + file.FileName + ".xlsx"))
            {
                file.IsConverted = true;
                file.Active = false;
            }
            else
            {
                file.Active = true;
            }
        }

        protected void SetListItemsSource(string[] paths,
            ObservableCollection<DataFile> dataFiles)
        {
            foreach (var path in paths)
            {
                var tempPathManager = new PathManager(path);
                var file = new DataFile()
                {
                    FileName = tempPathManager.FileName,
                    FilePath = tempPathManager.ReturnFilePath()
                };
                if (file.FileName.StartsWith("~$")) continue;
                if (dataFiles.Contains(file, new DataFileComparer())) continue;
                CheckIfConvertedVersionExists(file);
                dataFiles.Add(file);
            }
        }
        
        public void PerformDialogTask(ObservableCollection<DataFile> dataFiles)
        {
            DialogResult result = OpenDialog();
            if (result == DialogResult.OK)
            {
                DialogBehaviour(dataFiles);
            }
        }
    }
}
