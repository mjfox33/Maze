using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Maze
{
    public abstract class Grid<T>
    {
        public int Rows { get; }
        public int Columns { get; }
        public virtual int Size => Rows * Columns;
        public abstract int LinkedCells { get; }

        public abstract IEnumerable<T> DeadEnds { get; }

        public abstract List<List<T>> Cells { get; }

        protected readonly Random RandomNumberGenerator = new Random();

        public virtual Distances<T> Distances { get; set; }

        protected Grid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }
        

        public abstract T this[int row, int col] { get; }

        public virtual T RandomCell
        {
            get
            {
                var row = RandomNumberGenerator.Next(Rows);
                var col = RandomNumberGenerator.Next(Columns);
                return this[row, col];
            }
        }

        public abstract IEnumerable<T> EachCell { get; }

        public abstract IEnumerable<IEnumerable<T>> EachRow { get; }

        public string ContentsOfCell(Cell<T> cell)
        {
            return Distances != null && Distances[cell] >= 0 ? Distances[cell].ToString("x3") : "   ";
        }

        public virtual Color? BackgroundColorForCell(Cell<T> cell)
        {
            return null;
        }

        public abstract Image ToImage(int cellSize = 1, bool useBackgrounds = false);



    }
}

