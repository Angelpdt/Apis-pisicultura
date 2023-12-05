using Datos_Api.Modelos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Hosting;

namespace Datos_Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DatosController : ControllerBase
    {
        #region CadenalSQl
        private readonly string cadenaSQL;

        public DatosController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }
        #endregion

        #region Listar 
        [HttpGet]
        [Route("Listar")]
        public IActionResult Listar()
        {
            List<Datos> listar = new List<Datos>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listar", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            listar.Add(new Datos
                            {
                                Num_l = Convert.ToInt32(rd["Num_l"]),
                                Temp = rd["Temp"].ToString(),
                                Ph = rd["Ph"].ToString(),
                                Niv_ox = rd["Niv_ox"].ToString(),
                                Tbdz = rd["Tbdz"].ToString(),
                                Cant_cmd = rd["Cant_cmd"].ToString(),
                                Sal = rd["Sal"].ToString(),
                                Alc = rd["Alc"].ToString(),
                            });
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = listar });
            }
            catch (SqlException sqlError)
            {
                // Manejar específicamente las excepciones de SQL
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error de SQL", detalle = sqlError.Message });
            }
            catch (Exception error)
            {
                // Manejar otras excepciones
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno del servidor", detalle = error.Message });


            }

        }
        #endregion
        #region Listar_S
        [HttpGet]
        [Route("Listar_S")]
        public IActionResult Listar_S()
        {
            List<Datos> listar = new List<Datos>();
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listar_S", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            listar.Add(new Datos
                            {
                                Num_l = Convert.ToInt32(rd["Num_l"]),
                                Temp = rd["Temp"].ToString(),
                                Ph = rd["Ph"].ToString(),
                                Niv_ox = rd["Niv_ox"].ToString(),
                                Tbdz = rd["Tbdz"].ToString(),
                                Cant_cmd = rd["Cant_cmd"].ToString(),
                                Sal = rd["Sal"].ToString(),
                                Alc = rd["Alc"].ToString()
                            });
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = listar });
            }
            catch (SqlException sqlError)
            {
                // Manejar específicamente las excepciones de SQL
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error de SQL", detalle = sqlError.Message });
            }
            catch (Exception error)
            {
                // Manejar otras excepciones
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno del servidor", detalle = error.Message });


            }

        }
        #endregion
        #region Obtener

        [HttpGet]
        [Route("Obtener/{Datos:int}")]
        public IActionResult Obtener(int Num_l)
        {
            List<Datos> lista = new List<Datos>();
            Datos producto = new Datos();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();

                    using (var cmd = new SqlCommand("sp_Obtener", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Agrega el parámetro a la consulta almacenada
                        cmd.Parameters.AddWithValue("@Num_l", Num_l);

                        using (var rd = cmd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                lista.Add(new Datos
                                {
                                    Num_l = Convert.ToInt32(rd["Num_l"]),
                                    Temp = rd["Temp"].ToString(),
                                    Ph = rd["Ph"].ToString(),
                                    Niv_ox = rd["Niv_ox"].ToString(),
                                    Tbdz = rd["Tbdz"].ToString(),
                                    Cant_cmd = rd["Cant_cmd"].ToString(),
                                    Sal = rd["Sal"].ToString(),
                                    Alc = rd["Alc"].ToString()
                                });
                            }
                        }
                    }
                }

                producto = lista.FirstOrDefault(item => item.Num_l == Num_l);

                if (producto != null)
                {
                    return Ok(new { mensaje = "ok", response = producto });
                }
                else
                {
                    return NotFound(new { mensaje = "Producto no encontrado", response = producto });
                }
            }
            catch (SqlException sqlError)
            {
                // Manejar específicamente las excepciones de SQL
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error de SQL", detalle = sqlError.Message, response = producto });
            }
            catch (Exception error)
            {
                // Manejar otras excepciones
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno del servidor", detalle = error.Message, response = producto });
            }
        }

        #endregion
        #region Guardar

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Datos objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();

                    using (var cmd = new SqlCommand("sp_Ingresar", conexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("Num_l", objeto.Num_l);
                        cmd.Parameters.AddWithValue("Temp", objeto.Temp);
                        cmd.Parameters.AddWithValue("Ph", objeto.Ph);
                        cmd.Parameters.AddWithValue("Niv_ox", objeto.Niv_ox);
                        cmd.Parameters.AddWithValue("Tbdz", objeto.Tbdz);
                        cmd.Parameters.AddWithValue("Cant_cmd", objeto.Cant_cmd);
                        cmd.Parameters.AddWithValue("Sal", objeto.Sal);
                        cmd.Parameters.AddWithValue("Alc", objeto.Alc);


                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok(new { mensaje = "agregado" });
            }
            catch (SqlException sqlError)
            {
                // Manejar específicamente las excepciones de SQL
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error de SQL", detalle = sqlError.Message });
            }
            catch (Exception error)
            {
                // Manejar otras excepciones
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno del servidor", detalle = error.Message });
            }
        }

        #endregion
        #region Editar
        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Datos objeto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_Editar", conexion);
                    cmd.Parameters.AddWithValue("@Num_l", objeto.Num_l == 0 ? DBNull.Value : objeto.Num_l);
                    cmd.Parameters.AddWithValue("@Temp", objeto.Temp is null ? DBNull.Value : objeto.Temp);
                    cmd.Parameters.AddWithValue("@Ph", objeto.Ph is null ? DBNull.Value : objeto.Ph);
                    cmd.Parameters.AddWithValue("@Niv_ox", objeto.Niv_ox is null ? DBNull.Value : objeto.Niv_ox);
                    cmd.Parameters.AddWithValue("@Tbdz", objeto.Tbdz is null ? DBNull.Value : objeto.Tbdz);
                    cmd.Parameters.AddWithValue("@Cant_cmd", objeto.Cant_cmd is null ? DBNull.Value : objeto.Cant_cmd);
                    cmd.Parameters.AddWithValue("@Sal", objeto.Sal is null ? DBNull.Value : objeto.Sal);
                    cmd.Parameters.AddWithValue("@Alc", objeto.Alc is null ? DBNull.Value : objeto.Alc);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "editado" });
            }
            catch (SqlException sqlError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error de SQL", detalle = sqlError.Message });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno del servidor", detalle = error.Message });
            }
        }
        #endregion
        #region Eliminar

        
        [HttpDelete]
        [Route("Eliminar/{idProducto:int}")]
        public IActionResult Eliminar(int idProducto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_Eliminar", conexion);
                    cmd.Parameters.AddWithValue("@Num_l", idProducto);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "eliminado" });
            }
            catch (SqlException sqlError)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error de SQL", detalle = sqlError.Message });
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = "Error interno del servidor", detalle = error.Message });
            }

            



        }
        #endregion
    }
}