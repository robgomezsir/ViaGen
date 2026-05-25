using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ViaGen.Core;
using ViaGen.Planet;

namespace ViaGen.UI
{
    public static class ViaGenIcons
    {
        private const string SheetResourcePath = "Art/UI/IconSheet";
        private const string AppIconResourcePath = "Art/UI/AppIcon";
        private const string IconsFolderPath = "Art/UI/Icons";

        private static readonly Dictionary<ViaGenIconId, Sprite> Cache = new();
        private static Sprite _appIcon;
        private static bool _loaded;

        public static Sprite AppIcon
        {
            get { EnsureLoaded(); return _appIcon; }
        }

        public static Sprite Get(ViaGenIconId id)
        {
            EnsureLoaded();
            if (Cache.TryGetValue(id, out var sprite) && sprite != null)
                return sprite;
            return Resources.Load<Sprite>($"{IconsFolderPath}{id}");
        }

        public static ViaGenIconId ForPlanet(PlanetEmotion planet) => planet switch
        {
            PlanetEmotion.Luto => ViaGenIconId.BrokenRocket,
            PlanetEmotion.Culpa => ViaGenIconId.MonitorAlert,
            PlanetEmotion.Medo => ViaGenIconId.Vortex,
            PlanetEmotion.Nostalgia => ViaGenIconId.TeddyBear,
            PlanetEmotion.Esperanca => ViaGenIconId.PlanetSmile,
            _ => ViaGenIconId.PlanetRing
        };

        public static Image CreateIconImage(Transform parent, ViaGenIconId iconId, Vector2 size)
        {
            var sprite = Get(iconId);
            if (sprite == null) return null;
            var go = new GameObject($"Icon_{iconId}");
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.sizeDelta = size;
            var img = go.AddComponent<Image>();
            img.sprite = sprite;
            img.preserveAspect = true;
            img.raycastTarget = false;
            return img;
        }

        private static void EnsureLoaded()
        {
            if (_loaded) return;
            _loaded = true;

            _appIcon = Resources.Load<Sprite>(AppIconResourcePath);
            var sprites = Resources.LoadAll<Sprite>(SheetResourcePath);
            foreach (var sprite in sprites)
            {
                if (sprite == null) continue;
                if (TryParseIconName(sprite.name, out var id))
                    Cache[id] = sprite;
            }
        }

        private static bool TryParseIconName(string name, out ViaGenIconId id)
        {
            id = default;
            if (string.IsNullOrEmpty(name)) return false;
            var key = name;
            var underscore = name.IndexOf('_');
            if (underscore >= 0 && underscore < name.Length - 1)
                key = name[(underscore + 1)..];
            return System.Enum.TryParse(key, true, out id);
        }
    }
}
