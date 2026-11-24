
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using NSwag;
using NSwag.CodeGeneration.CSharp;
namespace ConfidentVotesAPI.CustomApiGenerator{
    public class ClientGenerator(string domain, int port)
    {
        public async Task GenerateClient()
        {   
            var httpClient = new HttpClient();
            var swaggerJson = await httpClient.GetStringAsync($"{domain}:{port}/swagger/v1/swagger.json");
            var document = await OpenApiDocument.FromJsonAsync(swaggerJson);

            var settings = new CSharpClientGeneratorSettings
            {
                ClassName = "ConfidentVoteClient",
                CSharpGeneratorSettings = { Namespace = "ConfidentVoteAPI" }
            };

            var generator = new CSharpClientGenerator(document, settings);
            var code = generator.GenerateFile();

            await File.WriteAllTextAsync("GeneratedApiClient.cs", code);
        }
    }
}
