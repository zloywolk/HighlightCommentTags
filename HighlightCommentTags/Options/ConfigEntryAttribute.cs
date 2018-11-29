using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighlightCommentTags.Options {
    internal class ConfigEntryAttribute : Attribute {
        public string Name { get; set; }
        public Type Type { get; set; }
        public bool RequestRestart { get; set; }
        public string ClassificationTypeName { get; set; }

        public ConfigEntryAttribute(string name, Type type = null) {
            this.Name = name;
            this.Type = type;
        }
    }
}
