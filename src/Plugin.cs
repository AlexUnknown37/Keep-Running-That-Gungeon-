using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace KeepRunning
{
    // Stops Enter the Gungeon from pausing (and optionally muting) when you tab out.
    [BepInPlugin(Guid, Name, ModVersion)]
    [BepInProcess("EtG.exe")]
    public class Plugin : BaseUnityPlugin
    {
        public const string Guid = "keeprunning.etg";
        public const string Name = "Keep Running";
        public const string ModVersion = "0.1.0";

        // Settings, saved in BepInEx/config/keeprunning.etg.cfg.
        public static ConfigEntry<bool> PauseWhenUnfocused;
        public static ConfigEntry<bool> MuteWhenUnfocused;

        private bool originalRunInBackground;

        private void Awake()
        {
            PauseWhenUnfocused = Config.Bind("General", "PauseWhenUnfocused", false,
                "Pause the game when you tab out (the game's normal behaviour).");
            MuteWhenUnfocused = Config.Bind("General", "MuteWhenUnfocused", true,
                "Silence the game's audio when you tab out (the game's normal behaviour).");

            originalRunInBackground = Application.runInBackground;
            new Harmony(Guid).PatchAll(typeof(Patches));
            Logger.LogInfo(Name + " " + ModVersion + " loaded. Pause when unfocused: " + PauseWhenUnfocused.Value
                + ", mute when unfocused: " + MuteWhenUnfocused.Value);
        }

        private void Update()
        {
            // Unity itself also stops updating an unfocused game unless runInBackground is on.
            bool wanted = PauseWhenUnfocused.Value ? originalRunInBackground : true;
            if (Application.runInBackground != wanted)
                Application.runInBackground = wanted;
        }
    }

    public static class Patches
    {
        private static readonly FieldInfo hasFocus = AccessTools.Field(typeof(GameManager), "m_applicationHasFocus");

        // GameManager.OnApplicationFocus(false) calls Pause(). Skip it, but still record the focus state.
        [HarmonyPrefix]
        [HarmonyPatch(typeof(GameManager), "OnApplicationFocus")]
        private static bool BeforeGameFocus(GameManager __instance, bool __0)
        {
            if (Plugin.PauseWhenUnfocused.Value)
                return true;
            hasFocus.SetValue(__instance, __0);
            return false;
        }

        // The sound engine is suspended through ActivateAudio(false) when the window loses focus.
        [HarmonyPrefix]
        [HarmonyPatch(typeof(AkSoundEngineController), "ActivateAudio")]
        private static bool BeforeActivateAudio(bool __0)
        {
            return __0 || Plugin.MuteWhenUnfocused.Value;
        }
    }
}
