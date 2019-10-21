using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace ExcelDBConverter
{
    public class DataManager
    {
        private IDeleteCondition deleteCondition;
        public ProgressUIComponent ProgressUIComponent { get; }
        public ObservableCollection<DataFile> DataFiles { get; set; }
        public List<DirectoryObservator> DirectoryObservators { get; set; }

        public DataManager()
        {
            DataFiles = new ObservableCollection<DataFile>();
            ProgressUIComponent = new ProgressUIComponent();
            DirectoryObservators = new List<DirectoryObservator>();
            SetDeletionType();
        }

        public void ConvertData()
        {
            Task task = new Task(() =>
            {
                UIManager.SetButtonsAvailability(false);
                var excelConverter = new Converter(ProgressUIComponent);
                foreach (var dataFile in DataFiles)
                {
                    if (dataFile.Active)
                    {
                        ProgressUIComponent.IsActive = true;
                        ProgressUIComponent.LabelNameText = dataFile.FileName;
                        var fullPath = dataFile.FilePath + "\\" + dataFile.FileName;
                        excelConverter.StartConvert(fullPath);
                        dataFile.IsConverted = true;
                        dataFile.Active = false;
                        ResetProgressBar();
                    }
                }
                excelConverter.QuitApplication();
                UIManager.SetButtonsAvailability(true);
                ProgressUIComponent.IsActive = false;
            });
            task.Start();
        }

        private void ResetProgressBar()
        {
            ProgressUIComponent.ProgressValue = 0;
        }

        public void ManageDirectoryObservators()
        {
            var distinctDataFilesPaths = DataFiles.Select(n => n.FilePath).Distinct().ToList();
            foreach (var path in distinctDataFilesPaths)
            {
                if(!DirectoryObservators.Any(n => n.ObservablePath.Equals(path)))
                {
                    var directoryObservator = new DirectoryObservator(path,
                                                                      DataFiles,
                                                                      DirectoryObservators);
                    directoryObservator.Run();
                    DirectoryObservators.Add(directoryObservator);
                }
            }
        }

        public void SetItemsActive(bool isActive)
        {
            foreach (var item in DataFiles)
            {
                item.Active = isActive;
            }
        }

        public void SetDeletionType()
        {
            var conditionType = DeletionConditionDict.ReturnConditionType(UIManager.DeleteTypeCbx.SelectedValue.ToString());
            deleteCondition = conditionType;
        }

        public void DeleteDataFilesBasedOnCondition()
        {
            for (int i = 0; i < DataFiles.Count; i++)
            {
                if (deleteCondition.Condition(DataFiles[i]))
                {
                    DataFiles.Remove(DataFiles[i]);
                    i--;
                }
            }
        }
    }
}