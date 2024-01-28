using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using TadaLib;
using AnnulusGames.SceneSystem;

namespace App
{
    /// <summary>
    /// Awake�O��ManagerScene�������Ń��[�h����N���X
    /// </summary>
    public class ManagerSceneLoader : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void LoadManagerScene()
        {
            Scenes.LoadScenes("GlobalManager");
        }
    }
}