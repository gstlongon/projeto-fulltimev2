
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.ConstrainedExecution;
using Microsoft.IdentityModel.Tokens.Configuration;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class Trajeto
    {
        [Key]
        public int Id { get; set; }
        public int MotoristaId { get; set; }
        public string DataInicial { get; set; }
        public string Encomendas { get; set; }
        public int PesoTotal { get; set; }
        public string DataFinal { get; set; }
        public string Distancia { get; set; }
        public string LinhaMapa { get; set; }

        public Trajeto(int motoristaId, string dataInicial, string encomendas, int pesoTotal, string dataFinal, string distancia, string linhaMapa)
        {
            MotoristaId = motoristaId;
            DataInicial = dataInicial;
            Encomendas = encomendas;
            PesoTotal = pesoTotal;
            DataFinal = dataFinal;
            Distancia = distancia;
            LinhaMapa = linhaMapa;
        }
    }
}
