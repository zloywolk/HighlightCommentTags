using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace HighlightCommentTags.Tools {
    internal static class AppInfo {
        public static string Version => typeof(CommentTagsPackage).Assembly.GetName().Version.ToString();

        public static string Title => typeof(CommentTagsPackage).Assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;

        public static string Copyright => typeof(CommentTagsPackage).Assembly.GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;

        public static string FullName => $"{AppInfo.Title} v{AppInfo.Version} {AppInfo.Copyright}";
    }
}
