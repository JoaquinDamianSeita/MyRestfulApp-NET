using MyRestfulApp_NET.HttpClients.Resources;

namespace MyRestfulApp_NET.Domain.Services;

public interface IPaisesService
{
    Task<Country> ObtenerInformacionPais(string codigoPais);
}
