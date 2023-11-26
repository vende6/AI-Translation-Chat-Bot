﻿using System.Text;
using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Configuration;
var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();   
string proxy = "https://aoai.hacktogether.net";
string key = "d3419237-31b7-4c01-98c8-1ea5debe33f3";
// the full url is appended by /v1/api
Uri proxyUrl = new(proxy + "/v1/api");
// the full key is appended by "/YOUR-GITHUB-ALIAS"
AzureKeyCredential token = new(key + "/vende6");
// instantiate the client with the "full" values for the url and key/token
OpenAIClient openAIClient = new(proxyUrl, token);

var systemPrompt = "You are a virtual agent that helps users translate passages from English to other languages.";

var userPrompt = 
    """
    Translate this into 1. German, 2. French, 3. Italian and 4. Bosnian:

    What rooms do you have available?

    1.
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
var response = await openAIClient.GetChatCompletionsAsync(completionOptions);
Console.WriteLine(response.Value.Choices.First().Message.Content);