using System.Data;
using Microsoft.Data.SqlClient;
using proyectoWEBSITESmeall.Infrastructure;
using proyectoWEBSITESmeall.Dtos;
using Dapper;
using System.Threading.Tasks;

namespace proyectoWEBSITESmeall.Repositories
{
    public class OrdenRepository
    {
        private readonly IDbConnectionFactory _factory;
        public OrdenRepository(IDbConnectionFactory factory) { _factory = factory; }

        public async Task<int> CrearOrdenDesdeCarritoAsync(int idCarrito, string? observaciones)
        {
            using var cn = _factory.Create();
            using var cmd = new SqlCommand("dbo.sp_CrearOrdenDesdeCarrito", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdCarrito", idCarrito);
            cmd.Parameters.AddWithValue("@Observaciones", (object?)observaciones ?? System.DBNull.Value);
            var pOut = new SqlParameter("@IdOrden", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(pOut);
            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return (int)pOut.Value;
        }

        public async Task<OrdenDto?> ObtenerOrdenCompletaAsync(int idOrden)
        {
            using var cn = _factory.Create();
            const string sql = @"SELECT TOP 1 o.IdOrden, o.IdCarrito, o.FechaRegistro, o.Estado, o.Total
                                 FROM Orden o WHERE o.IdOrden = @id";
            return await cn.QueryFirstOrDefaultAsync<OrdenDto>(sql, new { id = idOrden });
        }
    }
}
