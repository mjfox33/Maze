using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class TriangleCell : Cell<TriangleCell>
    {
        public TriangleCell(int row, int column) : base(row, column)
        {
            Links = new HashSet<Cell<TriangleCell>>();
        }

        public bool IsUpright => (Row + Column) % 2 == 0;

        public TriangleCell North;
        public TriangleCell South;
        public TriangleCell East;
        public TriangleCell West;

        public override HashSet<Cell<TriangleCell>> Links { get; }
        public override TriangleCell RightNeighbor { get; }
        public override TriangleCell TopNeighbor { get; }

        public override IEnumerable<TriangleCell> Neighbors
        {
            get
            {
                if (West != null) yield return West;
                if (East != null) yield return East;
                if (!IsUpright && North != null) yield return North;
                if (IsUpright && South != null) yield return South;
            }
        }
    }
}
