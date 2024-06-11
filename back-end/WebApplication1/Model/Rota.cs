
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.ConstrainedExecution;
using Microsoft.IdentityModel.Tokens.Configuration;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class Rota
    {
        [Key]
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int MotoristaId { get; set; }
        public int LojaId { get; set; }
        public string Data { get; set; }
        public string Kg { get; set; }   
        public string Distancia { get; set; }
        public string Duracao { get; set; }
        public string DataFinal { get; internal set; }
        public string DataInicial { get; internal set; }
        public int PesoTotal { get; internal set; }

        public Rota(int clienteId, int motoristaId, int lojaId, string data, string kg, string distancia, string duracao)
        {
            ClienteId = clienteId;
            MotoristaId = motoristaId;
            LojaId = lojaId;
            Data = data;
            Kg = kg;
            Distancia = distancia;
            Duracao = duracao;
        }
    }
}
