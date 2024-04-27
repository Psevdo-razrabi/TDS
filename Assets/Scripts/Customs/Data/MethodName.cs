namespace Customs.Data
{
    [System.Serializable]
    public class MethodName
    {
        public string id;
        public string methodName;

        public MethodName(string id, string methodName)
        {
            this.id = id;
            this.methodName = methodName;
        }
    }
}