using WebApplication1.Model;

namespace WebApplication1.DTO
{
    public class EncomendaRequest
    {
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


        public Encomenda toModel()
            => new Encomenda(ClienteId, LojaId, Kg, LatitudeOrigem, LongitudeOrigem, LatitudeDestino, LongitudeDestino, Distancia, Duracao, Status);
    }
}
