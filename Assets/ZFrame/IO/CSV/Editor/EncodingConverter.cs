using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class EncodingConverter
{
    [MenuItem("ZFrame/Encoding/GBK -> UTF-8")]
    private static void ConvertGBKToUTF8()
    {
        if (!(Selection.activeObject is TextAsset)) return;

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(path) || Path.GetExtension(path) == "cs" || Path.GetExtension(path) == "js")
            return;

        string content;
        using (StreamReader reader = new StreamReader(path, Encoding.GetEncoding("GBK")))
        {
            content = reader.ReadToEnd();
        }
        if(string.IsNullOrEmpty(content))
            return;

        using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
        {
            writer.Write(content);
        }

        AssetDatabase.Refresh();
    }

    [MenuItem("ZFrame/Encoding/UTF-8 -> GBK")]
    private static void ConvertUTF8ToGBK()
    {
        if (!(Selection.activeObject is TextAsset)) return;

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(path) || Path.GetExtension(path) == "cs" || Path.GetExtension(path) == "js")
            return;

        using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
        {
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.GetEncoding("GBK")))
            {
                writer.Write(reader.ReadToEnd());
            }
        }

        AssetDatabase.Refresh();
    }
}