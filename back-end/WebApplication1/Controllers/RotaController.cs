using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Context;
using WebApplication1.Model;
using WebApplication1.DTO;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication1.ControllersRota
{
    [Route("api/[controller]")]
    [ApiController]
    public class RotaController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public RotaController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET: api/<ClienteController>
        [HttpGet]
        public ActionResult<List<Rota>> Get()
        {
            try
            {
                var rotas = _dataContext.Rota.ToList();
                return Ok(rotas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados: " + ex.Message);
            }
        }

        // GET api/<ClienteController>/5
        [HttpGet("{id}")]
        public ActionResult<Rota> Get(int id)
        {
            var rota = _dataContext.Rota.FirstOrDefault(x => x.Id == id);
            if (rota == null)
            {
                return BadRequest("ID não existente");
            }
            return rota;
        }

        // POST api/<ClienteController>
        [HttpPost]
        public ActionResult<Rota> Post([FromBody] RotaRequest rotaRequest)
        {
            if (ModelState.IsValid)
            {
                var rota = rotaRequest.toModel();
                _dataContext.Rota.Add(rota);
                _dataContext.SaveChanges();
                return CreatedAtAction(nameof(Get), new { id = rota.Id }, rota);
            }
            return BadRequest(ModelState);
        }

        // PUT api/<ClienteController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] RotaRequest rotaRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rotaExistente = _dataContext.Rota.FirstOrDefault(x => x.Id == id);
            if (rotaExistente == null)
            {
                return NotFound("Rota não encontrado");
            }

            // Atualizar clienteExistente com os dados de clienteRequest
            rotaExistente.ClienteId = rotaRequest.ClienteID;
            rotaExistente.MotoristaId = rotaRequest.MotoristaId;
            rotaExistente.LojaId = rotaRequest.LojaId;
            rotaExistente.Data = rotaRequest.Data;
            rotaExistente.Kg = rotaRequest.Kg;
            rotaExistente.Distancia = rotaRequest.Distancia;
            rotaExistente.Duracao = rotaRequest.Duracao;
            // Outros campos a serem atualizados

            _dataContext.Rota.Update(rotaExistente);
            _dataContext.SaveChanges();

            return NoContent();
        }

        // DELETE api/<ClienteController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var rota = _dataContext.Rota.FirstOrDefault(x => x.Id == id);
            if (rota == null)
            {
                return NotFound("Rota não encontrado");
            }

            _dataContext.Rota.Remove(rota);
            _dataContext.SaveChanges();

            return NoContent();
        }
    }
}
