using ApiGameCatalog.Entities;
using ApiGameCatalog.Exceptions;
using ApiGameCatalog.InputModel;
using ApiGameCatalog.Repositoriy;
using ApiGameCatalog.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiGameCatalog.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;

        public GameService(IGameRepository gameRepository)
        {
            _gameRepository = gameRepository;
        }

        public async Task<List<GameViewModel>> Get(int page, int quantity)
        {
            var games = await _gameRepository.Get(page, quantity);

            return games.Select(game => new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            }).ToList();
        }

        public async Task<GameViewModel> Get(Guid id)
        {
            var game = await _gameRepository.Get(id);

            if (game == null)
                return null;

            return new GameViewModel
            {
                Id = game.Id,
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            };
        }

        public async Task<GameViewModel> AddGame(GameInputModel game)
        {
            var entityGame = await _gameRepository.Get(game.Name, game.Producer);

            if (entityGame.Count > 0)
                throw new GameAlreadyRegisteredException();

            var gameInsert = new Game
            {
                Id = Guid.NewGuid(),
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            };

            await _gameRepository.AddGame(gameInsert);

            return new GameViewModel
            {
                Id = Guid.NewGuid(),
                Name = game.Name,
                Producer = game.Producer,
                Price = game.Price
            };
        }

        public async Task UpdateGame(Guid id, GameInputModel game)
        {
            var gameEntity = await _gameRepository.Get(id);

            if (gameEntity == null)
                throw new GameNotRegisteredException();

            gameEntity.Name = game.Name;
            gameEntity.Producer = game.Producer;
            gameEntity.Price = game.Price;

            await _gameRepository.UpdateGame(gameEntity);
        }

        public async Task UpdateGame(Guid id, double price)
        {
            var gameEntity = await _gameRepository.Get(id);

            if (gameEntity == null)
                throw new GameNotRegisteredException();

            gameEntity.Price = price;

            await _gameRepository.UpdateGame(gameEntity);
        }

        public async Task RemoveGame(Guid id)
        {
            var game = await _gameRepository.Get(id);

            if (game == null)
                throw new GameNotRegisteredException();

            await _gameRepository.RemoveGame(id);
        }

        public void Dispose()
        {
            _gameRepository?.Dispose();
        }

    }
}
