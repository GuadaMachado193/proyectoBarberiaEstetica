using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using barberiaEstetica.API.Models;

namespace barberiaEstetica.API.Controllers
{
    public class TurnosController : ApiController
    {
        private BarberiaEsteticaEntities db = new BarberiaEsteticaEntities();

        // GET: api/Turnos
        public IHttpActionResult GetTurnos()
        {
            var turnos = db.Turnos
                .Select(t => new {
                    t.TurnoID,
                    t.ClienteID,
                    t.EmpleadoID,
                    t.ServicioID,
                    t.Fecha,
                    t.HoraInicio,
                    t.HoraFin,
                    t.Estado,
                    t.Notas,
                    t.FechaModificacion,
                    ClienteNombre = t.Clientes.Nombre + " " + t.Clientes.Apellido,
                    EmpleadoNombre = t.Empleados.Nombre + " " + t.Empleados.Apellido,
                    ServicioNombre = t.Servicios.Nombre
                })
                .ToList();
            return Ok(turnos);
        }

        // GET: api/Turnos/5
        public IHttpActionResult GetTurno(int id)
        {
            var turno = db.Turnos
                .Where(t => t.TurnoID == id)
                .Select(t => new {
                    t.TurnoID,
                    t.ClienteID,
                    t.EmpleadoID,
                    t.ServicioID,
                    t.Fecha,
                    t.HoraInicio,
                    t.HoraFin,
                    t.Estado,
                    t.Notas,
                    t.FechaModificacion,
                    ClienteNombre = t.Clientes.Nombre + " " + t.Clientes.Apellido,
                    EmpleadoNombre = t.Empleados.Nombre + " " + t.Empleados.Apellido,
                    ServicioNombre = t.Servicios.Nombre
                })
                .FirstOrDefault();

            if (turno == null)
            {
                return NotFound();
            }

            return Ok(turno);
        }

        // GET: api/Turnos/Empleado/5
        [Route("api/Turnos/Empleado/{id}")]
        public IHttpActionResult GetTurnosPorEmpleado(int id)
        {
            var turnos = db.Turnos
                .Where(t => t.EmpleadoID == id)
                .Select(t => new {
                    t.TurnoID,
                    t.ClienteID,
                    t.EmpleadoID,
                    t.ServicioID,
                    t.Fecha,
                    t.HoraInicio,
                    t.HoraFin,
                    t.Estado,
                    t.Notas,
                    t.FechaModificacion,
                    ClienteNombre = t.Clientes.Nombre + " " + t.Clientes.Apellido,
                    EmpleadoNombre = t.Empleados.Nombre + " " + t.Empleados.Apellido,
                    ServicioNombre = t.Servicios.Nombre
                })
                .ToList();
            return Ok(turnos);
        }

        // GET: api/Turnos/Cliente/5
        [Route("api/Turnos/Cliente/{id}")]
        public IHttpActionResult GetTurnosPorCliente(int id)
        {
            var turnos = db.Turnos
                .Where(t => t.ClienteID == id)
                .Select(t => new {
                    t.TurnoID,
                    t.ClienteID,
                    t.EmpleadoID,
                    t.ServicioID,
                    t.Fecha,
                    t.HoraInicio,
                    t.HoraFin,
                    t.Estado,
                    t.Notas,
                    t.FechaModificacion,
                    ClienteNombre = t.Clientes.Nombre + " " + t.Clientes.Apellido,
                    EmpleadoNombre = t.Empleados.Nombre + " " + t.Empleados.Apellido,
                    ServicioNombre = t.Servicios.Nombre
                })
                .ToList();
            return Ok(turnos);
        }

        // POST: api/Turnos
        public IHttpActionResult PostTurno(Turnos turno)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar disponibilidad del empleado
            var turnoExistente = db.Turnos
                .Any(t => t.EmpleadoID == turno.EmpleadoID &&
                         t.Fecha == turno.Fecha &&
                         ((turno.HoraInicio >= t.HoraInicio && turno.HoraInicio < t.HoraFin) ||
                          (turno.HoraFin > t.HoraInicio && turno.HoraFin <= t.HoraFin)));

            if (turnoExistente)
            {
                return BadRequest("El empleado ya tiene un turno asignado en ese horario");
            }

            db.Turnos.Add(turno);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = turno.TurnoID }, turno);
        }

        // PUT: api/Turnos/5
        public IHttpActionResult PutTurno(int id, Turnos turno)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != turno.TurnoID)
            {
                return BadRequest();
            }

            db.Entry(turno).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                if (!TurnoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // PUT: api/Turnos/5/Estado
        [Route("api/Turnos/{id}/Estado")]
        public IHttpActionResult PutEstadoTurno(int id, [FromBody] string nuevoEstado)
        {
            var turno = db.Turnos.Find(id);
            if (turno == null)
            {
                return NotFound();
            }

            turno.Estado = nuevoEstado;
            turno.FechaModificacion = DateTime.Now;

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Turnos/5
        public IHttpActionResult DeleteTurno(int id)
        {
            Turnos turno = db.Turnos.Find(id);
            if (turno == null)
            {
                return NotFound();
            }

            db.Turnos.Remove(turno);
            db.SaveChanges();

            return Ok(turno);
        }

        private bool TurnoExists(int id)
        {
            return db.Turnos.Count(e => e.TurnoID == id) > 0;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
} 