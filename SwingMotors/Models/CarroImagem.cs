using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvaliacaoFinalWestn.Models
{
    public class CarroImagem
    {
        public CarroImagem(int carroId, byte[] imagem, string contentType)
        {
            CarroId = carroId;
            Imagem = imagem;
            ContentType = contentType;

        }

        public CarroImagem()
        {
            
        }

        public int Id { get; set; }
        public int CarroId { get; set; }
        public Carro Carro { get; set; }

        public byte[] Imagem { get; set; }
        public string ContentType { get; set; }


    }
}