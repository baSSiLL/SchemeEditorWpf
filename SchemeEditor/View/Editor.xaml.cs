using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            walls.CollectionChanged += walls_CollectionChanged;

            wallDrawTool.Init(this);
            roomDefineTool.Init(this);

            Scale = 1;
        }

        public bool GlobalKeyDown(KeyEventArgs args)
        {
            switch (args.Key)
            {
                case Key.Escape:
                    if (HasActiveTool)
                    {
                        activeTool.StopEditing();
                    }
                    else
                    {
                        return false;
                    }
                    break;
                case Key.OemMinus:
                    ScaleDelta(-120);
                    break;
                case Key.OemPlus:
                    ScaleDelta(120);
                    break;
                default:
                    if (HasActiveTool)
                    {
                        return activeTool.KeyDown(args.Key);
                    }
                    else
                    {
                        return false;
                    }
            }

            return true;
        }

        public EditorToolKind ActiveToolKind
        {
            get 
            {
                if (activeTool == wallDrawTool)
                    return EditorToolKind.WallDraw;
                else if (activeTool == roomDefineTool)
                    return EditorToolKind.RoomDefine;
                else
                    return EditorToolKind.None;
            }
            set
            {
                IEditorTool newTool;
                switch (value)
                {
                    case EditorToolKind.WallDraw:
                        newTool = wallDrawTool;
                        break;
                    case EditorToolKind.RoomDefine:
                        newTool = roomDefineTool;
                        break;
                    default:
                        newTool = null;
                        break;
                }

                if (newTool != activeTool)
                {
                    if (activeTool != null)
                        activeTool.Visibility = Visibility.Hidden;

                    activeTool = newTool;

                    if (activeTool != null)
                        activeTool.Visibility = Visibility.Visible;
                }
            }
        }

        private bool HasActiveTool
        {
            get { return activeTool != null; }
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
            if (!HasActiveTool)
                return;

            var pos = e.GetPosition(background);
            if (pos.X >= 0 && pos.X < BackgroundImage.Width &&
                pos.Y >= 0 && pos.Y < BackgroundImage.Height)
            {
                e.Handled = activeTool.MouseDown(pos);
            }
        }

        private void Content_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!HasActiveTool)
                return;

            var pos = e.GetPosition(background);
            if (pos.X >= 0 && pos.X < BackgroundImage.Width &&
                pos.Y >= 0 && pos.Y < BackgroundImage.Height)
            {
                e.Handled = activeTool.MouseUp(pos);
            }
        }

        private void Content_MouseMove(object sender, MouseEventArgs e)
        {
            if (!HasActiveTool)
                return;

            var pos = e.GetPosition(background);
            if (pos.X >= 0 && pos.X < BackgroundImage.Width &&
                pos.Y >= 0 && pos.Y < BackgroundImage.Height)
            {
                e.Handled = activeTool.MouseMove(pos);
            }
        }

        private void ScaleDelta(int delta)
        {
            double coeff = Math.Pow(1.25, delta / 120);
            Scale = Math.Min(10, Math.Max(0.1, coeff * Scale));
            background.LayoutTransform = new ScaleTransform() { ScaleX = scale, ScaleY = scale };
        }

        public double Scale
        {
            get { return scale; }
            private set
            {
                scale = value;
                var scaleTransform = new ScaleTransform { ScaleX = scale, ScaleY = scale };
                background.LayoutTransform = scaleTransform;
                wallsPath.LayoutTransform = scaleTransform;
                wallDrawTool.LayoutTransform = scaleTransform;
                roomDefineTool.LayoutTransform = scaleTransform;
                WallThickness = 3 / scale;

                if (ScaleChanged != null)
                {
                    ScaleChanged(this, EventArgs.Empty);
                }
            }
        }
        private double scale = 1;

        public event EventHandler ScaleChanged;

        public IList<Wall> Walls
        {
            get { return walls; }
        }
        private readonly ObservableCollection<Wall> walls = new ObservableCollection<Wall>();

        void walls_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateWallsPath();
        }

        public static Geometry BuildWallsGeometry(IEnumerable<Wall> walls)
        {
            var sb = new StringBuilder();
            foreach (var wall in walls)
            {
                sb.AppendFormat(CultureInfo.InvariantCulture, " M {0},{1} L {2},{3}",
                    wall.Start.X, wall.Start.Y, wall.End.X, wall.End.Y);
            }
            return Geometry.Parse(sb.ToString());
        }

        private void UpdateWallsPath()
        {
            WallsPathData = BuildWallsGeometry(Walls);
        }

        private IEditorTool activeTool;
    }
}
