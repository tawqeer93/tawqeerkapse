using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class LudoDice : MonoBehaviour
{
    private Thread receiveThread;
    private UdpClient client;
    public int port = 4210;  // UDP Port to listen on (same as ESP32)
    public string receivedData;
    private bool isDiceLocked = false;  // Lock the dice after rolling
    private float diceLockDuration = 5.0f;  // Lock duration (e.g., 5 seconds)
    private float diceLockTimer = 0.0f;

    private volatile bool isReceiving = false;  // Use 'volatile' for thread-safe control

    void Start()
    {
        StartUdpListener();  // Start listening for UDP messages
    }

    void StartUdpListener()
    {
        isReceiving = true;  // Set the flag to indicate that data is being received
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;  // Set the thread to run in the background
        receiveThread.Start();
        Debug.Log($"Listening for UDP packets on port {port}.");
    }

    void ReceiveData()
    {
        try
        {
            client = new UdpClient(port);
            IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, port);

            while (isReceiving)  // Keep receiving data while the flag is true
            {
                try
                {
                    byte[] data = client.Receive(ref anyIP);  // Receive UDP data
                    string text = Encoding.UTF8.GetString(data);
                    receivedData = text.Trim();  // Store received dice roll

                    Debug.Log($"Received dice roll: {receivedData}");
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode != SocketError.Interrupted)
                    {
                        Debug.LogError($"UDP Receive error: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception err)
        {
            Debug.LogError($"Error setting up UDP client: {err.Message}");
        }
    }

    void Update()
    {
        //if (!string.IsNullOrEmpty(receivedData))  // If new data is received
        //{
        //    int diceRoll = int.Parse(receivedData);  // Parse the dice roll result from received data

        //    // Call the GameScript's DiceRoll method with the dice roll result
        //    FindObjectOfType<GameScript>().DiceRoll(diceRoll);

        //    receivedData = "";  // Reset the received data after handling it

        //}
        
        // Check if a dice roll is locked (for a duration after receiving data)
            if (isDiceLocked)
            {
                diceLockTimer += Time.deltaTime; // Increment timer

                if (diceLockTimer >= diceLockDuration)  // After lock duration, unlock the dice
                {
                    isDiceLocked = false;
                    diceLockTimer = 0.0f;  // Reset the timer
                    receivedData = "";     // Clear the received data (to prevent showing old values)
                    //FindObjectOfType<GameScript>().ResetDice(); // Add method to reset dice visuals in GameScript
                }
            }

            if (!string.IsNullOrEmpty(receivedData) && !isDiceLocked)  // If new data is received and the dice isn't locked
            {
                int diceRoll = int.Parse(receivedData);  // Parse the dice roll result from received data

                // Call the GameScript's DiceRoll method with the dice roll result
                FindObjectOfType<GameScript>().DiceRoll(diceRoll);

                receivedData = "";  // Reset the received data after handling it
                isDiceLocked = true; // Lock the dice to prevent multiple updates during the cooldown
                diceLockTimer = 0.0f;  // Reset the timer
            }
    }


    void OnApplicationQuit()
    {
        StopUdpListener();  // Stop the UDP listener when the application quits
    }

    void StopUdpListener()
    {
        isReceiving = false;  // Set the flag to stop receiving data

        // Wait for the thread to terminate gracefully
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Join();  // Block until the thread finishes execution
        }

        // Close the UDP client to release the port
        if (client != null)
        {
            client.Close();
        }

        Debug.Log("UDP listener stopped.");
    }
}
