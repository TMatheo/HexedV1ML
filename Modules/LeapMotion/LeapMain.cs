using System.Net.Sockets;
using UnityEngine;

namespace LUXED.Modules.LeapMotion
{
    internal class LeapMain
    {
        public static GameObject LeapTrackingRoot;
        public static GestureMatcher.GesturesData GestureData;
        public static GameObject[] LeapHands;
        public static LeapObjects.HandObject[] LeapFrame;

        private static Socket Server;
        private static Socket Client;
        /*
        public static void Init()
        {
            if (!InternalSettings.isLeapMotion) return;

            GestureData = new GestureMatcher.GesturesData();
            LeapHands = new GameObject[GestureMatcher.GesturesData.ms_handsCount];

            LeapTrackingRoot = new GameObject("LeapTrackingRoot");
            LeapTrackingRoot.transform.localPosition = Vector3.zero;
            LeapTrackingRoot.transform.localRotation = Quaternion.identity;
            UnityEngine.Object.DontDestroyOnLoad(LeapTrackingRoot);

            for (int i = 0; i < GestureMatcher.GesturesData.ms_handsCount; i++)
            {
                LeapHands[i] = new GameObject("LeapHand" + i);
                LeapHands[i].transform.parent = LeapTrackingRoot.transform;
                LeapHands[i].transform.localPosition = Vector3.zero;
                LeapHands[i].transform.localRotation = Quaternion.identity;
                UnityEngine.Object.DontDestroyOnLoad(LeapHands[i]);
            }

            if (LeapTrackingRoot != null)
            {
                LeapTrackingRoot.transform.parent = Camera.main.transform;
                LeapTrackingRoot.transform.localPosition = new Vector3(0, -0.5f, 0.3f);
                LeapTrackingRoot.transform.localRotation = Quaternion.Euler(0, 0, 0);
            }

            if (!File.Exists(ConfigManager.BaseFolder + "\\LeapMotion\\LeapManager.exe")) return;

            Process Leap = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    FileName = ConfigManager.BaseFolder + "\\LeapMotion\\LeapManager.exe",
                    WorkingDirectory = ConfigManager.BaseFolder + "\\LeapMotion",
                    CreateNoWindow = true,
                }
            };
            Leap.Start();

            IPHostEntry host = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = host.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 1337);

            Server = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Server.Bind(localEndPoint);
            Server.Listen(10);

            Client = Server.Accept();

            HDLogger.Log("Leap Device Connected", HDLogger.LogsType.Info);

            new Thread(() =>
            {
                try
                {
                    while (!Leap.HasExited && Client != null && Client.Connected)
                    {
                        byte[] bytes = new byte[15000];
                        int bytesRec = Client.Receive(bytes);

                        if (bytesRec == 0)
                        {
                            break;
                        }

                        string message = Encoding.UTF8.GetString(bytes, 0, bytesRec).Trim('\0');

                        try
                        {
                            LeapFrame = JsonSerializer.Deserialize<LeapObjects.HandObject[]>(message);
                        }
                        catch (JsonException)
                        {
                            LeapFrame = null;

                            for (int i = 0; i < GestureMatcher.GesturesData.ms_handsCount; i++)
                            {
                                GestureData.m_handsPresenses[i] = false;
                            }
                        }
                    }
                }
                catch (SocketException ex)
                {
                    HDLogger.Log("Socket exception in Leap thread: " + ex.Message, HDLogger.LogsType.Info);
                }
                catch (Exception ex)
                {
                    HDLogger.Log($"Exception in Leap thread:{ex.Message}",HDLogger.LogsType.Info);
                }
            }).Start();
        }
    }*/
    }
}
