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
    /// Interaction logic for NewItem.xaml
    /// </summary>
    public partial class NewItem : Window
    {
        public NewItem()
        {
            InitializeComponent();
        }

        public string RoomTitle
        {
            get { return roomTitle; }
            set
            {
                roomTitle = value;
                label.Content = string.Format("Located in room '{0}'", value);
            }
        }
        private string roomTitle;

        public string Title
        {
            get { return titleBox.Text; }
        }

        public string QRCode
        {
            get { return qrCodeBox.Text; }
        }

        public bool Visible
        {
            get { return visibleBox.IsChecked.Value; }
        }

        public bool SpecificLocation
        {
            get { return specificLocationBox.IsChecked.Value; }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
