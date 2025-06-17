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
    public class ServiciosController : ApiController
    {
        private BarberiaEsteticaEntities db = new BarberiaEsteticaEntities();

        // GET: api/Servicios
        public IHttpActionResult GetServicios()
        {
            var servicios = db.Servicios
                .Select(s => new {
                    s.ServicioID,
                    s.Nombre,
                    s.Descripcion,
                    s.Precio,
                    s.DuracionMinutos,
                    s.Estado
                })
                .ToList();
            return Ok(servicios);
        }

        // GET: api/Servicios/5
        public IHttpActionResult GetServicio(int id)
        {
            var servicio = db.Servicios
                .Where(s => s.ServicioID == id)
                .Select(s => new {
                    s.ServicioID,
                    s.Nombre,
                    s.Descripcion,
                    s.Precio,
                    s.DuracionMinutos,
                    s.Estado
                })
                .FirstOrDefault();

            if (servicio == null)
            {
                return NotFound();
            }

            return Ok(servicio);
        }

        // POST: api/Servicios
        public IHttpActionResult PostServicio(Servicios servicio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Servicios.Add(servicio);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = servicio.ServicioID }, servicio);
        }

        // PUT: api/Servicios/5
        public IHttpActionResult PutServicio(int id, Servicios servicio)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != servicio.ServicioID)
            {
                return BadRequest();
            }

            db.Entry(servicio).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                if (!ServicioExists(id))
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

        // DELETE: api/Servicios/5
        public IHttpActionResult DeleteServicio(int id)
        {
            Servicios servicio = db.Servicios.Find(id);
            if (servicio == null)
            {
                return NotFound();
            }

            db.Servicios.Remove(servicio);
            db.SaveChanges();

            return Ok(servicio);
        }

        private bool ServicioExists(int id)
        {
            return db.Servicios.Count(e => e.ServicioID == id) > 0;
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