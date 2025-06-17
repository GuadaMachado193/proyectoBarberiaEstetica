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
    public class UsuariosController : ApiController
    {
        private BarberiaEsteticaEntities db = new BarberiaEsteticaEntities();

        // GET: api/Usuarios
        public IHttpActionResult GetUsuarios()
        {
            var usuarios = db.Usuarios
                .Select(u => new {
                    u.UsuarioID,
                    u.Usuario,
                    u.Email,
                    u.Nombre,
                    u.Apellido,
                    u.Telefono,
                    u.Estado,
                    u.RolID
                })
                .ToList();
            return Ok(usuarios);
        }

        // GET: api/Usuarios/5
        public IHttpActionResult GetUsuario(int id)
        {
            var usuario = db.Usuarios
                .Where(u => u.UsuarioID == id)
                .Select(u => new {
                    u.UsuarioID,
                    u.Usuario,
                    u.Email,
                    u.Nombre,
                    u.Apellido,
                    u.Telefono,
                    u.Estado,
                    u.RolID
                })
                .FirstOrDefault();

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // POST: api/Usuarios
        public IHttpActionResult PostUsuario(Usuarios usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Usuarios.Add(usuario);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = usuario.UsuarioID }, usuario);
        }

        // PUT: api/Usuarios/5
        public IHttpActionResult PutUsuario(int id, Usuarios usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.UsuarioID)
            {
                return BadRequest();
            }

            db.Entry(usuario).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                if (!UsuarioExists(id))
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

        // DELETE: api/Usuarios/5
        public IHttpActionResult DeleteUsuario(int id)
        {
            Usuarios usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            db.Usuarios.Remove(usuario);
            db.SaveChanges();

            return Ok(usuario);
        }

        private bool UsuarioExists(int id)
        {
            return db.Usuarios.Count(e => e.UsuarioID == id) > 0;
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