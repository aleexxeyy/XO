using Game.Models;
using GameHub.Models;

namespace Game.Services
{
    public interface IXOService
    {
        XO CreateGame(GameHubs hub);
        Task<XO?> MakeMoveAsync(XO game, int row, int col);
        bool CheckWinner(XO game);
        Task<bool> SetWinnerAsync(XO game, string winnerSymbol);
    }
}
