using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QdLayout
{
    public static class PlateRoomConfigManager
    {
        public static Dictionary<PlateBuildGroupType, Dictionary<string, List<RoomConfig>>> RoomConfigs = new Dictionary<PlateBuildGroupType, Dictionary<string, List<RoomConfig>>>();
        public static void Init()
        {
            var parentPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var filePath = parentPath + "RoomConfig\\RoomConfig.json";
            if (!File.Exists(filePath))
                return;

            using (var json = File.Open(filePath, FileMode.Open))
            {
                if (json.Length < 10) return;
                var jsonDoc = JsonDocument.Parse(json);
                var docEle = jsonDoc.RootElement;

                var grpTypesProps = docEle.EnumerateObject();
                foreach (var grpTypesProp in grpTypesProps)
                {
                    var grpTypeName = grpTypesProp.Name;
                    var grpType = (PlateBuildGroupType)Enum.Parse(typeof(PlateBuildGroupType), grpTypeName);
                    if (!RoomConfigs.ContainsKey(grpType))
                    {
                        RoomConfigs.Add(grpType, new Dictionary<string, List<RoomConfig>>());
                    }
                    var roomProps = grpTypesProp.Value.EnumerateObject();
                    foreach (var roomProp in roomProps)
                    {
                        var roomName = roomProp.Name;

                        if (!RoomConfigs[grpType].ContainsKey(roomName))
                        {
                            RoomConfigs[grpType].Add(roomName, new List<RoomConfig>());
                        }
                        var roomConfigProps = roomProp.Value.EnumerateArray();
                        foreach (var roomConfigProp in roomConfigProps)
                        {
                            var roomConfig = new RoomConfig();
                            var roomConfigPropEle = roomConfigProp;

                            roomConfig.PlateBuildType = grpType;
                            roomConfig.RoomName = roomName;
                            roomConfig.ConfigName = roomConfigPropEle.ReadStringProperty(nameof(roomConfig.ConfigName));
                            roomConfig.ConfigFilePath = roomConfigPropEle.ReadStringProperty(nameof(roomConfig.ConfigFilePath));
                            roomConfig.Thumbnail = roomConfigPropEle.ReadStringProperty(nameof(roomConfig.Thumbnail));

                            RoomConfigs[grpType][roomName].Add(roomConfig);
                        }
                    }
                }
            }
        }

        public static void Save()
        {
            var parentPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            var filePath = parentPath + "RoomConfig\\RoomConfig.json";

            using (var stream = new MemoryStream())
            {
                var options = SaveManager.Options;
                var soptions = SaveManager.Soptions;
                using (var writer = new Utf8JsonWriter(stream, options))
                {
                    writer.WriteStartObject();
                    foreach (var grpTypeKvp in RoomConfigs)
                    {
                        var grpTypeName = grpTypeKvp.Key.ToString();
                        writer.WritePropertyName(grpTypeName);
                        writer.WriteStartObject();
                        foreach (var roomNameKvp in grpTypeKvp.Value)
                        {
                            var roomName = roomNameKvp.Key.ToString();
                            writer.WritePropertyName(roomName);
                            writer.WriteStartArray();
                            foreach (var roomConfig in roomNameKvp.Value)
                            {
                                writer.WriteStartObject();

                                writer.WritePropertyName(nameof(roomConfig.ConfigName));
                                writer.WriteStringValue(roomConfig.ConfigName);

                                writer.WritePropertyName(nameof(roomConfig.ConfigFilePath));
                                writer.WriteStringValue(roomConfig.ConfigFilePath);

                                writer.WritePropertyName(nameof(roomConfig.Thumbnail));
                                writer.WriteStringValue(roomConfig.Thumbnail);

                                writer.WriteEndObject();
                            }
                            writer.WriteEndArray();
                        }
                        writer.WriteEndObject();
                    }
                    writer.WriteEndObject();

                    writer.Flush();
                }

                var json = Encoding.UTF8.GetString(stream.ToArray());

                var folderPath = System.IO.Path.GetDirectoryName(filePath);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                if (!File.Exists(filePath))
                {
                    File.Create(filePath);
                }
                File.WriteAllText(filePath, json);
            }
        }

        public static List<RoomConfig> GetRoomConfig(PlateBuildGroupType plateGrpType, string roomName)
        {
            if (RoomConfigs.TryGetValue(plateGrpType, out var roomConfigDic))
            {
                if (roomConfigDic.TryGetValue(roomName, out var roomConfigs))
                {
                    return roomConfigs;
                }
            }
            return null;
        }

        public static RoomConfig GetRoomConfig(PlateBuildGroupType plateGrpType, string roomName, string roomConfigName)
        {
            var roomConfigs = GetRoomConfig(plateGrpType, roomName);
            if (roomConfigs == null || roomConfigs.Count == 0)
            {
                return null;
            }
            return roomConfigs.FirstOrDefault(rc => rc.ConfigName == roomConfigName);
        }

        public static void InsertRoomConfig(RoomConfig roomConfig)
        {
            if (!RoomConfigs.ContainsKey(roomConfig.PlateBuildType))
            {
                RoomConfigs.Add(roomConfig.PlateBuildType, new Dictionary<string, List<RoomConfig>>()
                {
                    {
                        roomConfig.RoomName,
                        new List<RoomConfig>()
                        {
                            roomConfig
                        }
                    }
                });
                return;
            }

            if (!RoomConfigs[roomConfig.PlateBuildType].ContainsKey(roomConfig.RoomName))
            {
                RoomConfigs[roomConfig.PlateBuildType].Add(roomConfig.RoomName, new List<RoomConfig>()
                {
                    roomConfig
                });

                return;
            }

            RoomConfigs[roomConfig.PlateBuildType][roomConfig.RoomName].Add(roomConfig);
        }
    }

    public class RoomConfig
    {
        public PlateBuildGroupType PlateBuildType { get; set; }

        public string RoomName { get; set; }

        public string ConfigName { get; set; }

        public string ConfigFilePath { get; set; }
        public string Thumbnail { get; set; }
    }
}
