using MyRestfulApp_NET_API.HttpClients.Resources;

namespace MyRestfulApp_NET_API.Domain.Services;

public interface IBusquedaService
{
    Task<SearchResult> ObtenerInformacionTermino(string term);
}
