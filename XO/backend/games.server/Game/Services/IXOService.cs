using Game.Models;
using System;

namespace Game.Services
{
    public interface IXOService
    {
        TicTacToe? GetGame(Guid gameId);
        Task<TicTacToe?> JoinGameAsync(Guid gameId, string playerNickname);
        TicTacToe? MakeMove(Guid gameId, string username, int row, int col);
    }
}