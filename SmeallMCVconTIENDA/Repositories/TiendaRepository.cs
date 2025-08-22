using Dapper;
using Microsoft.Data.SqlClient;
using proyectoWEBSITESmeall.Dtos;
using proyectoWEBSITESmeall.Infrastructure;
using System.Threading.Tasks;

namespace proyectoWEBSITESmeall.Repositories
{
    public class TiendaRepository
    {
        private readonly IDbConnectionFactory _factory;
        public TiendaRepository(IDbConnectionFactory factory) { _factory = factory; }

        public async Task<PagedResult<ProductoDto>> ListarProductosAsync(string? q, int page, int pageSize)
        {
            using var cn = _factory.Create();
            await cn.OpenAsync();

            var where = string.IsNullOrWhiteSpace(q) ? "" : "WHERE p.Nombre LIKE @like";
            var total = await cn.ExecuteScalarAsync<int>($"SELECT COUNT(1) FROM Producto p {where}", new { like = $"%{q}%" });

            var sql = $@"
SELECT p.IdProducto, p.Nombre, p.Precio, p.StockTotal, p.Descripcion
FROM Producto p
{where}
ORDER BY p.IdProducto DESC
OFFSET (@offset) ROWS FETCH NEXT (@limit) ROWS ONLY;";

            var items = (await cn.QueryAsync<ProductoDto>(sql, new
            {
                like = $"%{q}%",
                offset = (page - 1) * pageSize,
                limit = pageSize
            })).AsList();

            return new PagedResult<ProductoDto>
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                Items = items
            };
        }

        public async Task<ProductoDto?> ObtenerProductoAsync(int idProducto)
        {
            using var cn = _factory.Create();
            const string sql = @"SELECT TOP 1 p.IdProducto, p.Nombre, p.Precio, p.StockTotal, p.Descripcion
                                 FROM Producto p WHERE p.IdProducto = @id";
            return await cn.QueryFirstOrDefaultAsync<ProductoDto>(sql, new { id = idProducto });
        }
    }
}
