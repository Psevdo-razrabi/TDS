using System;

namespace Customs
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ContextMenuAttribute : Attribute
    {
        public string Label { get; }

        public ContextMenuAttribute(string label)
        {
            Label = label;
        }
    }
}