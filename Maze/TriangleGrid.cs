using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class TriangleGrid : Grid<TriangleCell>
    {
        public TriangleGrid(int rows, int columns) : base(rows, columns)
        {
            PrepareGrid();
            ConfigureCells();
        }

        private void PrepareGrid()
        {
            for (var row = 0; row < Rows; row++)
            {
                var temp = new List<TriangleCell>();
                for (var col = 0; col < Columns; col++)
                {
                    temp.Add(new TriangleCell(row, col));
                }
                Cells.Add(temp);
            }
        }

        private void ConfigureCells()
        {
            foreach (var cell in Cells.SelectMany(x => x.ToArray()))
            {
                if (cell == null) continue;
                var row = cell.Row;
                var col = cell.Column;
                cell.West = this[row, col - 1];
                cell.East = this[row, col + 1];

                if (cell.IsUpright)
                {
                    cell.South = this[row + 1, col];
                }
                else
                {
                    cell.North = this[row - 1, col];
                }
            }
        }

        public override Image ToImage(int cellSize = 10, CellBorderWidth cellBorderWidth = CellBorderWidth.Normal, bool useBackgrounds = false)
        {
            var halfWidth = cellSize / 2.0;
            var height = cellSize * Math.Sqrt(3) / 2.0;
            var halfHeight = height / 2;
            
            var imageWidth = Convert.ToInt32(cellSize * (Columns + 1) / 2);
            var imageHeight = Convert.ToInt32(height * Rows);

            var img = new Bitmap(imageWidth + 1, imageHeight + 1);

            using (var graph = Graphics.FromImage(img))
            {
                var imageSize = new Rectangle(0, 0, imageWidth + 1, imageHeight + 1);
                graph.FillRectangle(Brushes.White, imageSize);

                var penSize = cellSize / 10;
                var pen = new Pen(Color.Black, penSize);
                foreach (var cell in EachCell)
                {
                    var xCenter = halfWidth + cell.Column * halfWidth;
                    var yCenter = halfHeight + cell.Row * height;

                    var xWest = Convert.ToInt32(xCenter - halfWidth);
                    var xMiddle = Convert.ToInt32(xCenter);
                    var xEast = Convert.ToInt32(xCenter + halfWidth);

                    var yApex = cell.IsUpright ? Convert.ToInt32(yCenter - halfHeight) : Convert.ToInt32(yCenter + halfHeight);
                    var yBase = cell.IsUpright ? Convert.ToInt32(yCenter + halfHeight) : Convert.ToInt32(yCenter - halfHeight);

                    if (useBackgrounds)
                    {
                        var points = new Point[]
                        {
                            new Point(xWest, yBase),
                            new Point(xMiddle, yApex),
                            new Point(xEast, yBase),
                        };

                        var color = BackgroundColorForCell(cell);
                        if (!color.HasValue) continue;
                        var brush = new SolidBrush(color.Value);
                        graph.FillPolygon(brush, points);
                    }

                    if (cell.West == null)
                    {
                        graph.DrawLine(pen, xWest, yBase, xMiddle, yApex);
                    }

                    if (!cell.IsLinked(cell.East))
                    {
                        graph.DrawLine(pen, xEast, yBase, xMiddle, yApex);
                    }

                    var noSouth = cell.IsUpright && cell.South == null;
                    var notLinked = !cell.IsUpright && !cell.IsLinked(cell.North);

                    if (noSouth || notLinked)
                    {
                        graph.DrawLine(pen, xEast, yBase, xWest, yBase);
                    }

                }
            }

            return img;
        }
    }
}
