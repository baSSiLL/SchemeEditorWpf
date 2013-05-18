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
using System.Windows.Shapes;

namespace SchemeEditor.View
{
    /// <summary>
    /// Interaction logic for RoomInfo.xaml
    /// </summary>
    public partial class RoomInfo : Window
    {
        public RoomInfo()
        {
            InitializeComponent();
        }

        public string Title
        {
            get { return title.Text; }
        }

        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                chooseColor.Background = new SolidColorBrush(value);
            }
        }
        private Color color;

        private void OK_Click(object sender, RoutedEventArgs args)
        {
            DialogResult = true;
        }

        private void ChooseColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.ColorDialog();
            dialog.AnyColor = true;
            dialog.Color = System.Drawing.Color.FromArgb(Color.R, Color.G, Color.B);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Color = Color.FromArgb(255, dialog.Color.R, dialog.Color.G, dialog.Color.B);
            }
        }
    }
}
