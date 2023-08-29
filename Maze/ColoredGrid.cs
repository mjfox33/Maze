using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class ColoredGrid : RectangularGrid
    {
        public ColoredGrid(int rows, int columns) : base(rows, columns)
        {
            
        }

        public override Distances<RectangularCell> Distances
        {
            get => base.Distances;
            set
            {
                var maxCellAndDistance = value.MaxCellAndDistance;
                _maximumDistance = maxCellAndDistance.Item2;
                base.Distances = value;
            }
        }

        private int _maximumDistance;

        public override Color? BackgroundColorForCell(Cell<RectangularCell> cell)
        {
            var distance = Distances[cell];
            var intensity = Convert.ToDouble(_maximumDistance - distance) / _maximumDistance;
            var dark = (int)Math.Ceiling(255 * intensity);
            var light = (int)Math.Ceiling(128 + 127 * intensity);
            return Color.FromArgb(dark, dark, light);
        }
    }
}
