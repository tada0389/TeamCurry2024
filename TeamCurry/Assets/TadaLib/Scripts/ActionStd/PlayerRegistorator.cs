using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using TadaLib.ActionStd;
using UniRx;

namespace TadaLib.ActionStd
{
    /// <summary>
    /// PlayerRegistorator
    /// </summary>
    public class PlayerRegistorator
        : MonoBehaviour
    {
        #region プロパティ
        #endregion

        #region メソッド
        #endregion

        #region MonoBehavior の実装
        void Start()
        {
#if UNITY_EDITOR
            if (PlayerManager.Instance is null)
            {
                Assert.IsTrue(false, "PlayerManagerのGameObjectがシーン内にありません。");
                return;
            }
#endif
            PlayerManager.RegisterPlayer(gameObject, _number);
        }
        #endregion

        #region privateフィールド
        [SerializeField]
        [Header("プレイヤーID")]
        int _number = 0;
        #endregion

        #region privateメソッド
        #endregion
    }
}