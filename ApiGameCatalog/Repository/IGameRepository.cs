using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiGameCatalog.Entities;
using ApiGameCatalog.InputModel;

namespace ApiGameCatalog.Repositoriy
{
    public interface IGameRepository : IDisposable
    {
        Task<List<Game>> Get(int page, int quantity);
        Task<Game> Get(Guid id);
        Task<List<Game>> Get(string name, string producer);
        Task AddGame(Game game);
        Task UpdateGame(Game game);
        Task RemoveGame(Guid id);
    }
}
