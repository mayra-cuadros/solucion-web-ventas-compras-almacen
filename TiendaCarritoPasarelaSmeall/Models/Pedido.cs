namespace TiendaCarritoPasarelaSmeall.Models
{
    public class Pedido
    {
        public int Numero { get; set; }
        public List<CarritoItem> Items { get; set; } = new();
        public string MetodoPago { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public decimal MontoTotal { get; set; }
        public DateTime Fecha { get; set; }
    }
}
