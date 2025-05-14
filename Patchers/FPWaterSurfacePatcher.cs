using UnityEngine.SceneManagement;

namespace FP2_Hidden_Palace_Mod.Patchers
{
    internal class FPWaterSurfacePatcher
    {
        // The Y value the ocean object should move to.
        static int targetY = -3736;

        // Reference to the ocean object.
        static GameObject ocean;

        /// <summary>
        /// Changes the water level throughout Hidden Palace.
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FPWaterSurface), "Update")]
        private static void HiddenPalace2013WaterLevel()
        {
            // If we haven't found a player yet, abort.
            if (FPPlayerPatcher.player == null)
                return;

            // If we haven't found the ocean yet, then look for it and abort.
            if (ocean == null)
            {
                ocean = UnityEngine.GameObject.Find("Ocean");
                return;
            }

            // Check if we're in Hidden Palace 2013.
            if (SceneManager.GetActiveScene().name == "HiddenPalace2013")
            {
                // Set the target heights depending on the player's X position.
                if (FPPlayerPatcher.player.transform.position.x < 7424) targetY = -3736;
                else if (FPPlayerPatcher.player.transform.position.x >= 7424) targetY = -3072;

                // Change the Y position of the ocean object depending on the target value.
                if (ocean.transform.position.y < targetY) ocean.transform.position = new(ocean.transform.position.x, ocean.transform.position.y + (8 * FPStage.deltaTime), ocean.transform.position.z);
                if (ocean.transform.position.y > targetY) ocean.transform.position = new(ocean.transform.position.x, ocean.transform.position.y - (8 * FPStage.deltaTime), ocean.transform.position.z);
            }

            // Check if we're in Proto Palace.
            if (SceneManager.GetActiveScene().name == "ProtoPalace")
            {
                // Instantly snap the water to a certain height depending on the player's position.
                if (FPPlayerPatcher.player.transform.position.x < 12800) ocean.transform.position = new(ocean.transform.position.x, -3072, ocean.transform.position.z);
                else if (FPPlayerPatcher.player.transform.position.x >= 12800) ocean.transform.position = new(ocean.transform.position.x, -3568, ocean.transform.position.z);
            }
        }
    }
}
