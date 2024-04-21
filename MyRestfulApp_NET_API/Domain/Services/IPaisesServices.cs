using MyRestfulApp_NET_API.HttpClients.Resources;

namespace MyRestfulApp_NET_API.Domain.Services;

public interface IPaisesService
{
    Task<Country> ObtenerInformacionPais(string codigoPais);
}
