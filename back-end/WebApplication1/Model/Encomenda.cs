using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Model
{
    public class Encomenda
    {
        [Key]
        public int Id { get; set; }
        public int ClienteId { get; set; } 
        public int LojaId { get; set; }
        public int Kg { get; set; }
        public float LatitudeOrigem { get; set; }
        public float LongitudeOrigem { get; set; }
        public float LatitudeDestino { get; set; }
        public float LongitudeDestino { get; set; }
        public string Distancia { get; set; }
        public string Duracao { get; set; }
        public string Status { get; set; }



        public Encomenda(int clienteId, int lojaId, int kg, float latitudeOrigem, float longitudeOrigem, float latitudeDestino, float longitudeDestino, string distancia, string duracao, string status)
        {
            ClienteId = clienteId;
            LojaId = lojaId;
            Kg = kg;
            LatitudeOrigem = latitudeOrigem;
            LongitudeOrigem = longitudeOrigem;
            LatitudeDestino = latitudeDestino;
            LongitudeDestino = longitudeDestino;

            Distancia = distancia;
            Duracao = duracao;
            Status = status;

        }

  
    }
}
