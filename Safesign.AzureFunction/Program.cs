using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services => 
        { 
        services.AddHttpClient("Api", options => options.BaseAddress = new Uri("http://localhost:5113"));
        //services.AddSignalR().AddAzureSignalR("Endpoint=https://safesignsignalr.service.signalr.net;AccessKey=4iFSZUcwMfefVvenpuTGLzQsPawAytpxL5+ccrBxCNU=;Version=1.0;");
        services.AddSignalR();
        })
    .Build();

host.Run();
