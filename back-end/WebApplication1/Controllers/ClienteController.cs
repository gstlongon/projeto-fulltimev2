using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Context;
using WebApplication1.Model;
using WebApplication1.DTO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public ClienteController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET: api/<ClienteController>
        [HttpGet]
        public ActionResult<List<Cliente>> Get()
        {
            try
            {
                var clientes = _dataContext.Cliente.OrderByDescending(c => c.Id).ToList();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados: " + ex.Message);
            }
        }

        [HttpGet("b={busca}")]
        public ActionResult<List<Cliente>> Get(string busca)
        {
            try
            {
                if (string.IsNullOrEmpty(busca))
                {
                    var clientes = _dataContext.Cliente.OrderByDescending(c => c.Id).ToList();
                    return Ok(clientes);
                }
                else
                {
                    var clientes = _dataContext.Cliente.Where(c =>
                        c.Nome.Contains(busca)
                    ).ToList();
                    return Ok(clientes);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados: " + ex.Message);
            }
        }


        // GET api/<ClienteController>/5
        [HttpGet("{id}")]
        public ActionResult<Cliente> Get(int id)
        {
            var cliente = _dataContext.Cliente.FirstOrDefault(x => x.Id == id);
            if (cliente == null)
            {
                return BadRequest("ID não existente");
            }
            return cliente;
        }

        // POST api/<ClienteController>
        [HttpPost]
        public ActionResult<Cliente> Post([FromBody] ClienteRequest clienteRequest)
        {
            if (ModelState.IsValid)
            {
                var cliente = clienteRequest.toModel();
                _dataContext.Cliente.Add(cliente);
                _dataContext.SaveChanges();
                return CreatedAtAction(nameof(Get), new { id = cliente.Id }, cliente);
            }
            return BadRequest(ModelState);
        }

        // PUT api/<ClienteController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] ClienteRequest clienteRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var clienteExistente = _dataContext.Cliente.FirstOrDefault(x => x.Id == id);
            if (clienteExistente == null)
            {
                return NotFound("Cliente não encontrado");
            }

            // Atualizar clienteExistente com os dados de clienteRequest
            clienteExistente.Nome = clienteRequest.Nome;
            clienteExistente.Email = clienteRequest.Email;
            clienteExistente.Cpf = clienteRequest.Cpf;
            clienteExistente.Telefone = clienteRequest.Telefone;
            clienteExistente.Logradouro = clienteRequest.Logradouro;
            clienteExistente.Bairro = clienteRequest.Bairro;
            clienteExistente.Cep = clienteRequest.Cep;
            clienteExistente.Numero = clienteRequest.Numero;
            clienteExistente.Cidade = clienteRequest.Cidade;
            clienteExistente.Estado = clienteRequest.Estado;
            clienteExistente.Latitude = clienteRequest.Latitude;
            clienteExistente.Longitude = clienteRequest.Longitude;
            // Outros campos a serem atualizados

            _dataContext.Cliente.Update(clienteExistente);
            _dataContext.SaveChanges();

            return NoContent();
        }

        // DELETE api/<ClienteController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var cliente = _dataContext.Cliente.FirstOrDefault(x => x.Id == id);
            if (cliente == null)
            {
                return NotFound("Cliente não encontrado");
            }

            _dataContext.Cliente.Remove(cliente);
            _dataContext.SaveChanges();

            return NoContent();
        }
        [HttpDelete]
        public IActionResult DeleteAll()
        {
            try
            {
                var clientes = _dataContext.Cliente.ToList();
                if (clientes.Count == 0)
                {
                    return NotFound("Nenhum cliente encontrada para apagar");
                }

                _dataContext.Cliente.RemoveRange(clientes);
                _dataContext.SaveChanges();
                _dataContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Cliente', RESEED, 0)");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao apagar todas as clientes: {ex.Message}");
            }
        }
    }
}
