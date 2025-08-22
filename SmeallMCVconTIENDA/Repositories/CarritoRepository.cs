using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using proyectoWEBSITESmeall.Dtos;
using proyectoWEBSITESmeall.Infrastructure;
using System.Threading.Tasks;
using System.Linq;

namespace proyectoWEBSITESmeall.Repositories
{
    public class CarritoRepository
    {
        private readonly IDbConnectionFactory _factory;
        public CarritoRepository(IDbConnectionFactory factory) { _factory = factory; }

        public async Task<int> CrearORecuperarCarritoAsync(int? idUsuario, string? sessionId)
        {
            using var cn = _factory.Create();
            using var cmd = new SqlCommand("dbo.sp_CrearORecuperarCarrito", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdUsuario", (object?)idUsuario ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@SessionId", (object?)sessionId ?? System.DBNull.Value);
            var pOut = new SqlParameter("@IdCarrito", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(pOut);
            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return (int)pOut.Value;
        }

        public async Task AgregarOActualizarItemAsync(int idCarrito, int idProducto, int cantidad)
        {
            using var cn = _factory.Create();
            using var cmd = new SqlCommand("dbo.sp_AgregarOActualizarItemCarrito", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdCarrito", idCarrito);
            cmd.Parameters.AddWithValue("@IdProducto", idProducto);
            cmd.Parameters.AddWithValue("@Cantidad", cantidad);
            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task CambiarCantidadAsync(int idCarrito, int idProducto, int cantidad)
        {
            using var cn = _factory.Create();
            using var cmd = new SqlCommand("dbo.sp_CambiarCantidadItemCarrito", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdCarrito", idCarrito);
            cmd.Parameters.AddWithValue("@IdProducto", idProducto);
            cmd.Parameters.AddWithValue("@Cantidad", cantidad);
            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarItemAsync(int idCarrito, int idProducto)
        {
            using var cn = _factory.Create();
            using var cmd = new SqlCommand("dbo.sp_EliminarItemCarrito", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdCarrito", idCarrito);
            cmd.Parameters.AddWithValue("@IdProducto", idProducto);
            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task VaciarCarritoAsync(int idCarrito)
        {
            using var cn = _factory.Create();
            using var cmd = new SqlCommand("dbo.sp_VaciarCarrito", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdCarrito", idCarrito);
            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<CarritoDto?> ObtenerCarritoAsync(int idCarrito)
        {
            using var cn = _factory.Create();
            await cn.OpenAsync();

            var head = await cn.QueryFirstOrDefaultAsync<(int IdCarrito, int? IdUsuario, string? SessionId)>(
                "SELECT IdCarrito, IdUsuario, SessionId FROM Carrito WHERE IdCarrito = @id",
                new { id = idCarrito });

            if (head.IdCarrito == 0) return null;

            var itemsSql = @"
SELECT ci.IdProducto, p.Nombre as NombreProducto, ci.Cantidad, 
       ISNULL(ci.PrecioUnitario, p.Precio) as PrecioUnitario
FROM CarritoItem ci
JOIN Producto p ON p.IdProducto = ci.IdProducto
WHERE ci.IdCarrito = @id
ORDER BY ci.IdCarritoItem";

            var items = (await cn.QueryAsync<CarritoItemDto>(itemsSql, new { id = idCarrito })).AsList();

            return new CarritoDto
            {
                IdCarrito = head.IdCarrito,
                IdUsuario = head.IdUsuario,
                SessionId = head.SessionId,
                Items = items,
                Total = items.Sum(i => i.Subtotal)
            };
        }
    }
}
