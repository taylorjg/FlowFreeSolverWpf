using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FlowFreeSolverWpf.Model;
using FlowFreeSolverWpf.ViewModel;
using Path = System.Windows.Shapes.Path;

namespace FlowFreeSolverWpf.View
{
    public partial class BoardControl : IBoardControl
    {
        private int _gridSize;
        private readonly Color _gridLineColour = Colors.Yellow;
        private const double GridLineThickness = 0.8;
        private const double GridLineHalfThickness = GridLineThickness / 2;

        private enum TagType
        {
            GridLine,
            HighlightRectangle,
            Path,
            Dot
        }

        private readonly IDictionary<Coords, Ellipse> _coordsToDots = new Dictionary<Coords, Ellipse>();

        public BoardControl()
        {
            InitializeComponent();
            BoardCanvas.MouseLeftButtonDown += BoardCanvasOnMouseLeftButtonDown;
        }

        public event EventHandler<CellClickedEventArgs> CellClicked;

        private void RaiseCellClicked(Coords coords)
        {
            var handler = CellClicked;

            if (handler != null)
            {
                handler(this, new CellClickedEventArgs(coords));
            }
        }

        public int GridSize
        {
            get
            {
                return _gridSize;
            }
            set
            {
                _gridSize = value;
                ClearAll();
                DrawGrid();
            }
        }

        public void ClearAll()
        {
            ClearDots();
            ClearPaths();
            ClearGridLines();
        }

        public void ClearDots()
        {
            RemoveChildrenWithTagType(TagType.Dot);
            _coordsToDots.Clear();
        }

        public void ClearPaths()
        {
            RemoveChildrenWithTagType(TagType.Path);
            RemoveChildrenWithTagType(TagType.HighlightRectangle);
        }

        public void ClearGridLines()
        {
            RemoveChildrenWithTagType(TagType.GridLine);
        }

        public void DrawGrid()
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
                    Tag = TagType.GridLine,
                    SnapsToDevicePixels = true,
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
                    Tag = TagType.GridLine,
                    SnapsToDevicePixels = true,
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

        public void AddDot(Coords coords, DotColour dotColour)
        {
            var aw = ActualWidth;
            var ah = ActualHeight;
            var sw = (aw - GridLineThickness) / GridSize;
            var sh = (ah - GridLineThickness) / GridSize;

            var dot = new Ellipse
                {
                    Tag = TagType.Dot,
                    Width = sw * 6 / 8,
                    Height = sh * 6 / 8,
                    Fill = new SolidColorBrush(dotColour.ToWpfColour())
                };

            var cellRect = new Rect(coords.X * sw + GridLineHalfThickness, (GridSize - coords.Y - 1) * sh + GridLineHalfThickness, sw, sh);
            cellRect.Inflate(-sw/8, -sh/8);

            Canvas.SetLeft(dot, cellRect.Left);
            Canvas.SetTop(dot, cellRect.Top);

            BoardCanvas.Children.Add(dot);
            _coordsToDots.Add(coords, dot);
        }

        public void RemoveDot(Coords coords)
        {
            if (_coordsToDots.ContainsKey(coords))
            {
                BoardCanvas.Children.Remove(_coordsToDots[coords]);
                _coordsToDots.Remove(coords);
            }
        }

        public void DrawPath(ColourPair colourPair, Model.Path path)
        {
            var aw = ActualWidth;
            var ah = ActualHeight;
            var sw = (aw - GridLineThickness) / GridSize;
            var sh = (ah - GridLineThickness) / GridSize;

            var pathColour = colourPair.DotColour.ToWpfColour();

            var points = new List<Point>();

            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var coords in path.CoordsList)
            {
                var x = GridLineHalfThickness + (coords.X * sw) + (sw / 2);
                var y = GridLineHalfThickness + ((GridSize - coords.Y - 1) * sh) + (sh / 2);
                points.Add(new Point(x, y));
            }
            // ReSharper restore LoopCanBeConvertedToQuery

            var polyLineSegment = new PolyLineSegment(points, true);
            var pathFigure = new PathFigure { StartPoint = points.First() };
            pathFigure.Segments.Add(polyLineSegment);
            var pathGeometry = new PathGeometry();
            pathGeometry.Figures.Add(pathFigure);
            var polyLinePath = new Path
            {
                Tag = TagType.Path,
                Stroke = new SolidColorBrush(pathColour),
                StrokeThickness = sw / 3,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeLineJoin = PenLineJoin.Round,
                Data = pathGeometry
            };

            BoardCanvas.Children.Add(polyLinePath);

            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var coords in path.CoordsList)
            {
                var cellRect = new Rect(coords.X * sw + GridLineHalfThickness, (GridSize - coords.Y - 1) * sh + GridLineHalfThickness, sw, sh);
                var highlightRectangle = new Rectangle
                    {
                        Tag = TagType.HighlightRectangle,
                        Width = cellRect.Width,
                        Height = cellRect.Height,
                        Fill = new SolidColorBrush(Color.FromArgb(0x80, pathColour.R, pathColour.G, pathColour.B))
                    };
                Canvas.SetLeft(highlightRectangle, cellRect.Left);
                Canvas.SetTop(highlightRectangle, cellRect.Top);
                BoardCanvas.Children.Add(highlightRectangle);
            }
            // ReSharper restore LoopCanBeConvertedToQuery
        }

        private void RemoveChildrenWithTagType(TagType tagType)
        {
            var elementsToRemove = new List<UIElement>();

            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var element in BoardCanvas.Children)
            {
                var frameworkElement = element as FrameworkElement;
                if (frameworkElement != null)
                {
                    if (frameworkElement.Tag is TagType)
                    {
                        if ((TagType)frameworkElement.Tag == tagType)
                        {
                            elementsToRemove.Add(frameworkElement);
                        }
                    }
                }
            }
            // ReSharper restore LoopCanBeConvertedToQuery

            foreach (var element in elementsToRemove)
            {
                BoardCanvas.Children.Remove(element);
            }
        }

        private void BoardCanvasOnMouseLeftButtonDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var aw = ActualWidth;
            var ah = ActualHeight;
            var sw = (aw - GridLineThickness) / GridSize;
            var sh = (ah - GridLineThickness) / GridSize;

            var pt = mouseButtonEventArgs.MouseDevice.GetPosition(this);

            for (var x = 0; x < GridSize; x++)
            {
                for (var y = 0; y < GridSize; y++)
                {
                    var cellRect = new Rect(x * sw + GridLineHalfThickness, (GridSize - y - 1) * sh + GridLineHalfThickness, sw, sh);
                    if (cellRect.Contains(pt))
                    {
                        RaiseCellClicked(new Coords(x, y));
                        return;
                    }
                }
            }
        }
    }
}
