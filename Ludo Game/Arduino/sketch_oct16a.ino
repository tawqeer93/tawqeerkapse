#include <WiFi.h>
#include <Adafruit_MPU6050.h>
#include <Adafruit_Sensor.h>
#include <Wire.h>
#include <WiFiUdp.h>

// WiFi
const char *ssid = "kapse’s iPhone";   
const char *password = "987654321";  

// UDP setup
WiFiUDP udp;
const char *udpAddress = "172.20.10.2";  // IP address of Unity PC
const int udpPort = 4210;  // Port for communication
const int localUdpPort = 4211;  // Port to receive unlock message

Adafruit_MPU6050 mpu;
unsigned long lastSendTime = 0;
unsigned long shakeResetTime = 0; 
const unsigned long debounceTime = 1000;
const float shakeThreshold = 6.0;
bool diceRolled = false;  
bool diceLocked = false;

void setup() {
    Serial.begin(115200);

    // Initialize MPU6050
    if (!mpu.begin()) {
        Serial.println("Failed to find MPU6050 chip!");
        while (1) {
            delay(10);
        }
    }

    mpu.setAccelerometerRange(MPU6050_RANGE_8_G);
    Serial.println("MPU6050 connected!");

    // Connect to WiFi
    WiFi.begin(ssid, password);
    while (WiFi.status() != WL_CONNECTED) {
        delay(800);
        Serial.println("Connecting to WiFi...");
    }
    Serial.println("Connected to WiFi!");
    Serial.print("ESP32 IP Address: ");
    Serial.println(WiFi.localIP());  // Print IP Address after connection

    // Start UDP communication
    udp.begin(udpPort);

    // Start listening for unlock messages on another port
    udp.begin(localUdpPort);
}

void loop() {
    // Listen for unlock message from Unity
    int packetSize = udp.parsePacket();
    if (packetSize) {
        char incomingPacket[255];
        int len = udp.read(incomingPacket, 255);
        if (len > 0) {
            incomingPacket[len] = 0;
        }
        String message = String(incomingPacket);

        Serial.print("Received message: ");
        Serial.println(message);

        // If "unlock" message is received, unlock the dice
        if (message == "unlock") {
            unlockDice();
            Serial.println("Received 'unlock' message from Unity.");
        }
    }

    sensors_event_t a, g, temp;
    mpu.getEvent(&a, &g, &temp);

    // Print raw accelerometer data
    Serial.print("Acceleration X: ");
    Serial.print(a.acceleration.x);
    Serial.print(", Y: ");
    Serial.print(a.acceleration.y);
    Serial.print(", Z: ");
    Serial.println(a.acceleration.z);

    float totalAcceleration = sqrt(sq(a.acceleration.x) + sq(a.acceleration.y) + sq(a.acceleration.z));

    // Check if enough time has passed to allow new shake detection
    if (millis() - shakeResetTime > debounceTime) {
        diceRolled = false;
    }

    // Detect shaking (if the total acceleration exceeds the threshold and dice isn't locked)
    if (totalAcceleration > shakeThreshold && !diceRolled && !diceLocked) {
        Serial.println("Shake detected!");

        // Determine which side of the dice is facing up
        int diceSide = determineDiceSide(a.acceleration.x, a.acceleration.y, a.acceleration.z);
        
        if (diceSide > 0) {
            sendDiceRoll(diceSide);  // Send the detected dice side to Unity
            diceRolled = true;
            diceLocked = true;  // Lock the dice after a roll

            shakeResetTime = millis();  // Set the reset time
        }
    }

    delay(100);
}

int determineDiceSide(float ax, float ay, float az) {
    const float gravity = 9.8;
    const float threshold = 3.0;

    if (abs(az - gravity) < threshold) return 1;  
    if (abs(az + gravity) < threshold) return 6;  
    if (abs(ax - gravity) < threshold) return 2;  
    if (abs(ax + gravity) < threshold) return 5;  
    if (abs(ay - gravity) < threshold) return 3;  
    if (abs(ay + gravity) < threshold) return 4;  

    return 0;
}

void sendDiceRoll(int roll) {
    udp.beginPacket(udpAddress, udpPort);
    udp.print(roll);  
    udp.endPacket();
    Serial.println("Dice side sent to Unity.");
}

void unlockDice() {
    diceLocked = false;  // Reset lock for next roll
    Serial.println("Dice unlocked for next turn.");
}
