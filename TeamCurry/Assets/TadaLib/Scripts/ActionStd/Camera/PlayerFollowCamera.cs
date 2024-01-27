using System.Collections;
using System.Collections.Generic;
using TadaLib.Camera;
using TadaLib.Extension;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using static UnityEngine.GraphicsBuffer;

namespace TadaLib.ActionStd.Camera
{
    /// <summary>
    /// 横スクロールアクション用のカメラ
    /// </summary>
    public class PlayerFollowCamera : ICamera
    {
        #region static メソッド
        /// <summary>
        /// メインカメラをこのカメラへ切り替える
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static PlayerFollowCamera SwitchCamera(PlayerFllowCameraSetting setting, bool isSceneStartCamera = false)
        {
            var camera = new PlayerFollowCamera();
            camera.SetCameraSetting(setting);
            Switcher.SwitchCamera(camera, isSceneStartCamera);

            return camera;
        }
        #endregion

        #region メソッド
        public void SetCameraSetting(PlayerFllowCameraSetting setting)
        {
            _setting = setting;
        }

#if UNITY_EDITOR
        public void DrawDebug(in UpdateData data)
        {
            var centerPos = data.EditCamera.transform.position;
            centerPos.z = 0.0f;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(centerPos + Vector3.up * 10.0f, centerPos + Vector3.down * 10.0f);
        }
#endif
        #endregion

        #region ICamera の実装
        public void OnSceneStartInitialize(in UpdateData data)
        {
            var player = data.PlayerObj;
            if (player != null)
            {
                var playerData = player.GetComponent<IDataHolder>();
                data.EditCamera.fieldOfView = _setting.Fov;
                _targetPos = player.transform.position;
                data.EditCamera.transform.position = _targetPos + _setting.BaseOffset;
            }
        }

        public void Update(in UpdateData data)
        {
            var player = data.PlayerObj;
            if (player != null)
            {
                var playerData = player.GetComponent<IDataHolder>();

                if (playerData.IsDead)
                {
                    // 死亡中はカメラを止める
                    return;
                }

                data.EditCamera.focalLength = Util.InterpUtil.Linier(data.EditCamera.fieldOfView, _setting.Fov, _setting.ErpRateFov, data.DeltaTime);
                var playerMoveOffsetRateX = -playerData.MoveVelocityRate.x;

                // 加速時、減速時でパラメータを変える
                var erpRate = playerMoveOffsetRateX switch
                {
                    float rate when rate * _playerMoveOffsetRateX < 0.0f => _setting.ErpRateMoveOffsetXDecel,
                    float rate when Mathf.Abs(rate) > Mathf.Abs(_playerMoveOffsetRateX) => _setting.ErpRateMoveOffsetXAccel,
                    _ => _setting.ErpRateMoveOffsetXDecel
                };

                _playerMoveOffsetRateX = Util.InterpUtil.Linier(_playerMoveOffsetRateX, playerMoveOffsetRateX, erpRate, data.DeltaTime);

                var targetPos = player.transform.position;

                // 地面座標を基準とする
                var erpRateGround = _setting.ErpRateGround;
                if (targetPos.y >= playerData.LastLandingPos.y)
                {
                    var diffY = targetPos.y - playerData.LastLandingPos.y;
                    var followDiffYThr = 14.0f;
                    if (diffY < followDiffYThr)
                    {
                        targetPos.y = playerData.LastLandingPos.y;
                    }
                    else
                    {
                        // 画面外に行ってしまうため、多少は追従する
                        // 補間率も一時的に早める
                        erpRateGround *= 2.0f;
                        targetPos.y = playerData.LastLandingPos.y + (diffY - followDiffYThr);
                    }
                }
                else
                {
                    // 落下中なので地面方向を若干見る
                    targetPos.y += Mathf.Min(0.0f, playerData.Velocity.y * _setting.PlayerVelYReflectRate);
                }

                // y軸だけ補完する
                targetPos.y = Util.InterpUtil.Linier(_targetPos.y, targetPos.y, erpRateGround, data.DeltaTime);
                _targetPos = targetPos;
                data.EditCamera.transform.position = _targetPos + _setting.BaseOffset + Vector3.right * (_playerMoveOffsetRateX * _setting.PlayerMoveMaxOffsetX);
            }
        }
        #endregion

        #region private フィールド
        Vector3 _targetPos;
        float _playerMoveOffsetRateX = 0.0f;

        PlayerFllowCameraSetting _setting = null;
        #endregion
    }
}