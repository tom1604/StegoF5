using System;


namespace StegoF5.Extensions
{
    public class Matrix
    {
        private readonly byte[,] _data;

        public int Rows { get; }

        public int Columns { get; }

        public Matrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _data = new byte[rows, columns];
            ProcessFunctionOverData(SetToZero);
        }

        public void ProcessFunctionOverData(Action<int, int> func)
        {
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Columns; j++)
                {
                    func(i, j);
                }
            }
        }

        private void SetToZero(int i, int j)
        {
            _data[i, j] = 0;
        }

        public byte this[int x, int y] 
        {
            get => _data[x, y];
            set => _data[x, y] = value;
        }

        public byte[] SumColomn(int first, int second)
        {
            var result = new byte[Rows];
            for (var i = 0; i < Columns; i++)
            {
                result[i] = (byte)((_data[first, i] + _data[second, i]) % 2);
            }

            return result;
        }
    }
}
