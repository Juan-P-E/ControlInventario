namespace ControlInventario
{
    public class Producto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio { get; set; }

        // Propiedad calculada para saber el valor total en stock
        public decimal ValorTotal => Cantidad * Precio;
    }
}
