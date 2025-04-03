using Game.Models;
using GameHub.Hubs;
using GameHub.Models;
using GameHub.Repositories;

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

            if (gameHub?.Id == null)
                throw new ArgumentNullException(nameof(hubId), "Hub id is null");
            
            var game = new XO
            {
                Id = gameHub.Id,
                Board = new string[3,3],
                CurrentPlayer = gameHub.PlayerX,
                PlayerX = gameHub.PlayerX,
                PlayerO = gameHub.PlayerO,
            };

            return game;
        }

        public bool CheckWinner(XO game)
        {
            throw new NotImplementedException();
        }

        
        public async Task<XO?> MakeMoveAsync(XO game, int row, int col)
        {
            throw new NotImplementedException();
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
            gameHub.Id = game.Id;

            await _hubRepository.UpdateHub(gameHub);
        }
    }
}
