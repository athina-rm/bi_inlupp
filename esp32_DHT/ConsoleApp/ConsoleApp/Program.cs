using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{    
    class Program
    {
        private static DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("HostName=atFirstHub.azure-devices.net;DeviceId=ConsoleApp1;SharedAccessKey=dzonNl9LmrfMlvx5ikqSTyePLqz8lwdmgIQRn/rasRk=", TransportType.Mqtt);

        static async Task Main(string[] args)
        {
            while (true)
            {
                await SendMessageAsync(JsonConvert.SerializeObject(new { deviceId = "consoleDevice", temperature = 11, humidity = 44, lumen = 976 }));
                await Task.Delay(5000);
                Console.Write(JsonConvert.SerializeObject(new { deviceId = "consoleDevice", temperature = 11, humidity = 44, lumen = 976 }));
            }
        }
        private static async Task SendMessageAsync(string message)
        {
            var msg = new Message(Encoding.UTF8.GetBytes(message));
            await deviceClient.SendEventAsync(msg);
        }
    }
}
