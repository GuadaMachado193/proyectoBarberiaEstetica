using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using barberiaEstetica.API.Models;
using System.Security.Cryptography;
using System.Text;

namespace barberiaEstetica.API.Controllers
{
    public class AuthController : ApiController
    {
        private BarberiaEsteticaEntities db = new BarberiaEsteticaEntities();

        // POST: api/Auth/Login
        [HttpPost]
        [Route("api/Auth/Login")]
        public IHttpActionResult Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = db.Usuarios
                .FirstOrDefault(u => u.Usuario == model.Usuario && u.Clave == model.Clave);

            if (usuario == null)
            {
                return BadRequest("Usuario o contraseña incorrectos");
            }

            if (usuario.Estado != "activo")
            {
                return BadRequest("Usuario inactivo");
            }

            var rol = db.Roles.FirstOrDefault(r => r.RolID == usuario.RolID);

            return Ok(new
            {
                UsuarioID = usuario.UsuarioID,
                Usuario = usuario.Usuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Rol = rol?.Nombre,
                RolID = usuario.RolID
            });
        }

        // POST: api/Auth/Registrar
        [HttpPost]
        [Route("api/Auth/Registrar")]
        public IHttpActionResult Registrar([FromBody] RegistroModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar si el usuario ya existe
            if (db.Usuarios.Any(u => u.Usuario == model.Usuario))
            {
                return BadRequest("El nombre de usuario ya está en uso");
            }

            // Verificar si el email ya existe
            if (db.Usuarios.Any(u => u.Email == model.Email))
            {
                return BadRequest("El email ya está registrado");
            }

            var usuario = new Usuarios
            {
                Usuario = model.Usuario,
                Clave = model.Clave,
                Email = model.Email,
                Nombre = model.Nombre,
                Apellido = model.Apellido,
                Telefono = model.Telefono,
                Estado = "activo",
                RolID = 3 // Por defecto, rol de Cliente
            };

            db.Usuarios.Add(usuario);
            db.SaveChanges();

            return Ok(new
            {
                UsuarioID = usuario.UsuarioID,
                Usuario = usuario.Usuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                RolID = usuario.RolID
            });
        }

        // GET: api/Auth/VerificarEmail
        [HttpGet]
        [Route("api/Auth/VerificarEmail")]
        public IHttpActionResult VerificarEmail(string email)
        {
            var existe = db.Usuarios.Any(u => u.Email == email);
            return Ok(new { Existe = existe });
        }

        // GET: api/Auth/VerificarUsuario
        [HttpGet]
        [Route("api/Auth/VerificarUsuario")]
        public IHttpActionResult VerificarUsuario(string usuario)
        {
            var existe = db.Usuarios.Any(u => u.Usuario == usuario);
            return Ok(new { Existe = existe });
        }
    }

    public class LoginModel
    {
        public string Usuario { get; set; }
        public string Clave { get; set; }
    }

    public class RegistroModel
    {
        public string Usuario { get; set; }
        public string Clave { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
    }
} 