using System;
using UnityEngine;

namespace ViaGen.Core
{
    public enum MainMenuAction
    {
        Continue,
        NewGame,
        Memories,
        Ship,
        Options,
        Quit
    }

    [Serializable]
    public class MainMenuEntry
    {
        public MainMenuAction action;
        public ViaGenIconId iconId = ViaGenIconId.Rocket;
        public string titleKey;
        public string subtitleKey;
        public bool requiresSave;
    }

    [CreateAssetMenu(fileName = "MainMenuConfig", menuName = "ViaGen/Main Menu Config")]
    public class MainMenuConfig : ScriptableObject
    {
        public MainMenuEntry[] entries =
        {
            new() { action = MainMenuAction.Continue, iconId = ViaGenIconId.Journey, titleKey = "menu_continue_title", subtitleKey = "menu_continue_sub", requiresSave = true },
            new() { action = MainMenuAction.NewGame, iconId = ViaGenIconId.Rocket, titleKey = "menu_new_title", subtitleKey = "menu_new_sub" },
            new() { action = MainMenuAction.Memories, iconId = ViaGenIconId.Photos, titleKey = "menu_memories_title", subtitleKey = "menu_memories_sub" },
            new() { action = MainMenuAction.Ship, iconId = ViaGenIconId.RocketRepair, titleKey = "menu_ship_title", subtitleKey = "menu_ship_sub" },
            new() { action = MainMenuAction.Options, iconId = ViaGenIconId.Gear, titleKey = "menu_options_title", subtitleKey = "menu_options_sub" },
            new() { action = MainMenuAction.Quit, iconId = ViaGenIconId.Gear, titleKey = "menu_quit_title", subtitleKey = "menu_quit_sub" }
        };

        public float staggerDelay = 0.08f;
        public float fadeDuration = 0.35f;
        public Color hoverColor = new(0f, 0.9f, 1f, 1f);
    }
}
