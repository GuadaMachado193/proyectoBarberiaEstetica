﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace barberiaEstetica.API.Controllers
{
    public class ClientesController : ApiController
    {
        // GET: api/Clientes
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Clientes/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Clientes
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Clientes/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Clientes/5
        public void Delete(int id)
        {
        }
    }
}
