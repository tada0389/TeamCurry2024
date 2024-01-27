
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using System.Linq;

namespace TadaLib.Editor
{
    public class SceneLaunchWindow : EditorWindow
    {

        [MenuItem("Rainier/Scene Launcher")]
        static void Open()
        {
            GetWindow<SceneLaunchWindow>("SceneLauncher");
        }

        void OnFocus()
        {
            this.ReloadScenes();
        }

        void OnGUI()
        {
            GUILayout.Label("※ボタンを押すと現在のシーンの変更が保存されます");

            if (this._sceneArray == null) { this.ReloadScenes(); }

            if (this._sceneArray.Length == 0)
            {
                EditorGUILayout.LabelField("シーンファイル(.unity)が存在しません");
                return;
            }

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying);
            this._scrollPos = EditorGUILayout.BeginScrollView(this._scrollPos);

            GUILayout.Label("登録済みのシーン");
            DisplayBuildSettingScenes();

            GUILayout.Label("全てのシーン");
            DisplayAllScenes();

            EditorGUILayout.EndScrollView();
            EditorGUI.EndDisabledGroup();
        }

        /// <summary>
        /// BuildSettings に登録されているシーンを一覧表示
        /// </summary>
        void DisplayBuildSettingScenes()
        {
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            foreach (var scene in scenes)
            {
                string[] strs = scene.path.Split('/');

                string sceneName = strs[strs.Length - 1].Replace(".unity", string.Empty);
                if (GUILayout.Button(sceneName))
                {
                    EditorApplication.SaveScene();//危険かも
                    //EditorSceneManager.SaveScene()
                    EditorSceneManager.OpenScene(scene.path);
                }
            }
        }

        /// <summary>
        /// アセット内のすべてのシーンを一覧表示
        /// </summary>
        void DisplayAllScenes()
        {
            foreach (var scene in _sceneArray)
            {
                string[] strs = scene.name.Split('/');

                string sceneName = strs[strs.Length - 1].Replace(".unity", string.Empty);
                if (GUILayout.Button(sceneName))
                {
                    EditorApplication.SaveScene();//危険かも
                    //EditorSceneManager.SaveScene()
                    var scenePath = AssetDatabase.GetAssetPath(scene);
                    EditorSceneManager.OpenScene(scenePath);
                }
            }
        }

        /// <summary>
        /// シーン一覧の再読み込み
        /// </summary>
        void ReloadScenes()
        {
            this._sceneArray = GetAllSceneAssets().ToArray();
        }

        /// <summary>
        /// プロジェクト内に存在するすべてのシーンファイルを取得する
        /// </summary>
        static IEnumerable<SceneAsset> GetAllSceneAssets()
        {
            return AssetDatabase.FindAssets("t:SceneAsset")
           .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
           .Select(path => AssetDatabase.LoadAssetAtPath(path, typeof(SceneAsset)))
           .Where(obj => obj != null)
           .Select(obj => (SceneAsset)obj);
        }

        #region privateフィールド
        SceneAsset[] _sceneArray;
        Vector2 _scrollPos = Vector2.zero;
        #endregion
    }
}