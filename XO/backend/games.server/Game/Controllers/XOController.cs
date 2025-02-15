using Game.Models;
using Game.Services;
using Microsoft.AspNetCore.Mvc;

namespace Game.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class XOController : ControllerBase
    {
        private readonly IXOService _xoService;

        public XOController(IXOService xoService)
        {
            _xoService = xoService;
        }

        [HttpPost("{gameId}/join")]
        public async Task<ActionResult<TicTacToe>> JoinGame(Guid gameId, [FromQuery] string playerNickname)
        {
            var game = await _xoService.JoinGameAsync(gameId, playerNickname);
            return game is not null
                ? Ok(game)
                : NotFound("Game not found or already started.");
        }

        [HttpPost("{gameId}/move")]
        public ActionResult<TicTacToe> MakeMove(Guid gameId, [FromQuery] string username, int row, int col)
        {
            var game = _xoService.MakeMove(gameId, username, row, col);
            return game is not null
                ? Ok(game)
                : BadRequest("Invalid move.");
        }

        [HttpGet("{gameId}")]
        public ActionResult<TicTacToe> GetGame(Guid gameId)
        {
            var game = _xoService.GetGame(gameId);
            return game is not null
                ? Ok(game)
                : NotFound("Game not found.");
        }
    }
}