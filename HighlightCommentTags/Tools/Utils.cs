using MediaColor = System.Windows.Media.Color;
using DrawingColor = System.Drawing.Color;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using HighlightCommentTags.Editor;

namespace HighlightCommentTags.Tools {
    internal static class Utils {
        public static MediaColor ConvertColor(DrawingColor color) {
            return MediaColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static DrawingColor ConvertColor(MediaColor color) {
            return DrawingColor.FromArgb(color.A, color.R, color.G, color.B);
        }

        public class CommentTagsPosition { 
}

        public static IEnumerable<(int Start, int Length, CommentTagsType Type)> GetCommentTags(string line) {
            (int Start, int Length) found;

            if ((found = FindTag(line, "todo")).Start != -1) {
                yield return (found.Start, found.Length, CommentTagsType.Todo);
            }

            if ((found = FindTag(line, "note")).Start != -1) {
                yield return (found.Start, found.Length, CommentTagsType.Note);
            }

            if ((found = FindTag(line, "debug")).Start != -1) {
                yield return (found.Start, found.Length, CommentTagsType.Debug);
            }

            if ((found = FindTag(line, "fixme")).Start != -1) {
                yield return (found.Start, found.Length, CommentTagsType.Fixme);
            }

            if ((found = FindTag(line, "fixed")).Start != -1) {
                yield return (found.Start, found.Length, CommentTagsType.Fixed);
            }

            yield break;
        }

        private static (int Start, int Length) FindTag(string line, string tag) {
            int start = -1;

            if ((start = line.IndexOf(tag, System.StringComparison.OrdinalIgnoreCase)) != -1) {
                return (start, tag.Length);
            }

            return (-1, -1);
        }
    }
}
