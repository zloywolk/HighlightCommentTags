using Microsoft.VisualStudio.Text.Tagging;

namespace HighlightCommentTags.Editor {
    internal sealed class CommentTagsToken : ITag {
        public CommentTagsType Type { get; private set; }
        internal readonly string Tag;

        public CommentTagsToken(CommentTagsType type, string tag) {
            this.Type = type;
            this.Tag = tag;
        }
    }
}