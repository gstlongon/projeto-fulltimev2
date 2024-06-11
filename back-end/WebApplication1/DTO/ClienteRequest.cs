using WebApplication1.Model;

namespace WebApplication1.DTO
{
    public class ClienteRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Cpf { get; set; }

        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }


        public Cliente toModel() 
            => new Cliente(Nome, Email, Cpf, Telefone, Logradouro, Bairro, Cep, Numero, Cidade, Estado, Latitude, Longitude);
    }
}
