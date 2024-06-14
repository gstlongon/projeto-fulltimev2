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
    public class TrajetoController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public TrajetoController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET: api/<TrajetoController>
        [HttpGet]
        public ActionResult<List<Trajeto>> Get()
        {
            try
            {
                var trajetos = _dataContext.Trajeto.OrderByDescending(c => c.Id).ToList();
                return Ok(trajetos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados: " + ex.Message);
            }
        }
        [HttpGet("b={busca}")]
        public ActionResult<List<Trajeto>> Get(float busca)
        {
            try
            {
                List<Trajeto> trajetos;

                if (busca == 0)
                {
                    trajetos = _dataContext.Trajeto.OrderByDescending(c => c.Id).ToList();
                }
                else
                {
                    trajetos = _dataContext.Trajeto.Where(c => c.Id == busca).ToList();
                }

                return Ok(trajetos);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados: " + ex.Message);
            }
        }
        // GET api/<TrajetoController>/5
        [HttpGet("{id}")]
        public ActionResult<Trajeto> Get(int id)
        {
            var trajeto = _dataContext.Trajeto.FirstOrDefault(x => x.Id == id);
            if (trajeto == null)
            {
                return BadRequest("ID não existente");
            }
            return trajeto;
        }

        // POST api/<TrajetoController>
        [HttpPost]
        public ActionResult<Trajeto> Post([FromBody] TrajetoRequest trajetoRequest)
        {
            if (ModelState.IsValid)
            {
                var trajeto = trajetoRequest.toModel();
                _dataContext.Trajeto.Add(trajeto);
                _dataContext.SaveChanges();
                return CreatedAtAction(nameof(Get), new { id = trajeto.Id }, trajeto);
            }
            return BadRequest(ModelState);
        }

        // PUT api/<TrajetoController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] TrajetoRequest trajetoRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trajetoExistente = _dataContext.Trajeto.FirstOrDefault(x => x.Id == id);
            if (trajetoExistente == null)
            {
                return NotFound("Trajeto não encontrado");
            }

            // Atualizar trajetoExistente com os dados de trajetoRequest
            trajetoExistente.MotoristaId = trajetoRequest.MotoristaId;
            trajetoExistente.DataInicial = trajetoRequest.DataInicial;
            trajetoExistente.Encomendas = trajetoRequest.Encomendas;
            trajetoExistente.PesoTotal = trajetoRequest.PesoTotal;
            trajetoExistente.DataFinal = trajetoRequest.DataFinal;
            trajetoExistente.Distancia = trajetoRequest.Distancia;
            trajetoExistente.LinhaMapa = trajetoRequest.LinhaMapa;
            // Outros campos a serem atualizados

            _dataContext.Trajeto.Update(trajetoExistente);
            _dataContext.SaveChanges();

            return NoContent();
        }

        // DELETE api/<TrajetoController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var trajeto = _dataContext.Trajeto.FirstOrDefault(x => x.Id == id);
            if (trajeto == null)
            {
                return NotFound("Trajeto não encontrado");
            }

            _dataContext.Trajeto.Remove(trajeto);
            _dataContext.SaveChanges();

            return NoContent();
        }
        [HttpDelete]
        public IActionResult DeleteAll()
        {
            try
            {
                var trajetos = _dataContext.Trajeto.ToList();
                if (trajetos.Count == 0)
                {
                    return NotFound("Nenhuma trajeto encontrado para apagar");
                }

                _dataContext.Trajeto.RemoveRange(trajetos);
                _dataContext.SaveChanges();
                _dataContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Trajeto', RESEED, 0)");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao apagar todas os trajetos: {ex.Message}");
            }
        }
    }
}
