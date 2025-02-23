using Game.Dto;
using Game.Models;
using Game.Services;
using GameHub.Models;
using GameHub.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Game.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class XOController : ControllerBase
    {
        private readonly IXOService _xoService;
        private readonly IGameHubRepository _hubRepository;

        public XOController(IXOService xoService, IGameHubRepository hubRepository)
        {
            _xoService = xoService;
            _hubRepository = hubRepository;
        }

        [HttpPost("create/{gameId}")]
        public async Task<IActionResult> CreateGame(Guid hubId)
        {
            var hub = await _hubRepository.GetHub(hubId);
            if (hub == null)
                return NotFound($"GameHub with ID {hubId} not found.");

            var game = _xoService.CreateGame(hub);
            return Ok(game);
        }

        [HttpPost("{gameId}/move")]
        public async Task<IActionResult> MakeMove(Guid gameId, [FromBody] MoveRequest request)
        {
            var hub = await _hubRepository.GetHub(gameId);
            if (hub == null)
                return NotFound($"GameHub with ID { gameId} not found.");

            var game = await _xoService.CreateGame(hub);
            game.Board = request.Board;
            game.CurrentPlayer = request.CurrentPlayer;

            var updatedGame = await _xoService.MakeMoveAsync(game, request.Row, request.Col);
            if (updatedGame == null)
                return BadRequest("Impossible to make a move.");

            return Ok(updatedGame);
        }

        [HttpPost("{gameId}/set-winner")]
        public async Task<IActionResult> SetWinner(Guid gameId, [FromBody] SetWinnerRequest request)
        {
            var hub = await _hubRepository.GetHub(gameId);
            if (hub == null)
                return NotFound($"GameHub with ID {gameId} not found.");

            var game = await _xoService.CreateGame(hub);
            var result = await _xoService.SetWinnerAsync(game, request.WinnerSymbol);

            if (!result)
                return BadRequest("The winner could not be determined.");

            return Ok("The winner has been established.");
        }
    }
}
