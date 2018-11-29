using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HighlightCommentTags.Editor {
    [Export(typeof(ITaggerProvider))]
    [ContentType("C/C++")]
    [TagType(typeof(CommentTagsToken))]
    [Name("C/C++ Comment Tags Token Tag Provider")]
    [Order(Before = "default")]
    internal sealed class CommentTagsTokenProvider : ITaggerProvider {
        [Import]
        internal readonly ITextSearchService _searchService = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag {
            Func<ITagger<T>> sc = () => {
                return new CppCommentTagsTokenTagger(buffer, this._searchService) as ITagger<T>;
            };

            return buffer.Properties.GetOrCreateSingletonProperty(sc);
        }
    }
}
