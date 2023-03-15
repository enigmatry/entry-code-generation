using System;

namespace Enigmatry.Entry.CodeGeneration.Configuration;

public class ApiClientInfo
{
    public string ApiClientName { get; set; }
    public bool HasApiClient => ApiClientName.HasContent();

    public ApiClientInfo(string apiClientName)
    {
        ApiClientName = apiClientName;
    }

    public static ApiClientInfo NoApiClient()
    {
        return new ApiClientInfo(String.Empty);
    }
}