using MyRestfulApp_NET.HttpClients.Resources;

namespace MyRestfulApp_NET.Domain.Services;

public interface IBusquedaService
{
    Task<SearchResult> ObtenerInformacionTermino(string term);
}
