using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using SmhiWeather;

namespace ConsoleApp1
{
    class Program
    {
        private static DeviceClient deviceClient = DeviceClient.CreateFromConnectionString("HostName=atFirstHub.azure-devices.net;DeviceId=ConsoleApp1;SharedAccessKey=dzonNl9LmrfMlvx5ikqSTyePLqz8lwdmgIQRn/rasRk=", TransportType.Mqtt);

        static async Task Main(string[] args)
        {            
            ISmhi smhi = new Smhi(59.3417m, 18.0549m);
            var currentWeather = smhi.GetCurrentWeather();
            var json = JsonConvert.SerializeObject(new { DateTime.Now, currentWeather.Temperature, currentWeather.RelativeHumidity, currentWeather.WindSpeed });
            await SendMessageAsync(json);
            await Task.Delay(5000);
            var prevTemp = currentWeather.Temperature;
            while (true)
            {
                currentWeather = smhi.GetCurrentWeather();
                if (currentWeather.Temperature > prevTemp || currentWeather.Temperature < prevTemp)
                {
                    await SendMessageAsync(JsonConvert.SerializeObject(new { DateTime.Now, currentWeather.Temperature, currentWeather.RelativeHumidity, currentWeather.WindSpeed }));
                    await Task.Delay(5000);
                    prevTemp = currentWeather.Temperature;
                }
            }
        }
        private static async Task SendMessageAsync(string message)
        {            
            var msg = new Message(Encoding.UTF8.GetBytes(message));
            msg.Properties["measurementType"] = "smhi";
            msg.Properties["school"] = "Nackademin";
            msg.Properties["student"] = "Athina";
            msg.Properties["latitude"] = "59.3417m";
            msg.Properties["longitude"] = "18.0549m";
            await deviceClient.SendEventAsync(msg);
            Console.WriteLine(message);
        }        
    }
}

