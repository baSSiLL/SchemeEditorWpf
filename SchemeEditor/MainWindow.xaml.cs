using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SchemeEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new AppViewModel();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (editor.GlobalKeyDown(e))
            {
                e.Handled = true;
            }
        }

        private void WallTool_Checked(object sender, RoutedEventArgs e)
        {
            editor.ActiveToolKind = View.EditorToolKind.WallDraw;
            UpdateToolsState(sender);
        }

        private void RoomTool_Checked(object sender, RoutedEventArgs e)
        {
            editor.ActiveToolKind = View.EditorToolKind.RoomDefine;
            UpdateToolsState(sender);
        }

        private void UpdateToolsState(object sender)
        {
            foreach (var toolButton in tools.Children.OfType<ToggleButton>())
            {
                if (toolButton != sender)
                {
                    toolButton.IsChecked = false;
                }
            }
        }
    }
}
