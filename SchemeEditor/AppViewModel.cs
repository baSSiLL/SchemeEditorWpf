using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using SchemeEditor.Tools;

namespace SchemeEditor
{
    class AppViewModel : DependencyObject
    {
        public AppViewModel()
        {
            openBackgroundCommand = new DelegateCommand(OpenBackground);
        }

        public ICommand OpenBackgroundCommand
        {
            get { return openBackgroundCommand; }
        }
        private readonly DelegateCommand openBackgroundCommand;

        public static readonly DependencyProperty BackgroundImageProperty =
            DependencyProperty.Register("BackgroundImage", typeof(ImageSource), typeof(AppViewModel));

        public ImageSource BackgroundImage
        {
            get { return (ImageSource)GetValue(BackgroundImageProperty); }
            set { SetValue(BackgroundImageProperty, value); }
        }

        private void OpenBackground()
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Images|*.png;*.jpg||";
            dialog.Multiselect = false;
            var result = dialog.ShowDialog();
            if (result == true)
            {
                backgroundImageBytes = File.ReadAllBytes(dialog.FileName);
#warning Use StreamSource for image
                BackgroundImage = new BitmapImage(new Uri("file://" + dialog.FileName));
            }
        }

        private byte[] backgroundImageBytes;
    }
}
