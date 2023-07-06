using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services => { 
        services.AddHttpClient("Api", options => options.BaseAddress = new Uri("https://localhost:7099"));})
    .Build();

host.Run();
