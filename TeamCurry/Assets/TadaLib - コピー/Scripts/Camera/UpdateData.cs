using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using TadaLib.ActionStd;
using UniRx;
using System;

namespace TadaLib.Camera
{
    /// <summary>
    /// カメラ更新用のデータ
    /// </summary>
    public struct UpdateData
    {
        #region static メソッド
        #endregion

        #region フィールド
        public UnityEngine.Camera EditCamera;
        public GameObject PlayerObj;
        public float DeltaTime;
        #endregion
    }

    ///// <summary>
    ///// カメラ更新用のデータ
    ///// </summary>
    //public struct UpdateData
    //{
    //    #region static 定数
    //    public static readonly int PlayerPositionMaxCount = 4;
    //    #endregion

    //    #region static メソッド
    //    #endregion

    //    #region メソッド
    //    public Vector3 PlayerPositionMain => _playerPos0;
    //    public Vector3 PlayerPosition(int idx)
    //    {
    //        Assert.IsTrue(idx >= 0 && idx < PlayerPositionMaxCount);
    //        return idx switch
    //        {
    //            0 => _playerPos0,
    //            1 => _playerPos1,
    //            2 => _playerPos2,
    //            3 => _playerPos3,
    //            _ => _playerPos0
    //        };
    //    }
    //    #endregion

    //    #region フィールド

    //    #endregion

    //    #region private フィールド
    //    Vector3 _playerPos0;
    //    Vector3 _playerPos1;
    //    Vector3 _playerPos2;
    //    Vector3 _playerPos3;
    //    #endregion
    //}
}