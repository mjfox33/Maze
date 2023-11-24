using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class HexGrid : Grid<HexCell>
    {
        public HexGrid(int rows, int columns) : base(rows, columns)
        {
            PrepareGrid();
            ConfigureCells();
        }

        private void PrepareGrid()
        {
            for (var row = 0; row < Rows; row++)
            {
                var temp = new List<HexCell>();
                for (var col = 0; col < Columns; col++)
                {
                    temp.Add(new HexCell(row, col));
                }
                Cells.Add(temp);
            }
        }

        protected void ConfigureCells()
        {
            foreach (var cell in Cells.SelectMany(x => x.ToArray()))
            {
                if (cell == null) continue;
                var row = cell.Row;
                var col = cell.Column;

                var northDiagonal = col % 2 == 0 ? row - 1 : row;
                var southDiagonal = northDiagonal + 1;

                cell.NorthWest = this[northDiagonal, col - 1];
                cell.North = this[row - 1, col];
                cell.NorthEast = this[northDiagonal, col + 1];
                cell.SouthWest = this[southDiagonal, col - 1];
                cell.South = this[row + 1, col];
                cell.SouthEast = this[southDiagonal, col + 1];
            }
        }

        public override Image ToImage(int cellSize = 10, CellBorderWidth cellBorderWidth = CellBorderWidth.Normal, bool useBackgrounds = false)
        {
            var aSize = cellSize / 2.0;
            var bSize = cellSize * Math.Sqrt(3) / 2.0;
            var width = cellSize * 2;
            var height = bSize * 2;

            var imageWidth = Convert.ToInt32(3 * aSize * Columns + aSize + 0.5);
            var imageHeight = Convert.ToInt32(height * Rows + bSize + 0.5);

            var img = new Bitmap(imageWidth + 1, imageHeight + 1);
            
            using (var graph = Graphics.FromImage(img))
            {
                var imageSize = new Rectangle(0, 0, imageWidth + 1, imageHeight + 1);
                graph.FillRectangle(Brushes.White, imageSize);

                var penSize = cellSize / 10;
                var pen = new Pen(Color.Black, penSize);
                foreach (var cell in EachCell)
                {
                    if (cell == null) continue;
                    var xCenter = cellSize + 3 * cell.Column * aSize;
                    var yCenter = bSize + cell.Row * height;
                    if (cell.Column % 2 == 1)
                    {
                        yCenter += bSize;
                    }

                    var xFarWest = Convert.ToInt32(xCenter - cellSize);
                    var xNearWest = Convert.ToInt32(xCenter - aSize);
                    var xNearEast = Convert.ToInt32(xCenter + aSize);
                    var xFarEast = Convert.ToInt32(xCenter + cellSize);

                    var yNorth = Convert.ToInt32(yCenter - bSize);
                    var yMiddle = Convert.ToInt32(yCenter);
                    var ySouth = Convert.ToInt32(yCenter + bSize);

                    if (useBackgrounds)
                    {
                        var points = new Point[]
                        {
                            new Point(xFarWest, yMiddle),
                            new Point(xNearWest, yNorth),
                            new Point(xNearEast, yNorth),
                            new Point(xFarEast, yMiddle),
                            new Point(xNearEast, ySouth),
                            new Point(xNearWest, ySouth),
                        };

                        var color = BackgroundColorForCell(cell);
                        if (!color.HasValue) continue;
                        var brush = new SolidBrush(color.Value);
                        graph.FillPolygon(brush, points);
                    }

                    if (cell.SouthWest == null)
                    {
                        graph.DrawLine(pen, xFarWest, yMiddle, xNearWest, ySouth);
                    }

                    if (cell.NorthWest == null)
                    {
                        graph.DrawLine(pen, xFarWest, yMiddle, xNearWest, yNorth);
                    }

                    if (cell.North == null)
                    {
                        graph.DrawLine(pen, xNearWest, yNorth, xNearEast, yNorth);
                    }

                    if (!cell.IsLinked(cell.NorthEast))
                    {
                        graph.DrawLine(pen, xNearEast, yNorth, xFarEast, yMiddle);
                    }

                    if (!cell.IsLinked(cell.SouthEast))
                    {
                        graph.DrawLine(pen, xFarEast, yMiddle, xNearEast, ySouth);
                    }

                    if (!cell.IsLinked(cell.South))
                    {
                        graph.DrawLine(pen, xNearEast, ySouth, xNearWest, ySouth);
                    }

                }
            }
            
            return img;
        }

    }
}
