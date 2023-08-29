using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class PolarCell : Cell<PolarCell>
    {
        public PolarCell Clockwise { get; set; }
        public PolarCell CounterClockwise { get; set; }
        public PolarCell Inward { get; set; }
        public List<PolarCell> Outward { get; }

        public PolarCell(int row, int column) : base(row, column)
        {
            Outward = new List<PolarCell>();
            Links = new HashSet<Cell<PolarCell>>();
        }

        public override PolarCell RightNeighbor => Clockwise;

        public override PolarCell TopNeighbor => Outward.FirstOrDefault();

        public override HashSet<Cell<PolarCell>> Links { get; }

        public override IEnumerable<PolarCell> Neighbors
        {
            get
            {
                if (Clockwise != null) yield return Clockwise;
                if (CounterClockwise != null) yield return CounterClockwise;
                if (Inward != null) yield return Inward;
                if (Outward != null)
                {
                    foreach (var cell in Outward)
                        yield return cell;
                };
            }
        }
    }
}
