using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using Newtonsoft.Json;
using SchemeEditor.Schema;

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

        private void ItemTool_Checked(object sender, RoutedEventArgs e)
        {
            editor.ActiveToolKind = View.EditorToolKind.ItemCreate;
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

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            editor.Accept();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var scheme = new Scheme
            {
                Rooms = editor.Rooms.ToArray(),
                Items = editor.Items.ToArray()
            };

            var dialog = new SaveFileDialog
            {
                Filter = "Schemes|*.json||",
                AddExtension = true
            };
            if (dialog.ShowDialog() == true)
            {
                Dictionary<string, object> dict = scheme.ToJsonObject();
                string jsonStr = JsonConvert.SerializeObject(dict);
                File.WriteAllText(dialog.FileName, jsonStr);
            }
        }
    }
}
