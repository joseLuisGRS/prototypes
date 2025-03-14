namespace Test.Controllers;

public class RoleControllerTest(TestContainerFactory<Program> factory) : IClassFixture<TestContainerFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();
    private static string _apiVersion = "v1";

    [Fact]
    public async Task Get_Roles_ReturnsOk_And_ListRoles()
    {
        //Arrange   
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/{_apiVersion}/roles");

        //Act
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var apiResponse = responseString.Deserialize<ApiResponse<IEnumerable<RoleDto>>>();

        //Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Roles retrieved successfully", apiResponse?.Message);
        Assert.True(apiResponse?.Data.ToList().Count > 0);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Get_RoleByName_ReturnOk_And_Role(Int64 id)
    {
        //Arrange   
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/{_apiVersion}/roles/{id}");

        //Act
        var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var apiResponse = responseString.Deserialize<ApiResponse<RoleDto>>();

        //Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Role retrieved successfully", apiResponse?.Message);
        Assert.True(apiResponse?.Data is not null);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    public async Task Get_RoleByName_ReturnNotFound(Int64 id)
    {
        //Arrange   
        var request = new HttpRequestMessage(HttpMethod.Get, $"/api/{_apiVersion}/roles/{id}");

        //Act
        var response = await _client.SendAsync(request);
        var responseString = await response.Content.ReadAsStringAsync();
        var apiResponse = responseString.Deserialize<ApiResponse<RoleDto>>();

        //Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal($"Role isn't found for id {id}", apiResponse?.Message);
    }

    [Fact]
    public async Task Add_Role_ResturnsOk()
    {
        //Arrange   
        var json = CreateRole().Serialize();
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var postRequest = new HttpRequestMessage(HttpMethod.Post, $"/api/{_apiVersion}/roles")
        {
            Content = content
        };

        //Act
        var response = await _client.SendAsync(postRequest);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var apiResponse = responseString.Deserialize<ApiResponse<RoleDto>>();

        //Assert
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("Role created successfully.", apiResponse?.Message);
    }

    [Fact]
    public async Task Add_Role_ResturnsBadRequest()
    {
        //Arrange   
        var json = CreateRoleError().Serialize();
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var postRequest = new HttpRequestMessage(HttpMethod.Post, $"/api/{_apiVersion}/roles")
        {
            Content = content
        };

        //Act
        var response = await _client.SendAsync(postRequest);
        var responseString = await response.Content.ReadAsStringAsync();
        var apiResponse = responseString.Deserialize<ApiResponse<RoleDto>>();

        //Assert
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("Role name already exists", apiResponse?.Message);
    }

    [Fact]
    public async Task Update_Role_ReturnsOk()
    {
        //Arrange   
        var json = UpdateRole().Serialize();
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var putRequest = new HttpRequestMessage(HttpMethod.Put, $"/api/{_apiVersion}/roles")
        {
            Content = content
        };

        //Act
        var response = await _client.SendAsync(putRequest);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var apiResponse = responseString.Deserialize<ApiResponse<RoleDto>>();

        //Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Role updated with success!", apiResponse?.Message);
    }

    [Fact]
    public async Task Update_Role_ReturnsNotFound()
    {
        //Arrange   
        var json = UpdateRoleError().Serialize();
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var putRequest = new HttpRequestMessage(HttpMethod.Put, $"/api/{_apiVersion}/roles")
        {
            Content = content
        };

        //Act
        var response = await _client.SendAsync(putRequest);
        var responseString = await response.Content.ReadAsStringAsync();
        var apiResponse = responseString.Deserialize<ApiResponse<RoleDto>>();

        //Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("Role not found", apiResponse?.Message);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task UpdateSatus_Role_ReturnsOk(Int64 id)
    {
        //Arrange   
        var patchRequest = new HttpRequestMessage(HttpMethod.Patch, $"/api/{_apiVersion}/roles/{id}/status");

        //Act
        var response = await _client.SendAsync(patchRequest);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var apiResponse = responseString.Deserialize<ApiResponse<string>>();

        //Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Role updated successfully.", apiResponse?.Message);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    public async Task UpdateSatus_Role_ReturnsNotFound(Int64 id)
    {
        //Arrange   
        var patchRequest = new HttpRequestMessage(HttpMethod.Patch, $"/api/{_apiVersion}/roles/{id}/status");

        //Act
        var response = await _client.SendAsync(patchRequest);
        var responseString = await response.Content.ReadAsStringAsync();
        var apiResponse = responseString.Deserialize<ApiResponse<string>>();

        //Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("Role not found", apiResponse?.Message);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Delete_Role_ReturnsOk(Int64 id)
    {
        //Arrange   
        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"/api/{_apiVersion}/roles/{id}");

        //Act
        var response = await _client.SendAsync(deleteRequest);
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        var apiResponse = responseString.Deserialize<ApiResponse<string>>();

        //Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Role deleted successfully.", apiResponse?.Message);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(20)]
    public async Task Delete_Role_ReturnsNotFound(Int64 id)
    {
        //Arrange   
        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"/api/{_apiVersion}/roles/{id}");

        //Act
        var response = await _client.SendAsync(deleteRequest);
        var responseString = await response.Content.ReadAsStringAsync();
        var apiResponse = responseString.Deserialize<ApiResponse<string>>();

        //Assert
        Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal("Role not found", apiResponse?.Message);
    }

}
