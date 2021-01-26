﻿using System;
using NUnit.Framework;

namespace BRCode.Testes
{
    public class GeradorQrCodeTest
    {
        private Gerador _gerador;

        [SetUp]
        public void SetUp()
        {
            _gerador = new Gerador();
        }

        [Test]
        public void Gerar_QrCodeDinamico_ValidaImagem()
        {
            var emv = "00020101021226710014BR.GOV.BCB.PIX2549banco.com.br/pix/5abe9dc1980943d88ffa87c5911ac17052040000530398654071000.005802BR5906Fulano6003SJC610812120000623450300017BR.GOV.BCB.BRCODE01051.0.063045FE2";
            var expected = "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";

            var actual = _gerador.GerarQrCode(emv);

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}