using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class PolarGrid : Grid<PolarCell>
    {
        public override int LinkedCells => EachCell.Count(x => x != null && x.Links.Any());

        public override IEnumerable<PolarCell> DeadEnds => EachCell.Where(x => x != null && x.Links.Count == 1);

        public override List<List<PolarCell>> Cells { get; }

        public override IEnumerable<IEnumerable<PolarCell>> EachRow => Cells;

        public override IEnumerable<PolarCell> EachCell
        {
            get
            {
                return Cells.SelectMany(x => x.ToArray());
            }
        }

        public PolarGrid(int rows, int columns) : base (rows, columns)
        {
            Cells = PrepareGrid();
            ConfigureCells();
        }

        private List<List<PolarCell>> PrepareGrid()
        {
            var rows = new List<List<PolarCell>>();
            var rowHeight = 1.0 / Rows;
            rows.Add(new List<PolarCell>() { new PolarCell(0, 0) });

            for (var row = 1; row < Rows; row++)
            {
                var circumference = 2.0 * Math.PI * row / Rows;
                var previousCount = rows[row - 1].Count;
                var estimatedWidth = circumference / previousCount;
                var ratio = Convert.ToInt32(Math.Round(estimatedWidth / rowHeight, 0));
                var cells = previousCount * ratio;
                var thisRow = new List<PolarCell>();
                for (var i = 0; i < cells; i++)
                {
                    thisRow.Add(new PolarCell(row, i));
                }
                rows.Add(thisRow);
            }
            return rows;
        }

        private void ConfigureCells()
        {
            foreach (var cell in EachCell)
            {
                var row = cell.Row;
                var column = cell.Column;
                if (row == 0) continue; 
                cell.Clockwise = this[row, column + 1];
                cell.CounterClockwise = this[row, column - 1];
                var ratio = Cells[row].Count / Cells[row - 1].Count;
                var parent = Cells[row - 1][column / ratio];
                parent.Outward.Add(cell);
                cell.Inward = parent;
            }
        }

        public override PolarCell RandomCell
        {
            get
            {
                var row = RandomNumberGenerator.Next(Cells.Count);
                var column = RandomNumberGenerator.Next(Cells[row].Count);
                return Cells[row][column];
            }
        }

        public override PolarCell this[int row, int col]
        {
            get
            {
                if (row < 0 || row > Rows - 1) return null;
                var idx = col % Cells[row].Count;
                idx = idx < 0 ? idx + Cells[row].Count : idx;
                return Cells[row][idx];
            }
        }


        public override Image ToImage(int cellSize = 1, bool useBackgrounds = false)
        {
            var size = 2 * Rows * cellSize;
            var img = new Bitmap(size + 1, size + 1);
            using (var graph = Graphics.FromImage(img))
            {
                var imageSize = new Rectangle(0, 0, size + 1, size + 1);
                graph.FillRectangle(Brushes.White, imageSize);
                var center = size / 2;

                //todo: make pen width variable base on image size and cell size
                var pen = new Pen(Color.Black, 4);
                foreach (var cell in EachCell)
                {
                    if (cell == null || cell.Row == 0) continue;

                    var columns = Cells[cell.Row].Count;
                    var theta = 2 * Math.PI / columns;
                    var innerRadius = cell.Row * cellSize * 1.0;
                    var outerRadius = (cell.Row + 1.0) * cellSize;
                    var thetaCcw = cell.Column * theta;
                    var thetaCw = (cell.Column + 1.0) * theta;

                    var ax = center + Convert.ToInt32(innerRadius * Math.Cos(thetaCcw));
                    var ay = center + Convert.ToInt32(innerRadius * Math.Sin(thetaCcw));
                    var bx = center + Convert.ToInt32(outerRadius * Math.Cos(thetaCcw));
                    var by = center + Convert.ToInt32(outerRadius * Math.Sin(thetaCcw));

                    var cx = center + Convert.ToInt32(innerRadius * Math.Cos(thetaCw));
                    var cy = center + Convert.ToInt32(innerRadius * Math.Sin(thetaCw));
                    var dx = center + Convert.ToInt32(outerRadius * Math.Cos(thetaCw));
                    var dy = center + Convert.ToInt32(outerRadius * Math.Sin(thetaCw));

                    if (!cell.IsLinked(cell.Inward))
                    {
                        graph.DrawLine(pen, ax, ay, cx, cy);
                    }

                    if (!cell.IsLinked(cell.Clockwise))
                    {
                        graph.DrawLine(pen, cx, cy, dx, dy);
                    }
                }

                graph.DrawEllipse(pen, 0, 0, 2 * Columns * cellSize, 2 * Rows * cellSize);
            }

            return img;
        }
    }
}
