using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FlowFreeSolverWpf.Model;
using Path = System.Windows.Shapes.Path;

namespace FlowFreeSolverWpf
{
    public class CellClickedEventArgs : EventArgs
    {
        public CellClickedEventArgs(Coords coords)
        {
            Coords = coords;
        }

        public Coords Coords { get; private set; }
    }

    public partial class BoardControl
    {
        private readonly Color _gridLineColour = Colors.Yellow;
        private const double GridLineThickness = 1;
        private const double GridLineHalfThickness = GridLineThickness / 2;
        private readonly IDictionary<Coords, Tuple<string, Ellipse>> _coordsToTagsAndDots = new Dictionary<Coords, Tuple<string, Ellipse>>();
        private readonly IList<Path> _paths = new List<Path>();
        private readonly IList<Rectangle> _highlightRectangles = new List<Rectangle>();

        public BoardControl()
        {
            InitializeComponent();
            BoardCanvas.MouseLeftButtonDown += BoardCanvasOnMouseLeftButtonDown;
        }

        public event EventHandler<CellClickedEventArgs> CellClicked;

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

        private void RaiseCellClicked(Coords coords)
        {
            var handler = CellClicked;

            if (handler != null)
            {
                handler(this, new CellClickedEventArgs(coords));
            }
        }

        public int GridSize { get; set; }

        public IList<ColourPair> GetColourPairs()
        {
            var dotsGroupedByTag = _coordsToTagsAndDots.GroupBy(kvp => kvp.Value.Item1,
                                                                kvp => new {Coords = kvp.Key})
                                                       .ToList();

            if (dotsGroupedByTag.Any(x => x.Count() != 2))
            {
                throw new InvalidOperationException("Dots must be in pairs!");
            }

            //if (!dotsGroupedByTag.Any())
            //{
            //    throw new InvalidOperationException("No pairs of dots!");
            //}

            return dotsGroupedByTag.Select(x =>
                {
                    var startCoords = x.ElementAt(0).Coords;
                    var endCoords = x.ElementAt(1).Coords;
                    var tag = x.Key;
                    return new ColourPair(startCoords, endCoords, tag);
                }).ToList();
        }

        public void DrawGrid()
        {
            DrawGridLines();
        }

        public void Clear()
        {
            ClearDotsAndPaths();
            BoardCanvas.Children.Clear();
        }

        public void ClearDotsAndPaths()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            _coordsToTagsAndDots.Select(kvp =>
                {
                    var ellipse = kvp.Value.Item2;
                    BoardCanvas.Children.Remove(ellipse);
                    return 0;
                }).ToList();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            _paths.Select(path =>
                {
                    BoardCanvas.Children.Remove(path);
                    return 0;
                }).ToList();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            _highlightRectangles.Select(path =>
            {
                BoardCanvas.Children.Remove(path);
                return 0;
            }).ToList();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed

            _coordsToTagsAndDots.Clear();
            _paths.Clear();
            _highlightRectangles.Clear();
        }

        public void AddDot(Coords coords, string tag)
        {
            if (IsDotAt(coords))
            {
                return;
            }

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

            var cellRect = new Rect(coords.X * sw + GridLineHalfThickness, (GridSize - coords.Y - 1) * sh + GridLineHalfThickness, sw, sh);
            cellRect.Inflate(-sw/8, -sh/8);

            Canvas.SetLeft(dot, cellRect.Left);
            Canvas.SetTop(dot, cellRect.Top);

            BoardCanvas.Children.Add(dot);
            _coordsToTagsAndDots.Add(coords, Tuple.Create(tag, dot));
        }

        public void RemoveDot(Coords coords)
        {
            if (_coordsToTagsAndDots.ContainsKey(coords))
            {
                BoardCanvas.Children.Remove(_coordsToTagsAndDots[coords].Item2);
                _coordsToTagsAndDots.Remove(coords);
            }
        }

        public bool IsDotAt(Coords coords)
        {
            return _coordsToTagsAndDots.ContainsKey(coords);
        }

        public void DrawPath(ColourPair colourPair, Model.Path path)
        {
            var aw = ActualWidth;
            var ah = ActualHeight;
            var sw = (aw - GridLineThickness) / GridSize;
            var sh = (ah - GridLineThickness) / GridSize;

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
                Stroke = new SolidColorBrush(MapTagToColour(colourPair.Tag)),
                StrokeThickness = sw / 4,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeLineJoin = PenLineJoin.Round,
                Data = pathGeometry
            };

            BoardCanvas.Children.Add(polyLinePath);
            _paths.Add(polyLinePath);

            var fillColour = MapTagToColour(colourPair.Tag);

            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (var coords in path.CoordsList)
            {
                var cellRect = new Rect(coords.X * sw + GridLineHalfThickness, (GridSize - coords.Y - 1) * sh + GridLineHalfThickness, sw, sh);
                var highlightRectangle = new Rectangle
                    {
                        Width = cellRect.Width,
                        Height = cellRect.Height,
                        Fill = new SolidColorBrush(Color.FromArgb(0x60, fillColour.R, fillColour.G, fillColour.B))
                    };
                Canvas.SetLeft(highlightRectangle, cellRect.Left);
                Canvas.SetTop(highlightRectangle, cellRect.Top);
                BoardCanvas.Children.Add(highlightRectangle);
                _highlightRectangles.Add(highlightRectangle);
            }
            // ReSharper restore LoopCanBeConvertedToQuery
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
            return ColourMap.MapTagToColour(tag);
        }
    }
}
