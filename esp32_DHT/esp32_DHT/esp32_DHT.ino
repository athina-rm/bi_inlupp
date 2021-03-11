#include <ArduinoJson.h>          //for json data deserialization (version 6)

#include <Esp32MQTTClient.h>      //for MQTT communication in Esp32

#include <DHT.h>

#include<WiFi.h>                  //for WiFi in Esp32

#define DEVICE_ID "esp32"
#define DHT_PIN 4
#define DHT_TYPE DHT11

const char* ssid="TP-Link_E746";  //wifi network's ssid
const char* password ="90780653"; //wifi network's password
static char* connectionString="HostName=atFirstHub.azure-devices.net;DeviceId=esp32;SharedAccessKey=ioFWHnF9O1p4/BSFyxyM40Ur7OZiueNTWBxzmYFIHG4=";  //connection String to connect the device to Azure
static bool _connected=false;     //state of esp32's connection to cloud
static DHT dht(DHT_PIN,DHT_TYPE); //creating an instance dht of DHT

float prevTemp=0;
time_t epochTime;

void setup() {
  Serial.begin(115200);                 //Serial communication initialization - only for debugging
  initWiFi();                           //initializing Wifi
  dht.begin();                          //initializing dht sensor module
  initIotHub();                         //establishing conection with Azure Iot Hub
  initEpochTime();
}

void loop() {    
  float temperature=dht.readTemperature();      //taking temperature data
  float humidity= dht.readHumidity();           //taking humidity data
  epochTime = time(NULL);
  if (temperature<=prevTemp-1 || temperature>=prevTemp+1)
  {
    char msg[256]; 
    StaticJsonDocument<256> jdoc;                  //declaring jsondocument variable      
    jdoc["epochTime"] = epochTime;
    jdoc["temperature"] = temperature;            //creating json document with the temperature and humidity data
    jdoc["humidity"] = humidity;
    serializeJson(jdoc, msg); 
        if(_connected ) {  
          EVENT_INSTANCE* message = Esp32MQTTClient_Event_Generate(msg, MESSAGE);
          Esp32MQTTClient_Event_AddProp(message, "measurementType", "DHT");
          Esp32MQTTClient_Event_AddProp(message, "School", "Nackademin");
          Esp32MQTTClient_Event_AddProp(message, "Student", "Athina Mannaraprayil");
          Esp32MQTTClient_SendEventInstance(message);     //sending to Azure
        }
    prevTemp=temperature;
    Serial.println(msg);                          //for debugging only
  }
} 
