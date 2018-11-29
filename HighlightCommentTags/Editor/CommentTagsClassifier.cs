using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;

using HighlightCommentTags.Properties;

namespace HighlightCommentTags.Editor {
    internal sealed class CommentTagsClassifier : ITagger<ClassificationTag> {
        event EventHandler<SnapshotSpanEventArgs> ITagger<ClassificationTag>.TagsChanged {
            add { }
            remove { }
        }

        private readonly ITextBuffer _buffer;
        private readonly ITagAggregator<CommentTagsToken> _aggregator;

        private readonly ClassificationTag _todo;
        private readonly ClassificationTag _note;
        private readonly ClassificationTag _debug;
        private readonly ClassificationTag _fixme;
        private readonly ClassificationTag _fixed;

        public CommentTagsClassifier(
            ITextBuffer buffer, 
            ITagAggregator<CommentTagsToken> aggregator,
            IClassificationTypeRegistryService registry) {

            this._buffer = buffer;
            this._aggregator = aggregator;

            this._todo = new ClassificationTag(registry.GetClassificationType(CommentTagsClassificationDefinition.ClassificationTypeNames.Todo));
            this._note = new ClassificationTag(registry.GetClassificationType(CommentTagsClassificationDefinition.ClassificationTypeNames.Note));
            this._debug = new ClassificationTag(registry.GetClassificationType(CommentTagsClassificationDefinition.ClassificationTypeNames.Debug));
            this._fixme = new ClassificationTag(registry.GetClassificationType(CommentTagsClassificationDefinition.ClassificationTypeNames.Fixme));
            this._fixed = new ClassificationTag(registry.GetClassificationType(CommentTagsClassificationDefinition.ClassificationTypeNames.Fixed));
        }

        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans) {
            if (Settings.Default.EnableTagsHighlighting) {
                if (spans.Count == 0) { yield break; }

                foreach (IMappingTagSpan<CommentTagsToken> tagSpan in this._aggregator.GetTags(spans)) {
                    NormalizedSnapshotSpanCollection tagSpans = tagSpan.Span.GetSpans(spans[0].Snapshot);

                    switch (tagSpan.Tag.Type) {
                        case CommentTagsType.Todo: yield return new TagSpan<ClassificationTag>(tagSpans[0], this._todo); break;
                        case CommentTagsType.Note: yield return new TagSpan<ClassificationTag>(tagSpans[0], this._note); break;
                        case CommentTagsType.Debug: yield return new TagSpan<ClassificationTag>(tagSpans[0], this._debug); break;
                        case CommentTagsType.Fixme: yield return new TagSpan<ClassificationTag>(tagSpans[0], this._fixme); break;
                        case CommentTagsType.Fixed: yield return new TagSpan<ClassificationTag>(tagSpans[0], this._fixed); break;
                    }
                } 
            }
        }
    }
}
