using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services => { 
        services.AddHttpClient("Api", options => options.BaseAddress = new Uri("http://localhost:5113"));})
    .Build();

host.Run();
