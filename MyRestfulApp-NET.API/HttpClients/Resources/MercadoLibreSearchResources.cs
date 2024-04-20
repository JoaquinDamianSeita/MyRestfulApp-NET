using Newtonsoft.Json;

namespace MyRestfulApp_NET.HttpClients.Resources;

public class Seller
{
    public int? Id { get; set; }
}

public class Paging
{
    public int? Total { get; set; }

    [JsonProperty("primary_results")]
    public int? PrimaryResults { get; set; }
    public int? Offset { get; set; }
    public int? Limit { get; set; }
}

public class Result
{
    public string? Id { get; set; }
    public string? Title { get; set; }

    [JsonProperty("site_id")]
    public string? SiteId { get; set; }
    public double? Price { get; set; }
    public Seller? Seller { get; set; }
    public string? Permalink { get; set; }
}

public class Sort
{
    public string? Id { get; set; }
    public string? Name { get; set; }
}

public class AvailableSort
{
    public string? Id { get; set; }
    public string? Name { get; set; }
}

public class Filter
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public List<FilterObjects>? Values { get; set; }
}

public class FilterObjects
{
    public string? Id { get; set; }
    public string? Name { get; set; }

    [JsonProperty(propertyName: "path_from_root")]
    public List<FilterObjectsPathFromRoot>? PathFromRoot { get; set; }
}

public class FilterObjectsPathFromRoot
{
    public string? Id { get; set; }
    public string? Name { get; set; }
}

public class AvailableFilter
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public List<AvailableFilterObjectValues>? Values { get; set; }
}

public class AvailableFilterObjectValues
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public int? Results { get; set; }
}

public class PdpTracking
{
    public bool? Group { get; set; }

    [JsonProperty(propertyName: "product_info")]
    public List<ProductInfo>? ProductInfo { get; set; }
}

public class ProductInfo
{
    public string? Id { get; set; }
    public int? Score { get; set; }
    public string? Status { get; set; }
}

public class SearchResult
{
    [JsonProperty("site_id")]
    public string? SiteId { get; set; }

    [JsonProperty("country_default_time_zone")]
    public string? CountryDefaultTimeZone { get; set; }
    public string? Query { get; set; }
    public Paging? Paging { get; set; }
    public List<Result>? Results { get; set; }
    public Sort? Sort { get; set; }

    [JsonProperty("available_sorts")]
    public List<AvailableSort>? AvailableSorts { get; set; }
    public List<Filter>? Filters { get; set; }

    [JsonProperty("available_filters")]
    public List<AvailableFilter>? AvailableFilters { get; set; }

    [JsonProperty("pdp_tracking")]
    public PdpTracking? PdpTracking { get; set; }
}
