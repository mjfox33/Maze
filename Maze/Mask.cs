using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class Mask
    {
        public int Rows { get; }
        public int Columns { get; }
        private readonly bool[,] _bitFlags;
        private static readonly Random RandomNumberGenerator = new Random();
        private static readonly char[] MaskCharactersThatDisableACell = { 'x', 'X' };
        public static Mask MaskFromTextFile(string filename)
        {
            var lines = File.ReadAllLines(filename);
            var rows = lines.Length;
            var columns = lines[0].Length;
            var mask = new Mask(rows, columns);
            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    mask[row, column] = !MaskCharactersThatDisableACell.Contains(lines[row][column]);
                }
            }
            return mask;
        }

        public static Mask MaskFromImageFile(string filename)
        {
            var image = new Bitmap(filename);
            var mask = new Mask(image.Height, image.Width);
            for (var row = 0; row < image.Height; row++)
            {
                for (var column = 0; column < image.Width; column++)
                {
                    mask[row, column] = image.GetPixel(column, row).ToArgb() != Color.Black.ToArgb();
                }
            }
            return mask;
        }

        public Mask(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _bitFlags = PrepareFlags();
        }

        private bool[,] PrepareFlags()
        {
            var result = new bool[Rows, Columns];
            for (var row = 0; row < Rows; row++)
            {
                for (var col = 0; col < Columns; col++)
                {
                    result[row, col] = true;
                }
            }
            return result;
        }

        public bool this[int row, int column]
        {
            get
            {
                if (row >= 0 && row < Rows && column >= 0 && column < Columns)
                {
                    return _bitFlags[row, column];
                } 
                return false;
            }

            set
            {
                if (row >= 0 && row < Rows && column >= 0 && column < Columns)
                {
                    _bitFlags[row, column] = value;
                }
            }
        }

        public int Count => _bitFlags.Cast<bool>().Count(flag => flag);

        public Tuple<int, int> RandomValidRowAndColumn
        {
            get
            {
                while (true)
                {
                    var row = RandomNumberGenerator.Next(Rows);
                    var column = RandomNumberGenerator.Next(Columns);
                    if (!_bitFlags[row, column])
                        continue;
                    return new Tuple<int, int>(row, column);
                }
            }
        }

    }
}
