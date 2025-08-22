using System;
using System.Collections.Generic;

namespace proyectoWEBSITESmeall.Dtos
{
    public class ProductoDto
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int StockTotal { get; set; }
        public string? Descripcion { get; set; }
    }

    public class PagedResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public List<T> Items { get; set; } = new();
    }
}
