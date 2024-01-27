using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using System.Linq;
using System.IO;
using System;

namespace TadaLib.Editor
{
    /// <summary>
    /// ステートファイル追加を監視して、親フォルダ内のStateSet.csファイルを自動編集する
    /// </summary>
    public class StateSetAutoUpdater : AssetPostprocessor
    {
        static readonly string kStateSetFile = "StateSet.cs";

#if !UNITY_CLOUD_BUILD
        // ステートファイルの追加を監視して、StateSet の更新
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            // すぐにResources.LoadAllで取得出来ない場合もあるので間を開けて実行
            EditorApplication.delayCall += () =>
            {
                importedAssets.ToList().ForEach(asset =>
                {
                    if (asset.Contains("State") && asset.Contains(".cs"))
                    {
                        //TryUpdateStateSet(asset);
                    }
                });
            };
        }

#endif

        /// <summary>
        /// StateSet.cs ファイルを自動更新する
        /// @memo: StateSet.cs のフィールドの並び順は変更したくないので、追加分だけを更新する
        /// </summary>
        /// <param name="importedAsset"></param>
        /// <returns></returns>
        static bool TryUpdateStateSet(string importedAsset)
        {
            // 親フォルダに StateSet.cs があるか
            var importedFileInfo = new FileInfo(importedAsset);
            var parentDirectory = importedFileInfo.Directory.Parent.FullName;

            var targetFile = Path.Combine(parentDirectory, kStateSetFile);
            if (!File.Exists(targetFile))
            {
                return false;
            }

            var sr = new StreamReader(targetFile, System.Text.Encoding.UTF8);
            var text = sr.ReadToEnd();
            sr.Close();

            var textTmp = text;

            var targetStr1 = "// @auto added above1";
            var targetStr2 = "// @auto added above2";

            // StateHoge.cs => _stateHoge を取得
            var fileName = importedFileInfo.Name.Replace(".cs", "");
            var fieldName = $"_{char.ToLower(fileName[0])}{fileName.Substring(1)}";

            if (textTmp.Contains(targetStr1)){
                var newStr = $"stateMachine.AddState({fieldName});";
                newStr += Environment.NewLine;
                newStr += $"            {targetStr1}";
                text = text.Replace(targetStr1, newStr);
            }

            if (textTmp.Contains(targetStr2))
            {
                var newStr = $"[SerializeField]";
                newStr += Environment.NewLine;
                newStr += $"        State.{fileName} {fieldName};";
                newStr += Environment.NewLine;
                newStr += $"        {targetStr2}";
                text = text.Replace(targetStr2, newStr);
            }

            File.WriteAllText(targetFile, text);

            Debug.Log($"{targetFile} を自動更新しました");

            return true;
        }
    }
}