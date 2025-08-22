using System.Threading.Tasks;
using proyectoWEBSITESmeall.Repositories;
using proyectoWEBSITESmeall.Dtos;

namespace proyectoWEBSITESmeall.Services
{
    public class OrdenService
    {
        private readonly OrdenRepository _repo;
        public OrdenService(OrdenRepository repo) { _repo = repo; }

        public Task<int> CrearDesdeCarritoAsync(int idCarrito, string? obs)
            => _repo.CrearOrdenDesdeCarritoAsync(idCarrito, obs);

        public Task<OrdenDto?> ObtenerAsync(int idOrden)
            => _repo.ObtenerOrdenCompletaAsync(idOrden);
    }
}
