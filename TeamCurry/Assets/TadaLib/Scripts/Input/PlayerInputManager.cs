using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.Input
{
    /// <summary>
    /// PlayerInpuManager
    /// </summary>
    public class PlayerInputManager
        : TadaLib.Util.SingletonMonoBehaviour<PlayerInputManager>
    {
        #region プロパティ
        public int MaxPlayerCount { get; } = 2;
        #endregion

        #region メソッド
        public PlayerInputReader GetInput(int number = 0)
        {
            Assert.IsTrue(number < _playerInputs.Count);
            return _playerInputs[number];
        }
        #endregion

        #region privateメソッド
        #endregion

        #region privateフィールド
        [SerializeField]
        List<PlayerInputReader> _playerInputs;
        #endregion
    }
}