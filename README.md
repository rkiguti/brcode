# brcode
Criação do QRCode para o Pix em C# (.NET Standard)
- Geração do EMV utilizado para o Pix copia e cola
- Geração da imagem do QRCode

A classe foi desenvolvida de forma simplificada para atender o mínimo necessário, que atende a grande maioria dos casos. O padrão EMV do Pix possui vários outros campos opcionais que podem ser utilizados que não foram implementados.

Para a geração da imagem do QRCode foi utilizado o pacote nuget QRCoder. O método de geração do QRCode retorna uma imagem PNG no formato base64.

### Exemplo geração de QRCode Estático:

```c#
var gerador = new Gerador();

var pix = new Pix
{
    ChavePix = "9cc115c6-fe66-4ac5-bcd4-015b0605b0b6",
    NomeBeneficiario = "Fulano",
    UtilizadoUmaVez = false,
    Cidade = "SJC",
    Cep = "12120000"
};

var emv = gerador.GerarEmv(pix);
var imageBase64 = gerador.GerarQrCode(pix);
```

### Exemplo geração de QRCode Dinâmico:

```c#
var gerador = new Gerador();

var pix = new Pix
{
    Location = "banco.com.br/pix/5abe9dc1980943d88ffa87c5911ac",
    NomeBeneficiario = "Fulano",
    UtilizadoUmaVez = true,
    Cidade = "SJC",
    Valor = 1000,
    Cep = "12120000"
};

var emv = gerador.GerarEmv(pix);
var imageBase64 = gerador.GerarQrCode(pix);
```

**Observação:** O campo location se refere à URL do Payload retornada no campo Location das APIs dos bancos.
