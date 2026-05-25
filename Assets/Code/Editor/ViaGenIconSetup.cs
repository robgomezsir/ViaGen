#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ViaGen.Core;

namespace ViaGen.Editor
{
    public static class ViaGenIconSetup
    {
        private static string AppIconPath => ViaGenAssetPaths.ResourcesArtUiAppIcon + ".png";
        private static string IconSheetPath => ViaGenAssetPaths.ResourcesArtUiIconSheet + ".png";
        private const int SheetWidth = 1024;
        private const int SheetHeight = 1536;
        private const int HeaderPx = 210;
        private const int GridCols = 6;
        private const int GridRows = 7;

        [MenuItem("ViaGen/Assets/Setup Icons And App Icon")]
        public static void SetupIconsMenu() => SetupIconsInternal();

        public static void SetupIconsInternal()
        {
            ViaGenProjectStructure.CreateAllFolders();
            ConfigureAppIconImport();
            ConfigureIconSheetImport();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[ViaGen] Icones configurados.");
        }

        private static void ConfigureAppIconImport()
        {
            var importer = AssetImporter.GetAtPath(AppIconPath) as TextureImporter;
            if (importer == null) return;
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.alphaIsTransparency = true;
            importer.mipmapEnabled = false;
            importer.maxTextureSize = 1024;
            importer.SaveAndReimport();
        }

        private static void ConfigureIconSheetImport()
        {
            var importer = AssetImporter.GetAtPath(IconSheetPath) as TextureImporter;
            if (importer == null) return;
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.alphaIsTransparency = true;
            importer.mipmapEnabled = false;
            importer.maxTextureSize = 2048;

            var cellW = SheetWidth / GridCols;
            var contentH = SheetHeight - HeaderPx;
            var cellH = contentH / GridRows;
            var sprites = new List<SpriteMetaData>();
            var names = System.Enum.GetNames(typeof(ViaGenIconId));
            for (var row = 0; row < GridRows; row++)
            {
                for (var col = 0; col < GridCols; col++)
                {
                    var index = row * GridCols + col;
                    if (index >= names.Length) break;
                    sprites.Add(new SpriteMetaData
                    {
                        name = names[index],
                        rect = new Rect(col * cellW, SheetHeight - HeaderPx - (row + 1) * cellH, cellW, cellH),
                        alignment = (int)SpriteAlignment.Center,
                        pivot = new Vector2(0.5f, 0.5f)
                    });
                }
            }
            importer.spritesheet = sprites.ToArray();
            importer.SaveAndReimport();
        }
    }
}
#endif
