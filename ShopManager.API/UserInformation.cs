using System.Security.Claims;
using Newtonsoft.Json;

namespace ShopManager.API;

public class UserInformation
{
    public UserInformation(string userName, Guid userId, string role)
    {
        UserName = userName;
        UserId = userId;
        Role = role;
    }

    [JsonProperty(ClaimTypes.Name)]
    public string UserName { get; init; }

    [JsonProperty(ClaimTypes.NameIdentifier)]
    public Guid UserId { get; init; }

    [JsonProperty(ClaimTypes.Role)]
    public string Role { get; init; }
}