using UnityEditor;
using System.IO;
using UnityEngine;

public class SpriteImportTool : EditorWindow
{
    // 外部图片文件夹路径
    private string externalImagePath = Application.dataPath + "/Image/RoleCard/";
    // Unity项目内目标文件夹
    private string targetAssetPath = Application.dataPath + "/Sprites/RoleCard/";

    [MenuItem("Tools/批量导入图片为Sprite")]
    public static void OpenWindow()
    {
        GetWindow<SpriteImportTool>("图片批量导入");
    }

    void OnGUI()
    {
        GUILayout.Label("外部图片文件夹路径：");
        externalImagePath = GUILayout.TextField(externalImagePath);
        GUILayout.Label("Unity目标文件夹路径：");
        targetAssetPath = GUILayout.TextField(targetAssetPath);
        if (GUILayout.Button("开始导入"))
        {
            ImportImages();
        }
    }

    void ImportImages()
    {
        if (!Directory.Exists(externalImagePath))
        {
            EditorUtility.DisplayDialog("错误", "外部图片文件夹不存在！", "确定");
            return;
        }
        // 创建Unity目标文件夹（若不存在）
        if (!Directory.Exists(targetAssetPath))
        {
            Directory.CreateDirectory(targetAssetPath);
        }
        // 获取外部图片文件（PNG/JPG）
        string[] imageFiles = Directory.GetFiles(externalImagePath, "*.*", SearchOption.TopDirectoryOnly);
        foreach (string file in imageFiles)
        {
            string ext = Path.GetExtension(file).ToLower();
            if (ext != ".png" && ext != ".jpg" && ext != ".jpeg") continue;
            // 复制文件到Unity项目目录
            string fileName = Path.GetFileName(file);
            string destPath = Path.Combine(targetAssetPath, fileName);
            File.Copy(file, destPath, true);
            // 刷新Unity资源数据库
            AssetDatabase.Refresh();
            // 设置图片为Sprite格式
            TextureImporter importer = AssetImporter.GetAtPath(destPath) as TextureImporter;
            if (importer != null)
            {
                importer.textureType = TextureImporterType.Sprite;
                importer.spritePixelsPerUnit = 100; // 像素单位，UI常用100
                importer.filterMode = FilterMode.Bilinear;
                importer.textureCompression = TextureImporterCompression.Compressed; // 标准压缩
                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
            }
        }
        EditorUtility.DisplayDialog("完成", $"成功导入{imageFiles.Length}张图片！", "确定");
    }
}