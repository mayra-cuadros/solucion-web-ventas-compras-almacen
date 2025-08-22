using System.Data;
using Microsoft.Data.SqlClient;
using proyectoWEBSITESmeall.Infrastructure;
using proyectoWEBSITESmeall.Dtos;
using System.Threading.Tasks;

namespace proyectoWEBSITESmeall.Repositories
{
    public class PagoRepository
    {
        private readonly IDbConnectionFactory _factory;
        public PagoRepository(IDbConnectionFactory factory) { _factory = factory; }

        public async Task<int> CrearPagoAsync(CrearPagoDto dto)
        {
            using var cn = _factory.Create();
            using var cmd = new SqlCommand("dbo.sp_CrearPago", cn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@IdOrden", dto.IdOrden);
            cmd.Parameters.AddWithValue("@Metodo", dto.Metodo);
            cmd.Parameters.AddWithValue("@Monto", dto.Monto);
            cmd.Parameters.AddWithValue("@Moneda", dto.Moneda ?? (object)System.DBNull.Value);

            cmd.Parameters.AddWithValue("@TarjetaMarca", (object?)dto.TarjetaMarca ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@AuthorizationCode", (object?)dto.AuthorizationCode ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@CardLast4", (object?)dto.CardLast4 ?? System.DBNull.Value);

            cmd.Parameters.AddWithValue("@WalletTelefono", (object?)dto.WalletTelefono ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@WalletRef", (object?)dto.WalletRef ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@QrData", (object?)dto.QrData ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@QrExpiry", (object?)dto.QrExpiry ?? System.DBNull.Value);

            cmd.Parameters.AddWithValue("@CipCode", (object?)dto.CipCode ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@CipExpiry", (object?)dto.CipExpiry ?? System.DBNull.Value);

            cmd.Parameters.AddWithValue("@ExternalId", (object?)dto.ExternalId ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@ReturnUrl", (object?)dto.ReturnUrl ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@Observaciones", (object?)dto.Observaciones ?? System.DBNull.Value);

            var pOut = new SqlParameter("@IdPago", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(pOut);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return (int)pOut.Value;
        }

        public async Task ActualizarEstadoPagoAsync(ActualizarEstadoPagoDto dto)
        {
            using var cn = _factory.Create();
            using var cmd = new SqlCommand("dbo.sp_ActualizarEstadoPago", cn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@IdPago", dto.IdPago);
            cmd.Parameters.AddWithValue("@NuevoEstado", dto.NuevoEstado);
            cmd.Parameters.AddWithValue("@AuthorizationCode", (object?)dto.AuthorizationCode ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@ExternalId", (object?)dto.ExternalId ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@CardLast4", (object?)dto.CardLast4 ?? System.DBNull.Value);
            cmd.Parameters.AddWithValue("@Observaciones", (object?)dto.Observaciones ?? System.DBNull.Value);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<int> RegistrarWebhookAsync(WebhookDto dto)
        {
            using var cn = _factory.Create();
            using var cmd = new SqlCommand("dbo.sp_RegistrarWebhookPago", cn) { CommandType = CommandType.StoredProcedure };

            cmd.Parameters.AddWithValue("@Origen", dto.Origen);
            cmd.Parameters.AddWithValue("@EventType", dto.EventType);
            cmd.Parameters.AddWithValue("@Payload", dto.Payload);
            cmd.Parameters.AddWithValue("@IdPago", (object?)dto.IdPago ?? System.DBNull.Value);

            var pOut = new SqlParameter("@IdWebhook", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(pOut);

            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
            return (int)pOut.Value;
        }
    }
}
