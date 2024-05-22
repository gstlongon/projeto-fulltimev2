using WebApplication1.Model;

namespace WebApplication1.DTO
{
    public class ClienteResponse
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }

        public string Telefone { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public ClienteResponse(Cliente cliente)
        {
            Nome = cliente.Nome;
            Email = cliente.Email;
            Cpf = cliente.Cpf;
            Telefone = cliente.Telefone;
            Logradouro = cliente.Logradouro;
            Bairro = cliente.Bairro;
            Cep = cliente.Cep;
            Numero = cliente.Numero;
            Cidade = cliente.Cidade;
            Estado = cliente.Estado;
        }
    }
}
