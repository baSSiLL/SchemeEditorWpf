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
using SchemeEditor.Schema;

namespace SchemeEditor.View
{
    /// <summary>
    /// Interaction logic for ItemCreateTool.xaml
    /// </summary>
    public partial class ItemCreateTool : UserControl, IEditorTool
    {
        public ItemCreateTool()
        {
            InitializeComponent();
        }

        private Editor editor;

        public void Init(Editor editor)
        {
            this.editor = editor;
        }

        public new bool MouseDown(Point position)
        {
            var room = editor.GetRoomFromPosition(position);
            if (room != null)
            {
                var itemInfo = new NewItem
                {
                    RoomTitle = room.Title
                };
                if (itemInfo.ShowDialog() == true)
                {
                    Item item;
                    if (itemInfo.SpecificLocation)
                    {
                        item = new CoordinateItem { Location = position };
                    }
                    else
                    {
                        item = new RoomItem();
                    }
                    item.Room = room;
                    item.Title = itemInfo.Title;
                    item.QRCode = itemInfo.QRCode;
                    item.Visible = itemInfo.Visible;
                    editor.Items.Add(item);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public new bool MouseMove(Point position)
        {
            return false;
        }

        public new bool MouseUp(Point position)
        {
            return false;
        }

        public new bool KeyDown(Key key)
        {
            return false;
        }

        public void StopEditing()
        {
            
        }

        public void Accept()
        {
            
        }
    }
}
