using System.IO;
using UnityEditor;
using UnityEngine;

namespace ZFrame.Debugger
{
    public class DebugConfig : ScriptableObject
    {
#if UNITY_EDITOR
        [MenuItem("Assets/Create/DebugConfig")]
        public static void CreateAsset()
        {
            DebugConfig asset = CreateInstance<DebugConfig>();

            asset.exceptionStyle = new GUIStyle();
            asset.errorStyle = new GUIStyle();
            asset.warningStyle = new GUIStyle();
            asset.logStyle = new GUIStyle();

            asset.exceptionStyle.normal.textColor = Color.blue;
            asset.errorStyle.normal.textColor = Color.red;
            asset.warningStyle.normal.textColor = Color.yellow;
            asset.logStyle.normal.textColor = Color.white;

            asset.logStyle.wordWrap = true;
            asset.warningStyle.wordWrap = true;
            asset.errorStyle.wordWrap = true;
            asset.exceptionStyle.wordWrap = true;

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "")
            {
                path = "Assets";
            }
            else if (Path.GetExtension(path) != "")
            {
                path =
                    path.Replace(
                        Path.GetFileName(
                            AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            string assetPathAndName =
                AssetDatabase.GenerateUniqueAssetPath(path + "/DebugConfig.asset");

            AssetDatabase.CreateAsset(asset, assetPathAndName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;
        }
#endif


        [Header("Enable debug")] public bool enableDebug = true;
        [Header("Enable trace")] public bool enableTrace = true;
        [Header("Enable TickCount")] public bool enableTime = true;

        [Header("Max debug lines")] [Range(1, 20)] public int maxDebugLines = 10;
        [Header("Enable exceptions")] public bool enableExceptions = true;
        [Header("Enable errors")] public bool enableErrors = true;
        [Header("Enable warnings")] public bool enableWarnings;
        [Header("Enable logs")] public bool enableLogs;

        [Header("Exception Style settings")] public GUIStyle exceptionStyle;
        [Header("Error Style settings")] public GUIStyle errorStyle;
        [Header("Warning Style settings")] public GUIStyle warningStyle;
        [Header("Log Style settings")] public GUIStyle logStyle;
    }
}