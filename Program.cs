using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace ControlInventario
{
    class Program
    {
        static List<Producto> inventario = new();
        static string archivoDatos = "inventario.json";
        static int siguienteId = 1;

        static void Main(string[] args)
        {
            CargarInventario();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("==== CONTROL DE INVENTARIO ====");
                Console.WriteLine("1. Agregar producto");
                Console.WriteLine("2. Listar productos");
                Console.WriteLine("3. Eliminar producto");
                Console.WriteLine("4. Modificar producto");
                Console.WriteLine("0. Salir");
                Console.Write("Opción: ");

                string? opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        AgregarProducto();
                        break;
                    case "2":
                        ListarProductos();
                        break;
                    case "3":
                        EliminarProducto();
                        break;
                    case "4":
                        ModificarProducto();
                        break;
                    case "0":
                        GuardarInventario();
                        return;
                    default:
                        Console.WriteLine("Opción inválida. Presione una tecla para continuar...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void AgregarProducto()
        {
            Console.Write("Nombre: ");
            string? nombre = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("Nombre inválido.");
                Console.ReadKey();
                return;
            }

            Console.Write("Cantidad: ");
            if (!int.TryParse(Console.ReadLine(), out int cantidad))
            {
                Console.WriteLine("Cantidad inválida.");
                Console.ReadKey();
                return;
            }

            Console.Write("Precio: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal precio))
            {
                Console.WriteLine("Precio inválido.");
                Console.ReadKey();
                return;
            }

            inventario.Add(new Producto
            {
                Id = siguienteId++,
                Nombre = nombre,
                Cantidad = cantidad,
                Precio = precio
            });

            GuardarInventario();
            Console.WriteLine("Producto agregado correctamente.");
            Console.ReadKey();
        }

        static void ListarProductos()
        {
            Console.WriteLine("\nID\tNombre\tCantidad\tPrecio\tValor Total");
            foreach (var p in inventario)
            {
                Console.WriteLine($"{p.Id}\t{p.Nombre}\t{p.Cantidad}\t{p.Precio:C}\t{p.ValorTotal:C}");
            }

            int totalCantidad = 0;
            decimal totalValor = 0;
            foreach (var p in inventario)
            {
                totalCantidad += p.Cantidad;
                totalValor += p.ValorTotal;
            }

            Console.WriteLine($"\nTotal de unidades: {totalCantidad}");
            Console.WriteLine($"Valor total inventario: {totalValor:C}");
            Console.WriteLine("\nPresione una tecla para continuar...");
            Console.ReadKey();
        }

        static void EliminarProducto()
        {
            Console.Write("Ingrese el ID del producto a eliminar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var producto = inventario.Find(p => p.Id == id);
                if (producto != null)
                {
                    inventario.Remove(producto);
                    GuardarInventario();
                    Console.WriteLine("Producto eliminado correctamente.");
                }
                else
                {
                    Console.WriteLine("No se encontró un producto con ese ID.");
                }
            }
            else
            {
                Console.WriteLine("ID inválido.");
            }
            Console.WriteLine("Presione una tecla para continuar...");
            Console.ReadKey();
        }

        static void ModificarProducto()
        {
            Console.Write("Ingrese el ID del producto a modificar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var producto = inventario.Find(p => p.Id == id);
                if (producto != null)
                {
                    Console.WriteLine($"Producto actual: {producto.Nombre} - Cantidad: {producto.Cantidad} - Precio: {producto.Precio:C}");

                    Console.Write("Nuevo nombre (o Enter para dejar igual): ");
                    string? nuevoNombre = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nuevoNombre))
                        producto.Nombre = nuevoNombre;

                    Console.Write("Nueva cantidad (o Enter para dejar igual): ");
                    string? nuevaCantidadStr = Console.ReadLine();
                    if (int.TryParse(nuevaCantidadStr, out int nuevaCantidad))
                        producto.Cantidad = nuevaCantidad;

                    Console.Write("Nuevo precio (o Enter para dejar igual): ");
                    string? nuevoPrecioStr = Console.ReadLine();
                    if (decimal.TryParse(nuevoPrecioStr, out decimal nuevoPrecio))
                        producto.Precio = nuevoPrecio;

                    GuardarInventario();
                    Console.WriteLine("Producto modificado correctamente.");
                }
                else
                {
                    Console.WriteLine("No se encontró un producto con ese ID.");
                }
            }
            else
            {
                Console.WriteLine("ID inválido.");
            }
            Console.WriteLine("Presione una tecla para continuar...");
            Console.ReadKey();
        }

        static void GuardarInventario()
        {
            string json = JsonSerializer.Serialize(inventario, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(archivoDatos, json);
        }

        static void CargarInventario()
        {
            if (File.Exists(archivoDatos))
            {
                string json = File.ReadAllText(archivoDatos);
                inventario = JsonSerializer.Deserialize<List<Producto>>(json) ?? new();
                if (inventario.Count > 0)
                    siguienteId = inventario[^1].Id + 1;
            }
        }
    }
}
