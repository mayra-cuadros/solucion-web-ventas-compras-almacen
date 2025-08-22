using System.Threading.Tasks;
using proyectoWEBSITESmeall.Repositories;

namespace proyectoWEBSITESmeall.Services
{
    public class CarritoService
    {
        private readonly CarritoRepository _repo;
        public CarritoService(CarritoRepository repo) { _repo = repo; }

        public Task<int> CrearORecuperarAsync(int? idUsuario, string? sessionId)
            => _repo.CrearORecuperarCarritoAsync(idUsuario, sessionId);

        public Task AgregarOActualizarItemAsync(int idCarrito, int idProducto, int cantidad)
            => _repo.AgregarOActualizarItemAsync(idCarrito, idProducto, cantidad);

        public Task CambiarCantidadAsync(int idCarrito, int idProducto, int cantidad)
            => _repo.CambiarCantidadAsync(idCarrito, idProducto, cantidad);

        public Task EliminarItemAsync(int idCarrito, int idProducto)
            => _repo.EliminarItemAsync(idCarrito, idProducto);

        public Task VaciarCarritoAsync(int idCarrito)
            => _repo.VaciarCarritoAsync(idCarrito);

        public Task<proyectoWEBSITESmeall.Dtos.CarritoDto?> ObtenerAsync(int idCarrito)
            => _repo.ObtenerCarritoAsync(idCarrito);
    }
}
