
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.ConstrainedExecution;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class Loja
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cnpj { get; set; }
        public string Telefone { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public Loja(string nome, string email, string cnpj, string telefone, string logradouro, string bairro, string cep, string numero, string cidade, string estado, float latitude, float longitude)
        {
            Nome = nome;
            Email = email;
            Cnpj = cnpj;
            Telefone = telefone;
            Logradouro = logradouro;
            Bairro = bairro;
            Cep = cep;
            Numero = numero;
            Cidade = cidade;
            Estado = estado;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
