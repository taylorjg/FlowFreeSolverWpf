using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using FlowFreeSolverWpf.Model;

namespace FlowFreeSolverWpf
{
    public partial class BoardControl
    {
        private readonly Color _gridLineColour = Colors.Yellow;
        private const double GridLineThickness = 1;
        private const double GridLineHalfThickness = GridLineThickness / 2;

        public BoardControl()
        {
            InitializeComponent();
        }

        public int GridSize { get; set; }

        public void DrawGrid()
        {
            DrawGridLines();
        }

        public void Clear()
        {
            BoardCanvas.Children.Clear();
        }

        public void AddDot(Coords coords, string tag)
        {
            var aw = ActualWidth;
            var ah = ActualHeight;
            var sw = (aw - GridLineThickness) / GridSize;
            var sh = (ah - GridLineThickness) / GridSize;

            var dot = new Ellipse
                {
                    Width = sw * 6 / 8,
                    Height = sh * 6 / 8,
                    Fill = new SolidColorBrush(MapTagToColour(tag))
                };

            var rect = new Rect(coords.X * sw + GridLineHalfThickness, (GridSize - coords.Y - 1) * sh + GridLineHalfThickness, sw, sh);
            rect.Inflate(-sw/8, -sh/8);

            Canvas.SetLeft(dot, rect.Left);
            Canvas.SetTop(dot, rect.Top);

            BoardCanvas.Children.Add(dot);
        }

        private void DrawGridLines()
        {
            var aw = ActualWidth;
            var ah = ActualHeight;
            var sw = (aw - GridLineThickness) / GridSize;
            var sh = (ah - GridLineThickness) / GridSize;
            
            var gridLineBrush = new SolidColorBrush(_gridLineColour);
            
            // Horizontal grid lines
            for (var row = 0; row <= GridSize; row++)
            {
                var line = new Line
                    {
                        Stroke = gridLineBrush,
                        StrokeThickness = GridLineThickness,
                        X1 = 0,
                        Y1 = row * sh + GridLineHalfThickness,
                        X2 = aw,
                        Y2 = row * sh + GridLineHalfThickness
                    };
                BoardCanvas.Children.Add(line);
            }
            
            // Vertical grid lines
            for (var col = 0; col <= GridSize; col++)
            {
                var line = new Line
                {
                    Stroke = gridLineBrush,
                    StrokeThickness = GridLineThickness,
                    X1 = col * sw + GridLineHalfThickness,
                    Y1 = 0,
                    X2 = col * sw + GridLineHalfThickness,
                    Y2 = ah
                };
                BoardCanvas.Children.Add(line);
            }
        }

        private static Color MapTagToColour(string tag)
        {
            switch (tag)
            {
                case "A":
                    return Colors.Blue;

                case "B":
                    return Colors.Orange;

                case "C":
                    return Colors.Red;

                case "D":
                    return Colors.Green;

                case "E":
                    return Colors.Cyan;

                case "F":
                    return Colors.Yellow;

                default:
                    throw new InvalidOperationException(string.Format("Unknown tag, '{0}'.", tag));
            }
        }
    }
}
