#include <ESP8266WiFi.h>
#include <DHT.h>
#include <RTClib.h>
#include <Wire.h>
#include <LiquidCrystal_I2C.h>
#include <SoftwareSerial.h>
#include <PubSubClient.h>

// Pin and sensor type definitions
#define DHTPIN 2        // DHT11 sensor data pin connected to digital pin 2.
#define DHTTYPE DHT11   // Using the DHT11 sensor.

// Object instantiations
DHT dht(DHTPIN, DHTTYPE);                  // DHT sensor instance.
RTC_DS3231 rtc;                            // RTC module instance.
LiquidCrystal_I2C lcd(0x27, 16, 2);        // LCD instance (I2C address: 0x27, 16 columns, 2 rows).
SoftwareSerial bluetooth(10, 11);          // Bluetooth instance (RX: pin 10, TX: pin 11).

// Wi-Fi credentials
const char* ssid = "your_SSID";            // Replace with your Wi-Fi SSID.
const char* password = "your_PASSWORD";    // Replace with your Wi-Fi password.

// MQTT server details
const char* mqtt_server = "your_mqtt_broker_ip";  // Replace with your MQTT broker IP.

WiFiClient espClient;                      // Wi-Fi client for MQTT connection.
PubSubClient client(espClient);            // MQTT client.

void reconnect() {
    // Keep attempting to reconnect until successful
    while (!client.connected()) {
        Serial.print("Attempting MQTT connection...");
        // Attempt to connect to MQTT broker
        if (client.connect("ArduinoClient")) {
            Serial.println("connected");
            // Publish a connection announcement
            client.publish("gym/connection", "Arduino connected");
        } else {
            Serial.print("failed, rc=");
            Serial.print(client.state());
            Serial.println(" try again in 5 seconds");
            delay(5000);  // Wait 5 seconds before retrying
        }
    }
}

void setup() {
    Serial.begin(115200);             // Initialize serial communication for debugging.
    bluetooth.begin(9600);            // Initialize Bluetooth communication.
    WiFi.begin(ssid, password);       // Start connecting to the Wi-Fi network.
    client.setServer(mqtt_server, 1883);  // Set MQTT server and port.
    
    // Wait for Wi-Fi connection
    while (WiFi.status() != WL_CONNECTED) {
        delay(500);
        Serial.print(".");            // Print dots while attempting to connect.
    }
    Serial.println("Connected to Wi-Fi");

    dht.begin();                      // Initialize the DHT sensor.
    if (!rtc.begin()) {               // Initialize the RTC module.
        Serial.println("Couldn't find RTC");
        while (1);                    // Halt if the RTC module is not found.
    }
    if (rtc.lostPower()) {            // Check if RTC module lost power.
        Serial.println("RTC lost power, setting the time!");
        rtc.adjust(DateTime(F(__DATE__), F(__TIME__)));  // Set RTC to the current compile time.
    }
    lcd.begin();                      // Initialize the LCD display.
    lcd.backlight();                  // Turn on the LCD backlight.
}

void loop() {
    // Reconnect to MQTT broker if the connection is lost
    if (!client.connected()) {
        reconnect();
    }
    client.loop();  // Maintain MQTT connection
    
    float h = dht.readHumidity();     // Read humidity from DHT sensor.
    float t = dht.readTemperature();  // Read temperature from DHT sensor.
    bool buttonPressed = digitalRead(3); // Read button state from digital pin 3.
    
    // Check for sensor read errors
    if (isnan(h) || isnan(t)) {
        Serial.println("Failed to read from DHT sensor!");
        return;
    }
    
    DateTime now = rtc.now();         // Get current time from RTC module.
    
    // Display temperature on the LCD.
    lcd.setCursor(0, 0);
    lcd.print("Temp: ");
    lcd.print(t);
    lcd.print(" C");
    
    // Display humidity on the LCD.
    lcd.setCursor(0, 1);
    lcd.print("Hum: ");
    lcd.print(h);
    lcd.print(" %");

    // Construct payload string with sensor data and button state.
    String payload = "Temp: " + String(t) + " C, Hum: " + String(h) + " %, Button: " + String(buttonPressed);
    client.publish("gym/sensors", payload.c_str());  // Publish data to MQTT topic.
    
    // Print data to Serial Monitor for debugging.
    Serial.print("Humidity: ");
    Serial.print(h);
    Serial.print(" %\t");
    Serial.print("Temperature: ");
    Serial.print(t);
    Serial.print(" *C ");
    Serial.print("Button: ");
    Serial.println(buttonPressed);
    
    delay(2000);  // Wait for 2 seconds before repeating the loop.
}
