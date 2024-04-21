using MyRestfulApp_NET_API.Domain.HttpClients;
using MyRestfulApp_NET_API.HttpClients.Resources;

namespace MyRestfulApp_NET_API.Domain.Services;

public class BusquedaService : IBusquedaService
{
    private readonly IMercadoLibreClient _mercadoLibreClient;

    public BusquedaService(IMercadoLibreClient mercadoLibreClient)
    {
        _mercadoLibreClient = mercadoLibreClient;
    }

    public async Task<SearchResult> ObtenerInformacionTermino(string term)
    {
        return await _mercadoLibreClient.GetTermInfo(term);
    }
}
