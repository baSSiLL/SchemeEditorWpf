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
    /// Interaction logic for RoomDefineTool.xaml
    /// </summary>
    public partial class RoomDefineTool : UserControl, IEditorTool
    {
        public RoomDefineTool()
        {
            InitializeComponent();
        }

        private Editor editor;

        public void Init(Editor editor)
        {
            this.editor = editor;
            editor.ScaleChanged += editor_ScaleChanged;
        }

        public static readonly DependencyProperty SelectedWallsDataProperty =
            DependencyProperty.Register("SelectedWallsData", typeof(Geometry), typeof(RoomDefineTool));

        public Geometry SelectedWallsData
        {
            get { return (Geometry)GetValue(SelectedWallsDataProperty); }
            set { SetValue(SelectedWallsDataProperty, value); }
        }

        public static readonly DependencyProperty SelectedWallThicknessProperty =
            DependencyProperty.Register("SelectedWallThickness", typeof(double), typeof(RoomDefineTool));

        public double SelectedWallThickness
        {
            get { return (double)GetValue(SelectedWallThicknessProperty); }
            set { SetValue(SelectedWallThicknessProperty, value); }
        }

        public static readonly DependencyProperty RoomDataProperty =
            DependencyProperty.Register("RoomData", typeof(Geometry), typeof(RoomDefineTool));

        public Geometry RoomData
        {
            get { return (Geometry)GetValue(RoomDataProperty); }
            set { SetValue(RoomDataProperty, value); }
        }

        public static readonly DependencyProperty SelectionDataProperty =
            DependencyProperty.Register("SelectionData", typeof(Geometry), typeof(RoomDefineTool));

        public Geometry SelectionData
        {
            get { return (Geometry)GetValue(SelectionDataProperty); }
            set { SetValue(SelectionDataProperty, value); }
        }

        void editor_ScaleChanged(object sender, EventArgs e)
        {
            SelectedWallThickness = 4 / editor.Scale;
        }

        public new bool MouseDown(Point position)
        {
            startPoint = position;
            if (room == null)
            {
                room = new Room();
                previouslySelectedWalls.Clear();
            }
            return true;
        }

        public new bool MouseUp(Point position)
        {
            if (room != null && startPoint != null)
            {
                var selectedWalls = GetSelectedWalls(position);
                UpdateSelectionData(position);
                previouslySelectedWalls.AddRange(selectedWalls);

                startPoint = null;
                SelectionData = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        public new bool MouseMove(Point position)
        {
            if (room != null && startPoint != null)
            {
                UpdateSelectionData(position);
                return true;
            }
            else
            {
                return false;
            }
        }

        public new bool KeyDown(Key key)
        {
            switch (key)
            {
                case Key.Z:
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        if (editor.Rooms.Any())
                        {
                            editor.Rooms.RemoveAt(editor.Rooms.Count - 1);
                        }
                        return true;
                    }
                    break;
            }
            return false;
        }

        public void StopEditing()
        {
            startPoint = null;
            room = null;
            SelectionData = null;
            SelectedWallsData = null;
            RoomData = null;
        }

        public void Accept()
        {
            if (RoomData != null)
            {
                var roomInfo = new RoomInfo();
                roomInfo.Color = Colors.YellowGreen;
                var result = roomInfo.ShowDialog();
                if (result == true)
                {
                    room.Color = roomInfo.Color;
                    room.Title = roomInfo.Title;
                    editor.Rooms.Add(room);
                }
            }

            StopEditing();
        }

        private void UpdateSelectionData(Point position)
        {
            var geometry = new RectangleGeometry();
            geometry.Rect = new Rect(startPoint.Value, position);
            SelectionData = geometry;

            var selectedWalls = editor.Walls.Where(
                w => geometry.Rect.Contains(w.Start) && geometry.Rect.Contains(w.End)).
                Concat(previouslySelectedWalls).Distinct().ToList();
            SelectedWallsData = Editor.BuildWallsGeometry(selectedWalls);

            if (selectedWalls.Skip(1).Any())
            {
                room.SetWalls(selectedWalls);
                RoomData = Editor.BuildRoomGeometry(room);
            }
        }

        private IEnumerable<Wall> GetSelectedWalls(Point position)
        {
            var rect = new Rect(startPoint.Value, position);
            return editor.Walls.Where(
                w => rect.Contains(w.Start) && rect.Contains(w.End));
        }

        private Point? startPoint;
        private List<Wall> previouslySelectedWalls = new List<Wall>();
        private Room room;
    }
}
