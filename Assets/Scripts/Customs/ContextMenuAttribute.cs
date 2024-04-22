using System;

namespace Customs
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ContextMenuAttribute : Attribute
    {
        public string Label { get; set; }

        public ContextMenuAttribute(string label)
        {
            Label = label;
        }
    }
}