using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : UserControl
    {
        public Editor()
        {
            InitializeComponent();
            Scale = 1;
        }

        public void StopEditing()
        {
            TempWallData = null;
            LastPoint = null;
        }

        public static readonly DependencyProperty BackgroundImageProperty =
            DependencyProperty.Register("BackgroundImage", typeof(ImageSource), typeof(Editor));
        
        public ImageSource BackgroundImage
        {
            get { return (ImageSource)GetValue(BackgroundImageProperty); }
            set { SetValue(BackgroundImageProperty, value); }
        }

        public static readonly DependencyProperty WallThicknessProperty =
            DependencyProperty.Register("WallThickness", typeof(double), typeof(Editor));

        public double WallThickness
        {
            get { return (double)GetValue(WallThicknessProperty); }
            set { SetValue(WallThicknessProperty, value); }
        }

        public static readonly DependencyProperty WallsPathDataProperty =
            DependencyProperty.Register("WallsPathData", typeof(Geometry), typeof(Editor));

        public Geometry WallsPathData
        {
            get { return (Geometry)GetValue(WallsPathDataProperty); }
            set { SetValue(WallsPathDataProperty, value); }
        }

        public static readonly DependencyProperty TempWallDataProperty =
            DependencyProperty.Register("TempWallData", typeof(LineGeometry), typeof(Editor));

        public LineGeometry TempWallData
        {
            get { return (LineGeometry)GetValue(TempWallDataProperty); }
            set { SetValue(TempWallDataProperty, value); }
        }

        public static readonly DependencyProperty SelectedPointDataProperty =
            DependencyProperty.Register("SelectedPointData", typeof(Geometry), typeof(Editor));

        public Geometry SelectedPointData
        {
            get { return (Geometry)GetValue(SelectedPointDataProperty); }
            set { SetValue(SelectedPointDataProperty, value); }
        }

        public static readonly DependencyProperty SelectedPointRadiusProperty =
            DependencyProperty.Register("SelectedPointRadius", typeof(double), typeof(Editor));

        public double SelectedPointRadius
        {
            get { return (double)GetValue(SelectedPointRadiusProperty); }
            set { SetValue(SelectedPointRadiusProperty, value); }
        }

        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            base.OnPreviewMouseWheel(e);

            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                ScaleDelta(e.Delta);
                e.Handled = true;
            }
        }

        private void Content_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(background);
            if (pos.X >= 0 && pos.X < BackgroundImage.Width &&
                pos.Y >= 0 && pos.Y < BackgroundImage.Height)
            {
                PlaceWall(pos);
                e.Handled = true;
            }
        }

        private void Content_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(background);
            if (pos.X >= 0 && pos.X < BackgroundImage.Width &&
                pos.Y >= 0 && pos.Y < BackgroundImage.Height)
            {
                PlaceTempWall(pos);
                e.Handled = true;
            }
        }

        private void ScaleDelta(int delta)
        {
            double coeff = Math.Pow(1.25, delta / 120);
            Scale = Math.Min(10, Math.Max(0.1, coeff * Scale));
            background.LayoutTransform = new ScaleTransform() { ScaleX = scale, ScaleY = scale };
        }

        private void PlaceWall(Point position)
        {
            if (LastPoint == null)
            {
                LastPoint = position;
            }
            else
            {
                walls.Add(new Wall { Start = LastPoint.Value, End = position });
                UpdateWallsPath();
                LastPoint = position;
            }
            TempWallData = null;
        }

        private void UpdateWallsPath()
        {
            var sb = new StringBuilder();
            foreach (var wall in walls)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, " M {0},{1} L {2},{3}",
                    wall.Start.X, wall.Start.Y, wall.End.X, wall.End.Y);
            }
            WallsPathData = Geometry.Parse(sb.ToString());
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
                var binding = new Binding { Path = new PropertyPath("SelectedPointRadius"), Source = this};
                BindingOperations.SetBinding(geometry, EllipseGeometry.RadiusXProperty, binding);
                BindingOperations.SetBinding(geometry, EllipseGeometry.RadiusYProperty, binding);
            }

            SelectedPointData = geometry;
        }

        private double Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                var scaleTransform = new ScaleTransform { ScaleX = scale, ScaleY = scale };
                background.LayoutTransform = scaleTransform;
                wallsPath.LayoutTransform = scaleTransform;
                wallDrawTool.LayoutTransform = scaleTransform;
                WallThickness = 3 / scale;
                SelectedPointRadius = 5 / scale;
            }
        }
        private double scale = 1;

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

        private readonly List<Wall> walls = new List<Wall>();
    }
}
