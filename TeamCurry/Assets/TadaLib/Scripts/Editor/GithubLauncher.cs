using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using TadaLib;
using TadaLib.ProcSystem;
using TadaLib.Input;

namespace TadaLib.Editor
{
    /// <summary>
    /// Githubをブラウザで起動
    /// </summary>
    public class GithubLauncher
    {
        [MenuItem("Rainier/Githubをブラウザで起動")]
        static void OpenGithubBrowser()
        {
            System.Diagnostics.Process.Start("https://github.com/tada0389/Rainier");
        }
    }
}