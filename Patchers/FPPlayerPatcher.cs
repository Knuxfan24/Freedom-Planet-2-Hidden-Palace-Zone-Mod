namespace FP2_Hidden_Palace_Mod.Patchers
{
    internal class FPPlayerPatcher
    {
        // A reference to the player object.
        public static FPPlayer player;

        /// <summary>
        /// Gets and stores the player object.
        /// </summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(FPPlayer), "Start")]
        private static void GetInstance(FPPlayer __instance) => player = __instance;
    }
}
