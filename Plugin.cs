global using HarmonyLib;
global using System;
global using System.Collections.Generic;
global using UnityEngine;

using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using FP2_Hidden_Palace_Mod.Patchers;
using FP2Lib.Stage;
using System.IO;

namespace FP2_Hidden_Palace_Mod
{
    [BepInPlugin("K24_FP2_HiddenPalace", "Hidden Palace Zone", "0.0.1")]
    [BepInDependency("000.kuborro.libraries.fp2.fp2lib")]
    public class Plugin : BaseUnityPlugin
    {
        // The asset and scene bundles exported from the Unity project.
        public static AssetBundle hpzAssetBundle;
        public static AssetBundle hpzSceneBundle;

        // The audio for both Track #10 and Mystic Cave 2P.
        public static AudioClip hpzMusic;
        public static AudioClip mcz2PMusic;

        // The config option to determine the music overrides.
        public static ConfigEntry<int> musicOption;

        // Logger.
        public static ManualLogSource consoleLog;

        private void Awake()
        {
            // Set up the logger.
            consoleLog = Logger;

            // Check for the asset bundles.
            if (!File.Exists($@"{Paths.GameRootPath}\mod_overrides\hiddenpalace.assets") || !File.Exists($@"{Paths.GameRootPath}\mod_overrides\hiddenpalace.scene"))
            {
                consoleLog.LogError("Failed to find either the Assets or Scene files! Please ensure they are correctly located in your Freedom Planet 2's mod_overrides folder.");
                return;
            }

            // Get the config option.
            musicOption = Config.Bind("Sound",
                                      "Music",
                                      0,
                                      "Determines which music track to use.\n0: Default (Mystic Cave 2P in 2013, Track #10 in Proto Palace)\n1: Mystic Cave 2P\n2: Track #10");

            // Load our asset and scene bundles.
            hpzAssetBundle = AssetBundle.LoadFromFile($@"{Paths.GameRootPath}\mod_overrides\hiddenpalace.assets");
            hpzSceneBundle = AssetBundle.LoadFromFile($@"{Paths.GameRootPath}\mod_overrides\hiddenpalace.scene");
            
            // If we're in Debug Mode, then print all the asset names from the asset bundle.
            #if DEBUG
            foreach (string assetName in hpzAssetBundle.GetAllAssetNames())
                consoleLog.LogInfo(assetName);
            #endif

            // Define and register Proto Palace.
            CustomStage protoPalace = new()
            {
                uid = "K24_ProtoPalaceZone",
                name = "Proto Palace Zone",
                description = "A recreation of the unfinished version of Hidden Palace Zone found in the Sonic 2 prototypes.",
                author = "Knuxfan24",
                version = "1.0.0",
                parTime = 6000,
                sceneName = "ProtoPalace",
                preview = hpzAssetBundle.LoadAsset<Sprite>("proto_preview")
            };
            FP2Lib.Stage.StageHandler.RegisterStage(protoPalace);

            // Define and register Hidden Palace.
            CustomStage hiddenPalace2013 = new()
            {
                uid = "K24_HiddenPalaceZone2013",
                name = "Hidden Palace Zone (2013)",
                description = "A recreation of the 2013 version of Hidden Palace Zone found in the Sonic 2 mobile/Sonic Origins releases.",
                author = "Knuxfan24",
                version = "1.0.0",
                parTime = 24000,
                sceneName = "HiddenPalace2013",
                preview = hpzAssetBundle.LoadAsset<Sprite>("hpz2013_preview")
            };
            FP2Lib.Stage.StageHandler.RegisterStage(hiddenPalace2013);

            // Load the two music tracks from the asset bundle.
            hpzMusic = hpzAssetBundle.LoadAsset<AudioClip>("m_stage_hiddenpalace");
            mcz2PMusic = hpzAssetBundle.LoadAsset<AudioClip>("m_stage_mysticcave2");

            // Register vinyls for the two tracks.
            FP2Lib.Vinyl.VinylHandler.RegisterVinyl("k24.vinyl_hpz_proper", "Hidden Palace Zone", hpzMusic, FP2Lib.Vinyl.VAddToShop.All, 11);
            FP2Lib.Vinyl.VinylHandler.RegisterVinyl("k24.vinyl_hpz_mcz2p", "Mystic Cave Zone (2-Player)", mcz2PMusic, FP2Lib.Vinyl.VAddToShop.All, 11);

            // Patch all the functions that need patching.
            Harmony.CreateAndPatchAll(typeof(FPAudioPatcher));
            Harmony.CreateAndPatchAll(typeof(FPCheckpointPatcher));
            Harmony.CreateAndPatchAll(typeof(FPPlayerPatcher));
            Harmony.CreateAndPatchAll(typeof(FPWaterSurfacePatcher));
            Harmony.CreateAndPatchAll(typeof(PipeTunnelPatcher));
            Harmony.CreateAndPatchAll(typeof(PlantBlockPatcher));
            Harmony.CreateAndPatchAll(typeof(MergaBlueMoonPatcher));
            Harmony.CreateAndPatchAll(typeof(StageModifications));
        }
    }
}
