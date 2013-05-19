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
    /// Interaction logic for WallDrawTool.xaml
    /// </summary>
    public partial class WallDrawTool : UserControl, IEditorTool
    {
        public WallDrawTool()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TempWallDataProperty =
            DependencyProperty.Register("TempWallData", typeof(LineGeometry), typeof(WallDrawTool));

        public LineGeometry TempWallData
        {
            get { return (LineGeometry)GetValue(TempWallDataProperty); }
            set { SetValue(TempWallDataProperty, value); }
        }

        public static readonly DependencyProperty SelectedPointDataProperty =
            DependencyProperty.Register("SelectedPointData", typeof(Geometry), typeof(WallDrawTool));

        public Geometry SelectedPointData
        {
            get { return (Geometry)GetValue(SelectedPointDataProperty); }
            set { SetValue(SelectedPointDataProperty, value); }
        }

        public static readonly DependencyProperty SelectedPointRadiusProperty =
            DependencyProperty.Register("SelectedPointRadius", typeof(double), typeof(WallDrawTool));

        public double SelectedPointRadius
        {
            get { return (double)GetValue(SelectedPointRadiusProperty); }
            set { SetValue(SelectedPointRadiusProperty, value); }
        }


        public void Init(Editor editor)
        {
            this.editor = editor;
            editor.ScaleChanged += editor_ScaleChanged;
        }

        public void StopEditing()
        {
            TempWallData = null;
            LastPoint = null;
        }

        public void Accept()
        {
            StopEditing();
        }

        public new bool MouseDown(Point pos)
        {
            pos = CorrectPosition(pos);
            PlaceWall(pos);
            return true;
        }

        public new bool MouseUp(Point pos)
        {
            return false;
        }

        public new bool MouseMove(Point pos)
        {
            if (LastPoint != null)
            {
                pos = CorrectPosition(pos);
                PlaceTempWall(pos);
                return true;
            }
            else
            {
                pos = CorrectPosition(pos);
                UpdateSelectedPoint(pos);
                return true;
            }

        }

        public new bool KeyDown(Key key)
        {
            switch (key)
            {
                case Key.Z:
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        if (editor.Walls.Any())
                        {
                            editor.Walls.RemoveAt(editor.Walls.Count - 1);
                        }
                        return true;
                    }
                    break;
            }
            return false;
        }


        void editor_ScaleChanged(object sender, EventArgs e)
        {
            SelectedPointRadius = 5 / editor.Scale;
        }



        private void PlaceWall(Point position)
        {
            if (LastPoint == null)
            {
                LastPoint = position;
            }
            else
            {
                editor.Walls.Add(new Wall { Start = LastPoint.Value, End = position });
                LastPoint = position;
            }
            TempWallData = null;
        }

        private void PlaceTempWall(Point pos)
        {
            if (LastPoint != null)
            {
                TempWallData = new LineGeometry { StartPoint = LastPoint.Value, EndPoint = pos };
            }
        }

        private void UpdateSelectedPoint(Point? point)
        {
            Geometry geometry = null;
            if (point != null)
            {
                geometry = new EllipseGeometry
                {
                    Center = point.Value,
                };
                var binding = new Binding { Path = new PropertyPath("SelectedPointRadius"), Source = this };
                BindingOperations.SetBinding(geometry, EllipseGeometry.RadiusXProperty, binding);
                BindingOperations.SetBinding(geometry, EllipseGeometry.RadiusYProperty, binding);
            }

            SelectedPointData = geometry;
        }

        private Point CorrectPosition(Point pos)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (LastPoint != null)
                {
                    var diff = Point.Subtract(pos, LastPoint.Value);
                    if (Math.Abs(diff.X) < Math.Abs(diff.Y))
                    {
                        pos.X = LastPoint.Value.X;
                        var wall = editor.Walls.FirstOrDefault(
                            w => Math.Abs(w.Start.Y - pos.Y) * editor.Scale < 10);
                        if (wall != null)
                        {
                            pos.Y = wall.Start.Y;
                        }
                        else
                        {
                            wall = editor.Walls.FirstOrDefault(
                                w => Math.Abs(w.End.Y - pos.Y) * editor.Scale < 10);
                            if (wall != null)
                            {
                                pos.Y = wall.End.Y;
                            }
                        }
                    }
                    else
                    {
                        pos.Y = LastPoint.Value.Y;
                        var wall = editor.Walls.FirstOrDefault(
                            w => Math.Abs(w.Start.X - pos.X) * editor.Scale < 10);
                        if (wall != null)
                        {
                            pos.X = wall.Start.X;
                        }
                        else
                        {
                            wall = editor.Walls.FirstOrDefault(
                                w => Math.Abs(w.End.X - pos.X) * editor.Scale < 10);
                            if (wall != null)
                            {
                                pos.X = wall.End.X;
                            }
                        }
                    }
                }
            }
            else
            {
                var minDist = double.MaxValue;
                Point? closestPoint = null;
                foreach (var w in editor.Walls)
                {
                    var dist = Utils.GetPointsDistance(w.Start, pos);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closestPoint = w.Start;
                    }
                    dist = Utils.GetPointsDistance(w.End, pos);
                    if (dist < minDist)
                    {
                        minDist = dist;
                        closestPoint = w.End;
                    }
                }
                if (minDist * editor.Scale < 10)
                {
                    pos = closestPoint.Value;
                }
            }

            return pos;
        }

        public Point? LastPoint
        {
            get { return lastPoint; }
            set
            {
                lastPoint = value;
                UpdateSelectedPoint(lastPoint);
            }
        }
        private Point? lastPoint;

        private Editor editor;
    }
}
