using System.Data;
using System.Data.SqlClient;
using MiWebAPI.Models;

namespace MiWebAPI.Data
{
    public class ProductoData
    {
        private readonly string conexion;

        public ProductoData(IConfiguration configuration)
        {
            conexion = configuration.GetConnectionString("CadenaSQL")!;
        }

        public async Task<List<ProductoDetalle>> Lista()
        {
            List<ProductoDetalle> lista = new List<ProductoDetalle>();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("dbo.PRODUCTO_DETALLE_LISTAR", con);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lista.Add(new ProductoDetalle
                        {
                            Id = Convert.ToInt64(reader["Id"]),
                            NombreProducto = reader["NombreProducto"].ToString(),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            Cantidad = Convert.ToInt32(reader["Cantidad"]),
                            Total = Convert.ToDecimal(reader["Total"]),
                            Descripcion = reader["Descripcion"].ToString()
                        });
                    }
                }
            }

            return lista;
        }

        public async Task<ProductoDetalle> Obtener(int Id)
        {
            ProductoDetalle objeto = new ProductoDetalle();

            using (var con = new SqlConnection(conexion))
            {
                await con.OpenAsync();

                SqlCommand cmd = new SqlCommand("dbo.PRODUCTO_DETALLE_CONSULTA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Id);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        objeto = new ProductoDetalle
                        {
                            Id = Convert.ToInt64(reader["Id"]),
                            NombreProducto = reader["NombreProducto"].ToString(),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            Cantidad = Convert.ToInt32(reader["Cantidad"]),
                            Total = Convert.ToDecimal(reader["Total"]),
                            Descripcion = reader["Descripcion"].ToString()
                        };
                    }
                }
            }

            return objeto;
        }

        public async Task<bool> Crear(ProductoDetalle objeto)
        {
            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("dbo.PRODUCTO_DETALLE_AGREGA", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@nombreProducto", objeto.NombreProducto);
                cmd.Parameters.AddWithValue("@precio", objeto.Precio);
                cmd.Parameters.AddWithValue("@cantidad", objeto.Cantidad);
                cmd.Parameters.AddWithValue("@total", objeto.Total);
                cmd.Parameters.AddWithValue("@descripcion", objeto.Descripcion);

                await con.OpenAsync();

                return await cmd.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> Editar(ProductoDetalle objeto)
        {
            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("dbo.PRODUCTO_DETALLE_EDITA", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", objeto.Id);
                cmd.Parameters.AddWithValue("@nombreProducto", objeto.NombreProducto);
                cmd.Parameters.AddWithValue("@precio", objeto.Precio);
                cmd.Parameters.AddWithValue("@cantidad", objeto.Cantidad);
                cmd.Parameters.AddWithValue("@total", objeto.Total);
                cmd.Parameters.AddWithValue("@descripcion", objeto.Descripcion);

                await con.OpenAsync();

                return await cmd.ExecuteNonQueryAsync() > 0;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            using (var con = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("dbo.PRODUCTO_DETALLE_ELIMINA", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                await con.OpenAsync();

                return await cmd.ExecuteNonQueryAsync() > 0;
            }
        }
    }
}