using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection; // Necesario para IConfiguration
using Microsoft.Extensions.Hosting; // Necesario para IWebHostEnvironment
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using API.Modelo;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProduccionControllers : ControllerBase
    {
        private readonly string cadenaSQL;
        public ProduccionControllers(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }
        [HttpGet]
        [Route("Lista")]

        public IActionResult Lista()
        {

            List<produccion> lista = new List<produccion>(); try

            {

                using (var conexion = new SqlConnection(cadenaSQL))

                {

                    conexion.Open();

                    var cmd = new SqlCommand("sp_listar_produccion", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())

                    {

                        while (reader.Read())

                        {

                            lista.Add(new produccion

                            {

                                id_lote = Convert.ToInt32(reader["id_lote"]),
                                cantidad = reader ["cantidad"].ToString(),
                                fecha_inicio = reader["fecha_inicio"].ToString(),
                                fecha_finalizacion = reader["fecha_finalizacion"].ToString(),
                                vendido = reader["vendido"].ToString(),
                               
                            });

                        }

                    }

                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });

            }

            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista });
            }
        }
        [HttpDelete]
        [Route("Borrar/{id}")]
        public IActionResult Borrar(int id_lote)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_borrar_produccion", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar el parámetro de ID para el borrado
                    cmd.Parameters.Add(new SqlParameter("@id_lote", id_lote));

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        return StatusCode(StatusCodes.Status200OK, new { mensaje = "El producto ha sido borrado." });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Producto no encontrado." });
                    }
                }
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPut]
        [Route("Actualizar/{id}")]
        public IActionResult Actualizar(int id, [FromBody] produccion productoActualizado)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_actualizar_produccion", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros necesarios para la actualización
                    cmd.Parameters.Add(new SqlParameter("@id_lote", id));
                    cmd.Parameters.Add(new SqlParameter("@cantidad", productoActualizado.cantidad));
                    cmd.Parameters.Add(new SqlParameter("@fecha_inicio", productoActualizado.@fecha_inicio));
                    cmd.Parameters.Add(new SqlParameter("@fecha_finalizacion", productoActualizado.fecha_finalizacion));
                    cmd.Parameters.Add(new SqlParameter("vendido", productoActualizado.vendido));
                    

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        return StatusCode(StatusCodes.Status200OK, new { mensaje = "El producto ha sido actualizado." });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Producto no encontrado." });
                    }
                }
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
        [HttpPost]
        [Route("Ingresar")]
        public IActionResult Ingresar([FromBody] produccion nuevoProducto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_ingresar_produccion", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros necesarios para la inserción
                    cmd.Parameters.Add(new SqlParameter("@cantidad", nuevoProducto.cantidad));
                    cmd.Parameters.Add(new SqlParameter("@fecha_inicio", nuevoProducto.fecha_inicio));
                    cmd.Parameters.Add(new SqlParameter("@fecha_finalizacion", nuevoProducto.fecha_finalizacion));
                    cmd.Parameters.Add(new SqlParameter("@vendido", nuevoProducto.vendido));
                    

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if (filasAfectadas > 0)
                    {
                        return StatusCode(StatusCodes.Status201Created, new { mensaje = "El producto ha sido ingresado." });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status400BadRequest, new { mensaje = "Error al ingresar el producto." });
                    }
                }
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }
    }
}

    