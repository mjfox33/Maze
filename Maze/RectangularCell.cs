using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class RectangularCell : Cell<RectangularCell>
    {
        public RectangularCell(int row, int column) : base(row, column)
        {
            Links = new HashSet<Cell<RectangularCell>>();
        }

        public RectangularCell North;
        public RectangularCell South;
        public RectangularCell East;
        public RectangularCell West;

        public override RectangularCell RightNeighbor => East;

        public override RectangularCell TopNeighbor => North;

        public override IEnumerable<RectangularCell> Neighbors
        {
            get
            {
                if (North != null) yield return North;
                if (South != null) yield return South;
                if (East != null) yield return East;
                if (West != null) yield return West;
            }
        }

        public override HashSet<Cell<RectangularCell>> Links { get; } 
    }
}
