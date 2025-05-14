using UnityEngine.SceneManagement;

namespace FP2_Hidden_Palace_Mod.Patchers
{
    internal class FPAudioPatcher
    {
        /// <summary>
        /// Changes the Hidden Palace (2013) and Proto Palace music based on the config option.
        /// </summary>
        /// <param name="bgmMusic">The music that the PlayMusic function in FPAudio has been told to use.</param>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(FPAudio), nameof(FPAudio.PlayMusic), new Type[] { typeof(AudioClip), typeof(float) })]
        static void MusicConfigOption(ref AudioClip bgmMusic)
        {
            // Check a few things before continuing. Namely:
            // That there is a song set to play at all.
            // That the config value isn't the default one.
            // And that we're in either Proto Palace or Hidden Palace (2013).
            if (bgmMusic != null && Plugin.musicOption.Value != 0 && (SceneManager.GetActiveScene().name == "ProtoPalace" || SceneManager.GetActiveScene().name == "HiddenPalace2013"))
            {
                // Check the config option's value and the current song name and change it if approriate.
                if (Plugin.musicOption.Value == 1 && bgmMusic.name == "M_Stage_HiddenPalace") bgmMusic = Plugin.mcz2PMusic;
                if (Plugin.musicOption.Value == 2 && bgmMusic.name == "M_Stage_MysticCave2") bgmMusic = Plugin.hpzMusic;
            }
        }
    }
}
