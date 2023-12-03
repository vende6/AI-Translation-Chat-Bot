using System.Text;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Models;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();   


string proxy ="https://aoai.hacktogether.net";
string key ="d3419237-31b7-4c01-98c8-1ea5debe33f3";
string aiSearchEndpoint ="https://azureaisearchservice-aichatbot.search.windows.net";
string keySearch ="aIg0drewc2pdGZ9td0wmhCAYEbGMJSc2Jd7aV1TRhXAzSeDXeZRB";
// the full url is appended by /v1/api
Uri proxyUrl = new(proxy + "/v1/api");
// the full key is appended by "/YOUR-GITHUB-ALIAS"
AzureKeyCredential token = new(key + "/vende6");
AzureKeyCredential tokenSearch = new AzureKeyCredential(keySearch);
// instantiate the client with the "full" values for the url and key/token
OpenAIClient openAIClient = new(proxyUrl, token);


 var searchClient = new SearchClient(new Uri(aiSearchEndpoint), "index01", tokenSearch);
 Console.WriteLine(searchClient.GetDocumentCount());

// SearchIndexClient searchIndexClient = new SearchIndexClient(new Uri(aiSearchEndpoint), tokenSearch);
// SearchIndex index = new SearchIndex("index01")
// {
//     Fields = new FieldBuilder().Build(typeof(Path)),
//     Suggesters =
//     {
//         // Suggest query terms from the HotelName field.
//         new SearchSuggester("sg", "Title")
//     }
// };

// searchIndexClient.CreateIndex(index);


SearchResults<string> response = searchClient.Search<string>("jsonlfile");
foreach (SearchResult<string> result in response.GetResults())
{
    string doc = result.Document;
    Console.WriteLine($"{doc}: {doc}");
}
 
var documentContentList = await searchClient.GetDocumentAsync<object>("aHR0cHM6Ly9zYWF6dXJlYmxvYnN0b3JlYWNjb3VudC5ibG9iLmNvcmUud2luZG93cy5uZXQvaW5kZXgwMS1jaHVua3MvYUhSMGNITTZMeTl6WVdGNmRYSmxZbXh2WW5OMGIzSmxZV05qYjNWdWRDNWliRzlpTG1OdmNtVXVkMmx1Wkc5M2N5NXVaWFF2Wm1sc1pYVndiRzloWkMxcGJtUmxlREF4TDNSeVlXTnJjeVV5TUMwbE1qQm1kV3hzY0dGMGFDNWtiMk40MC9jb250ZW50X2NodW5rc18xLmpzb241");

Console.WriteLine(documentContentList);

var systemPrompt = "You are a virtual agent that helps users translate passages from English to other languages.";

var userPrompt = 
    """
    Translate this into: 1. German, 2. French, 3. Italian and 4. Bosnian:

    What rooms do you have available?

    
    """;


  var assistantPrompt = """
                        You're an AI assistant for developers, helping them write code more efficiently.
                        You're name is **Blazor 📎 Clippy** and you're an expert Blazor developer.
                        You're also an expert in ASP.NET Core, C#, TypeScript, and even JavaScript.
                        You will always reply with a Markdown formatted response.
                        """;

 var wynPrompt = """ What's your name? """;


 var introductoryPrompt = """Hi, my name is **Blazor 📎 Clippy**! Nice to meet you. """;
    
ChatCompletionsOptions completionOptions = new() {
    MaxTokens=60,
    Temperature=0f,
    FrequencyPenalty=0.0f,
    PresencePenalty=0.0f,
    NucleusSamplingFactor=1,
    DeploymentName = "gpt-35-turbo"
};
completionOptions.Messages.Add(new ChatMessage(ChatRole.System, systemPrompt));
completionOptions.Messages.Add(new ChatMessage(ChatRole.User, userPrompt));
//completionOptions.Messages.Add(new ChatMessage(ChatRole.User, wynPrompt));
//completionOptions.Messages.Add(new ChatMessage(ChatRole.User, introductoryPrompt));
var completions = await openAIClient.GetChatCompletionsAsync(completionOptions);
Console.WriteLine(completions.Value.Choices.First().Message.Content);

