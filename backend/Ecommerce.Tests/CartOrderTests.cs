using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Ecommerce.Tests;

public class CartOrderTests : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CartOrderTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<string> LoginAndGetTokenAsync(string email = "john@example.com", string password = "Customer123!")
    {
        var response = await _client.PostAsJsonAsync("/api/auth/login", new { email, password });
        var data = await response.Content.ReadFromJsonAsync<JsonElement>();
        return data.GetProperty("accessToken").GetString()!;
    }

    private void SetAuthToken(string token) =>
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    private void ClearAuth() =>
        _client.DefaultRequestHeaders.Authorization = null;

    [Fact]
    public async Task GetCart_Unauthenticated_ReturnsUnauthorized()
    {
        ClearAuth();
        var response = await _client.GetAsync("/api/cart");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AddToCart_AndGetCart_ReturnsCartWithItem()
    {
        var token = await LoginAndGetTokenAsync();
        SetAuthToken(token);

        // Get a product ID
        var productsResponse = await _client.GetAsync("/api/products?pageSize=1");
        var products = await productsResponse.Content.ReadFromJsonAsync<JsonElement>();
        var productId = products.GetProperty("items")[0].GetProperty("id").GetInt32();

        // Add to cart
        var addResponse = await _client.PostAsJsonAsync("/api/cart/items", new { productId, qty = 2 });
        Assert.Equal(HttpStatusCode.OK, addResponse.StatusCode);

        // Verify cart
        var cartResponse = await _client.GetAsync("/api/cart");
        Assert.Equal(HttpStatusCode.OK, cartResponse.StatusCode);

        var cart = await cartResponse.Content.ReadFromJsonAsync<JsonElement>();
        Assert.True(cart.TryGetProperty("items", out var items));
        Assert.True(items.GetArrayLength() > 0, "Cart should have at least one item");

        ClearAuth();
    }

    [Fact]
    public async Task ApplyCoupon_ValidCode_ReducesTotal()
    {
        var token = await LoginAndGetTokenAsync();
        SetAuthToken(token);

        // Add an item first
        var productsResponse = await _client.GetAsync("/api/products?pageSize=1&sort=price-desc");
        var products = await productsResponse.Content.ReadFromJsonAsync<JsonElement>();
        var productId = products.GetProperty("items")[0].GetProperty("id").GetInt32();
        await _client.PostAsJsonAsync("/api/cart/items", new { productId, qty = 1 });

        // Apply coupon
        var couponResponse = await _client.PostAsJsonAsync("/api/cart/coupon", new { couponCode = "FREESHIP" });
        Assert.Equal(HttpStatusCode.OK, couponResponse.StatusCode);

        // Check shipping is 0
        var cartResponse = await _client.GetAsync("/api/cart");
        var cart = await cartResponse.Content.ReadFromJsonAsync<JsonElement>();
        var shipping = cart.GetProperty("shipping").GetDecimal();
        Assert.Equal(0m, shipping);

        ClearAuth();
    }

    [Fact]
    public async Task ApplyCoupon_InvalidCode_ReturnsBadRequest()
    {
        var token = await LoginAndGetTokenAsync();
        SetAuthToken(token);

        var response = await _client.PostAsJsonAsync("/api/cart/coupon", new { couponCode = "FAKECODE" });
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        ClearAuth();
    }

    [Fact]
    public async Task UpdateCartItem_QuantityZero_RemovesItem()
    {
        var token = await LoginAndGetTokenAsync();
        SetAuthToken(token);

        // Add item
        var productsResponse = await _client.GetAsync("/api/products?pageSize=1");
        var products = await productsResponse.Content.ReadFromJsonAsync<JsonElement>();
        var productId = products.GetProperty("items")[0].GetProperty("id").GetInt32();
        await _client.PostAsJsonAsync("/api/cart/items", new { productId, qty = 2 });

        // Update to qty=0 (should remove)
        var updateResponse = await _client.PutAsJsonAsync($"/api/cart/items/{productId}", new { qty = 0 });
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

        ClearAuth();
    }

    [Fact]
    public async Task CartPricing_GST18Percent_IsCorrect()
    {
        var token = await LoginAndGetTokenAsync();
        SetAuthToken(token);

        // Clear cart first
        await _client.DeleteAsync("/api/cart");

        // Add a product with a known price
        var productsResponse = await _client.GetAsync("/api/products?pageSize=1&sort=price-asc");
        var products = await productsResponse.Content.ReadFromJsonAsync<JsonElement>();
        var product = products.GetProperty("items")[0];
        var productId = product.GetProperty("id").GetInt32();
        var price = product.GetProperty("price").GetDecimal();

        await _client.PostAsJsonAsync("/api/cart/items", new { productId, qty = 1 });

        var cartResponse = await _client.GetAsync("/api/cart");
        var cart = await cartResponse.Content.ReadFromJsonAsync<JsonElement>();

        var subtotal = cart.GetProperty("subtotal").GetDecimal();
        var tax = cart.GetProperty("tax").GetDecimal();

        // GST should be 18%
        var expectedTax = Math.Round(subtotal * 0.18m, 2);
        Assert.Equal(expectedTax, tax);

        ClearAuth();
    }

    [Fact]
    public async Task GetOrders_Unauthenticated_ReturnsUnauthorized()
    {
        ClearAuth();
        var response = await _client.GetAsync("/api/orders");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
