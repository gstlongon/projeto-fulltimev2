using WebApplication1.Model;

namespace WebApplication1.DTO
{
    public class MotoristaRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string TipoVeiculo { get; set; }
        public string PlacaVeiculo { get; set; }

        public string Cpf { get; set; }

        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }


        public Motorista toModel() 
            => new Motorista(Nome, Email, Cpf, Telefone, TipoVeiculo, PlacaVeiculo, Logradouro, Bairro, Cep, Numero, Cidade, Estado);
    }
}
