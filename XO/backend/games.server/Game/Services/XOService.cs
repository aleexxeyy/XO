using Game.Models;
using GameHub.Models;
using GameHub.Repositories;
using GameHub.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Services
{
    public class XOService : IXOService
    {
        private readonly IGameHubRepository _hubRepository;

        public XOService(IGameHubRepository hubRepository)
        {
            _hubRepository = hubRepository;
        }

        public async Task<XO> CreateGame(Guid hubId)
        {
            var gameHub = await _hubRepository.GetHub(hubId);

            var game = new XO
            {
                Id = gameHub.Id,
                Board = new List<List<string>>
                {
                    new() { "", "", "" },
                    new() { "", "", "" },
                    new() { "", "", "" }
                },
                CurrentPlayer = gameHub.PlayerX,
                PlayerX = gameHub.PlayerX,
                PlayerO = gameHub.PlayerO,
            };

            return game;
        }

        public bool CheckWinner(XO game)
        {
            List<List<string>> board = game.Board;
            int size = board.Count;

            for (int i = 0; i < size; i++)
            {
                if (!string.IsNullOrEmpty(board[i][0]) && board[i].All(cell => cell == board[i][0]))
                    return true;
                if (!string.IsNullOrEmpty(board[0][i]) && Enumerable.Range(1, size - 1).All(j => board[j][i] == board[0][i]))
                    return true;
            }

            if (!string.IsNullOrEmpty(board[0][0]) && Enumerable.Range(1, size - 1).All(i => board[i][i] == board[0][0]))
                return true;
            if (!string.IsNullOrEmpty(board[0][size - 1]) && Enumerable.Range(1, size - 1).All(i => board[i][size - 1 - i] == board[0][size - 1]))
                return true;

            return false;
        }

        public async Task<XO?> MakeMoveAsync(XO game, int row, int col)
        {
            if (game == null || row < 0 || row >= game.Board.Count || col < 0 || col >= game.Board[row].Count || !string.IsNullOrEmpty(game.Board[row][col]))
                return null;

            game.Board[row][col] = game.CurrentPlayer;

            if (CheckWinner(game))
            {
                game.Winner = game.CurrentPlayer == "X" ? game.PlayerX : game.PlayerO;
                game.IsGameOver = true;
            }
            else
            {
                game.CurrentPlayer = game.CurrentPlayer == "X" ? "O" : "X";
            }

            await UpdateGameAsync(game);

            return game;
        }


        public async Task<bool> SetWinnerAsync(XO game, string winnerSymbol)
        {
            if (game == null || string.IsNullOrEmpty(winnerSymbol))
                return false;

            game.Winner = winnerSymbol == "X" ? game.PlayerX : game.PlayerO;
            game.IsGameOver = true;
            await UpdateGameAsync(game);
            return true;
        }

        private async Task UpdateGameAsync(XO game)
        {
            var gameHub = await _hubRepository.GetHub(game.Id);
            if (gameHub == null)
            {
                throw new InvalidOperationException($"GameHub with ID {game.Id} not found.");
            }

            game.PlayerX = gameHub.PlayerX;
            game.PlayerO = gameHub.PlayerO;
            gameHub.Status = game.IsGameOver ? "finished" : "in progress";

            await _hubRepository.UpdateHub(gameHub);
        }
    }
}
