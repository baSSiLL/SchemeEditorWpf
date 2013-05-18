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

        private void OK_Click(object sender, RoutedEventArgs args)
        {
            DialogResult = true;
        }
    }
}
