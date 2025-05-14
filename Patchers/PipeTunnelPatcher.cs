using UnityEngine.SceneManagement;

namespace FP2_Hidden_Palace_Mod.Patchers
{
    internal class PipeTunnelPatcher
    {
        /// <summary>
        /// Stops the Shenlin Park pipe used to fake the upside down quarter pipe from activating under certain conditions.
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PipeTunnel), "PipeEnter")]
        private static bool PipeHack()
        {
            // If we haven't found the player or we're not in Hidden Palace, then run the original function.
            if (FPPlayerPatcher.player == null || SceneManager.GetActiveScene().name != "HiddenPalace2013")
                return true;

            // If the player isn't moving upwards, or their x or y values are higher than a specific point, then don't run the original function.
            if (FPPlayerPatcher.player.velocity.y <= 0 || FPPlayerPatcher.player.position.x >= 3232 || FPPlayerPatcher.player.position.y >= -3024)
                return false;

            // Run the original function.
            return true;
        }
    }
}
