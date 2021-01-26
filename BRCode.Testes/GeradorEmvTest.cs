using System;
using NUnit.Framework;

namespace BRCode.Testes
{
    public class GeradorEmvTest
    {
        private Gerador _gerador;

        [SetUp]
        public void Setup()
        {
            _gerador = new Gerador();
        }

        [Test]
        public void Gerar_QrCodeDinamico_ValidaBRCode()
        {
            // Arrange
            var pix = new Pix
            {
                Location = $"banco.com.br/pix/5abe9dc1980943d88ffa87c5911ac",
                NomeBeneficiario = "Fulano",
                UtilizadoUmaVez = true,
                Cidade = "SJC",
                Valor = 1000,
                Cep = "12120000"
            };

            var expected = "00020101021226680014BR.GOV.BCB.PIX2546banco.com.br/pix/5abe9dc1980943d88ffa87c5911ac52040000530398654071000.005802BR5906Fulano6003SJC610812120000623450300017BR.GOV.BCB.BRCODE01051.0.063045552";

            // Act
            var actual = _gerador.GerarEmv(pix);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Gerar_QrCodeEstatico_ValidaBRCode()
        {
            // Arrange
            var pix = new Pix
            {
                ChavePix = "9cc115c6-fe66-4ac5-bcd4-015b0605b0b6",
                TxId = "1b95872004384c66ba16f2d2c",
                NomeBeneficiario = "Fulano",
                UtilizadoUmaVez = false,
                Cidade = "SJC",
                Cep = "12120000"
            };

            var expected = "00020126580014BR.GOV.BCB.PIX01369cc115c6-fe66-4ac5-bcd4-015b0605b0b65204000053039865802BR5906Fulano6003SJC610812120000626305251b95872004384c66ba16f2d2c50300017BR.GOV.BCB.BRCODE01051.0.063045EA0";

            // Act
            var actual = _gerador.GerarEmv(pix);

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}