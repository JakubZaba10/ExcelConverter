using Microsoft.Office.Interop.Excel;

namespace ExcelDBConverter
{
    public class Converter
    {
        private Application app;
        private ProgressUIComponent progressUIComponent;

        public Converter(ProgressUIComponent _progressUIComponent)
        {
            app = new Application();
            progressUIComponent = _progressUIComponent;
        }
       
        public void StartConvert(string path)
        {
            var pathManager = new PathManager(path);
            var fileManager = new FileManager();
            var excelManager = new ExcelManager(app,progressUIComponent);
            fileManager.CreateDirectory(pathManager.PathWithResultFolder);
            excelManager.ConvertFormatToCSV(pathManager.UserInput, pathManager.CsvFilePath);
            excelManager.GetFileStructure(pathManager.CsvFilePath);
            fileManager.DeleteFile(pathManager.CsvFilePath);
            excelManager.CreateFormatedStructure();
            excelManager.CreateFileWithStyles(pathManager);
        }
        public void QuitApplication() => app.Quit();
    }
}
