using System;
using System.Collections.Generic;
using Customs.Data;

namespace SaveSystem
{
    [Serializable]
    public class GameData
    {
        public List<MethodName> MethodName = new();
        public List<MethodLabelName> MethodLabelName = new();
    }
    
}