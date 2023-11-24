using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Maze
{
    public abstract class Grid<T> where T : Cell<T>
    {
        public int Rows { get; }
        public int Columns { get; }
        public virtual int Size => Rows * Columns;
        public int LinkedCells => EachCell.Count(x => x != null && x.Links.Any());
        public IEnumerable<T> DeadEnds => EachCell.Where(x => x != null && x.Links.Count == 1);
        public List<List<T>> Cells { get; }
        public IEnumerable<T> EachCell => Cells.SelectMany(x => x.ToArray());
        public IEnumerable<IEnumerable<T>> EachRow => Cells;

        public string ContentsOfCell(Cell<T> cell)
        {
            return Distances != null && Distances[cell] >= 0 ? Distances[cell].ToString("x3") : "   ";
        }

        private Distances<T> _distances;
        public Distances<T> Distances
        {
            get => _distances;
            set
            {
                var maxCellAndDistance = value.MaxCellAndDistance;
                _maximumDistance = maxCellAndDistance.Item2;
                _distances = value;
            }
        }

        private int _maximumDistance;

        public Color? BackgroundColorForCell(Cell<T> cell)
        {
            var distance = Distances[cell];
            var intensity = Convert.ToDouble(_maximumDistance - distance) / _maximumDistance;
            var dark = (int)Math.Ceiling(255 * intensity);
            var light = (int)Math.Ceiling(128 + 127 * intensity);
            return Color.FromArgb(dark, dark, light);
        }


        protected readonly Random RandomNumberGenerator = new Random();

        protected Grid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Cells = new List<List<T>>();
        }

        public virtual T this[int row, int col] 
        {
            get
            {
                if (row < 0 || row > Rows - 1) return null;
                if (col < 0 || col > Columns - 1) return null;
                return Cells[row][col];
            }
        }

        public virtual T RandomCell
        {
            get
            {
                var row = RandomNumberGenerator.Next(Rows);
                var col = RandomNumberGenerator.Next(Columns);
                return this[row, col];
            }
        }

        public abstract Image ToImage(int cellSize = 10, CellBorderWidth cellBorderWidth = CellBorderWidth.Normal, bool useBackgrounds = false);

        public enum CellBorderWidth
        {
            None = 0,
            Thin = 1 << 0,
            Normal = 1 << 1,
            Thick = 1 << 2,
        }
    }
}

