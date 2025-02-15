using GameHub.Models;
using GameHub.Repositories;

namespace GameHub.Services
{
    public class GameHubsService : IGameHubsService
    {
        private readonly IGameHubRepository _gameHubRepository;

        public GameHubsService(IGameHubRepository gameHubRepository)
        {
            _gameHubRepository = gameHubRepository;
        }

        public async Task<List<GameHubs>> GetGames()
        {
            return await _gameHubRepository.GetListHubs();
        }

        public async Task<GameHubs> CreateHub(string creator)
        {
            var gameHub = new GameHubs
            {
                Id = Guid.NewGuid(),
                Creator = creator,
                PlayerX = creator,
                CreatedAt = DateTime.UtcNow,
                Status = "second player expected"
            };

            return await _gameHubRepository.AddHub(gameHub);
        }

        public async Task<GameHubs?> JoinGame(Guid gameId, string player)
        {
            var game = await _gameHubRepository.GetHub(gameId);
            if (game == null || !string.IsNullOrEmpty(game.PlayerO))
            {
                return null;
            }

            game.PlayerO = player;
            game.Status = "in progress";

            return await _gameHubRepository.UpdateHub(game);
        }
    }
}
