
namespace ConnectFour.Models
{
    public class GameBoard
    {
        public int Rows { get; }
        public int Cols { get; }

        private readonly Player[,] _cells;

        public GameBoard(int rows = 6, int cols = 7)
        {
            Rows = rows;
            Cols = cols;
            _cells = new Player[Rows, Cols];
            Reset();
        }

        public Player GetCell(int r, int c) => _cells[r, c];

        public void Reset()
        {
            for (int r = 0; r < Rows; r++)
                for (int c = 0; c < Cols; c++)
                    _cells[r, c] = Player.None;
        }

        public bool CanDrop(int col)
        {
            if (col < 0 || col >= Cols) return false;
            return _cells[0, col] == Player.None; // top cell empty => column not full
        }

        // Returns row where disc lands, or -1 if can't drop
        public int Drop(int col, Player p)
        {
            if (!CanDrop(col)) return -1;
            for (int r = Rows - 1; r >= 0; r--)
            {
                if (_cells[r, col] == Player.None)
                {
                    _cells[r, col] = p;
                    return r;
                }
            }
            return -1;
        }

        public bool IsFull()
        {
            for (int c = 0; c < Cols; c++)
                if (_cells[0, c] == Player.None) return false;
            return true;
        }

        public bool CheckWin(int lastRow, int lastCol, Player p)
        {
            if (p == Player.None || lastRow < 0 || lastCol < 0) return false;

            int CountDir(int dr, int dc)
            {
                int count = 1;
                int r = lastRow + dr, c = lastCol + dc;
                while (r >= 0 && r < Rows && c >= 0 && c < Cols && _cells[r, c] == p)
                { count++; r += dr; c += dc; }
                r = lastRow - dr; c = lastCol - dc;
                while (r >= 0 && r < Rows && c >= 0 && c < Cols && _cells[r, c] == p)
                { count++; r -= dr; c -= dc; }
                return count;
            }

            return CountDir(0, 1) >= 4   // horizontal
                || CountDir(1, 0) >= 4   // vertical
                || CountDir(1, 1) >= 4   // diag ↘
                || CountDir(-1, 1) >= 4; // diag ↗
        }
    }
}
