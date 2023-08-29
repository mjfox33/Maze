using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class MaskedGrid : RectangularGrid
    {
        public Mask Mask { get; }

        public MaskedGrid(Mask mask) : base(mask.Rows, mask.Columns)
        {
            Mask = mask;
            PrepareMaskedGrid();
            ConfigureCells();
        }

        private void PrepareMaskedGrid()
        {
            var result = new Cell<RectangularCell>[Rows, Columns];
            for (var row = 0; row < Rows; row++)
            {
                for (var col = 0; col < Columns; col++)
                {
                    if (!Mask[row, col])
                        Cells[row][col] = null;
                }
            }
        }

        public override RectangularCell RandomCell
        {
            get
            {
                var cell = Mask.RandomValidRowAndColumn;
                return this[cell.Item1, cell.Item2];
            }
        }

        public override int Size => Mask.Count;
    }
}
