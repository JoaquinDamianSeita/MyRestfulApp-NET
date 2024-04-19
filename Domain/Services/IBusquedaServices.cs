public interface IBusquedaService
{
    Task<SearchResult> ObtenerInformacionTermino(string term);
}
