using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public static class Algorithm
    {
        private static readonly Random RandomNumberGenerator = new Random();

        #region Biased Algorithms
        public static void BinaryTree(RectangularGrid grid)
        {
            foreach (var cell in grid.EachCell)
            {
                if (cell == null) continue;

                var neighbors = new List<RectangularCell>();
                if (cell.North != null)
                {
                    neighbors.Add(cell.North);
                }

                if (cell.East != null)
                {
                    neighbors.Add(cell.East);
                }

                if (neighbors.Count <= 0) continue;
                var index = RandomNumberGenerator.Next(neighbors.Count);
                var neighbor = neighbors[index];
                cell.Link(neighbor);
            }
        }

        public static void Sidewinder(RectangularGrid grid)
        {
            foreach (var row in grid.EachRow)
            {
                var run = new List<RectangularCell>();
                foreach (var cell in row)
                {
                    run.Add(cell);
                    var isAtEastBoundary = cell.East == null;
                    var isAtNorthBoundary = cell.North == null;
                    var shouldClose = isAtEastBoundary || (!isAtNorthBoundary && RandomNumberGenerator.Next(2) == 0);
                    if (shouldClose)
                    {
                        var sample = RandomNumberGenerator.Next(run.Count);
                        var member = run[sample];
                        if (member.North != null)
                        {
                            member.Link(member.North);
                        }

                        run.Clear();
                    }
                    else
                    {
                        cell.Link(cell.East);
                    }
                }
            }
        }
        #endregion

        #region Unbiased Algorithms

        public static void AldousBroder(RectangularGrid grid, RectangularCell startCell = null)
        {
            var cell = startCell ?? grid.RandomCell;
            var unvisited = grid.Size - 1;
            while (unvisited > 0)
            {
                var index = RandomNumberGenerator.Next(cell.Neighbors.Count());
                var neighbor = cell.Neighbors.ElementAt(index);
                
                if (neighbor.Links.Count == 0)
                {
                    cell.Link(neighbor);
                    unvisited--;
                }

                cell = neighbor;
            }
        }

        public static void Wilson(RectangularGrid grid)
        {
            var unvisited = new HashSet<RectangularCell>(grid.EachCell);
            var first = unvisited.ElementAt(RandomNumberGenerator.Next(unvisited.Count));
            unvisited.Remove(first);

            while (unvisited.Any())
            {
                var cell = unvisited.ElementAt(RandomNumberGenerator.Next(unvisited.Count));
                var path = new List<RectangularCell> { cell };

                while (unvisited.Contains(cell))
                {
                    cell = cell.Neighbors.ElementAt(RandomNumberGenerator.Next(cell.Neighbors.Count()));
                    var position = path.IndexOf(cell);
                    if (position == -1)
                    {
                        path.Add(cell);
                    }
                    else
                    {
                        path = path.GetRange(0, position + 1);
                    }
                }

                for (var i = 0; i < path.Count - 1; i++)
                {
                    path[i].Link(path[i+1]);
                    unvisited.Remove(path[i]);
                }

            }

        }

        public static void HuntAndKill(RectangularGrid grid, RectangularCell startCell = null)
        {
            var current =startCell ?? grid.RandomCell;
            
            while (current != null)
            {
                var unvisitedNeighbors = current.Neighbors.Where(x => !x.Links.Any()).ToArray();
                RectangularCell neighbor;

                if (unvisitedNeighbors.Any())
                {
                    neighbor = unvisitedNeighbors.ElementAt(RandomNumberGenerator.Next(unvisitedNeighbors.Length));
                    current.Link(neighbor);
                    current = neighbor;
                }
                else
                {
                    current = null;

                    foreach (var cell in grid.EachCell)
                    {
                        var visitedNeighbors = cell.Neighbors.Where(x => x.Links.Any()).ToArray();

                        if (cell.Links.Any() || !visitedNeighbors.Any()) continue;

                        current = cell;
                        neighbor = visitedNeighbors.ElementAt(RandomNumberGenerator.Next(visitedNeighbors.Length));
                        current.Link(neighbor);
                        break;
                    }
                }
            }
        }

        public static void RecursiveBacktracker(RectangularGrid grid, RectangularCell startCell = null)
        {
            var currentCell = startCell ?? grid.RandomCell;
            var stack = new Stack<RectangularCell>();
            stack.Push(currentCell);

            while (stack.Any())
            {
                currentCell = stack.Peek();
                var neighbors = currentCell.Neighbors.Where(x => !x.Links.Any()).ToArray();
                if (neighbors.Any())
                {
                    var neighbor = neighbors.ElementAt(RandomNumberGenerator.Next(neighbors.Length));
                    currentCell.Link(neighbor);
                    stack.Push(neighbor);
                }
                else
                {
                    stack.Pop();
                }
            }
        }

        public static void RecursiveBacktracker(PolarGrid grid, PolarCell startCell = null)
        {
            var currentCell = startCell ?? grid.RandomCell;
            var stack = new Stack<PolarCell>();
            stack.Push(currentCell);

            while (stack.Any())
            {
                currentCell = stack.Peek();
                var neighbors = currentCell.Neighbors.Where(x => !x.Links.Any()).ToArray();
                if (neighbors.Any())
                {
                    var neighbor = neighbors.ElementAt(RandomNumberGenerator.Next(neighbors.Length));
                    currentCell.Link(neighbor);
                    stack.Push(neighbor);
                }
                else
                {
                    stack.Pop();
                }
            }
        }
        #endregion

    }
}
