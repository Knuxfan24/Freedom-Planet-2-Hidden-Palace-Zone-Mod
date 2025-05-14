using HarmonyLib;

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

        /// <summary>
        /// Force kills the player's velocity when they're in a scripted tube.
        /// </summary>
        public static void State_Scripted_Tube()
        {
            player.velocity.x = 0f;
            player.velocity.y = 0f;
        }

        /// <summary>
        /// Stops the moves in Carol's ground state from being executed by anyone other than her.
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(FPPlayer), "Action_Carol_GroundMoves")]
        private static bool BlockCarolInput()
        {
            // Check if the player isn't Carol, if so, then don't run the original function.
            if (player.characterID != FPCharacterID.CAROL && player.characterID != FPCharacterID.BIKECAROL)
                return false;

            // Run the original function so Carol isn't stripped of her moves.
            return true;
        }
    }
}
