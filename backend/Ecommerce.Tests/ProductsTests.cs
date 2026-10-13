using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Ecommerce.Tests;

public class ProductsTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProductsTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCategories_ReturnsNonEmptyArray()
    {
        var response = await _client.GetAsync("/api/products/categories");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(JsonValueKind.Array, result.ValueKind);
        Assert.True(result.GetArrayLength() > 0, "Should return at least one category");
    }

    [Fact]
    public async Task GetProducts_DefaultPage_ReturnsPagedResult()
    {
        var response = await _client.GetAsync("/api/products?page=1&pageSize=10");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadFromJsonAsync<JsonElement>();

        Assert.True(result.TryGetProperty("items", out var items), "Should have 'items' property");
        Assert.Equal(JsonValueKind.Array, items.ValueKind);
        Assert.True(result.TryGetProperty("totalCount", out var total));
        Assert.True(total.GetInt32() > 0, "Total product count should be > 0");
    }

    [Fact]
    public async Task GetProducts_FilterByCategory_ReturnsOnlyCategoryProducts()
    {
        // First get categories to find a valid ID
        var catResponse = await _client.GetAsync("/api/products/categories");
        var cats = await catResponse.Content.ReadFromJsonAsync<JsonElement>();
        var firstCatId = cats[0].GetProperty("id").GetInt32();

        var response = await _client.GetAsync($"/api/products?categoryId={firstCatId}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        var items = result.GetProperty("items");
        foreach (var item in items.EnumerateArray())
        {
            Assert.Equal(firstCatId, item.GetProperty("categoryId").GetInt32());
        }
    }

    [Fact]
    public async Task GetProducts_SearchQuery_ReturnsMatchingProducts()
    {
        var response = await _client.GetAsync("/api/products?search=headphones");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(result.TryGetProperty("items", out _));
    }

    [Fact]
    public async Task GetProducts_SortByPriceAsc_ReturnsOrderedResults()
    {
        var response = await _client.GetAsync("/api/products?sort=price-asc&pageSize=5");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<JsonElement>();
        var items = result.GetProperty("items").EnumerateArray().ToList();
        for (int i = 1; i < items.Count; i++)
        {
            var prev = items[i - 1].GetProperty("price").GetDecimal();
            var curr = items[i].GetProperty("price").GetDecimal();
            Assert.True(prev <= curr, "Products should be sorted by ascending price");
        }
    }

    [Fact]
    public async Task GetProduct_ByValidId_ReturnsProduct()
    {
        // Get the first product from the list
        var listResponse = await _client.GetAsync("/api/products?pageSize=1");
        var list = await listResponse.Content.ReadFromJsonAsync<JsonElement>();
        var id = list.GetProperty("items")[0].GetProperty("id").GetInt32();

        var response = await _client.GetAsync($"/api/products/{id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var product = await response.Content.ReadFromJsonAsync<JsonElement>();
        Assert.Equal(id, product.GetProperty("id").GetInt32());
        Assert.True(product.TryGetProperty("name", out _));
        Assert.True(product.TryGetProperty("price", out _));
    }

    [Fact]
    public async Task GetProduct_ByInvalidId_ReturnsNotFound()
    {
        var response = await _client.GetAsync("/api/products/999999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetProducts_Pagination_WorksCorrectly()
    {
        var page1 = await _client.GetAsync("/api/products?page=1&pageSize=5");
        var page2 = await _client.GetAsync("/api/products?page=2&pageSize=5");

        var p1Data = await page1.Content.ReadFromJsonAsync<JsonElement>();
        var p2Data = await page2.Content.ReadFromJsonAsync<JsonElement>();

        var p1Items = p1Data.GetProperty("items").EnumerateArray().Select(i => i.GetProperty("id").GetInt32()).ToList();
        var p2Items = p2Data.GetProperty("items").EnumerateArray().Select(i => i.GetProperty("id").GetInt32()).ToList();

        // Ensure pages don't overlap
        Assert.Empty(p1Items.Intersect(p2Items));
    }
}
