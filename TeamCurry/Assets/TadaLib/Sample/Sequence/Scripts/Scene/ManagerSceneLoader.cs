using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using TadaLib;
using AnnulusGames.SceneSystem;

namespace TadaLib.Sample.Sequence.Scene
{
    /// <summary>
    /// Awake前にManagerSceneを自動でロードするクラス
    /// </summary>
    public class ManagerSceneLoader : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void LoadManagerScene()
        {
            if (!UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Contains("Sequence"))
            {
                // サンプルシーン用のクラスのため、必要なときだけ処理する
                return;
            }

            Scenes.LoadScenes("TadaLibManager", "TadaLibGlobalManager");
            TadaLib.Scene.TransitionManager.SetNeedReloadScenes("TadaLibManager");
        }

        static bool TryRoadScene(string sceneName)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName).IsValid())
            {
                // 既に存在する
                return false;
            }

            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            return true;
        }
    }
}