using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiagramDesigner.Helpers
{
    public class ToolBoxData
    {
        public string DescriptionText { get; private set; }
        public Type Type { get; private set; }

        public ToolBoxData(string imageUrl, Type type)
        {
            this.DescriptionText = imageUrl;
            this.Type = type;
        }
    }
}
