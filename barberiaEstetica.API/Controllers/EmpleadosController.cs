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
    public class EmpleadosController : ApiController
    {
        private BarberiaEsteticaEntities db = new BarberiaEsteticaEntities();

        // GET: api/Empleados
        public IHttpActionResult GetEmpleados()
        {
            var empleados = db.Empleados
                .Select(e => new {
                    e.EmpleadoID,
                    e.Nombre,
                    e.Apellido,
                    e.Telefono,
                    e.HorarioInicio,
                    e.HorarioFin,
                    e.Estado
                })
                .ToList();
            return Ok(empleados);
        }

        // GET: api/Empleados/5
        public IHttpActionResult GetEmpleado(int id)
        {
            var empleado = db.Empleados
                .Where(e => e.EmpleadoID == id)
                .Select(e => new {
                    e.EmpleadoID,
                    e.Nombre,
                    e.Apellido,
                    e.Telefono,
                    e.HorarioInicio,
                    e.HorarioFin,
                    e.Estado
                })
                .FirstOrDefault();

            if (empleado == null)
            {
                return NotFound();
            }

            return Ok(empleado);
        }

        // POST: api/Empleados
        public IHttpActionResult PostEmpleado(Empleados empleado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Empleados.Add(empleado);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = empleado.EmpleadoID }, empleado);
        }

        // PUT: api/Empleados/5
        public IHttpActionResult PutEmpleado(int id, Empleados empleado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != empleado.EmpleadoID)
            {
                return BadRequest();
            }

            db.Entry(empleado).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                if (!EmpleadoExists(id))
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

        // DELETE: api/Empleados/5
        public IHttpActionResult DeleteEmpleado(int id)
        {
            Empleados empleado = db.Empleados.Find(id);
            if (empleado == null)
            {
                return NotFound();
            }

            db.Empleados.Remove(empleado);
            db.SaveChanges();

            return Ok(empleado);
        }

        private bool EmpleadoExists(int id)
        {
            return db.Empleados.Count(e => e.EmpleadoID == id) > 0;
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