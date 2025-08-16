using ConnectFour.ViewModels.Base;

namespace ConnectFour.ViewModels
{
    public class CellViewModel : BaseViewModel
    {
        private Player _player;
        public int Row { get; }
        public int Col { get; }

        public Player Player
        {
            get => _player;
            set { _player = value; Raise(); }
        }

        public CellViewModel(int row, int col) { Row = row; Col = col; }
    }
}
