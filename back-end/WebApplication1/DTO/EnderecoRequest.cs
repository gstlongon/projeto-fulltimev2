using WebApplication1.Model;

namespace WebApplication1.DTO
{
    public class EnderecoRequest
    {
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public int ClienteId { get; set; }

        public Endereco toModel()
            => new Endereco(Logradouro, Bairro, Cep, Numero, Cidade, Estado, ClienteId);
    }
}
