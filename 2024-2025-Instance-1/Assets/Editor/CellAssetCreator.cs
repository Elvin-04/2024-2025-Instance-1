using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public static class CellAssetCreator
    {
        [MenuItem("Assets/Create/Grid/Cell")]
        public static void CreateCellAsset()
        {
            // Create an instance of the custom Cell class
            Cell newCell = ScriptableObject.CreateInstance<Cell>();

            // Prompt for saving the asset in the Project window
            string path = EditorUtility.SaveFilePanelInProject(
                "Save Cell Asset",
                "NewCell",
                "asset",
                "Select a location to save the Cell asset."
            );

            if (!string.IsNullOrEmpty(path))
            {
                // Create the asset at the chosen location
                AssetDatabase.CreateAsset(newCell, path);
                AssetDatabase.SaveAssets();

                Debug.Log($"Cell asset created at {path}");
            }
        }
    }
}