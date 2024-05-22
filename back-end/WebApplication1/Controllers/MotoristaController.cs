using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Context;
using WebApplication1.Model;
using WebApplication1.DTO;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.ControllersMotorista
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotoristaController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public MotoristaController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET: api/<ClienteController>
        [HttpGet]
        public ActionResult<List<Motorista>> Get()
        {
            try
            {
                var motoristas = _dataContext.Motorista.ToList();
                return Ok(motoristas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados: " + ex.Message);
            }
        }

        // GET api/<ClienteController>/5
        [HttpGet("{id}")]
        public ActionResult<Motorista> Get(int id)
        {
            var motorista = _dataContext.Motorista.FirstOrDefault(x => x.Id == id);
            if (motorista == null)
            {
                return BadRequest("ID não existente");
            }
            return motorista;
        }

        // POST api/<ClienteController>
        [HttpPost]
        public ActionResult<Motorista> Post([FromBody] MotoristaRequest motoristaRequest)
        {
            if (ModelState.IsValid)
            {
                var motorista = motoristaRequest.toModel();
                _dataContext.Motorista.Add(motorista);
                _dataContext.SaveChanges();
                return CreatedAtAction(nameof(Get), new { id = motorista.Id }, motorista);
            }
            return BadRequest(ModelState);
        }

        // PUT api/<ClienteController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] MotoristaRequest motoristaRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var motoristaExistente = _dataContext.Motorista.FirstOrDefault(x => x.Id == id);
            if (motoristaExistente == null)
            {
                return NotFound("Motorista não encontrado");
            }

            // Atualizar clienteExistente com os dados de clienteRequest
            motoristaExistente.Nome = motoristaRequest.Nome;
            motoristaExistente.Email = motoristaRequest.Email;
            motoristaExistente.Cpf = motoristaRequest.Cpf;
            motoristaExistente.Telefone = motoristaRequest.Telefone;
            motoristaExistente.TipoVeiculo = motoristaRequest.TipoVeiculo;
            motoristaExistente.PlacaVeiculo = motoristaRequest.PlacaVeiculo;
            motoristaExistente.Logradouro = motoristaRequest.Logradouro;
            motoristaExistente.Bairro = motoristaRequest.Bairro;
            motoristaExistente.Cep = motoristaRequest.Cep;
            motoristaExistente.Numero = motoristaRequest.Numero;
            motoristaExistente.Cidade = motoristaRequest.Cidade;
            motoristaExistente.Estado = motoristaRequest.Estado;
            // Outros campos a serem atualizados

            _dataContext.Motorista.Update(motoristaExistente);
            _dataContext.SaveChanges();

            return NoContent();
        }

        // DELETE api/<ClienteController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var motorista = _dataContext.Motorista.FirstOrDefault(x => x.Id == id);
            if (motorista == null)
            {
                return NotFound("Motorista não encontrado");
            }

            _dataContext.Motorista.Remove(motorista);
            _dataContext.SaveChanges();

            return NoContent();
        }
    }
}
