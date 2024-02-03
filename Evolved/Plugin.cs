using BepInEx;
using UnityEngine;
using System.IO;
using System.Reflection;
using LethalLib.Modules;
using System.Collections.Generic;
using HarmonyLib;

namespace Evolved
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin instance;

        private readonly Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);

        private Dictionary<string, int> assetDict = new()
        {
            {"Loafers", 40}, {"LouisVuitton", 5}, {"Rolex", 5}, {"Pistol", 30}, {"mp5", 25}, {"ModelCar1", 25}, {"ModelCar2", 20},
            {"ModelCar3", 15}, {"ModelCar4", 10}, {"ModelCar5", 5}, {"CultMask", 10}, {"shield", 8}, {"ComputerKeyboard", 40},
            {"DinnerPlate", 50}, {"Kettle", 50}, {"PersonStatue", 15}, {"LargeToyCar", 48}, {"BeanBagChair", 37}, {"BirdStatue1", 15},
            {"BirdStatue2", 10}, {"Camera", 20}, {"CoffeeMaker", 25}, {"GoldenApple", 2}, {"LargePottery1", 15}, {"LargePottery2", 20},
            {"LargePottery3", 25}, {"MiniFridge", 20}, {"RetroTV", 15}, {"Television", 30}, {"DiningChair", 40},{"Dumbbell", 40},
            {"KetchupBottle", 45}, {"Microwave", 30}, {"MiniAirHockeyTable", 10}, {"PastryDish", 40}, {"PCMonitor", 30}, {"ToiletPaper", 50},
            {"SniperScope", 10}, {"Pottery1", 40}
        };


        private static string ASSET_DIR = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "itemmod");
        private static string SCRAP_PATH = "Assets/scrap/";
        private static string ITEM_PATH = "Assets/Items/";
        private AssetBundle bundle = AssetBundle.LoadFromFile(ASSET_DIR);



        void Awake()
        {
            instance = this;


            Logger.LogInfo("asset bundle is loaded!");
            foreach (KeyValuePair<string, int> item in assetDict)
            {
                string path = $"{SCRAP_PATH}{item.Key}/{item.Key}.asset";
                Logger.LogInfo($"Loading: {path}");
                Item asset = bundle.LoadAsset<Item>(path);
                if (!asset.twoHanded)
                    asset.allowDroppingAheadOfPlayer = true;
                if (asset.minValue > asset.maxValue)
                    Logger.LogInfo($"Asset ${asset.name} has improper min/max values");
                Logger.LogInfo("asset loaded");
                NetworkPrefabs.RegisterNetworkPrefab(asset.spawnPrefab);
                Logger.LogInfo("prefab network loaded");
                Utilities.FixMixerGroups(asset.spawnPrefab);
                Logger.LogInfo("prefab mixer groups loaded");

                Items.RegisterScrap(asset, item.Value, Levels.LevelTypes.All);
                Logger.LogInfo("scrap registered");
            }
            Logger.LogInfo("All assets are loaded and registered!");
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

            Item speaker = bundle.LoadAsset<Item>($"{ITEM_PATH}Speaker/Speaker.asset");
            Logger.LogInfo("Loading speaker");
            NetworkPrefabs.RegisterNetworkPrefab(speaker.spawnPrefab);
            Utilities.FixMixerGroups(speaker.spawnPrefab);
            TerminalNode node = ScriptableObject.CreateInstance<TerminalNode>();
            node.clearPreviousText = true;
            node.displayText = "This is a speaker";
            Items.RegisterShopItem(speaker, null, null, node, 0);
        }
    }
}
