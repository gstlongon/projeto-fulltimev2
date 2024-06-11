using WebApplication1.Model;

namespace WebApplication1.DTO
{
    public class RotaRequest
    {
        public int ClienteID { get; set; }
        public int MotoristaId { get; set; }
        public int LojaId { get; set; }
        public string Data { get; set; }
        public string Kg { get; set; }
        public string Distancia { get; set; }
        public string Duracao { get; set; }


        public Rota toModel() 
            => new Rota(ClienteID, MotoristaId, LojaId, Data, Kg,  Distancia, Duracao);
    }
}
