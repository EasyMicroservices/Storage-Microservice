using EasyMicroservices.Laboratory.Engine;
using EasyMicroservices.Laboratory.Engine.Net.Http;

namespace EasyMicroservices.StorageMicroservice.Clients.Tests
{
    public class MicroserviceClientTest
    {
        const int Port = 7184;
        string _routeAddress = "";
        public static HttpClient HttpClient { get; set;  } = new HttpClient();
        public MicroserviceClientTest()
        {
            _routeAddress = $"http://localhost:{Port}";
        }
        static bool _isInitialized = false;
        static SemaphoreSlim Semaphore = new SemaphoreSlim(1);
        async Task OnInitialize()
        {
            if (_isInitialized)
                return;
            try
            {
                await Semaphore.WaitAsync();
                _isInitialized = true;

                ResourceManager resourceManager = new ResourceManager();
                HttpHandler httpHandler = new HttpHandler(resourceManager);
                await httpHandler.Start(Port);
                resourceManager.Append(@$"GET *RequestSkipBody* HTTP/1.1
Host: localhost:{Port}
Accept: text/plain*RequestSkipBody*"
,
@"HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8
Content-Length: 0

{""result"":[],""isSuccess"":true,""error"":null}");
            }
            finally
            {
                Semaphore.Release();
            }
        }

        [Fact]
        public async Task GetAllTestTest()
        {
            //await OnInitialize();
            //var microserviceClient = new WhiteLables.GeneratedServices.MicroserviceClient(_routeAddress, HttpClient);
            //var microservices = await microserviceClient.GetAllAsync();
            //Assert.True(microservices.IsSuccess);
        }
    }
}