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
    public class LojaController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public LojaController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        // GET: api/<LojaController>
        [HttpGet]
        public ActionResult<List<Loja>> Get()
        {
            try
            {
                var lojas = _dataContext.Loja.OrderByDescending(c => c.Id).ToList();
                return Ok(lojas);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados: " + ex.Message);
            }
        }
        [HttpGet("b={busca}")]
        public ActionResult<List<Loja>> Get(string busca)
        {
            try
            {
                if (string.IsNullOrEmpty(busca))
                {
                    var lojas = _dataContext.Loja.OrderByDescending(c => c.Id).ToList();
                    return Ok(lojas);
                }
                else
                {
                    var lojas = _dataContext.Loja.Where(c =>
                        c.Nome.Contains(busca)
                    ).ToList();
                    return Ok(lojas);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao acessar a base de dados: " + ex.Message);
            }
        }


        // GET api/<LojaController>/5
        [HttpGet("{id}")]
        public ActionResult<Loja> Get(int id)
        {
            var loja = _dataContext.Loja.FirstOrDefault(x => x.Id == id);
            if (loja == null)
            {
                return BadRequest("ID não existente");
            }
            return loja;
        }

        // POST api/<LojaController>
        [HttpPost]
        public ActionResult<Loja> Post([FromBody] LojaRequest lojaRequest)
        {
            if (ModelState.IsValid)
            {
                var loja = lojaRequest.toModel();
                _dataContext.Loja.Add(loja);
                _dataContext.SaveChanges();
                return CreatedAtAction(nameof(Get), new { id = loja.Id }, loja);
            }
            return BadRequest(ModelState);
        }

        // PUT api/<LojaController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] LojaRequest lojaRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var lojaExistente = _dataContext.Loja.FirstOrDefault(x => x.Id == id);
            if (lojaExistente == null)
            {
                return NotFound("Loja não encontrado");
            }

            // Atualizar lojaExistente com os dados de lojaRequest
            lojaExistente.Nome = lojaRequest.Nome;
            lojaExistente.Email = lojaRequest.Email;
            lojaExistente.Cnpj = lojaRequest.Cnpj;
            lojaExistente.Telefone = lojaRequest.Telefone;
            lojaExistente.Logradouro = lojaRequest.Logradouro;
            lojaExistente.Bairro = lojaRequest.Bairro;
            lojaExistente.Cep = lojaRequest.Cep;
            lojaExistente.Numero = lojaRequest.Numero;
            lojaExistente.Cidade = lojaRequest.Cidade;
            lojaExistente.Estado = lojaRequest.Estado;
            // Outros campos a serem atualizados

            _dataContext.Loja.Update(lojaExistente);
            _dataContext.SaveChanges();

            return NoContent();
        }

        // DELETE api/<LojaController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var loja = _dataContext.Loja.FirstOrDefault(x => x.Id == id);
            if (loja == null)
            {
                return NotFound("Loja não encontrado");
            }

            _dataContext.Loja.Remove(loja);
            _dataContext.SaveChanges();

            return NoContent();
        }
        [HttpDelete]
        public IActionResult DeleteAll()
        {
            try
            {
                var lojas = _dataContext.Loja.ToList();
                if (lojas.Count == 0)
                {
                    return NotFound("Nenhuma loja encontrada para apagar");
                }

                _dataContext.Loja.RemoveRange(lojas);
                _dataContext.SaveChanges();
                _dataContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Loja', RESEED, 0)");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao apagar todas as lojas: {ex.Message}");
            }
        }
    }
}
