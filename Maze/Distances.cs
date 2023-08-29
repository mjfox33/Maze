using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class Distances<T>

    {
        public Cell<T> Root { get; }
        public Dictionary<Cell<T>, int> CellDistance { get; }

        public IEnumerable<Cell<T>> Cells => CellDistance.Keys;

        public Distances(Cell<T> root)
        {
            Root = root;
            CellDistance = new Dictionary<Cell<T>, int>
            {
                [root] = 0
            };
        }

        public int this[Cell<T> cell] => CellDistance.ContainsKey(cell) ? CellDistance[cell] : -1;

        public void SetDistance(Cell<T> cell, int distance)
        {
            CellDistance[cell] = distance;
        }

        public Distances<T> PathToGoal(Cell<T> goal)
        {
            var current = goal;
            var breadcrumbs = new Distances<T>(Root);
            breadcrumbs.SetDistance(current, CellDistance[current]);

            while (current != Root)
            {
                foreach (var neighbor in current.Links)
                {
                    if (CellDistance[neighbor] >= CellDistance[current]) continue;
                    breadcrumbs.SetDistance(neighbor, CellDistance[neighbor]);
                    current = neighbor;
                }
            }

            return breadcrumbs;
        }

        public Tuple<Cell<T>, int> MaxCellAndDistance
        {
            get
            {
                var maxDistanceCell = CellDistance.Aggregate((x, y) => x.Value > y.Value ? x : y);
                return new Tuple<Cell<T>, int>(maxDistanceCell.Key, maxDistanceCell.Value);
            }
        }
    }
}
