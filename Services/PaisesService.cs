namespace MyRestfulApp_NET.Domain.Services;

public class PaisesService : IPaisesService
{
    private readonly IMercadoLibreClient _mercadoLibreClient;

    public PaisesService(IMercadoLibreClient mercadoLibreClient)
    {
        _mercadoLibreClient = mercadoLibreClient;
    }

    public async Task<Country> ObtenerInformacionPais(string codigoPais)
    {
        return await _mercadoLibreClient.GetCountryInfo(codigoPais);
    }
}
