using ConnectFour.Models;
using ConnectFour.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ConnectFour.ViewModels
{  
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly GameBoard _board;

        public int Rows => _board.Rows;
        public int Cols => _board.Cols;

        public ObservableCollection<CellViewModel> Cells { get; }

        private Player _currentPlayer;
        public Player CurrentPlayer
        {
            get => _currentPlayer;
            private set { _currentPlayer = value; Raise(); Raise(nameof(StatusText)); }
        }

        private Player _startingPlayer = Player.Red;
        public Player StartingPlayer
        {
            get => _startingPlayer;
            set { _startingPlayer = value; Raise(); }
        }
        public Player[] StartingPlayerOptions { get; } = new[] { Player.Red, Player.Yellow };

        private bool _gameOver;
        public bool GameOver
        {
            get => _gameOver;
            private set { _gameOver = value; Raise(); Raise(nameof(StatusText)); }
        }

        private Player _winner = Player.None;
        public Player Winner
        {
            get => _winner;
            private set { _winner = value; Raise(); Raise(nameof(StatusText)); }
        }

        public string StatusText
        {
            get
            {
                if (GameOver)
                {
                    if (Winner != Player.None) return $"{Winner} wins! 🎉  Click 'New Game' to play again.";
                    return "Draw! 🤝  Click 'New Game' to play again.";
                }
                return $"{CurrentPlayer} to move";
            }
        }

        public ICommand DropCommand { get; }
        public ICommand NewGameCommand { get; }

        public MainWindowViewModel()
        {
            _board = new GameBoard(6, 7);

            Cells = new ObservableCollection<CellViewModel>(
                Enumerable.Range(0, Rows)
                          .SelectMany(r => Enumerable.Range(0, Cols).Select(c => new CellViewModel(r, c)))
            );

            DropCommand = new RelayCommand(
                execute: p => DropDisc(Convert.ToInt32(p)),
                canExecute: p =>
                {
                    if (p == null) return false;
                    int col = Convert.ToInt32(p);
                    return !GameOver && _board.CanDrop(col);
                });

            NewGameCommand = new RelayCommand(_ =>
            {
                ResetBoardTo(StartingPlayer);
            });

            // Initial start: Red by default (or whatever StartingPlayer is)
            ResetBoardTo(StartingPlayer);
        }

        private void ResetBoardTo(Player starter)
        {
            _board.Reset();
            foreach (var cell in Cells) cell.Player = Player.None;

            Winner = Player.None;
            GameOver = false;
            CurrentPlayer = starter;

            CommandManager.InvalidateRequerySuggested();
        }

        private void DropDisc(int col)
        {
            if (GameOver) return;
            int row = _board.Drop(col, CurrentPlayer);
            if (row < 0) return;

            // Update VM cell
            Cells[row * Cols + col].Player = CurrentPlayer;

            // Win / Draw checks
            if (_board.CheckWin(row, col, CurrentPlayer))
            {
                Winner = CurrentPlayer;
                GameOver = true;
                CommandManager.InvalidateRequerySuggested();
                return;
            }

            if (_board.IsFull())
            {
                Winner = Player.None;
                GameOver = true;
                CommandManager.InvalidateRequerySuggested();
                return;
            }

            // Switch player
            CurrentPlayer = (CurrentPlayer == Player.Red) ? Player.Yellow : Player.Red;
            CommandManager.InvalidateRequerySuggested();
        }
    }
}