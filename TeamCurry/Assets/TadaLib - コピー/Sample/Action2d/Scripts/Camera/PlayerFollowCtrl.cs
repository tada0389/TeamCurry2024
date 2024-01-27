using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.ActionStd;
using TadaLib.Input;

namespace TadaLib.Sample.Action2d.Camera
{
    /// <summary>
    /// カメラのメイン処理
    /// </summary>
    public class PlayerFollowCtrl : BaseProc, IProcPostMove
    {
        #region プロパティ
        public Vector3 EyeOffset => new Vector3(0.0f, 0.0f, -10.0f);
        #endregion

        #region メソッド
        #endregion

        #region Monobehavior の実装
        /// <summary>
        /// 生成時の処理
        /// </summary>
        public void Start()
        {
            // 初期化
            var player = PlayerManager.TryGetMainPlayer();
            if (player != null)
            {
                var playerData = player.GetComponent<Actor.Player.DataHolder>();
                var camera = GetCamera();
                camera.orthographicSize = _cameraData.OrthoSize;
                _frontView = _cameraData.FrontViewOffset * Mathf.Sign(playerData.FaceVec.x);
                _targetPos = player.transform.position;
                camera.transform.position = _targetPos + Vector3.right * _frontView + (Vector3)_cameraData.BaseOffset + EyeOffset;
            }
        }

        /// <summary>
        /// 移動後の更新処理
        /// </summary>
        public void OnPostMove()
        {
            // 座標確定後にカメラ処理をする
            // @todo: リファクタリング
            //        MoveCtrlを直接見たくないなぁ
            // @todo: 補間が気持ち悪いので改善

            var player = PlayerManager.TryGetMainPlayer();
            if (player != null)
            {
                // 色々補間
                var playerData = player.GetComponent<Actor.Player.DataHolder>();

                if (playerData.IsDead)
                {
                    // 死亡中は何もしない
                    return;
                }

                var camera = GetCamera();
                camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, _cameraData.OrthoSize, _cameraData.ErpRateOrthoSize);
                var frontView = _cameraData.FrontViewOffset * Mathf.Sign(playerData.FaceVec.x);
                _frontView = Mathf.Lerp(_frontView, frontView, _cameraData.ErpRateFrontView);
                var targetPos = player.transform.position;
                var erpRateGround = _erpRateGround;
                if (player.transform.position.y >= playerData.LastLandingPos.y)
                {
                    var diff = player.transform.position.y - playerData.LastLandingPos.y;
                    if (diff < 14.0f)
                    {
                        targetPos.y = playerData.LastLandingPos.y;
                    }
                    else
                    {
                        // 画面外に行ってしまうため、多少は追従する
                        // 補間率も早める
                        erpRateGround *= 2.0f;
                        targetPos.y = playerData.LastLandingPos.y + (diff - 14.0f);
                    }
                }
                else
                {
                    // 落下中なので地面方向を若干見る
                    if (player.TryGetComponent<MoveCtrl>(out var comp))
                    {
                        targetPos.y += Mathf.Min(0.0f, comp.Velocity.y * _playerVelYRefrectRate);
                    }
                }

                // y軸だけ補間
                targetPos.y = Mathf.Lerp(_targetPos.y, targetPos.y, erpRateGround);
                _targetPos = targetPos;
                camera.transform.position = _targetPos + (Vector3)_cameraData.BaseOffset + Vector3.right * _frontView + EyeOffset;
            }
        }
        #endregion

        #region privateメソッド
        UnityEngine.Camera GetCamera()
        {
            return GetComponent<UnityEngine.Camera>();
        }
        #endregion

        #region privateフィールド
        [SerializeField]
        Data _cameraData;
        [SerializeField]
        float _erpRateGround = 0.2f;
        [SerializeField]
        float _playerVelYRefrectRate = 0.2f;
        Vector3 _targetPos;
        float _frontView = 0.0f;
        #endregion
    }
}