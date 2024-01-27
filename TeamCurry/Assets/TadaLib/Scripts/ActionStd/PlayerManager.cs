using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;
using UnityEngine.SceneManagement;
using System;

namespace TadaLib.ActionStd
{
    /// <summary>
    /// PlayerManager
    /// </summary>
    public class PlayerManager
        : BaseManagerProc<PlayerManager>
    {
        #region プロパティ
        public int MaxPlayerCount => 2;
        public int MainPlayerNumber => 0;

        public IObservable<GameObject> GetMainPlayerAsync => _asyncMainPlayerSubject;
        AsyncSubject<GameObject> _asyncMainPlayerSubject = new AsyncSubject<GameObject>();
        #endregion

        #region メソッド
        public static GameObject TryGetMainPlayer()
        {
            if (Instance._players is null)
            {
                Instance.InitPlayerList();
            }
            return Instance._players[Instance.MainPlayerNumber];
        }

        public static GameObject TryGetPlayer(int number)
        {
            if (Instance._players is null)
            {
                Instance.InitPlayerList();
            }
            Assert.IsTrue(number < Instance._players.Count);
            return Instance._players[number];
        }

        /// <summary>
        /// プレイヤーの登録
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="number"></param>
        public static void RegisterPlayer(GameObject obj, int number)
        {
            if (Instance._players is null)
            {
                Instance.InitPlayerList();
            }
            Assert.IsTrue(number < Instance._players.Count);
            Instance._players[number] = obj;

            if (number == Instance.MainPlayerNumber)
            {
                Instance._asyncMainPlayerSubject.OnNext(obj);
                Instance._asyncMainPlayerSubject.OnCompleted();
            }
        }
        #endregion

        #region privateメソッド
        void InitPlayerList()
        {
            _players = new List<GameObject>();
            for (int idx = 0; idx < MaxPlayerCount; ++idx)
            {
                _players.Add(null);
            }
        }
        #endregion

        #region privateフィールド
        List<GameObject> _players = null;
        #endregion
    }
}