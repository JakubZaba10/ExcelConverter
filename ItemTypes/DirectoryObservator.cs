using System.Collections.ObjectModel;
using System.IO;
using System.Security.Permissions;
using System.Linq;
using System.Collections.Generic;

namespace ExcelDBConverter
{
    public class DirectoryObservator
    {
        private ObservableCollection<DataFile> dataFiles;
        private List<DirectoryObservator> directoryObservators;
        private FileSystemWatcher watcher;
        public string ObservablePath { get; }

        public DirectoryObservator(string _observablePath,
                                   ObservableCollection<DataFile> _dataFiles,
                                   List<DirectoryObservator> _directoryObservators)
        {
            dataFiles = _dataFiles;
            ObservablePath = _observablePath;
            directoryObservators = _directoryObservators;
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void Run()
        {
            watcher = new FileSystemWatcher();
            watcher.Path = ObservablePath;
            watcher.NotifyFilter = NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.FileName
                                 | NotifyFilters.DirectoryName;
            watcher.Filter = "*.xlsx";
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnRenamed;
            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                var removedFilePath = new PathManager(e.FullPath);
                if (IsInDataFiles(removedFilePath))
                {
                    var dataFileToDelete = SearchFile(removedFilePath);
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        dataFiles.Remove(dataFileToDelete);
                    });
                    if (!dataFiles.Any(n => n.FilePath.Equals(dataFileToDelete.FilePath)))
                    {
                        directoryObservators.Remove(this);
                        watcher.Dispose();
                    }
                }
            }
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            var oldFilePath = new PathManager(e.OldFullPath);
            var newFileName = new PathManager(e.FullPath).FileName;
            if (IsInDataFiles(oldFilePath))
            {
                ChangeDataFileName(oldFilePath, newFileName);
            }
        }

        private void ChangeDataFileName(PathManager oldFilePath, string newFileName)
        {
            var dataFileToChange = SearchFile(oldFilePath);
            dataFileToChange.FileName = newFileName;
        }

        private DataFile SearchFile(PathManager pathManager)
        {
            return dataFiles.Where(n => n.FilePath == pathManager.ReturnFilePath())
                                    .Where(m => m.FileName == pathManager.FileName)
                                    .First();
        }

        private bool IsInDataFiles(PathManager pathManager)
        {
            return dataFiles.Any(n => n.FileName.Equals(pathManager.FileName)
                                      && n.FilePath.Equals(pathManager.ReturnFilePath()));
        }
    }
}
