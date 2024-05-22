using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class Endereco
    {
        [Key]
        public int Id { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public int ClienteId { get; set; } 
        public Cliente Cliente { get; set; }

        public Endereco(string logradouro, string bairro, string cep, string numero, string cidade, string estado, int id)
        {
            Logradouro = logradouro;
            Bairro = bairro;
            Cep = cep;
            Numero = numero;
            Cidade = cidade;
            Estado = estado;
            ClienteId = id;
        }

  
    }
}
