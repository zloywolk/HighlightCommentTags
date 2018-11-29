using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

using MediaColor = System.Windows.Media.Color;
using DrawingColor = System.Drawing.Color;
using System.Reflection;
using HighlightCommentTags.Properties;
using HighlightCommentTags.Tools;
using HighlightCommentTags.Editor;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio;
using System.Drawing;
using Microsoft.VisualStudio.Shell;

namespace HighlightCommentTags.Options {
    public class CommentTagsSettingsStore : INotifyPropertyChanged {
        private bool _enableTagsHighlighting;
        private MediaColor? _todoForeground;
        private MediaColor? _noteForeground;
        private MediaColor? _debugForeground;
        private MediaColor? _fixmeForeground;
        private MediaColor? _fixedForeground;

        private bool _changed = false;

        public bool RequestRestart { get; set; }

        [ConfigEntry("EnableTagsHighlighting", typeof(bool))]
        public bool EnableTagsHighlighting {
            get => _enableTagsHighlighting;
            set {
                _enableTagsHighlighting = value;
                OnPropertyChanged();
            }
        }

        [ConfigEntry("TodoForeground", typeof(DrawingColor), 
            ClassificationTypeName = CommentTagsClassificationDefinition.ClassificationTypeNames.Todo)]
        public MediaColor? TodoForeground {
            get => _todoForeground;
            set {
                _todoForeground = value;
                OnPropertyChanged();
            }
        }

        [ConfigEntry("NoteForeground", typeof(DrawingColor), 
            ClassificationTypeName = CommentTagsClassificationDefinition.ClassificationTypeNames.Note)]
        public MediaColor? NoteForeground {
            get => _noteForeground;
            set {
                _noteForeground = value;
                OnPropertyChanged();
            }
        }

        [ConfigEntry("DebugForeground", typeof(DrawingColor),
            ClassificationTypeName = CommentTagsClassificationDefinition.ClassificationTypeNames.Debug)]
        public MediaColor? DebugForeground {
            get => _debugForeground;
            set {
                _debugForeground = value;
                OnPropertyChanged();
            }
        }

        [ConfigEntry("FixmeForeground", typeof(DrawingColor),
            ClassificationTypeName = CommentTagsClassificationDefinition.ClassificationTypeNames.Fixme)]
        public MediaColor? FixmeForeground {
            get => _fixmeForeground;
            set {
                _fixmeForeground = value;
                OnPropertyChanged();
            }
        }

        [ConfigEntry("FixedForeground", typeof(DrawingColor),
            ClassificationTypeName = CommentTagsClassificationDefinition.ClassificationTypeNames.Fixed)]
        public MediaColor? FixedForeground {
            get => _fixedForeground;
            set {
                _fixedForeground = value;
                OnPropertyChanged();
            }
        }

        internal void Reset() {
            ThreadHelper.ThrowIfNotOnUIThread();
            var props = from prop in this.GetType().GetProperties()
                        where Attribute.IsDefined(prop, typeof(ConfigEntryAttribute))
                        select new {
                            Property = prop,
                            Value = prop.GetValue(this),
                            Attribute = prop.GetCustomAttribute<ConfigEntryAttribute>()
                        };

            foreach (var p in props) {
                if (p.Attribute.Type == typeof(DrawingColor)) {
                    var color = Utils.ConvertColor((MediaColor)p.Value);
                    if (color.ToArgb() != ((DrawingColor)Settings.Default[$"Default{p.Attribute.Name}"]).ToArgb()) {
                        p.Property.SetValue(this, Utils.ConvertColor((DrawingColor)Settings.Default[$"Default{p.Attribute.Name}"]));
                    }
                }
            }
        }

        internal void Load() {
            var props = from prop in this.GetType().GetProperties()
                        where Attribute.IsDefined(prop, typeof(ConfigEntryAttribute))
                        select new {
                            Property = prop,
                            Attribute = prop.GetCustomAttribute<ConfigEntryAttribute>()
                        };

            foreach (var p in props) {
                if (p.Attribute.Type == typeof(DrawingColor)) {
                    p.Property.SetValue(this, Utils.ConvertColor((DrawingColor)Settings.Default[p.Attribute.Name]));
                } else {
                    p.Property.SetValue(this, Settings.Default[p.Attribute.Name]);
                }
            }
        }

        internal bool Save() { 
            if (this._changed) { return this._changed; }

            var props = from prop in this.GetType().GetProperties()
                        where Attribute.IsDefined(prop, typeof(ConfigEntryAttribute))
                        select new {
                            Property = prop,
                            Value = prop.GetValue(this),
                            Attribute = prop.GetCustomAttribute<ConfigEntryAttribute>()
                        };

            foreach (var p in props) {
                if (p.Attribute.Type == typeof(DrawingColor)) {
                    var color = Utils.ConvertColor((MediaColor)p.Value);
                    if (color.ToArgb() != ((DrawingColor)Settings.Default[p.Attribute.Name]).ToArgb()) {
                        this._changed |= true;
                    }
                } else {
                    if (!(bool)p.Value.GetType().GetMethod(nameof(Equals), new[] { p.Property.PropertyType }).Invoke(p.Value, new []{ Settings.Default[p.Attribute.Name] })) {
                        this._changed |= true;
                    }
                }
            }

            return this._changed;
        }

        public void Commit(IVsFontAndColorStorage storage, IVsFontAndColorCacheManager cache) {
            ThreadHelper.ThrowIfNotOnUIThread();
            if (this._changed) {

                var props = from prop in this.GetType().GetProperties()
                            where Attribute.IsDefined(prop, typeof(ConfigEntryAttribute))
                            select new {
                                Property = prop,
                                Value = prop.GetValue(this),
                                Attribute = prop.GetCustomAttribute<ConfigEntryAttribute>()
                            };

                foreach (var p in props) {
                    if (p.Attribute.Type == typeof(DrawingColor)) {
                        var color = Utils.ConvertColor((MediaColor)p.Value);
                        if (color.ToArgb() != ((DrawingColor)Settings.Default[p.Attribute.Name]).ToArgb()) {
                            Settings.Default[p.Attribute.Name] = color;
                            // Update category Text Editor in the Fonts And Colors
                            if (!string.IsNullOrEmpty(p.Attribute.ClassificationTypeName)) {
                                var guid = new Guid(Guids.FontAndColorsTextEditorCategory);
                                var flags = __FCSTORAGEFLAGS.FCSF_LOADDEFAULTS | __FCSTORAGEFLAGS.FCSF_PROPAGATECHANGES;

                                if (storage != null) {
                                    if (storage.OpenCategory(ref guid, (uint)flags) != VSConstants.S_OK) return;

                                    storage.SetItem(p.Attribute.ClassificationTypeName, new[]{ new ColorableItemInfo
                                    {
                                        bForegroundValid = 1,
                                        bBackgroundValid = 1,
                                        crForeground = (uint)ColorTranslator.ToWin32(color),
                                        crBackground = (uint)ColorTranslator.ToWin32(color)
                                    }});
                                    storage.CloseCategory();
                                }
                            }
                        }
                    } else {
                        if (!(bool)p.Value.GetType().GetMethod(nameof(Equals), new[] { p.Property.PropertyType }).Invoke(p.Value, new[] { Settings.Default[p.Attribute.Name] })) {
                            Settings.Default[p.Attribute.Name] = p.Value;

                            this.RequestRestart |= p.Attribute.RequestRestart;
                        }
                    }
                }

                if (cache != null) {
                    cache.ClearAllCaches();
                    var guid = new Guid(Guids.Empty);
                    cache.RefreshCache(ref guid);
                    guid = new Guid(Guids.FontAndColorsTextEditorCategory);
                }

                Settings.Default.Save();

                this._changed = false;
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            var handler = this.PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
