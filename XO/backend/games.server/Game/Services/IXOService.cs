using Game.Models;
using GameHub.Models;

namespace Game.Services
{
    public interface IXOService
    {
        Task<XO> CreateGame(Guid hubId);
        Task<XO?> MakeMoveAsync(XO game, int row, int col);
        bool CheckWinner(XO game);
        Task<bool> SetWinnerAsync(XO game, string winnerSymbol);
    }
}
