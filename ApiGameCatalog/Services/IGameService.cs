using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGameCatalog.InputModel;
using ApiGameCatalog.ViewModel;

namespace ApiGameCatalog.Services
{
    public interface IGameService : IDisposable
    {
        Task<List<GameViewModel>> Get(int page, int quantity);
        Task<GameViewModel> Get(Guid id);
        Task<GameViewModel> AddGame(GameInputModel game);
        Task UpdateGame(Guid id, GameInputModel game);
        Task UpdateGame(Guid id, double price);
        Task RemoveGame(Guid id);
    }
}
