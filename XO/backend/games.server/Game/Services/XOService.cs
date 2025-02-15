using Game.Models;
using GameHub.Repositories;
using GameHub.Services;

namespace Game.Services
{
    public class XOService : IXOService
    {
        private readonly IGameHubsService _gameHubService;
        private readonly IGameHubRepository _gameHubRepository;
        private readonly Dictionary<Guid, TicTacToe> _games = new();

        public XOService(IGameHubsService gameHubService, IGameHubRepository gameHubRepository)
        {
            _gameHubService = gameHubService;
            _gameHubRepository = gameHubRepository;
        }
        public TicTacToe? GetGame(Guid gameId)
        {
            return _games.TryGetValue(gameId, out var game) ? game : null;
        }

        public async Task<TicTacToe?> JoinGameAsync(Guid gameId, string playerNickname)
        {
            var gameHub = await _gameHubService.GetGames(gameId);

            if (gameHub == null || gameHub.Status == "in progress") throw new Exception("Game not found or already running");

            var ticTacToeGame = new TicTacToe
            {
                Id = gameId,
                PlayerX = gameHub.Creator,
                PlayerO = playerNickname,
                Board = new string[3, 3],
                CurrentPlayer = gameHub.Creator
            };

            _games[gameId] = ticTacToeGame;

            gameHub.Status = "in progress";
            await _gameHubRepository.UpdateHub(gameHub);

            return ticTacToeGame;
        } 

        public TicTacToe? MakeMove(Guid gameId, string username, int row, int col)
        {
            if (!_games.TryGetValue(gameId, out var game) || game.IsGameOver)
                return null;

            if (row < 0 || row >= 3 || col < 0 || col >= 3)
                return null;

            if (!string.IsNullOrEmpty(game.Board[row, col]) || game.CurrentPlayer != username)
                return null;

            if (game.PlayerX == game.PlayerO)
                return null;

            string playerSymbol = username == game.PlayerX ? "X" : "O";
            game.Board[row, col] = playerSymbol;

            if (!CheckWinner(game))
            {
                game.CurrentPlayer = game.CurrentPlayer == game.PlayerX ? game.PlayerO : game.PlayerX;
            }

            return game;
        }

        private bool CheckWinner(TicTacToe game)
        {
            string[,] b = game.Board;

            for (int i = 0; i < 3; i++)
            {
                if (!string.IsNullOrEmpty(b[i, 0]) && b[i, 0] == b[i, 1] && b[i, 1] == b[i, 2])
                    return SetWinner(game, b[i, 0]);

                if (!string.IsNullOrEmpty(b[0, i]) && b[0, i] == b[1, i] && b[1, i] == b[2, i])
                    return SetWinner(game, b[0, i]);
            }

            if (!string.IsNullOrEmpty(b[0, 0]) && b[0, 0] == b[1, 1] && b[1, 1] == b[2, 2] ||
                !string.IsNullOrEmpty(b[0, 2]) && b[0, 2] == b[1, 1] && b[1, 1] == b[2, 0])
                return SetWinner(game, b[1, 1]);

            if (b.Cast<string>().All(cell => !string.IsNullOrEmpty(cell)))
            {
                game.IsGameOver = true;
                game.Winner = null;
            }

            return false;
        }

        private bool SetWinner(TicTacToe game, string winnerSymbol)
        {
            game.Winner = winnerSymbol == "X" ? game.PlayerX : game.PlayerO;
            game.IsGameOver = true;
            return true;
        }
    }
}