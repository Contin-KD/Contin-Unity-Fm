using excellent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class ConfigTool
{
    [MenuItem("Tools/�������� &#c")]
    public static void ExportConfig()
    {
        List<Type> types = new List<Type>();
        foreach (var type in Assembly.Load("Assembly-CSharp-Editor").GetTypes())
        {
            if (type.Namespace == "ConfigDefinition")
            {
                types.Add(type);
            }
        }

        Excellent.Go(new ExportInfo()
        {
            Namespace = "Config",
            ConfigDefinitions = types.ToArray(),
            ExcelDirectory = Application.dataPath + "/../../design/config",
            SerializeDirectory = Application.dataPath + "/BundleAssets/Config",
            CodeDirectory = Application.dataPath + "/Scripts/Config/Code",
            WriteExcel = false,
            WithUnity = true,
            //BundleOffset = BundleLoader.BundleOffset,
            OnLog = OnLog,
        });
        AssetDatabase.Refresh();
        Debug.Log("�����������");
    }

    [MenuItem("Tools/�������ýṹ &#v")]
    public static void WriteAndExportConfig()
    {
        Directory.Delete(Application.dataPath + "/Scripts/Config/Code", true);

        List<Type> types = new List<Type>();
        foreach (var type in Assembly.Load("Assembly-CSharp-Editor").GetTypes())
        {
            if (type.Namespace == "ConfigDefinition")
            {
                types.Add(type);
            }
        }

        Excellent.Go(new ExportInfo()
        {
            Namespace = "Config",
            ConfigDefinitions = types.ToArray(),
            ExcelDirectory = Application.dataPath + "/../../design/config",
            SerializeDirectory = Application.dataPath + "/BundleAssets/Config",
            CodeDirectory = Application.dataPath + "/Scripts/Config/Code",
            WriteExcel = true,
            WithUnity = true,
            //BundleOffset = BundleLoader.BundleOffset,
            OnLog = OnLog,
        });
        AssetDatabase.Refresh();
        Debug.Log("�������ýṹ�����ҵ����ɹ�");
    }

    private static void OnLog(string message)
    {
        Debug.Log(message);
    }
}