using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighlightCommentTags.Editor {
    internal static class CommentTagsClassificationDefinition {
        internal static class ClassificationTypeNames {
            public const string Todo = "todo-FD9B7530-7493-41B0-A0D3-375CA8667EE1";
            public const string Note = "note-8A8E1C3E-6764-4305-8C1C-76F5A1DDCAB3";
            public const string Debug = "debug-F5CE8732-A495-45AD-A965-5A9A33301315";
            public const string Fixme = "fixme-076B7480-90BB-45C6-BBC4-981EC37E92AF";
            public const string Fixed = "fixed-C8353676-BE3B-4E4A-8A1F-913BF5EE8F06";
        }

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(ClassificationTypeNames.Todo)]
        internal static ClassificationTypeDefinition todo = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(ClassificationTypeNames.Note)]
        internal static ClassificationTypeDefinition note = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(ClassificationTypeNames.Debug)]
        internal static ClassificationTypeDefinition debug = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(ClassificationTypeNames.Fixme)]
        internal static ClassificationTypeDefinition fixme = null;

        [Export(typeof(ClassificationTypeDefinition))]
        [Name(ClassificationTypeNames.Fixed)]
        internal static ClassificationTypeDefinition @fixed = null;
    }
}
