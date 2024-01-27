using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using TadaLib.ActionStd;
using TadaLib.Input;
using UniRx;

namespace TadaLib.ActionStd.Camera
{
    /// <summary>
    /// PlayerFllowCameraSetting処理
    /// </summary>
    [CreateAssetMenu(fileName = nameof(PlayerFllowCameraSetting), menuName = "ScriptableObjects/Camera/PlayerFollowCameraSetting")]
    public class PlayerFllowCameraSetting : ScriptableObject
    {
        #region フィールド
        [Header("Field of View")]
        public float Fov = 60.0f;

        [Header("Field of View の遷移補間率")]
        public float ErpRateFov = 0.02f;

        [Header("基本座標オフセット")]
        public Vector3 BaseOffset = new Vector3(0.0f, 5.5f, -20.0f);

        [Header("移動座標オフセットX")]
        public float PlayerMoveMaxOffsetX = 2.0f;

        [Header("移動座標オフセットXの遷移補間率(加速)")]
        public float ErpRateMoveOffsetXAccel = 1.0f;

        [Header("移動座標オフセットXの遷移補間率(減速)")]
        public float ErpRateMoveOffsetXDecel = 0.01f;

        [Header("着地時のカメラ移動補間率")]
        public float ErpRateGround = 0.01f;

        [Header("プレイヤーのY軸速度の反映補間率")]
        public float PlayerVelYReflectRate = 0.45f;
        #endregion
    }
}