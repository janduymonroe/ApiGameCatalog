using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGameCatalog.InputModel;
using ApiGameCatalog.ViewModel;
using ApiGameCatalog.Services;
using System.ComponentModel.DataAnnotations;
using ApiGameCatalog.Exceptions;

namespace ApiGameCatalog.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly IGameService _gameService;
        public GamesController(IGameService gameService)
        {
           _gameService = gameService;
        }

        /// <summary>
        /// Searching for games in page model
        /// </summary>
        /// <remarks>
        /// It's not possible return games without pagination
        /// </remarks>
        /// <param name="page">Set the page you search. Minimun 1</param>
        /// <param name="quantity">Set the amount of registries per page. Minimum 1 e Maximum 50</param>
        /// <response code="200">Return Game List</response>
        /// <response code="204">In case games not found</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GameViewModel>>> Get([FromQuery, Range(1, int.MaxValue)] int page =1, [FromQuery, Range(1, int.MaxValue)] int quantity = 1)
        {
            var games = await _gameService.Get(page, quantity);

            if (games.Count == 0)
            {
                return NoContent();
            }
            
            return Ok(games);
        }

        /// <summary>
        /// Get game by Id
        /// </summary>
        /// <param name="idGame">Game Id that you search</param>
        /// <response code="200">Return the game filtered</response>
        /// <response code="204">In case that game not found</response>
        [HttpGet("{idGame:guid}")]
        public async Task<ActionResult<GameViewModel>> Get([FromRoute] Guid idGame)
        {
            var game = await _gameService.Get(idGame);

            if (game == null)
            {
                return NoContent();
            }

            return Ok(game);

        }

        /// <summary>
        /// Add a game in catalog
        /// </summary>
        /// <param name="gameInputModel">Data Game to insert</param>
        /// <response code="200">In case of Success</response>
        /// <response code="422">In case this game has already been included</response> 
        [HttpPost]
        public async Task<ActionResult<GameViewModel>> AddGame([FromBody] GameInputModel gameInputModel)
        {
            try
            {
                var game = await _gameService.AddGame(gameInputModel);
                return Ok(game);
            }
            catch (GameAlreadyRegisteredException)
            {
                return UnprocessableEntity("There is already a registered game with that name for this producer");
            }
        }

        /// <summary>
        /// Update a game in catalog
        /// </summary>
        /// /// <param name="idGame">Game Id to update</param>
        /// <param name="gameInputModel">All data game that will change</param>
        /// <response code="200">In case of success update</response>
        /// <response code="404">In case that game was not found</response>   
        [HttpPut("{idGame:guid}")]
        public async Task<ActionResult> UpdateGame([FromRoute] Guid idGame, [FromBody]GameInputModel gameInputModel)
        {
            try
            {
                await _gameService.UpdateGame(idGame, gameInputModel);
                return Ok();
            }
            catch(GameNotRegisteredException)
            {
                return NotFound("Game not found");
            }
       
        }

        /// <summary>
        /// Update the game price
        /// </summary>
        /// /// <param name="idGame">Id game to updade</param>
        /// <param name="price">New game price</param>
        /// <response code="200">In case of success</response>
        /// <response code="404">In case that game was not found</response>   
        [HttpPatch("{idGame:guid}/price/{price:double}")]
        public async Task<ActionResult> UpdateGame([FromRoute] Guid idGame, [FromRoute] double price)
        {
            try
            {
                await _gameService.UpdateGame(idGame, price);
                return Ok();
            }
            catch(GameNotRegisteredException)
            {
                return NotFound("Game not found");
            }
        }

        /// <summary>
        /// Delete Game
        /// </summary>
        /// /// <param name="idGame">Gam Id to be removed</param>
        /// <response code="200">In case of success</response>
        /// <response code="404">Case that game was not found</response>   
        [HttpDelete("{idGame:guid}")]
        public async Task<ActionResult> RemoveGame([FromRoute] Guid idGame)
        {
            try
            {
                await _gameService.RemoveGame(idGame);

                return Ok();
            }
            catch(GameNotRegisteredException)
            {
                return NotFound("Game not found");
            }
        }

    
    }
}
