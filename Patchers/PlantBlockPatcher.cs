using UnityEngine.SceneManagement;

namespace FP2_Hidden_Palace_Mod.Patchers
{
    internal class PlantBlockPatcher
    {
        /// <summary>
        /// Stops the faded background sprite from being left behind if in Hidden Palace.
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(PlantBlock), "State_Death")]
        static void HideUponDeathInHPZ(PlantBlock __instance)
        {
            if (SceneManager.GetActiveScene().name == "HiddenPalace2013")
                __instance.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
