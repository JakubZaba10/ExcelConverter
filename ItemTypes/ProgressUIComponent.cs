namespace ExcelDBConverter
{
    public class ProgressUIComponent : PropertyChangeModel
    {
        private string labelNameText;
        private double progressValue;
        private bool isActive;

        public ProgressUIComponent()
        {
            LabelNameText = "";
            IsActive = false;
        }

        public string LabelNameText
        {
            get => labelNameText;
            set { labelNameText = value; OnPropertyChanged(); }
        }

        public double ProgressValue
        {
            get => progressValue;
            set { progressValue = value; OnPropertyChanged(); }
        }

        public bool IsActive
        {
            get => isActive;
            set { isActive = value; OnPropertyChanged(); }
        }
    }
}
