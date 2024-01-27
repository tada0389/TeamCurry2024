using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using System.Linq;
using System.Diagnostics;

namespace TadaLib.Editor
{
    public class GitWindow : EditorWindow
    {

        [MenuItem("Rainier/Git 操作")]
        static void Open()
        {
            GetWindow<GitWindow>("Git");
        }

        void OnGUI()
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("GitHub");
            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Open in Browser"))
            {
                OpenGitHub();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.EndVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("リポジトリ操作");

            GUILayout.BeginHorizontal();
            GUILayout.Label("Rainier", GUILayout.Width(70));
            _commitMessage = GUILayout.TextField(_commitMessage);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("TadaLib", GUILayout.Width(70));
            _commitMessageTadaLib = GUILayout.TextField(_commitMessageTadaLib);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Commit"))
            {
                GitCommit();
            }
            if (GUILayout.Button("Push"))
            {
                GitPush();
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.EndVertical();

            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Label("ユーザー設定");
            GUILayout.Label("リモートリポジトリURL", GUILayout.Width(100));
            _repositoryUrl = GUILayout.TextField(_repositoryUrl);
            GUILayout.EndVertical();
        }

        #region private メソッド
        void OpenGitHub()
        {
            Process.Start("https://github.com/tada0389/Rainier");
        }

        void GitCommit()
        {
            var commandList = new List<string>()
            {
                "submodule foreach git add -A",
                $"submodule foreach git commit -m \"{_commitMessageTadaLib}\"",
                "add -A",
                $"commit -m \"{_commitMessage}\"",
            };

            commandList.ForEach(GitCommand);
        }

        void GitPush()
        {
            var commandList = new List<string>()
            {
                //$"submodule foreach push {_repositoryUrl} master",
                //$"push {_repositoryUrl} main",
            };

            commandList.ForEach(GitCommand);
        }

        void GitCommand(string command)
        {
            var startInfo = new ProcessStartInfo("git");
            startInfo.Arguments = command;

            // ウィンドウを表示しない
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;

            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            // コマンド実行
            Process process = Process.Start(startInfo);

            // 標準出力・標準エラー出力・終了コードを取得する
            string standardOutput = process.StandardOutput.ReadToEnd();
            string standardError = process.StandardError.ReadToEnd();

            process.WaitForExit();

            process.Close();

            UnityEngine.Debug.Log(standardOutput);
            if (standardError != null && standardError != string.Empty)
            {
                UnityEngine.Debug.LogWarning(standardError);
            }
        }
        #endregion

        #region privateフィールド
        string _commitMessage = "";
        string _commitMessageTadaLib = "";
        string _repositoryUrl = "";
        #endregion
    }
}