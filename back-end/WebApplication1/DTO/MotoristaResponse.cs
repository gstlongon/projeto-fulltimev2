using WebApplication1.Model;

namespace WebApplication1.DTO
{
    public class MotoristaResponse
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }

        public string Telefone { get; set; }
        public string TipoVeiculo { get; set; }
        public string PlacaVeiculo { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public MotoristaResponse(Motorista motorista)
        {
            Nome = motorista.Nome;
            Email = motorista.Email;
            Cpf = motorista.Cpf;
            Telefone = motorista.Telefone;
            TipoVeiculo = motorista.TipoVeiculo;
            PlacaVeiculo = motorista.PlacaVeiculo;
            Logradouro = motorista.Logradouro;
            Bairro = motorista.Bairro;
            Cep = motorista.Cep;
            Numero = motorista.Numero;
            Cidade = motorista.Cidade;
            Estado = motorista.Estado;
        }
    }
}
