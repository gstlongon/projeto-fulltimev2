using WebApplication1.Model;

namespace WebApplication1.DTO
{
    public class EnderecoResponse
    {
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public Cliente Cliente { get; set; }

        public EnderecoResponse(Endereco endereco)
        {
            Logradouro = endereco.Logradouro;
            Bairro = endereco.Bairro;
            Cep = endereco.Cep;
            Numero = endereco.Numero;
            Cidade = endereco.Cidade;
            Estado = endereco.Estado;
            Cliente = endereco.Cliente;
        }
    }
}
