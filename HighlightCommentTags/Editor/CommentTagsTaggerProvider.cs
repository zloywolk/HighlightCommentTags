using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
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
    [TagType(typeof(ClassificationTag))]
    [Name("Default Comment Tags Tagger Provider")]
    [Order(After = Priority.High)]
    internal sealed class CommentTagsTaggerProvider : ITaggerProvider {
        [Import]
        private IClassificationTypeRegistryService _registry = null;

        [Import]
        private IBufferTagAggregatorFactoryService _factory = null;

        public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag {
            var aggregator = buffer.Properties.GetOrCreateSingletonProperty(() => _factory.CreateTagAggregator<CommentTagsToken>(buffer));

            return new CommentTagsClassifier(buffer, aggregator, this._registry) as ITagger<T>;
        }
    }
}
