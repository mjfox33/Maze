using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public static class GridTranslator
    {
        [Flags]
        public enum GridType : short
        {
            None = 0,
            Rectangular = 1 << 0,
            Hexagon = 1 << 1,
            Triangular = 1 << 2,
            MaskedRectangular = 1 << 3,
            CircularPolar = 1 << 4,
        }

        [Flags]
        public enum RectangularCellWallMask : short
        {
            None = 0,
            North = 1 << 0,
            East = 1 << 1,
            South = 1 << 2,
            West = 1 << 3,
        }

        [Flags]
        public enum TriangularCellWallMask : short
        {
            None = 0,
            NorthOrSouth = 1 << 0, // cell orientation will determine if its north or south
            East = 1 << 1,
            West = 1 << 2,
        }

        [Flags]
        public enum HexagonCellWallMask : short
        {
            None = 0,
            North = 1 << 0,
            NorthEast = 1 << 1,
            SouthEast = 1 << 2,
            South = 1 << 3,
            SouthWest = 1 << 4,
            NorthWest = 1 << 5,
        }

        public struct GridDefinitionTypeAndWalls
        {
            public GridType TypeOfGrid;
            public int[,] GridCells;
        }

        public static GridDefinitionTypeAndWalls RectangularGridToIntArray(RectangularGrid grid)
        {
            var result = new int[grid.Rows, grid.Columns];
            foreach (var cell in grid.EachCell)
            {
                var current = RectangularCellWallMask.None;
                if (cell.North == null || !cell.IsLinked(cell.North)) current |= RectangularCellWallMask.North;
                if (cell.East == null || !cell.IsLinked(cell.East)) current |= RectangularCellWallMask.East;
                if (cell.South == null || !cell.IsLinked(cell.South)) current |= RectangularCellWallMask.South;
                if (cell.West == null || !cell.IsLinked(cell.West)) current |= RectangularCellWallMask.West;
                result[cell.Row, cell.Column] = Convert.ToInt32(current);
            }

            return new GridDefinitionTypeAndWalls
            {
                TypeOfGrid = GridType.Rectangular,
                GridCells = result
            };
        }

        public static GridDefinitionTypeAndWalls TriangularGridToIntArray(TriangleGrid grid)
        {
            var result = new int[grid.Rows, grid.Columns];
            foreach (var cell in grid.EachCell)
            {
                var current = TriangularCellWallMask.None;
                if (cell.IsUpright && !cell.IsLinked(cell.South)) current |= TriangularCellWallMask.NorthOrSouth;
                if (!cell.IsUpright && !cell.IsLinked(cell.North)) current |= TriangularCellWallMask.NorthOrSouth;
                if (cell.East == null || !cell.IsLinked(cell.East)) current |= TriangularCellWallMask.East;
                if (cell.West == null || !cell.IsLinked(cell.West)) current |= TriangularCellWallMask.West;
                result[cell.Row, cell.Column] = Convert.ToInt32(current);
            }

            return new GridDefinitionTypeAndWalls
            {
                TypeOfGrid = GridType.Triangular,
                GridCells = result
            };
        }

        public static GridDefinitionTypeAndWalls HexagonGridToIntArray(HexGrid grid)
        {
            var result = new int[grid.Rows, grid.Columns];
            foreach (var cell in grid.EachCell)
            {
                var current = HexagonCellWallMask.None;
                
                if (cell.North == null || !cell.IsLinked(cell.North)) current |= HexagonCellWallMask.North;
                if (cell.NorthEast == null || !cell.IsLinked(cell.NorthEast)) current |= HexagonCellWallMask.NorthEast;
                if (cell.SouthEast == null || !cell.IsLinked(cell.SouthEast)) current |= HexagonCellWallMask.SouthEast;
                if (cell.South == null || !cell.IsLinked(cell.South)) current |= HexagonCellWallMask.South;
                if (cell.SouthWest == null || !cell.IsLinked(cell.SouthWest)) current |= HexagonCellWallMask.SouthWest;
                if (cell.NorthWest == null || !cell.IsLinked(cell.NorthWest)) current |= HexagonCellWallMask.NorthWest;
                
                result[cell.Row, cell.Column] = Convert.ToInt32(current);
            }

            return new GridDefinitionTypeAndWalls
            {
                TypeOfGrid = GridType.Hexagon,
                GridCells = result
            };
        }

        //todo: This one is a lot more challenging as the circular grid does not contain the same number of cells for each row
        public static GridDefinitionTypeAndWalls CircularPolarGridToIntArray(PolarGrid grid)
        {
            throw new NotImplementedException();
        }
    }
}
