using PESCAMAX.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection; // Necesario para IConfiguration
using Microsoft.Extensions.Hosting; // Necesario para IWebHostEnvironment
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PESCAMAX.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly string cadenaSQL;

        public ClientesController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("Listar")]
        public IActionResult Listar()
        {
            {
                List<Clientes> listar = new List<Clientes>();
                try
                {
                    using (var conexion = new SqlConnection(cadenaSQL))
                    {
                        conexion.Open();
                        var cmd = new SqlCommand("sp_listar_Clientes", conexion);
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (var rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                listar.Add(new Clientes
                                {
                                    ClienteID = Convert.ToInt32(rd["ClienteID"]),
                                    NombreCliente = rd["NombreCliente"].ToString(),
                                    Email = rd["Email"].ToString(),
                                    Telefono = rd["Telefono"].ToString()
                                });
                            }
                        }
                    }

                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = listar });
                }
                catch (Exception error)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = listar });
                }
            }
        }

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Clientes objeto)
        {
            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_guardar_Clientes", conexion); cmd.Parameters.AddWithValue("NombreCliente", objeto.NombreCliente); cmd.Parameters.AddWithValue("Email", objeto.Email); cmd.Parameters.AddWithValue("Telefono", objeto.Telefono); cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "agregado" });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });

            }
        }
        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Clientes objeto)
        {
            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_editar_Clientes", conexion);
                    cmd.Parameters.AddWithValue("ClienteID", objeto.ClienteID == 0 ? DBNull.Value : objeto.ClienteID); cmd.Parameters.AddWithValue("NombreCliente", objeto.NombreCliente is null ? DBNull.Value : objeto.NombreCliente); cmd.Parameters.AddWithValue("Email", objeto.Email is null ? DBNull.Value : objeto.Email); cmd.Parameters.AddWithValue("Telefono", objeto.Telefono is null ? DBNull.Value : objeto.Telefono);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "editado" });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });

            }


        }

        [HttpDelete]
        [Route("Eliminar/{ClienteID:int}")]
        private IActionResult Eliminar(int ClienteID)
        {
            try
            {

                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_eliminar_Cliente", conexion); cmd.Parameters.AddWithValue("ClienteID", ClienteID); cmd.CommandType = CommandType.StoredProcedure; cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "eliminado" });
            }
            catch (Exception error)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });

            }
        }
    }

}

