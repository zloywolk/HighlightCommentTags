using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Resx = HighlightCommentTags.Properties.Resources;

namespace HighlightCommentTags.Options {
    /// <summary>
    /// Interaction logic for UICommentTagsOptionPage.xaml
    /// </summary>
    public partial class UICommentTagsOptionPage : UserControl {
        private CommentTagsSettingsStore _settings;

        public UICommentTagsOptionPage(CommentTagsSettingsStore settings) {
            InitializeComponent();
            this._settings = settings;
            this.DataContext = this._settings;
        }

        private void btnResetDefaults_Click(object sender, RoutedEventArgs e) {
            ThreadHelper.ThrowIfNotOnUIThread();
            MessageBoxResult result = MessageBox.Show(
                Resx.ResetToDefaultsMessage,
                "Visual Studio", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes) { this._settings.Reset(); }
        }
    }
}
