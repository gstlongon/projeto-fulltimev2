using WebApplication1.Model;

namespace WebApplication1.DTO
{
    public class TrajetoRequest
    {
        public int MotoristaId { get; set; }
        public string DataInicial { get; set; }
        public string Encomendas { get; set; }
        public int PesoTotal { get; set; }
        public string DataFinal { get; set; }
        public string Distancia { get; set; }
        public string LinhaMapa { get; set; }


        public Trajeto toModel() 
            => new Trajeto(MotoristaId, DataInicial, Encomendas, PesoTotal, DataFinal,  Distancia,  LinhaMapa);
    }
}
