using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Context;
using WebApplication1.DTO;
using WebApplication1.Model;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnderecoController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public EnderecoController()
        {
            _dataContext = new DataContext();
        }

        // GET: api/<EnderecoController>
        [HttpGet]
        public ActionResult<List<Endereco>> Get()
        {
            return _dataContext.Endereco.ToList();

        }

        // GET api/<EnderecoController>/5
        [HttpGet("{id}")]
        public ActionResult<Endereco> Get(int id)
        {
            var endereco = _dataContext.Endereco.FirstOrDefault(x => x.Id == id);
            if (endereco == null)
            {
                return BadRequest("ID não existente");
            }
            return endereco;
        }

        // POST api/<EnderecoController>
        [HttpPost]
        public ActionResult<Endereco> Post([FromBody] EnderecoRequest enderecoRequest)
        {
            if (ModelState.IsValid)
            {
                var endereco = enderecoRequest.toModel();
                _dataContext.Endereco.Add(endereco);
                _dataContext.SaveChanges();
                return endereco;
            }
            return BadRequest(ModelState);
        }

        // PUT api/<EnderecoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EnderecoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}