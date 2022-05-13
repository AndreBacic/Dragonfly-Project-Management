using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DragonflyMVCApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TaskApiController : ControllerBase
    {
        // GET: api/<TaskApiController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TaskApiController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TaskApiController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TaskApiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TaskApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
