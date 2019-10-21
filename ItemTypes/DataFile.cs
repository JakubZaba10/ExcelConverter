namespace ExcelDBConverter
{
    public class DataFile : PropertyChangeModel
    {
        private string fileName;
        private bool active;
        private bool isConverted;

        public string FileName
        {
            get => fileName;
            set { fileName = value; OnPropertyChanged(); }
        }

        public string FilePath { get; set; }

        public bool Active
        {
            get => active;
            set { active = value; OnPropertyChanged(); }
        }

        public bool IsConverted
        {
            get => isConverted;
            set { isConverted = value; OnPropertyChanged(); }
        }
    }
}
