#include <ArduinoJson.h>          //for json data deserialization (version 6)

#include <Esp32MQTTClient.h>      //for MQTT communication in Esp32

#include <DHT.h>

#include<WiFi.h>                  //for WiFi in Esp32

#define INTERVAL 5000
#define DEVICE_ID "esp32"
#define MESSAGE_LEN_MAX 256
#define DHT_PIN 4
#define DHT_TYPE DHT11

const char* ssid="TP-Link_E746";  //wifi network's ssid
const char* password ="90780653"; //wifi network's password
static char* connectionString="HostName=atFirstHub.azure-devices.net;DeviceId=esp32;SharedAccessKey=ioFWHnF9O1p4/BSFyxyM40Ur7OZiueNTWBxzmYFIHG4=";  //connection String to connect the device to Azure
static bool _connected=false;     //state of esp32's connection to cloud
static DHT dht(DHT_PIN,DHT_TYPE); //creating an instance dht of DHT

void setup() {
  Serial.begin(115200);                 //Serial communication initialization - only for debugging
 // initWiFi();                           //initializing Wifi
 WiFi.begin(ssid,password);                //starting Wifi connection
  while (WiFi.status() != WL_CONNECTED) {   //printing '.' until the Wifi is connected
      delay(500);
      Serial.print(".");
    }
    Serial.println("");
    Serial.println("WiFi connected.");
    Serial.print("IP address: ");
    Serial.println(WiFi.localIP()); 
    dht.begin();                          //initializing dht sensor module
    initIotHub()                         //establishing conection with Azure Iot Hub
 
}

void loop() {
  float temperature=dht.readTemperature();      //taking temperature data
  float humidity= dht.readHumidity();           //taking humidity data
  char msg[40]; 
  StaticJsonDocument<40> jdoc;                  //declaring jsondocument variable  
  jdoc["temperature"] = temperature;            //creating json document with the temperature and humidity data
  jdoc["humidity"] = humidity;
  serializeJson(jdoc, msg); 
      if(_connected) {
           Esp32MQTTClient_SendEvent(msg);       //sending to Azure
        }
   delay(INTERVAL);
     Serial.println(msg);                          //for debugging only

} 
