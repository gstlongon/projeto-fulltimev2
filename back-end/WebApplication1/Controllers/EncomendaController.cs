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
    public class EncomendaController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public EncomendaController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET: api/<EncomendaController>
        [HttpGet]
        public ActionResult<List<Encomenda>> Get()
        {
            try
            {
                var encomendas = _dataContext.Encomenda.OrderByDescending(c => c.Id).ToList();
                return Ok(encomendas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados: " + ex.Message);
            }
        }

        [HttpGet("b={buscaa}")]
        public ActionResult<List<Encomenda>> Get(float busca)
        {
            try
            {
                List<Encomenda> encomendas;

                if (busca == 0)
                {
                    encomendas = _dataContext.Encomenda.OrderByDescending(c => c.Id).ToList();
                }
                else
                {
                    encomendas = _dataContext.Encomenda.Where(c => c.Id == busca).ToList();
                }

                return Ok(encomendas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados: " + ex.Message);
            }
        }

        // GET api/<EncomendaController>/5
        [HttpGet("{id}")]
        public ActionResult<Encomenda> Get(int id)
        {
            var encomenda = _dataContext.Encomenda.FirstOrDefault(x => x.Id == id);
            if (encomenda == null)
            {
                return BadRequest("ID não existente");
            }
            return encomenda;
        }

        // POST api/<EncomendaController>
        [HttpPost]
        public ActionResult<Encomenda> Post([FromBody] EncomendaRequest encomendaRequest)
        {
            if (ModelState.IsValid)
            {
                var encomenda = encomendaRequest.toModel();
                _dataContext.Encomenda.Add(encomenda);
                _dataContext.SaveChanges();
                return CreatedAtAction(nameof(Get), new { id = encomenda.Id }, encomenda);
            }
            return BadRequest(ModelState);
        }

        // PUT api/<EncomendaController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] EncomendaRequest encomendaRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var encomendaExistente = _dataContext.Encomenda.FirstOrDefault(x => x.Id == id);
            if (encomendaExistente == null)
            {
                return NotFound("Encomenda não encontrado");
            }

            // Atualizar encomendaExistente com os dados de encomendaRequest
            encomendaExistente.ClienteId = encomendaRequest.ClienteId;
            encomendaExistente.LojaId = encomendaRequest.LojaId;
            encomendaExistente.Kg = encomendaRequest.Kg;
            encomendaExistente.LatitudeOrigem = encomendaRequest.LatitudeOrigem;
            encomendaExistente.LongitudeOrigem = encomendaRequest.LongitudeOrigem;
            encomendaExistente.LatitudeDestino = encomendaRequest.LatitudeDestino;
            encomendaExistente.LongitudeDestino = encomendaRequest.LongitudeDestino;
            encomendaExistente.Distancia = encomendaRequest.Distancia;
            encomendaExistente.Duracao = encomendaRequest.Duracao;
            encomendaExistente.Status = encomendaRequest.Status;
            // Outros campos a serem atualizados

            _dataContext.Encomenda.Update(encomendaExistente);
            _dataContext.SaveChanges();

            return NoContent();
        }

        // DELETE api/<EncomendaController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var encomenda = _dataContext.Encomenda.FirstOrDefault(x => x.Id == id);
            if (encomenda == null)
            {
                return NotFound("Encomenda não encontrado");
            }

            _dataContext.Encomenda.Remove(encomenda);
            _dataContext.SaveChanges();

            return NoContent();
        }
        [HttpDelete]
        public IActionResult DeleteAll()
        {
            try
            {
                var encomendas = _dataContext.Encomenda.ToList();
                if (encomendas.Count == 0)
                {
                    return NotFound("Nenhuma encomenda encontrada para apagar");
                }

                _dataContext.Encomenda.RemoveRange(encomendas);
                _dataContext.SaveChanges();
                _dataContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Encomenda', RESEED, 0)");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao apagar todas as encomendas: {ex.Message}");
            }
        }
    }
}
