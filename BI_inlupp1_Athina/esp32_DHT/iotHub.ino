void initIotHub(){
  if (!Esp32MQTTClient_Init((const uint8_t *) connectionString)){   //checking if a successful connection could be established with Azure IoT Hub
    _connected=false;
    return;
  }
  _connected=true;
}
