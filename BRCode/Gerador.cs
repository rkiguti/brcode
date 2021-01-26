using System;
using System.Globalization;
using System.Text;
using QRCoder;

namespace BRCode
{
    public class Gerador
    {
        public string GerarEmv(Pix pix)
        {
            var emvStringBuilder = new StringBuilder();

            // Arranjo 00 (versão do payload QRCPS-MPM, fixo em “01”)
            AdicionarPayloadFormatIndicator(emvStringBuilder);
            // Arranjo 01 (Se o valor 12 estiver presente, significa que o BR Code só pode ser utilizado uma vez)
            AdicionarPointOfInitiationMethod(emvStringBuilder, pix);
            // Arranjo 26-51 (Dados do Pix)
            AdicionarMerchantAccountInformation(emvStringBuilder, pix);
            // Arranjo 52 (“0000” ou MCC ISO18245)
            AdicionarMerchantCategoryCode(emvStringBuilder);
            // Arranjo 53 (“986” – BRL: real brasileiro - ISO4217)
            AdicionarTransactionCurrency(emvStringBuilder);
            // Arranjo 54 (valor da transação. Ex.: “0”, “1.00”, “123.99”)
            AdicionarTransactionAmount(emvStringBuilder, pix);
            // Arranjo 58 (“BR” – Código de país ISO3166-1 alpha 2)
            AdicionarCountryCode(emvStringBuilder);
            // Arranjo 59 (nome do beneficiário/recebedor)
            AdicionarMerchantName(emvStringBuilder, pix);
            // Arranjo 60 (cidade onde é efetuada a transação)
            AdicionarMerchantCity(emvStringBuilder, pix);
            // Arranjo 61 (CEP da localidade onde é efetuada a transação)
            AdicionarPostalCode(emvStringBuilder, pix);
            // Arranjo 62 (TxId e versão)
            AdicionarAditionalDataFieldTemplate(emvStringBuilder, pix);
            // Arranjo 63 (CRC 16 CCITT)
            AdicionarCrc16(emvStringBuilder);

            return emvStringBuilder.ToString();
        }

        private void AdicionarCrc16(StringBuilder emvStringBuilder)
        {
            emvStringBuilder.Append("6304");

            var crc = new Crc16Ccitt(InitialCrcValue.NonZero1);
            var bytes = Encoding.ASCII.GetBytes(emvStringBuilder.ToString());
            var valor = crc.ComputeChecksum(bytes).ToString("X4");

            emvStringBuilder.Append(valor);
        }

        private void AdicionarAditionalDataFieldTemplate(StringBuilder emvStringBuilder, Pix pix)
        {
            var aditionalDataStringBuilder = new StringBuilder();

            // ID 05 - Reference Label (TxId para QRCode estático - limitado a 25 caracteres)
            if (!string.IsNullOrWhiteSpace(pix.TxId))
                aditionalDataStringBuilder.Append($"05{pix.TxId.Length:D2}{pix.TxId}");

            // ID 50 - Versão BRQ
            aditionalDataStringBuilder.Append(string.Intern("50300017BR.GOV.BCB.BRCODE01051.0.0"));

            var valor = aditionalDataStringBuilder.ToString();

            emvStringBuilder.Append($"62{valor.Length:D2}{valor}");
        }

        private void AdicionarPostalCode(StringBuilder emvStringBuilder, Pix pix)
        {
            if (!string.IsNullOrWhiteSpace(pix.Cep))
                emvStringBuilder.Append($"61{pix.Cep.Length:D2}{pix.Cep}");
        }

        private void AdicionarMerchantCity(StringBuilder emvStringBuilder, Pix pix)
        {
            var cidade = pix.Cidade.Length > 25 ? pix.Cidade.Substring(0, 25) : pix.Cidade;
            emvStringBuilder.Append($"60{cidade.Length:D2}{cidade}");
        }

        private void AdicionarMerchantName(StringBuilder emvStringBuilder, Pix pix)
        {
            var nome = pix.NomeBeneficiario.Length > 25 ?
                pix.NomeBeneficiario.Substring(0, 25) : pix.NomeBeneficiario;
            emvStringBuilder.Append($"59{nome.Length:D2}{nome}");
        }

        private void AdicionarCountryCode(StringBuilder emvStringBuilder)
        {
            emvStringBuilder.Append(string.Intern("5802BR"));
        }

        private void AdicionarTransactionAmount(StringBuilder emvStringBuilder, Pix pix)
        {
            if (pix.Valor.HasValue)
            {
                var valor = pix.Valor.Value.ToString("0.00", CultureInfo.InvariantCulture);
                emvStringBuilder.Append($"54{valor.Length:D2}{valor}");
            }
        }

        private void AdicionarTransactionCurrency(StringBuilder emvStringBuilder)
        {
            emvStringBuilder.Append(string.Intern("5303986"));
        }

        private void AdicionarMerchantCategoryCode(StringBuilder emvStringBuilder)
        {
            emvStringBuilder.Append(string.Intern("52040000"));
        }

        private void AdicionarMerchantAccountInformation(StringBuilder emvStringBuilder, Pix pix)
        {
            var maiStringBuilder = new StringBuilder();

            // ID 00 - GUI
            maiStringBuilder.Append(string.Intern("0014BR.GOV.BCB.PIX"));

            // ID 25 - URL do Payload
            if (!string.IsNullOrWhiteSpace(pix.Location))
                maiStringBuilder.Append($"25{pix.Location.Length:D2}{pix.Location}");
            // ID 01 - Chave PIX do Recebedor
            else if (!string.IsNullOrWhiteSpace(pix.ChavePix))
                maiStringBuilder.Append($"01{pix.ChavePix.Length:D2}{pix.ChavePix}");

            var valor = maiStringBuilder.ToString();

            emvStringBuilder.Append($"26{valor.Length:D2}{valor}");
        }

        private void AdicionarPointOfInitiationMethod(StringBuilder emvStringBuilder, Pix pix)
        {
            if (pix.UtilizadoUmaVez)
                emvStringBuilder.Append(string.Intern("010212"));
        }

        private void AdicionarPayloadFormatIndicator(StringBuilder emvStringBuilder)
        {
            emvStringBuilder.Append(string.Intern("000201"));
        }

        public string GerarQrCode(string emv)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(emv, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(20);

            return Convert.ToBase64String(qrCodeAsPngByteArr);
        }

        public string GerarQrCode(Pix pix)
        {
            return GerarQrCode(GerarEmv(pix));
        }
    }
}
