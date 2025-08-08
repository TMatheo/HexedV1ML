using ExitGames.Client.Photon;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRC.SDKBase;

namespace LUXED.Wrappers
{
    public static class HDLogger
    {
        private static readonly object consoleLock = new object();

        private const string Prefix = "[Melexed]";
        private static string Timestamp => $"[{DateTime.Now:HH:mm:ss}]";

        private static readonly Dictionary<LogsType, ConsoleColor> TypeColors = new Dictionary<LogsType, ConsoleColor>
        {
            { LogsType.Info, ConsoleColor.Green },
            { LogsType.Protection, ConsoleColor.Red },
            { LogsType.Udon, ConsoleColor.Red },
            { LogsType.Room, ConsoleColor.Cyan },
            { LogsType.Friends, ConsoleColor.Magenta },
            { LogsType.Chat, ConsoleColor.Blue },
            { LogsType.Group, ConsoleColor.DarkGray },
            { LogsType.Bot, ConsoleColor.DarkGray }
        };

        public static HashSet<LogsType> EnabledTypes { get; set; } = new HashSet<LogsType>(Enum.GetValues(typeof(LogsType)).Cast<LogsType>());

        public enum LogsType
        {
            Info,
            Protection,
            Udon,
            Room,
            Friends,
            Chat,
            Group,
            Bot
        }

        public static void Log(object obj, LogsType type)
        {
            if (!EnabledTypes.Contains(type)) return;

            TypeColors.TryGetValue(type, out var color);
            WriteLog(obj, $"[{type}]", color);
        }

        public static void LogError(object obj) =>
            WriteLog(obj, "[ERROR]", ConsoleColor.Red);

        public static void LogWarning(object obj) =>
            WriteLog(obj, "[WARNING]", ConsoleColor.Yellow);

        public static void LogDebug(object obj) =>
            WriteLog(obj, "[DEBUG]", ConsoleColor.Blue);

        public static void LogRaw(string message)
        {
            lock (consoleLock)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"{Timestamp} ");
                Console.ResetColor();
                Console.WriteLine(message);
            }
        }

        private static void WriteLog(object obj, string level, ConsoleColor color)
        {
            lock (consoleLock)
            {
                string content = obj?.SanitizeLog() ?? "NULL";

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"{Timestamp} ");
                Console.ResetColor();

                Console.Write($"{Prefix} ");
                Console.ForegroundColor = color;
                Console.Write($"{level} ");
                Console.ResetColor();

                Console.WriteLine(content);
            }
        }

        public static void LogRPC(VRC.Player player, VRC_EventHandler.VrcEvent ev, VRC_EventHandler.VrcBroadcastType broadcastType, int instagatorId, float fastForward, object[] decodedBytes)
        {
            try
            {
                var log = new StringBuilder()
                    .AppendLine()
                    .AppendLine("======= RPC EVENT =======")
                    .AppendLine($"ACTOR NUMBER     : {instagatorId}")
                    .AppendLine($"SENDER NAME      : {player.DisplayName()}")
                    .AppendLine($"BYTES DECODED    : {(decodedBytes.Length > 0 ? string.Join(" | ", decodedBytes.Where(b => b != null)) : "NULL")}")
                    .AppendLine($"EVENTTYPE        : {ev.EventType}")
                    .AppendLine($"PARAMETER STRING : {ev.ParameterString}")
                    .AppendLine($"PARAMETER INT    : {ev.ParameterInt}")
                    .AppendLine($"PARAMETER FLOAT  : {ev.ParameterFloat}")
                    .AppendLine($"PARAMETER BOOL   : {ev.ParameterBool}")
                    .AppendLine($"PARAMETER BOOLOP : {ev.ParameterBoolOp}")
                    .AppendLine($"TAKE OWNERSHIP   : {ev.TakeOwnershipOfTarget}")
                    .AppendLine($"BROADCAST TYPE   : {broadcastType}")
                    .AppendLine($"FAST FORWARD     : {fastForward}")
                    .AppendLine("======= END =======");

                Log(log.ToString(), LogsType.Info);
            }
            catch (Exception ex)
            {
                LogError($"Failed to log RPC event: {ex}");
            }
        }

        public static void LogEventData(EventData EventData)
        {
            try
            {
                if (OnEventLogIgnore.Contains(EventData.Code)) return;

                object CustomData = EventData.CustomData == null ? "NULL" : CPP2IL.BinaryConverter.IL2CPPToManaged(EventData.CustomData);

                Player PhotonPlayer = null;
                string SenderNAME = "NULL";

                if (EventData.Sender > 0) PhotonPlayer = GameHelper.VRCNetworkingClient.GetPhotonPlayer(EventData.Sender);
                if (PhotonPlayer != null) SenderNAME = PhotonPlayer.DisplayName() ?? "NO NAME";

                string IfBytes = null;
                if (CustomData.GetType() == typeof(byte[]))
                {
                    IfBytes = "  |  ";
                    byte[] arr = (byte[])CustomData;
                    IfBytes += string.Join(", ", arr);
                    IfBytes += $" [L: {arr.Length}]";
                }
                else if (CustomData.GetType() == typeof(ArraySegment<byte>))
                {
                    IfBytes = "  |  ";
                    ArraySegment<byte> segment = (ArraySegment<byte>)CustomData;
                    IfBytes += string.Join(", ", segment.Array);
                    IfBytes += $" [L: {segment.Count}]";
                    IfBytes += $" [O: {segment.Offset}]";
                }

                object ParameterData = "NULL";
                if (EventData.Parameters != null) ParameterData = CPP2IL.BinaryConverter.IL2CPPToManaged(EventData.Parameters);

                Log(string.Concat(new object[]
                {
                    Environment.NewLine,
                    $"======= ONEVENT {EventData.Code} =======", Environment.NewLine,
                    $"ACTOR NUMBER: {EventData.Sender}", Environment.NewLine,
                    $"SENDER NAME: {SenderNAME}", Environment.NewLine,
                    $"TYPE: {CustomData}", Environment.NewLine,
                    $"DATA: {(IfBytes == null ? CustomData : IfBytes)}", Environment.NewLine,
                    $"PARAMETER: {ParameterData}", Environment.NewLine,
                    $"MEMORY POINTER: {EventData.Pointer}", Environment.NewLine,
                    "======= END =======",
                }), LogsType.Info);
            }
            catch (Exception e)
            {
                LogError($"Failed to Log Event with code {EventData.Code} with Exception: {e}");
            }
        }

        public static void LogApi(BestHTTP.HTTPRequest request)
        {
            try
            {
                byte[] body = request.GetEntityBody();
                var log = new StringBuilder()
                    .AppendLine()
                    .AppendLine("======= API REQUEST =======")
                    .AppendLine($"METHOD   : {request.MethodType}")
                    .AppendLine($"ENDPOINT : {request.Uri}")
                    .AppendLine($"CONTENT  : {(body == null ? "NULL" : Encoding.UTF8.GetString(body))}")
                    .AppendLine("HEADERS  :")
                    .AppendLine(request.DumpHeaders())
                    .AppendLine("======= END =======");

                Log(log.ToString(), LogsType.Info);
            }
            catch (Exception ex)
            {
                LogError($"Failed to log API request: {ex}");
            }
        }

        private static readonly byte[] OnEventLogIgnore = new byte[] { 1, 8, 35, 66 };
        private static readonly byte[] OpRaiseLogIgnore = new byte[] { 8, 66 };
        private static readonly byte[] OperationLogIgnore = new byte[] { 253 };
    }

    internal static class LoggerExtensions
    { 
        public static string SanitizeLog(this object obj)
        {
            return obj.ToString()
                .Replace("\a", "a")
                .Replace("\u001B[", "u001B[")
                .Replace("\x1b", ""); // Remove escape characters
        }
    }
}
