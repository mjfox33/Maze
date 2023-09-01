using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class HexCell : Cell<HexCell>
    {
        public HexCell(int row, int column) : base(row, column)
        {
            Links = new HashSet<Cell<HexCell>>();
        }

        public HexCell NorthWest;
        public HexCell North;
        public HexCell NorthEast;
        public HexCell SouthWest;
        public HexCell South;
        public HexCell SouthEast;

        public override HexCell RightNeighbor => SouthEast;
        public override HexCell TopNeighbor => North;

        public override IEnumerable<HexCell> Neighbors
        {
            get
            {
                if (NorthWest != null) { yield return NorthWest; }
                if (North != null) { yield return North; }
                if (NorthEast != null) { yield return NorthEast; }
                if (SouthWest != null) { yield return SouthWest; }
                if (South != null) { yield return South; }
                if (SouthEast != null) { yield return SouthEast; }
            }
        }

        public override HashSet<Cell<HexCell>> Links { get; }
    }
}
