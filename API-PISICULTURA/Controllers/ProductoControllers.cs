using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using API.Modelo;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GastoControllers : ControllerBase
    {
        private readonly string cadenaSQL;

        public GastoControllers(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Gasto> lista = new List<Gasto>();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();

                    var cmd = new SqlCommand("sp_listar_gastos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Gasto
                            {
                                GastoID = Convert.ToInt32(reader["GastoID"]),
                                FechaGasto = reader["FechaGasto"].ToString(),
                                TipoGasto = reader["TipoGasto"].ToString(),
                                Descripcion = reader["Descripcion"].ToString(),
                                Monto = Convert.ToDecimal(reader["Monto"]),
                                Notas = reader["Notas"].ToString()
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

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Gasto nuevoGasto)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();

                    var cmd = new SqlCommand("sp_guardar_gasto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@fechaGasto", nuevoGasto.FechaGasto);
                    cmd.Parameters.AddWithValue("@tipoGasto", nuevoGasto.TipoGasto);
                    cmd.Parameters.AddWithValue("@descripcion", nuevoGasto.Descripcion);
                    cmd.Parameters.AddWithValue("@monto", nuevoGasto.Monto);
                    cmd.Parameters.AddWithValue("@notas", nuevoGasto.Notas);

                    cmd.ExecuteNonQuery();

                    return StatusCode(StatusCodes.Status201Created, new { mensaje = "Gasto guardado correctamente", response = nuevoGasto });
                }
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = nuevoGasto });
            }
        }

        [HttpDelete]
        [Route("Eliminar")]
        public IActionResult Eliminar(int id)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();

                    var cmd = new SqlCommand("sp_eliminar_gasto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@gastoID", id);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return StatusCode(StatusCodes.Status200OK, new { mensaje = "Gasto eliminado correctamente", gastoID = id });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Gasto no encontrado", gastoID = id });
                    }
                }
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, gastoID = id });
            }
        }

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar(int gastoID, [FromBody] Gasto gastoActualizado)
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();

                    var cmd = new SqlCommand("sp_editar_gasto", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@gastoID", gastoID);
                    cmd.Parameters.AddWithValue("@fechaGasto", gastoActualizado.FechaGasto);
                    cmd.Parameters.AddWithValue("@tipoGasto", gastoActualizado.TipoGasto);
                    cmd.Parameters.AddWithValue("@descripcion", gastoActualizado.Descripcion);
                    cmd.Parameters.AddWithValue("@monto", gastoActualizado.Monto);
                    cmd.Parameters.AddWithValue("@notas", gastoActualizado.Notas);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return StatusCode(StatusCodes.Status200OK, new { mensaje = "Gasto actualizado correctamente", gastoID = gastoID, response = gastoActualizado });
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { mensaje = "Gasto no encontrado", gastoID = gastoID });
                    }
                }
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, gastoID = gastoID });
            }
        }
        [HttpGet]
        [Route("ObtenerTotal")]
        public IActionResult ObtenerTotal()
        {
            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();

                    var cmd = new SqlCommand("sp_obtener_total_gastos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Utilizamos ExecuteScalar para obtener un solo valor (el total)
                    var total = Convert.ToDecimal(cmd.ExecuteScalar());

                    return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", totalGastos = total });
                }
            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

    }
}
