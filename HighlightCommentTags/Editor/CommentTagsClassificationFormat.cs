using HighlightCommentTags.Properties;
using HighlightCommentTags.Tools;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

using MediaColor = System.Windows.Media.Color;

namespace HighlightCommentTags.Editor {
    [Export(typeof(EditorFormatDefinition))] // export as EditorFormatDefinition otherwise the syntax coloring does not work
    [ClassificationType(ClassificationTypeNames = CommentTagsClassificationDefinition.ClassificationTypeNames.Todo)]
    [Name(CommentTagsClassificationDefinition.ClassificationTypeNames.Todo)]
    [UserVisible(true)] // sets this editor format definition visible for the user (in Tools>Options>Environment>Fonts and Colors>Text Editor
    [Order(After = Priority.High)] //set the priority to be after the default classifiers
    internal sealed class TodoClassificationFormatDef : ClassificationFormatDefinition {
        public TodoClassificationFormatDef() {
            this.DisplayName = "Comment Tag - Todo";
            var color = Utils.ConvertColor(Settings.Default.TodoForeground);
            this.ForegroundColor = Utils.ConvertColor(Settings.Default.TodoForeground);
            if (Settings.Default.UseTagBackground) {
                this.BackgroundColor = Utils.ConvertColor(Settings.Default.TodoForeground);
                this.BackgroundOpacity = Settings.Default.TagsBackgroundOpacity; 
            }
            this.IsBold = true;
        }
    }

    [Export(typeof(EditorFormatDefinition))] // export as EditorFormatDefinition otherwise the syntax coloring does not work
    [ClassificationType(ClassificationTypeNames = CommentTagsClassificationDefinition.ClassificationTypeNames.Note)]
    [Name(CommentTagsClassificationDefinition.ClassificationTypeNames.Note)]
    [UserVisible(true)] // sets this editor format definition visible for the user (in Tools>Options>Environment>Fonts and Colors>Text Editor
    [Order(After = Priority.High)] //set the priority to be after the default classifiers
    internal sealed class NoteClassificationFormatDef : ClassificationFormatDefinition {
        public NoteClassificationFormatDef() {
            this.DisplayName = "Comment Tag - Note";
            this.ForegroundColor = Utils.ConvertColor(Settings.Default.NoteForeground);
            if (Settings.Default.UseTagBackground) {
                this.BackgroundColor = Utils.ConvertColor(Settings.Default.NoteForeground);
                this.BackgroundOpacity = Settings.Default.TagsBackgroundOpacity; 
            }
            this.IsBold = true;
        }
    }

    [Export(typeof(EditorFormatDefinition))] // export as EditorFormatDefinition otherwise the syntax coloring does not work
    [ClassificationType(ClassificationTypeNames = CommentTagsClassificationDefinition.ClassificationTypeNames.Debug)]
    [Name(CommentTagsClassificationDefinition.ClassificationTypeNames.Debug)]
    [UserVisible(true)] // sets this editor format definition visible for the user (in Tools>Options>Environment>Fonts and Colors>Text Editor
    [Order(After = Priority.High)] //set the priority to be after the default classifiers
    internal sealed class DebugClassificationFormatDef : ClassificationFormatDefinition {
        public DebugClassificationFormatDef() {
            this.DisplayName = "Comment Tag - Debug";
            this.ForegroundColor = Utils.ConvertColor(Settings.Default.DebugForeground);
            if (Settings.Default.UseTagBackground) {
                this.BackgroundColor = Utils.ConvertColor(Settings.Default.DebugForeground);
                this.BackgroundOpacity = Settings.Default.TagsBackgroundOpacity; 
            }
            this.IsBold = true;
        }
    }

    [Export(typeof(EditorFormatDefinition))] // export as EditorFormatDefinition otherwise the syntax coloring does not work
    [ClassificationType(ClassificationTypeNames = CommentTagsClassificationDefinition.ClassificationTypeNames.Fixme)]
    [Name(CommentTagsClassificationDefinition.ClassificationTypeNames.Fixme)]
    [UserVisible(true)] // sets this editor format definition visible for the user (in Tools>Options>Environment>Fonts and Colors>Text Editor
    [Order(After = Priority.High)] //set the priority to be after the default classifiers
    internal sealed class FixmeClassificationFormatDef : ClassificationFormatDefinition {
        public FixmeClassificationFormatDef() {
            this.DisplayName = "Comment Tag - Fixme";
            this.ForegroundColor = Utils.ConvertColor(Settings.Default.FixmeForeground);
            if (Settings.Default.UseTagBackground) {
                this.BackgroundColor = Utils.ConvertColor(Settings.Default.FixmeForeground);
                this.BackgroundOpacity = Settings.Default.TagsBackgroundOpacity; 
            }
            this.IsBold = true;
        }
    }

    [Export(typeof(EditorFormatDefinition))] // export as EditorFormatDefinition otherwise the syntax coloring does not work
    [ClassificationType(ClassificationTypeNames = CommentTagsClassificationDefinition.ClassificationTypeNames.Fixed)]
    [Name(CommentTagsClassificationDefinition.ClassificationTypeNames.Fixed)]
    [UserVisible(true)] // sets this editor format definition visible for the user (in Tools>Options>Environment>Fonts and Colors>Text Editor
    [Order(After = Priority.High)] //set the priority to be after the default classifiers
    internal sealed class FixedClassificationFormatDef : ClassificationFormatDefinition {
        public FixedClassificationFormatDef() {
            this.DisplayName = "Comment Tag - Fixed";
            this.ForegroundColor = Utils.ConvertColor(Settings.Default.FixedForeground);
            if (Settings.Default.UseTagBackground){
                this.BackgroundColor = Utils.ConvertColor(Settings.Default.FixedForeground);
                this.BackgroundOpacity = Settings.Default.TagsBackgroundOpacity; 
            }
            this.IsBold = true;
        }
    }
}
