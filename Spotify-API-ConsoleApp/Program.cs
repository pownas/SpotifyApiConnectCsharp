using System.Text;
using Newtonsoft.Json.Linq;

class Program
{
    private static readonly HttpClient client = new HttpClient();
    private const string clientId = "YourSpotifyClientId";
    private const string clientSecret = "YourSpotifyClientSecret";

    static async Task Main(string[] args)
    {
        var token = await GetToken();
        //var trackInfo = await GetTrackInfo(token);
        //Console.WriteLine(trackInfo);

        var playlistInfo = await GetPlaylistInfo(token);
        Console.WriteLine(playlistInfo);
    }

    static async Task<string> GetToken()
    {
        var byteArray = Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}");
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

        var content = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        });

        var response = await client.PostAsync("https://accounts.spotify.com/api/token", content);
        var responseString = await response.Content.ReadAsStringAsync();

        return JObject.Parse(responseString)["access_token"].ToString();
    }

    // /// <example>https://developer.spotify.com/documentation/web-api/reference/get-track</example>
    // static async Task<string> GetTrackInfo(string token)
    // {
    //     client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    //     var response = await client.GetAsync("https://api.spotify.com/v1/tracks/4cOdK2wGLETKBW3PvgPWqT");
    //     return await response.Content.ReadAsStringAsync();
    // }

    /// <example>https://developer.spotify.com/documentation/web-api/reference/get-playlist</example>
    static async Task<string> GetPlaylistInfo(string token)
    {
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await client.GetAsync("https://api.spotify.com/v1/playlists/3cEYpjA9oz9GiPac4AsH4n?market=SV");
        return await response.Content.ReadAsStringAsync();
    }
}