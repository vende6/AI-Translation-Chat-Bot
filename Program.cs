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


string proxy ="<<this is where endpoint for open ai services endpoint>>";
string key ="d3419237-31b7-4c01-98c8-1ea5debe33f3";
string aiSearchEndpoint ="https://azureaisearchservice-aichatbot.search.windows.net";
string keySearch ="aIg0drewc2pdGZ9td0wmhCAYEbGMJSc2Jd7aV1TRhXAzSeDXeZRB";
Uri proxyUrl = new(proxy + "/v1/api");
AzureKeyCredential token = new(key + "/vende6");
AzureKeyCredential tokenSearch = new AzureKeyCredential(keySearch);
OpenAIClient openAIClient = new(proxyUrl, token);
var searchClient = new SearchClient(new Uri(aiSearchEndpoint), "index01", tokenSearch);
Console.WriteLine(searchClient.GetDocumentCount());
SearchResults<string> response = searchClient.Search<string>("jsonlfile");
 var documentContentList = await searchClient.GetDocumentAsync<object>("aHR0cHM6Ly9zYWF6dXJlYmxvYnN0b3JlYWNjb3VudC5ibG9iLmNvcmUud2luZG93cy5uZXQvaW5kZXgwMS1jaHVua3MvYUhSMGNITTZMeTl6WVdGNmRYSmxZbXh2WW5OMGIzSmxZV05qYjNWdWRDNWliRzlpTG1OdmNtVXVkMmx1Wkc5M2N5NXVaWFF2Wm1sc1pYVndiRzloWkMxcGJtUmxlREF4TDNSeVlXTnJjeVV5TUMwbE1qQm1kV3hzY0dGMGFDNWtiMk40MC9jb250ZW50X2NodW5rc18xLmpzb241");
Console.WriteLine(documentContentList);
var systemPrompt = "You are a virtual agent that helps users translate passages from English to other languages.";
var userPrompt = 
    @$"""



    Extract artists with the firstname of 'Solomun' or producers with lastname of 'Dragojević' and list all of their songs from {documentContentList}.

    
    """;    
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
var completions = await openAIClient.GetChatCompletionsAsync(completionOptions);
Console.WriteLine(completions.Value.Choices.First().Message.Content);



