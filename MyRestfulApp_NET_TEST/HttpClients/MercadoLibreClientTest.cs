using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using MyRestfulApp_NET_API.Common.Exceptions;
using MyRestfulApp_NET_API.Domain.HttpClients;
using MyRestfulApp_NET_API.HttpClients.Resources;
using Newtonsoft.Json;
using Xunit;

namespace MyRestfulApp_NET_TEST.HttpClients;

public class MercadoLibreClientFixture
{
    public Mock<IConfiguration> Configuration { get; set; }
    public MercadoLibreClientFixture()
    {
        var mockConfSection = new Mock<IConfigurationSection>();
        mockConfSection.Setup(m => m.Value).Returns("https://test.com/");

        Configuration = new Mock<IConfiguration>();
        Configuration.Setup(c => c.GetSection(It.IsAny<string>())).Returns(mockConfSection.Object);
    }
}

public class MercadoLibreClientTest : IClassFixture<MercadoLibreClientFixture>
{
    public MercadoLibreClientFixture _fixture;
    public MercadoLibreClientTest(MercadoLibreClientFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task MercadoLibreClientGetCountryInfoOk()
    {
        var expectedCountry = new Country { Id = "US", Name = "United States" };
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(expectedCountry))
        };
        var httpClient = MockHttpClientWithHttpResponse(responseMessage);

        var mercadoLibreClient = new MercadoLibreClient(httpClient, _fixture.Configuration.Object);

        var result = await mercadoLibreClient.GetCountryInfo("US");

        Assert.Equal(expectedCountry.Id, result.Id);
        Assert.Equal(expectedCountry.Name, result.Name);
    }

    [Fact]
    public async Task MercadoLibreClientGetCountryInfoError()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent(JsonConvert.SerializeObject(new { message = "Country not found" }))
        };
        var httpClient = MockHttpClientWithHttpResponse(responseMessage);
        var mercadoLibreClient = new MercadoLibreClient(httpClient, _fixture.Configuration.Object);

        await Assert.ThrowsAsync<ExternalApiException>(() => mercadoLibreClient.GetCountryInfo("US"));
    }

    [Fact]
    public async Task MercadoLibreClientGetTermInfoOk()
    {
        var expectedSearchResult = new SearchResult { Query = "test", Results = new List<Result>() };
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(expectedSearchResult))
        };
        var httpClient = MockHttpClientWithHttpResponse(responseMessage);
        var mercadoLibreClient = new MercadoLibreClient(httpClient, _fixture.Configuration.Object);

        var result = await mercadoLibreClient.GetTermInfo("test");

        Assert.Equal(expectedSearchResult.Query, result.Query);
        Assert.Empty(result.Results);
    }

    [Fact]
    public async Task MercadoLibreClientGetTermInfoError()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent(JsonConvert.SerializeObject(new { message = "Term not found" }))
        };
        var httpClient = MockHttpClientWithHttpResponse(responseMessage);
        var mercadoLibreClient = new MercadoLibreClient(httpClient, _fixture.Configuration.Object);

        await Assert.ThrowsAsync<ExternalApiException>(() => mercadoLibreClient.GetTermInfo("test"));
    }

    [Fact]
    public async Task MercadoLibreClientGetCurrenciesOk()
    {
        var expectedCurrencies = new List<Currency>
        {
            new() { Id = "USD", Description = "United States Dollar" },
            new() { Id = "EUR", Description = "Euro" }
        };
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(expectedCurrencies))
        };
        var httpClient = MockHttpClientWithHttpResponse(responseMessage);
        var mercadoLibreClient = new MercadoLibreClient(httpClient, _fixture.Configuration.Object);

        var result = await mercadoLibreClient.GetCurrencies();

        Assert.Equal(expectedCurrencies.Count, result.Count);
        Assert.Equal(expectedCurrencies[1].Id, result[0].Id);
        Assert.Equal(expectedCurrencies[1].Description, result[0].Description);
        Assert.Equal(expectedCurrencies[0].Id, result[1].Id);
        Assert.Equal(expectedCurrencies[0].Description, result[1].Description);
    }

    [Fact]
    public async Task MercadoLibreClientGetCurrenciesError()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent(JsonConvert.SerializeObject(new { message = "Currencies not found" }))
        };
        var httpClient = MockHttpClientWithHttpResponse(responseMessage);
        var mercadoLibreClient = new MercadoLibreClient(httpClient, _fixture.Configuration.Object);

        await Assert.ThrowsAsync<ExternalApiException>(() => mercadoLibreClient.GetCurrencies());
    }

    [Fact]
    public async Task MercadoLibreClientGetCurrencyConversionOk()
    {
        var expectedCurrencyConversion = new CurrencyConversion {  Ratio = 0.85M };
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(JsonConvert.SerializeObject(expectedCurrencyConversion))
        };
        var httpClient = MockHttpClientWithHttpResponse(responseMessage);
        var mercadoLibreClient = new MercadoLibreClient(httpClient, _fixture.Configuration.Object);

        var result = await mercadoLibreClient.GetCurrencyConversion("USD", "EUR");

        Assert.Equal(expectedCurrencyConversion.Ratio, result.Ratio);
    }

    [Fact]
    public async Task MercadoLibreClientGetCurrencyConversionError()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent(JsonConvert.SerializeObject(new { message = "Currency conversion not found" }))
        };
        var httpClient = MockHttpClientWithHttpResponse(responseMessage);
        var mercadoLibreClient = new MercadoLibreClient(httpClient, _fixture.Configuration.Object);

        var result = await mercadoLibreClient.GetCurrencyConversion("USD", "EUR");

        Assert.Equal(0, result.Ratio);
    }

    private static HttpClient MockHttpClientWithHttpResponse(HttpResponseMessage responseMessage)
    {
        responseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        return new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("http://test.com/"),
        };
    }

}
