using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public abstract class Cell<T>
    {
        protected Cell(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; }
        public int Column { get; }
        public abstract HashSet<Cell<T>> Links { get; }
        
        public abstract T RightNeighbor { get; }
        public abstract T TopNeighbor { get; }
        public abstract IEnumerable<T> Neighbors { get; }

        public bool IsLinked(Cell<T> cell)
        {
            return Links.Contains(cell);
        }

        public Distances<T> Distances
        {
            get
            {
                var result = new Distances<T>(this);
                var frontier = new List<Cell<T>>();
                frontier.Add(this);
                while (frontier.Count > 0)
                {
                    var localFrontier = new List<Cell<T>>();
                    foreach (var cell in frontier)
                    {
                        foreach (var linkedCell in cell.Links)
                        {
                            if (result[linkedCell] >= 0) continue;
                            result.SetDistance(linkedCell, result[cell] + 1);
                            localFrontier.Add(linkedCell);
                        }
                    }

                    frontier = localFrontier;
                }
                return result;
            }
        }

        public virtual void Link(Cell<T> cell, bool bidirectionally = true)
        {
            Links.Add(cell);
            if (bidirectionally) cell.Link(this, false);
        }

        public virtual void Unlink(Cell<T> cell, bool bidirectionally = true)
        {
            Links.Remove(cell);
            if (bidirectionally) cell.Unlink(this, false);
        }
    }
}
