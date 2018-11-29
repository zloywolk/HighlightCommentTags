using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;

using HighlightCommentTags.Properties;
using Resx = HighlightCommentTags.Properties.Resources;

namespace HighlightCommentTags.Options {
    [Guid(Guids.CommentTagsOptionsPage)]
    internal sealed class CommentTagsOptionsPage : UIElementDialogPage {
        private UICommentTagsOptionPage _child;

        internal CommentTagsSettingsStore _settings;
        internal UICommentTagsOptionPage Ui => this.Child as UICommentTagsOptionPage;

        protected override UIElement Child => _child = _child ?? new UICommentTagsOptionPage(this._settings);

        public CommentTagsOptionsPage() {
            this._settings = new CommentTagsSettingsStore();
        }

        private bool UpdateEnvInstance() {
            return false;
        }

        #region DialogPage

        protected override void OnActivate(CancelEventArgs e) {
            base.OnActivate(e);
            this._settings.Load();
        }

        protected override void OnDeactivate(CancelEventArgs e) {
            base.OnDeactivate(e);
            this._settings.Save();
        }

        protected override void OnApply(PageApplyEventArgs e) {
            base.OnApply(e);

            ThreadHelper.ThrowIfNotOnUIThread();
            if (this._settings.Save()) {
                this._settings.Commit((IVsFontAndColorStorage)this.GetService(typeof(SVsFontAndColorStorage)), 
                    (IVsFontAndColorCacheManager)this.GetService(typeof(SVsFontAndColorCacheManager)));

                if (this._settings.RequestRestart) {
                    MessageBoxResult result = MessageBox.Show(
                        Resx.RequestRestartMessage,
                        "Visual Studio",
                        MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
            }
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
        }

        #endregion
    }
}
