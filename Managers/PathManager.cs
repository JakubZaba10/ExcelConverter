using System.Linq;

namespace ExcelDBConverter
{
    public class PathManager
    {
        private string[] userInputSplit;

        public string UserInput { get; }
        public string CsvFilePath { get; private set; }
        public string PathWithResultFolder { get; private set; }
        public string FileName { get; private set; }

        public PathManager(string _userInput)
        {
            UserInput = _userInput;
            GetFileName();
            GetCsvFilePath();
            GetPathWithResultFolder();
        }

        private void GetFileName()
        {
            userInputSplit = UserInput.Split('\\');
            FileName = userInputSplit.Last().Split('.')[0];
        }

        private void GetCsvFilePath()
        {
            CsvFilePath = string.Join(@"\", userInputSplit.Take(userInputSplit.Count() - 1)) + @"\" + FileName + ".csv";
        }

        private void GetPathWithResultFolder()
        {
            PathWithResultFolder = string.Join(@"\", userInputSplit.Take(userInputSplit.Count() - 1)) + @"\Result";
        }

        public string ReturnFilePath()
        {
            var filePath = string.Join(@"\", userInputSplit.Take(userInputSplit.Count() - 1));
            return filePath;
        }
    }
}
