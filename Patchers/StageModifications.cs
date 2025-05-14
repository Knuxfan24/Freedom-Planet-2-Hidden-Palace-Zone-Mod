using FP2_Hidden_Palace_Mod.CustomObjectScripts;
using System.Linq;
using UnityEngine.SceneManagement;

namespace FP2_Hidden_Palace_Mod.Patchers
{
    internal class StageModifications
    {
        /// <summary>
        /// Adds the script to any monitor placed in Proto Palace or Hidden Palace 2013.
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FPStage), "Start")]
        private static void AddMonitorScripts()
        {
            // Check that we're in either Proto Palace or Hidden Palace.
            if (SceneManager.GetActiveScene().name == "ProtoPalace" || SceneManager.GetActiveScene().name == "HiddenPalace2013")
            {
                // Find all of the monitor objects.
                GameObject[] monitors = UnityEngine.Object.FindObjectsOfType<GameObject>().Where(x => x.name.StartsWith("Monitor")).ToArray();

                // Loop through each monitor object.
                foreach (GameObject monitor in monitors)
                {
                    // Get the name of this monitor by spliting it on the space in the name and discarding everything beyond it.
                    string name = monitor.name.Split(' ')[0];

                    // Create and attach the monitor script to this monitor's object.
                    ItemMonitor monitorScript = monitor.AddComponent<ItemMonitor>();

                    // Determine the type of monitor to place.
                    switch (name)
                    {
                        case "MonitorLife": monitorScript.itemType = MonitorTypes.LifePetals; break;
                        case "MonitorCrystals": monitorScript.itemType = MonitorTypes.Crystals; break;
                        case "Monitor1UP": monitorScript.itemType = MonitorTypes.ExtraLife; break;
                        case "MonitorCarol": monitorScript.itemType = MonitorTypes.Carol1UP; break;
                        case "MonitorInvincibility": monitorScript.itemType = MonitorTypes.Invincibility; break;
                        case "MonitorEarth": monitorScript.itemType = MonitorTypes.EarthShield; break;
                        case "MonitorAqua": monitorScript.itemType = MonitorTypes.AquaShield; break;
                        case "MonitorMetal": monitorScript.itemType = MonitorTypes.MetalShield; break;
                        case "MonitorFire": monitorScript.itemType = MonitorTypes.FireShield; break;

                        // Throw an error and destroy this monitor if the item type isn't handled.
                        default:
                            Plugin.consoleLog.LogError($"Unhandled Monitor Type {name}!");
                            GameObject.Destroy(monitor);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Constructs the waterfalls in Proto Palace and Hidden Palace from a hardcoded list.
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FPStage), "Start")]
        private static void AddWaterfalls()
        {
            // Find all of the waterfall objects.
            GameObject[] waterfallObjects = UnityEngine.Object.FindObjectsOfType<GameObject>().Where(x => x.name.StartsWith("Waterfall")).ToArray();

            // Loop through each waterfall in either Proto Palace or Hidden Palace and add the script while also setting the correct segment count.
            switch (SceneManager.GetActiveScene().name)
            {
                case "ProtoPalace":
                    foreach (GameObject waterfall in waterfallObjects)
                    {
                        switch (waterfall.name)
                        {
                            case "Waterfall": waterfall.AddComponent<HPZWaterfall>().segmentCount = 22; break;
                            case "Waterfall (1)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 20; break;
                            case "Waterfall (2)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 20; break;
                            case "Waterfall (3)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 1; break;
                            case "Waterfall (4)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 3; break;
                            case "Waterfall (5)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 12; break;
                            case "Waterfall (6)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 10; break;
                            case "Waterfall (7)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 10; break;
                        }
                    }
                    break;

                case "HiddenPalace2013":
                    foreach (GameObject waterfall in waterfallObjects)
                    {
                        switch (waterfall.name)
                        {
                            case "Waterfall": waterfall.AddComponent<HPZWaterfall>().segmentCount = 3; break;
                            case "Waterfall (1)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 5; break;
                            case "Waterfall (2)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 10; break;
                            case "Waterfall (3)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 32; break;
                            case "Waterfall (4)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 20; break;
                            case "Waterfall (5)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 14; break;
                            case "Waterfall (6)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 1; break;
                            case "Waterfall (7)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 3; break;
                            case "Waterfall (8)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 10; break;
                            case "Waterfall (9)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 10; break;
                            case "Waterfall (10)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 22; break;
                            case "Waterfall (11)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 20; break;
                            case "Waterfall (12)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 10; break;
                            case "Waterfall (13)": waterfall.AddComponent<HPZWaterfall>().segmentCount = 10; break;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Creates the scripted tubes in Hidden Palace from a hardcoded list.
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FPStage), "Start")]
        private static void AddScriptedTubes()
        {
            // Only carry out this function if we're in Hidden Palace.
            if (SceneManager.GetActiveScene().name != "HiddenPalace2013")
                return;

            // Find all the tube objects.
            GameObject[] tubeObjects = UnityEngine.Object.FindObjectsOfType<GameObject>().Where(x => x.name.StartsWith("ScriptedTube")).ToArray();

            // Loop through each tube object.
            foreach (var tube in tubeObjects)
            {
                // Add the script onto this tube and set it to be always active.
                ScriptedTube script = tube.AddComponent<ScriptedTube>();
                script.activationMode = FPActivationMode.ALWAYS_ACTIVE;

                // Set the points depending on which tube this is.
                switch (tube.name)
                {
                    case "ScriptedTube":
                        script.points =
                        [
                            new(6282, -2220),
                            new(6360, -2200),
                            new(6432, -2160),
                            new(6488, -2088),
                            new(6552, -2040),
                            new(6624, -2016),
                            new(6656, -2016)
                        ];
                        break;

                    case "ScriptedTube (1)":
                        script.points =
                        [
                            new(17016, -3040),
                            new(17104, -3048),
                            new(17176, -3088),
                            new(17232, -3144),
                            new(17290, -3198),
                            new(17372, -3236),
                            new(17458, -3212),
                            new(17492, -3130),
                            new(17492, -3056),
                            new(17440, -2992),
                            new(17370, -2994),
                            new(17300, -3022),
                            new(17244, -3070),
                            new(17160, -3136),
                            new(17082, -3150),
                            new(17018, -3130),
                            new(16972, -3072),
                            new(16970, -2984),
                            new(16972, -2876),
                            new(17000, -2816),
                            new(17056, -2788),
                            new(17152, -2784)
                        ];
                        break;
                }
            }
        }

        /// <summary>
        /// Adds the script to any Pipe Valve placed in Hidden Palace.
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FPStage), "Start")]
        private static void AddPipeValves()
        {
            // Only carry out this function if we're in Hidden Palace.
            if (SceneManager.GetActiveScene().name != "HiddenPalace2013")
                return;

            // Find all the valve objects.
            GameObject[] valveObjects = UnityEngine.Object.FindObjectsOfType<GameObject>().Where(x => x.name == "valve").ToArray();

            // Loop through each valve object, add the script and set it to be always active.
            foreach (GameObject valve in valveObjects)
            {
                HPZPipeValve script = valve.AddComponent<HPZPipeValve>();
                script.activationMode = FPActivationMode.ALWAYS_ACTIVE;
            }
        }

        /// <summary>
        /// Adds the scripts for the zoom tubes in Proto Palace and Hidden Palace from a hardcoded list.
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FPStage), "Start")]
        private static void AddZoomTubePoints()
        {
            // Find all of the zoom tube objects.
            GameObject[] zoomTubeObjects = UnityEngine.Object.FindObjectsOfType<GameObject>().Where(x => x.name.StartsWith("ZoomTubePoint")).ToArray();

            // Loop through each zoom tube in either Proto Palace or Hidden Palace and add the script while also setting the correct exit direction.
            switch (SceneManager.GetActiveScene().name)
            {
                case "ProtoPalace":
                    foreach (GameObject zoomTube in zoomTubeObjects)
                    {
                        switch (zoomTube.name)
                        {
                            case "ZoomTubePoint": zoomTube.AddComponent<ZoomTubePoint>().exitDirection = ExitDirection.Up; break;
                            case "ZoomTubePoint (1)": zoomTube.AddComponent<ZoomTubePoint>().exitDirection = ExitDirection.Right; break;
                            case "ZoomTubePoint (2)": zoomTube.AddComponent<ZoomTubePoint>().exitDirection = ExitDirection.Up; break;
                        }
                    }
                    break;

                case "HiddenPalace2013":
                    foreach (GameObject zoomTube in zoomTubeObjects)
                    {
                        switch (zoomTube.name)
                        {
                            case "ZoomTubePoint": zoomTube.AddComponent<ZoomTubePoint>().exitDirection = ExitDirection.Up; break;
                            case "ZoomTubePoint (1)": zoomTube.AddComponent<ZoomTubePoint>().exitDirection = ExitDirection.Up; break;
                            case "ZoomTubePoint (2)": zoomTube.AddComponent<ZoomTubePoint>().exitDirection = ExitDirection.Right; break;
                            case "ZoomTubePoint (3)": zoomTube.AddComponent<ZoomTubePoint>().exitDirection = ExitDirection.Up; break;
                            case "ZoomTubePoint (4)": zoomTube.AddComponent<ZoomTubePoint>().exitDirection = ExitDirection.Up; break;
                            case "ZoomTubePoint (5)": zoomTube.AddComponent<ZoomTubePoint>().exitDirection = ExitDirection.Right; break;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Adds the sinking platform script to any hpz_sinkingplatform object.
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FPStage), "Start")]
        private static void AddSinkingPlatformScripts()
        {
            // Find all of the sinking platform objects.
            GameObject[] sinkingPlatforms = UnityEngine.Object.FindObjectsOfType<GameObject>().Where(x => x.name.StartsWith("hpz_sinkingplatform")).ToArray();

            // Add the script to each one.
            foreach (GameObject platform in sinkingPlatforms)
                platform.AddComponent<HPZSinkingPlatform>();
        }
    }
}
