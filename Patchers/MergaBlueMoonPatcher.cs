using System.Linq;
using System.Reflection.Emit;
using UnityEngine.SceneManagement;

namespace FP2_Hidden_Palace_Mod.Patchers
{
    internal class MergaBlueMoonPatcher
    {
        /// <summary>
        /// Removes the line of code that causes Merga's Blue Moon object to deactivate her associated water object until she's spawned.
        /// </summary>
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(MergaBlueMoon), "Start")]
        static IEnumerable<CodeInstruction> RemoveSwimState_InAir(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            for (int i = 153; i <= 156; i++)
                codes[i].opcode = OpCodes.Nop;

            return codes.AsEnumerable();
        }

        /// <summary>
        /// Restores the removed line if the stage isn't Hidden Palace, just in case the base game relies on this quirk in some way.
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MergaBlueMoon), "Start")]
        static void RestoreWaterDeactivator(MergaBlueMoon __instance)
        {
            if (SceneManager.GetActiveScene().name != "HiddenPalace2013")
                __instance.waterFront.activationMode = FPActivationMode.NEVER_ACTIVE;
        }

        /// <summary>
        /// Allows the camera to move to the right of the Merga arena and kills the right boundry pusher.
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MergaBlueMoon), "Deactivate")]
        static void ChangeCameraBounds(MergaBlueMoon __instance)
        {
            if (SceneManager.GetActiveScene().name == "HiddenPalace2013")
            {
                __instance.hotspot.enabled = false;
                GameObject.Destroy(UnityEngine.GameObject.Find("RightBoundry"));
            }
        }
    }
}
