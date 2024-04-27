namespace Customs.Data
{
    [System.Serializable]
    public class MethodLabelName
    {
        public string id;
        public string labelName;

        public MethodLabelName(string id, string labelName)
        {
            this.id = id;
            this.labelName = labelName;
        }
    }
}