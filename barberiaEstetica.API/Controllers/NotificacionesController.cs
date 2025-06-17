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
    public class NotificacionesController : ApiController
    {
        private BarberiaEsteticaEntities db = new BarberiaEsteticaEntities();

        // GET: api/Notificaciones
        public IHttpActionResult GetNotificaciones()
        {
            var notificaciones = db.Notificaciones
                .Select(n => new {
                    n.NotificacionID,
                    n.TurnoID,
                    n.Tipo,
                    n.Estado,
                    n.FechaEnvio,
                    Turno = new {
                        ClienteNombre = n.Turnos.Clientes.Nombre + " " + n.Turnos.Clientes.Apellido,
                        EmpleadoNombre = n.Turnos.Empleados.Nombre + " " + n.Turnos.Empleados.Apellido,
                        ServicioNombre = n.Turnos.Servicios.Nombre,
                        Fecha = n.Turnos.Fecha,
                        HoraInicio = n.Turnos.HoraInicio
                    }
                })
                .OrderByDescending(n => n.FechaEnvio)
                .ToList();
            return Ok(notificaciones);
        }

        // GET: api/Notificaciones/Turno/5
        [Route("api/Notificaciones/Turno/{id}")]
        public IHttpActionResult GetNotificacionesPorTurno(int id)
        {
            var notificaciones = db.Notificaciones
                .Where(n => n.TurnoID == id)
                .Select(n => new {
                    n.NotificacionID,
                    n.TurnoID,
                    n.Tipo,
                    n.Estado,
                    n.FechaEnvio
                })
                .OrderByDescending(n => n.FechaEnvio)
                .ToList();
            return Ok(notificaciones);
        }

        // POST: api/Notificaciones
        public IHttpActionResult PostNotificacion(Notificaciones notificacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            notificacion.FechaEnvio = DateTime.Now;
            db.Notificaciones.Add(notificacion);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = notificacion.NotificacionID }, notificacion);
        }

        // PUT: api/Notificaciones/5/Estado
        [Route("api/Notificaciones/{id}/Estado")]
        public IHttpActionResult PutEstadoNotificacion(int id, [FromBody] string nuevoEstado)
        {
            var notificacion = db.Notificaciones.Find(id);
            if (notificacion == null)
            {
                return NotFound();
            }

            notificacion.Estado = nuevoEstado;
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/Notificaciones/5
        public IHttpActionResult DeleteNotificacion(int id)
        {
            Notificaciones notificacion = db.Notificaciones.Find(id);
            if (notificacion == null)
            {
                return NotFound();
            }

            db.Notificaciones.Remove(notificacion);
            db.SaveChanges();

            return Ok(notificacion);
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