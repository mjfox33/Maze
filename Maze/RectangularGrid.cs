using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class RectangularGrid : Grid<RectangularCell>
    {
        public RectangularGrid(int rows, int columns) : base(rows, columns)
        {
            PrepareGrid();
            ConfigureCells();
        }

        private void PrepareGrid()
        {
            for (var row = 0; row < Rows; row++)
            {
                var temp = new List<RectangularCell>();
                for (var col = 0; col < Columns; col++)
                {
                    temp.Add(new RectangularCell(row, col));
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
                cell.North = this[row - 1, col];
                cell.South = this[row + 1, col];
                cell.West = this[row, col - 1];
                cell.East = this[row, col + 1];
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("+");
            for (var i = 0; i < Columns; i++)
            {
                sb.Append("---+");
            }

            sb.Append("\n");

            foreach (var row in EachRow)
            {
                var top = "|";
                var bottom = "+";
                foreach (var cell in row)
                {
                    top += ContentsOfCell(cell);
                    var east = cell != null && cell.IsLinked(cell.East) ? " " : "|";
                    top += east;

                    var south = cell != null && cell.IsLinked(cell.South) ? "   " : "---";
                    bottom += south;
                    bottom += "+";
                }
                sb.AppendLine(top);
                sb.AppendLine(bottom);
            }

            return sb.ToString();
        }

        public override Image ToImage(int cellSize = 10, CellBorderWidth cellBorderWidth = CellBorderWidth.Normal, bool useBackgrounds = false)
        {
            var width = cellSize * Columns;
            var height = cellSize * Rows;
            var img = new Bitmap(width + 1, height + 1);
            using (var graph = Graphics.FromImage(img))
            {
                var imageSize = new Rectangle(0, 0, width + 1, height + 1);
                graph.FillRectangle(Brushes.White, imageSize);

                if (useBackgrounds)
                {
                    foreach (var cell in EachCell)
                    {
                        if (cell == null) continue;
                        var x1 = cell.Column * cellSize;
                        var y1 = cell.Row * cellSize;
                        var x2 = (cell.Column + 1) * cellSize;
                        var y2 = (cell.Row + 1) * cellSize;
                        var color = BackgroundColorForCell(cell);
                        if (!color.HasValue) continue;
                        var brush = new SolidBrush(color.Value);
                        graph.FillRectangle(brush, x1, y1, x2 - x1, y2 - y1);
                    }
                }


                var penWidth = cellSize / 10;
                var pen = new Pen(Color.Black, penWidth);
                foreach (var cell in EachCell)
                {
                    if (cell == null) continue;
                    var x1 = cell.Column * cellSize;
                    var y1 = cell.Row * cellSize;
                    var x2 = (cell.Column + 1) * cellSize;
                    var y2 = (cell.Row + 1) * cellSize;


                    if (cell.North == null)
                    {
                        graph.DrawLine(pen, x1, y1, x2, y1);
                    }

                    if (cell.West == null)
                    {
                        graph.DrawLine(pen, x1, y1, x1, y2);
                    }

                    if (!cell.IsLinked(cell.East))
                    {
                        graph.DrawLine(pen, x2, y1, x2, y2);
                    }

                    if (!cell.IsLinked(cell.South))
                    {
                        graph.DrawLine(pen, x1, y2, x2, y2);
                    }

                }
            }

            return img;
        }


    }
}
