using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 生成されるC#スクリプトのテンプレートカスタマイズ
/// </summary>

namespace TadaLib.Editor
{
    /// <summary>
    /// 共通処理
    /// </summary>
    class TemplateScriptCustomizerCommon
    {
        public static void CreateCusomizedScriptImpl<T>(string templateFile) where T : UnityEditor.ProjectWindowCallback.EndNameEditAction
        {
            var resourceFile = Path.Combine(
                Application.dataPath,
                $"TadaLib/Scripts/Editor/Data/{templateFile}");

            // unityで用意されているC#のアイコンを利用する
            var csIcon =
                EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;

            // ScriptableObjectのインスタンスとして作成する
            var endNameEditAction =
                ScriptableObject.CreateInstance<T>();

            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(
                0,                                                      // 多分適当でも大丈夫
                endNameEditAction,                                      // 実行するActionの指定
                "NewBehaviourScript.cs",                                // デフォルトのActionは呼ばれないため名前もここで指定
                csIcon,                                                 // アイコンの設定
                resourceFile);                                          // 使用するスクリプトテンプレート
        }

        public static void ActionImpl(string text, string pathName)
        {
            var encoding = new System.Text.UTF8Encoding(true, false);

            File.WriteAllText(pathName, text, encoding);

            AssetDatabase.ImportAsset(pathName);
            var asset = AssetDatabase.LoadAssetAtPath<MonoScript>(pathName);
            ProjectWindowUtil.ShowCreatedAsset(asset);
        }
    }

    /// <summary>
    /// コンポーネント用
    /// </summary>
    public class TemplateScriptCustomizerComponent : UnityEditor.ProjectWindowCallback.EndNameEditAction
    {
        [MenuItem("Assets/Create/Component C# Script", isValidateFunction: false, priority: 76)]
        private static void CreateCustomizedScript()
        {
            TemplateScriptCustomizerCommon.CreateCusomizedScriptImpl<TemplateScriptCustomizerComponent>(
                "C# Script-NewComponentScript.cs.txt");
        }

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var text = File.ReadAllText(resourceFile);

            var directoryName = Path.GetDirectoryName(pathName).Replace(@"\", ".").Replace("App.", "").Replace("Assets.", "").Replace("Scripts.","");
            var name = Path.GetFileNameWithoutExtension(pathName);
            var scriptName = name.Replace(" ", "");

            text = text.Replace("#DIRECTORYNAME#", directoryName);
            text = text.Replace("#SCRIPTNAME#", scriptName);
            text = text.Replace("#NOTRIM#", "");

            TemplateScriptCustomizerCommon.ActionImpl(text, pathName);
        }
    }

    /// <summary>
    /// ステート用
    /// </summary>
    public class TemplateScriptCustomizerState : UnityEditor.ProjectWindowCallback.EndNameEditAction
    {
        [MenuItem("Assets/Create/State C# Script", isValidateFunction: false, priority: 76)]
        private static void CreateCustomizedScript()
        {
            TemplateScriptCustomizerCommon.CreateCusomizedScriptImpl<TemplateScriptCustomizerState>(
                "C# Script-NewStateScript.cs.txt");
        }

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var text = File.ReadAllText(resourceFile);

            var directoryName = Path.GetDirectoryName(pathName).Replace(@"\", ".").Replace("App.", "").Replace("Assets.", "").Replace("Scripts.", "");
            var name = Path.GetFileNameWithoutExtension(pathName);
            var scriptName = name.Replace(" ", "");

            text = text.Replace("#DIRECTORYNAME#", directoryName);
            text = text.Replace("#SCRIPTNAME#", scriptName);
            text = text.Replace("#NOTRIM#", "");

            TemplateScriptCustomizerCommon.ActionImpl(text, pathName);
        }
    }

    /// <summary>
    /// Managerコンポーネント用
    /// </summary>
    public class TemplateScriptCustomizerManagerComponent : UnityEditor.ProjectWindowCallback.EndNameEditAction
    {
        [MenuItem("Assets/Create/ManagerComponent C# Script", isValidateFunction: false, priority: 76)]
        private static void CreateCustomizedScript()
        {
            TemplateScriptCustomizerCommon.CreateCusomizedScriptImpl<TemplateScriptCustomizerComponent>(
                "C# Script-NewManagerComponentScript.cs.txt");
        }

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var text = File.ReadAllText(resourceFile);

            var directoryName = Path.GetDirectoryName(pathName).Replace(@"\", ".").Replace("App.", "").Replace("Assets.", "").Replace("Scripts.", "");
            var name = Path.GetFileNameWithoutExtension(pathName);
            var scriptName = name.Replace(" ", "");

            text = text.Replace("#DIRECTORYNAME#", directoryName);
            text = text.Replace("#SCRIPTNAME#", scriptName);
            text = text.Replace("#NOTRIM#", "");

            TemplateScriptCustomizerCommon.ActionImpl(text, pathName);
        }
    }

    /// <summary>
    /// そのほか用
    /// </summary>
    public class TemplateScriptCustomizerDefault : UnityEditor.ProjectWindowCallback.EndNameEditAction
    {
        [MenuItem("Assets/Create/Default C# Script", isValidateFunction: false, priority: 76)]
        private static void CreateCustomizedScript()
        {
            TemplateScriptCustomizerCommon.CreateCusomizedScriptImpl<TemplateScriptCustomizerDefault>(
                "C# Script-NewDefaultScript.cs.txt");
        }

        public override void Action(int instanceId, string pathName, string resourceFile)
        {
            var text = File.ReadAllText(resourceFile);

            var directoryName = Path.GetDirectoryName(pathName).Replace(@"\", ".").Replace("App.", "").Replace("Assets.", "").Replace("Scripts.", "");
            var name = Path.GetFileNameWithoutExtension(pathName);
            var scriptName = name.Replace(" ", "");

            text = text.Replace("#DIRECTORYNAME#", directoryName);
            text = text.Replace("#SCRIPTNAME#", scriptName);
            text = text.Replace("#NOTRIM#", "");

            TemplateScriptCustomizerCommon.ActionImpl(text, pathName);
        }
    }
}
