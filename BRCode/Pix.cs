using System;
namespace BRCode
{
    public class Pix
    {
        public bool UtilizadoUmaVez { get; set; }

        /// <summary>
        /// Utilizado para o QRCode estático
        /// </summary>
        public string TxId { get; set; }

        public string ChavePix { get; set; }

        public string Location { get; set; }

        public decimal? Valor { get; set; }

        public string NomeBeneficiario { get; set; }

        public string Cidade { get; set; }

        public string Cep { get; set; }
    }
}
