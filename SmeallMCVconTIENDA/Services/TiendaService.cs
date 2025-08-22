using System.Threading.Tasks;
using proyectoWEBSITESmeall.Repositories;
using proyectoWEBSITESmeall.Dtos;

namespace proyectoWEBSITESmeall.Services
{
    public class TiendaService
    {
        private readonly TiendaRepository _repo;
        public TiendaService(TiendaRepository repo) { _repo = repo; }

        public Task<PagedResult<ProductoDto>> BuscarAsync(string? q, int page = 1, int pageSize = 20)
            => _repo.ListarProductosAsync(q, page, pageSize);

        public Task<ProductoDto?> ObtenerAsync(int idProducto)
            => _repo.ObtenerProductoAsync(idProducto);
    }
}
