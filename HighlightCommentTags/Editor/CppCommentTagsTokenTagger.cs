using HighlightCommentTags.Tools;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;

using System.Diagnostics;
using System.Linq;

namespace HighlightCommentTags.Editor {
    internal sealed class CppCommentTagsTokenTagger : ITagger<CommentTagsToken> {
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged = delegate {};

        private readonly CommentTagsToken _todo;
        private readonly CommentTagsToken _note;
        private readonly CommentTagsToken _debug;
        private readonly CommentTagsToken _fixme;
        private readonly CommentTagsToken _fixed;

        private readonly ITextBuffer _buffer;
        private readonly ITextSearchService _searchService;

        private NormalizedSnapshotSpanCollection _comments;

        internal CppCommentTagsTokenTagger(ITextBuffer buffer, ITextSearchService searchService) {
            this._buffer = buffer;
            this._searchService = searchService;

            this.UpdateCommentSnaps(this._buffer.CurrentSnapshot);

            this._todo = new CommentTagsToken(CommentTagsType.Todo, "todo");
            this._note = new CommentTagsToken(CommentTagsType.Note, "note");
            this._debug = new CommentTagsToken(CommentTagsType.Debug, "debug");
            this._fixme = new CommentTagsToken(CommentTagsType.Fixme, "fixme");
            this._fixed = new CommentTagsToken(CommentTagsType.Fixed, "fixed");

            this._buffer.Changed += BufferChanged;
        }

        /// <summary>
        /// Find all one line comments in the current text snapshot
        /// </summary>
        /// <param name="snapshot">Current text snapshot retrieved from the buffer</param>
        private void UpdateCommentSnaps(ITextSnapshot snapshot) {
            this._comments = new NormalizedSnapshotSpanCollection(
                this._searchService.FindAll(new FindData(@"\/\/\s+.*", snapshot) {
                    FindOptions = FindOptions.UseRegularExpressions | FindOptions.MatchCase
                }));
        }

        private void BufferChanged(object sender, TextContentChangedEventArgs e) {
            if (e.After != e.Before) {
                this.UpdateCommentSnaps(e.After);
                this.TagsChanged(this, new SnapshotSpanEventArgs(new SnapshotSpan(e.After, 0, e.After.Length)));
            }
        }

        public IEnumerable<ITagSpan<CommentTagsToken>> GetTags(NormalizedSnapshotSpanCollection spans) {
            if (spans.Count == 0) { yield break; }

            spans = NormalizedSnapshotSpanCollection.Intersection(spans, this._comments);

            foreach (var span in spans) {
                var line = span.GetText();

                var tagPositions = Utils.GetCommentTags(line);

                foreach (var tag in tagPositions) {
                    switch (tag.Type) {
                        case CommentTagsType.Todo:
                            yield return new TagSpan<CommentTagsToken>(
                                new SnapshotSpan(span.Snapshot, new Span(tag.Start + span.Start.Position, tag.Length)), this._todo);
                            break;
                        case CommentTagsType.Note:
                            yield return new TagSpan<CommentTagsToken>
                                (new SnapshotSpan(span.Snapshot, new Span(tag.Start + span.Start.Position, tag.Length)), this._note);
                            break;
                        case CommentTagsType.Debug:
                            yield return new TagSpan<CommentTagsToken>(
                                new SnapshotSpan(span.Snapshot, new Span(tag.Start + span.Start.Position, tag.Length)), this._debug);
                            break;
                        case CommentTagsType.Fixme:
                            yield return new TagSpan<CommentTagsToken>(
                                new SnapshotSpan(span.Snapshot, new Span(tag.Start + span.Start.Position, tag.Length)), this._fixme);
                            break;
                        case CommentTagsType.Fixed:
                            yield return new TagSpan<CommentTagsToken>(
                                new SnapshotSpan(span.Snapshot, new Span(tag.Start + span.Start.Position, tag.Length)), this._fixed);
                            break;
                    }
                }
            }
        }
    }
}
