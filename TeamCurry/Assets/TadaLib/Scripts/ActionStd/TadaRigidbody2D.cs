using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;

namespace TadaLib.ActionStd
{
    /// <summary>
    /// Rigidbody
    /// 壁、地面の埋め込みを防いだり、
    /// 坂道やすり抜け床対応ができている
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class TadaRigidbody2D
        : BaseProc
        , IProcUpdate
        , IProcPhysicsMove
    {
        #region プロパティ
        /// <summary>
        /// 最大何度の傾斜まで登れるか
        /// </summary>
        public float MaxClimbDegree { get; set; } = 80.0f;

        /// <summary>
        /// 接地している
        /// </summary>
        public bool IsGround { get; private set; } = true;
        /// <summary>
        /// 天井にぶつかっている
        /// </summary>
        public bool IsTopCollide { get; private set; } = false;
        /// <summary>
        /// X軸正の方向(ワールド座標)でぶつかっている
        /// </summary>
        public bool IsRightCollide { get; private set; } = false;
        /// <summary>
        /// X軸負の方向(ワールド座標)でぶつかっている
        /// </summary>
        public bool IsLeftCollide { get; private set; } = false;
        /// <summary>
        /// X軸正の方向(ワールド座標)で地形が近い
        /// </summary>
        public bool IsRightNearCollide { get; private set; } = false;
        /// <summary>
        /// X軸負の方向(ワールド座標)で地形が近い
        /// </summary>
        public bool IsLeftNearCollide { get; private set; } = false;

        /// <summary>
        /// 接地している地面情報
        /// </summary>
        public GroundInfo GroundInfo
        {
            get
            {
                Assert.IsTrue(IsGround);
                return _groundInfo;
            }
        }

        #endregion

        #region メソッド
        #endregion

        #region TadaLib.ProcSystem.IProcUpdate の実装
        /// <summary>
        /// 移動前の更新処理
        /// </summary>
        public void OnUpdate()
        {
            _beforeMovePos = transform.position;
        }
        #endregion

        #region TadaLib.ProcSystem.IProcPhysicsMove の実装
        /// <summary>
        /// 物理移動更新処理
        /// </summary>
        public void OnPhysicsMove()
        {
            // 全てのコンポーネント処理の最後に呼ばれる想定

            // 前処理で、自身のコリジョンがヒットされないようにレイヤーマスクを一時的に変更する
            var tmpLayer = gameObject.layer;
            gameObject.layer = Util.LayerUtil.LayerMaskToLayer(Sys.LayerMask.Player);

            // 物理エンジンと同期
            Physics2D.SyncTransforms();

            var fromPos = _beforeMovePos;
            var toPos = transform.position;

            // 今回の初期移動量
            var diff = (Vector2)(toPos - fromPos);
            // 移動床の移動差分を加える
            diff += _ridingMover?.MoveDiff ?? Vector2.zero;

            var hitBox = GetComponent<BoxCollider2D>();
            var scale = transform.localScale;
            var offset = hitBox.offset * scale;

            var isThroghMode = false;
            if (diff.y > 0.0f)
            {
                // 下から上はすり抜けモード
                isThroghMode = true;
            }
            else if (Input.InputUtil.GetAxis(gameObject, Input.AxisCode.Vertical) < 0.0f)
            {
                // 下入力をしていたらすり抜けモード
                isThroghMode = true;
            }

            // チェックに使うデータ
            var collideInfo = new CollideInfo()
            {
                Origin = (Vector2)fromPos + offset, // レイを飛ばす中心
                HitBoxHalfSize = hitBox.size * scale * 0.5f,
                IsThroughMode = isThroghMode,
            };

            // (X+&X-)/Y+/Y-/(X+&X-)の順にチェックする
            // 壁押し出しの差分
            var wallDiffX = 0.0f;
            {
                // 移動する壁対策で、先に左右チェックをする
                // ただし、登れない坂道(壁)のみで移動する
                var wallDiff = Vector2.zero;
                IsRightCollide = CheckRightCollide(ref wallDiff, collideInfo, isIgnoreSlope: true);
                IsLeftCollide = false;
                if (!IsRightCollide)
                {
                    IsLeftCollide = CheckLeftCollide(ref wallDiff, collideInfo, isIgnoreSlope: true);
                }
                // ここで生じたdiff差分は先に計算する
                if (IsRightCollide || IsLeftCollide)
                {
                    wallDiffX = wallDiff.x;
                    if (Mathf.Abs(wallDiffX) > kEpsilon)
                    {
                        var origin = collideInfo.Origin;
                        origin.x += wallDiffX;
                        collideInfo.Origin = origin;
                    }
                }

            }

            IsGround = CheckGroundCollide(ref diff, collideInfo);
            // 地面でヒットしていたら天井チェックは省く
            IsTopCollide = !IsGround && CheckTopCollide(ref diff, collideInfo);
            {
                var diffByRtCk = diff;
                var diffByLtCk = diff;
                IsRightCollide |= CheckRightCollide(ref diffByRtCk, collideInfo);
                IsLeftCollide |= CheckLeftCollide(ref diffByLtCk, collideInfo);

                if (IsLeftCollide && IsRightCollide)
                {
                    // 左右両方に接触していた場合は、移動元の方向に押し出す

                    if (diff.x >= 0.0f)
                    {
                        // 左 → 右 の移動なら、右壁ヒットを優先する
                        // 右壁ヒットを優先する 
                        diff = diffByRtCk;
                        if (Mathf.Abs(diff.x) > collideInfo.HitBoxHalfSize.x * 0.75f)
                        {
                            // 押し出し距離が足りない場合は極端に押し出す
                            // @todo: こんなことをしなくてよいように、押し出し距離の上限を伸ばす(rayを飛ばす始点を変更する)
                            diff.x *= 2.0f;
                        }
                        IsLeftCollide = false;
                    }
                    else
                    {
                        diff = diffByLtCk;
                        if (Mathf.Abs(diff.x) > collideInfo.HitBoxHalfSize.x * 0.75f)
                        {
                            // 押し出し距離が足りない場合は極端に押し出す
                            // @todo: こんなことをしなくてよいように、押し出し距離の上限を伸ばす(rayを飛ばす始点を変更する)
                            diff.x *= 2.0f;
                        }
                        IsRightCollide = false;
                    }
                }
                else if (IsLeftCollide)
                {
                    diff = diffByLtCk;
                }
                else // IsRightCollide
                {
                    diff = diffByRtCk;
                }
            }

            {
                // Nearチェック
                var diffDummy = diff + Vector2.right * _nearCollideDistance;
                IsRightNearCollide = CheckRightCollide(ref diffDummy, collideInfo, isUseGizmos: false);
                diffDummy = diff + Vector2.left * _nearCollideDistance;
                IsLeftNearCollide = CheckLeftCollide(ref diffDummy, collideInfo, isUseGizmos: false);
            }

            // レイヤーマスクの同期を解除
            gameObject.layer = tmpLayer;
            Physics2D.SyncTransforms();

            _ridingMover?.RegisterRidedFrame(gameObject);

            transform.position = _beforeMovePos + (Vector3)diff + Vector3.right * wallDiffX;

            //Debug.Log($"{IsLeftCollide}, {IsRightCollide}, {IsGround}, {IsTopCollide}");
        }
        #endregion

        #region privateメソッド
        // レイキャストを飛ばす(+ Debugの線を引く)
        RaycastHit2D LinecastWithGizmos(Vector2 from, Vector2 to, int layerMask)
        {
            var hit = Physics2D.Linecast(from, to, layerMask);
#if UNITY_EDITOR
            Debug.DrawLine(from, hit ? hit.point : to);
#endif
            return hit;
        }

        /// <summary>
        /// 地面方向の地形判定チェックをする
        /// </summary>
        /// <param name="diff"></param>
        /// <param name="info"></param>
        /// <returns>ヒットしたか</returns>
        bool CheckGroundCollide(ref Vector2 diff, in CollideInfo info)
        {
            // y軸負方向のチェック。3本のレイを飛ばす
            // 左端、中央、右端の順にチェック
            var mask = info.IsThroughMode ?
                (int)Sys.LayerMask.AllLandCollisionsExceptThrough :
                (int)Sys.LayerMask.AllLandCollisions;
            var rayLength = info.HitBoxHalfSize.y + 1.5f; // 下り坂を考慮して余裕を持たせる(下り坂の法線を取得するため)

            var offsetY = -info.HitBoxHalfSize.y * 0.25f; // 上方向への瞬間移動を弱めるために、チェック判定を下にずらす

            var origin = info.Origin;
            origin.y += offsetY;
            var originLeft = origin + new Vector2(-info.HitBoxHalfSize.x + kEpsilon, 0.0f);
            var originRight = origin + new Vector2(info.HitBoxHalfSize.x - kEpsilon, 0.0f);
            var hitDownLeft = LinecastWithGizmos(originLeft, originLeft + Vector2.up * (-rayLength + diff.y), mask);
            var hitDownCenter = LinecastWithGizmos(origin, origin + Vector2.up * (-rayLength + diff.y), mask);
            var hitDownRight = LinecastWithGizmos(originRight, originRight + Vector2.up * (-rayLength + diff.y), mask);

            // めり込んでいる分を上に持ち上げる
            var distanceToFoot = info.HitBoxHalfSize.y + offsetY;
            var newDiffY = diff.y;

            // 3つのレイのうち、ヒット座標との距離が最も短いのを採用する
            var mostTopHit = new RaycastHit2D();
            foreach (var hit in new List<RaycastHit2D>() { hitDownLeft, hitDownCenter, hitDownRight })
            {
                if (!hit)
                {
                    continue;
                }

                var moveDiffToGround = distanceToFoot - hit.distance;
                if (newDiffY > moveDiffToGround)
                {
                    continue;
                }

                if (!info.IsThroughMode)
                {
                    // すり抜けモードでないときも、すり抜ける床の場合は無視することがある
                    int layer = hit.collider.gameObject.layer;
                    // 対象のオブジェクトより下にいるときはカウントしない
                    // 移動しないすり抜ける床の時
                    if (((1 << layer) & (int)Sys.LayerMask.ThroughLandCollision) >= 1 && origin.y - distanceToFoot < hit.point.y - kEpsilon * 10.0f)
                    {
                        continue;
                    }

                    //if (layer == (int)Sys.LayerMask.ThroughLandCollisionMove) // 移動するすり抜ける床の時
                    //{
                    //    //float added_y = ray.collider.gameObject.GetComponent<Mover>().Diff.y;
                    //    //if (transform.position.y - length + added_y < ray.point.y - kEpsilon) return false;
                    //}
                }

                // 更新
                newDiffY = moveDiffToGround;
                mostTopHit = hit;
            }

            if (mostTopHit)
            {
                // 登れない勾配なら接地判定はなし
                {
                    var slopeRad = CalcSlopeRad(mostTopHit.normal);
                    if (!IsEnableClimbRad(slopeRad))
                    {
                        return false;
                    }
                }

                diff.y = newDiffY;
                // 移動床に乗っている場合は登録
                var landObj = mostTopHit.collider.gameObject;
                _ridingMover = landObj.GetComponent<MoveInfoCtrl>();

                // 坂道挙動
                // 進行方向の傾斜を見る
                var useHit = new RaycastHit2D();
                if (hitDownLeft && diff.x < kEpsilon)
                {
                    useHit = hitDownLeft;
                }
                else if (hitDownRight && diff.x > kEpsilon)
                {
                    useHit = hitDownRight;
                }
                else if (hitDownCenter)
                {
                    useHit = hitDownCenter;
                }

                // 見つからないときもある
                if (useHit)
                {
                    var theta = CalcSlopeRad(useHit.normal) - Mathf.PI * 0.5f;

                    // X軸の移動分を傾斜に応じて分配する
                    diff.y += diff.x * Mathf.Sin(theta);
                    diff.x *= Mathf.Cos(theta);

                    // 地面情報登録
                    _groundInfo = new GroundInfo()
                    {
                        Pos = mostTopHit.point,
                        Normal = useHit.normal,
                        Friction = 1.0f,
                    };

#if UNITY_EDITOR
                    Dbg.InstantPointDrawer.Add(useHit.point);
#endif
                }
                else
                {
                    // 地面情報の登録はする
                    _groundInfo = new GroundInfo()
                    {
                        Pos = mostTopHit.point,
                        Normal = mostTopHit.normal,
                        Friction = 1.0f,
                    };

#if UNITY_EDITOR
                    Dbg.InstantPointDrawer.Add(mostTopHit.point);
#endif
                }

                return true;
            }

            _ridingMover = null;
            return false;
        }

        /// <summary>
        /// 天井方向の地形判定チェックをする
        /// </summary>
        /// <param name="diff"></param>
        /// <param name="info"></param>
        /// <returns>ヒットしたか</returns>
        bool CheckTopCollide(ref Vector2 diff, in CollideInfo info)
        {
            // 左端、中央、右端の順にチェック
            var mask = (int)Sys.LayerMask.AllLandCollisionsExceptThrough;
            var rayLength = info.HitBoxHalfSize.y + kEpsilon;
            var originLeft = info.Origin + new Vector2(-info.HitBoxHalfSize.x + kEpsilon, 0.0f);
            var originRight = info.Origin + new Vector2(info.HitBoxHalfSize.x - kEpsilon, 0.0f);
            var hitUpLeft = LinecastWithGizmos(originLeft, originLeft + Vector2.up * (rayLength + diff.y), mask);
            var hitUpCenter = LinecastWithGizmos(info.Origin, info.Origin + Vector2.up * (rayLength + diff.y), mask);
            var hitUpRight = LinecastWithGizmos(originRight, originRight + Vector2.up * (rayLength + diff.y), mask);

            // めり込んでいる分を下げる
            var distanceToTop = info.HitBoxHalfSize.y;
            var newDiffY = diff.y;

            // 3つのレイのうち、ヒット座標との距離が最も短いのを採用する
            var mostButtomHit = new RaycastHit2D();
            foreach (var hit in new List<RaycastHit2D>() { hitUpLeft, hitUpCenter, hitUpRight })
            {
                if (!hit)
                {
                    continue;
                }

                var moveDiffToTop = distanceToTop - hit.distance;
                if (newDiffY < moveDiffToTop)
                {
                    continue;
                }

                // 更新
                newDiffY = moveDiffToTop;
                mostButtomHit = hit;
            }

            if (mostButtomHit)
            {
                // 左右スライド
                bool IsEnableSlide(ref Vector2 diff, in CollideInfo info, bool isLeftSlide)
                {
                    // 左右チェック、どこまでいけるか確かめる
                    var originUp = (isLeftSlide ? originLeft : originRight) + Vector2.up * (rayLength + diff.y);
                    var hit = LinecastWithGizmos(originUp, originUp + (isLeftSlide ? Vector2.right : Vector2.left) * (info.HitBoxHalfSize.x * 2.0f), mask);

                    if (!hit)
                    {
                        // これは呼ばれない想定
                        return false;
                    }

                    var addDiffX = info.HitBoxHalfSize.x + Mathf.Abs(originLeft.x - info.Origin.x) - hit.distance;
                    if (isLeftSlide)
                    {
                        addDiffX = -addDiffX;
                    }

                    // 特定距離以上ならダメ
                    if (Mathf.Abs(addDiffX) >= info.HitBoxHalfSize.x * 1.4f)
                    {
                        return false;
                    }

                    var newOrigin = info.Origin + Vector2.right * addDiffX;
                    // 上方向チェック(ボックス)、キャラクターが入れる隙間があるか
                    var hitBoxUp = Physics2D.BoxCast(
                        newOrigin,
                        new Vector2(info.HitBoxHalfSize.x * 0.8f, info.HitBoxHalfSize.y),
                        0.0f,
                        Vector2.up,
                        rayLength + diff.y,
                        (int)Sys.LayerMask.AllLandCollisionsExceptThrough
                        );

                    if (hitBoxUp)
                    {
                        // 隙間なし
                        return false;
                    }

                    // 左右壁チェック、キャラクターが入れる隙間があるか
                    var dummyDiff = new Vector2(addDiffX, diff.y);

                    if (isLeftSlide)
                    {
                        if (CheckLeftCollide(ref dummyDiff, info))
                        {
                            // 隙間なし
                            return false;
                        }
                    }
                    else
                    {
                        if (CheckRightCollide(ref dummyDiff, info))
                        {
                            // 隙間なし
                            return false;
                        }
                    }

                    // スライド
                    diff.x += addDiffX;
                    return true;
                }

                // キャラクタースライドのチェック
                // 天井にぶつかっていても少しだけ左右にずらせるならずらして天井当たりをなかったことにする
                if (!hitUpLeft)
                {
                    // 左側のチェックでぶつかっていないので、右にずらせるかチェック
                    if (IsEnableSlide(ref diff, info, isLeftSlide: true))
                    {
                        // スライドが成功したので天井衝突取り消し
                        return false;
                    }
                }

                if (!hitUpRight)
                {
                    // 右側のチェックでぶつかっていないので、左にずらせるかチェック
                    if (IsEnableSlide(ref diff, info, isLeftSlide: false))
                    {
                        // スライドが成功したので天井衝突取り消し
                        return false;
                    }
                }

                diff.y = newDiffY;
#if UNITY_EDITOR
                Dbg.InstantPointDrawer.Add(mostButtomHit.point);
#endif
                return true;
            }

            return false;
        }

        /// <summary>
        /// x軸正方向の地形判定チェックをする
        /// </summary>
        /// <param name="diff"></param>
        /// <param name="info"></param>
        /// <returns>ヒットしたか</returns>
        bool CheckRightCollide(ref Vector2 diff, in CollideInfo info, bool isIgnoreSlope = false, bool isUseGizmos = true)
        {
            var origin = info.Origin;
            // y軸方向は補正先を使う
            origin.y += diff.y;

            var boxSize = info.HitBoxHalfSize * 0.5f;
            var rayAddDistance = info.HitBoxHalfSize.x * 0.75f;

            // 軽い段差は登れるように、足元の判定は小さくする
            var ignoreSizeY = boxSize.y * 0.3f;
            origin.y += ignoreSizeY * 0.40f; // 頭の判定もやや小さくする

            var hit = Physics2D.BoxCast(
                origin,
                new Vector2(boxSize.x, boxSize.y - ignoreSizeY),
                0.0f, // angle // @todo: transform.rotationを見る
                Vector2.right,
                diff.x + rayAddDistance,
                (int)Sys.LayerMask.AllLandCollisionsExceptThrough
                );

#if UNITY_EDITOR
            if (isUseGizmos)
            {
                Debug.DrawLine(
                    origin,
                    hit ? hit.point : origin + new Vector2(info.HitBoxHalfSize.x + diff.x, 0f),
                    Color.blue
                    );
                _gizmosDataList.Add(new GizmosData()
                {
                    Pos = origin + Vector2.right * diff.x,
                    Size = new Vector2(info.HitBoxHalfSize.x, boxSize.y - ignoreSizeY),
                });
                if (hit)
                {
                    Dbg.InstantPointDrawer.Add(hit.point);
                }
            }
#endif
            if (!hit)
            {
                return false;
            }

            if (isIgnoreSlope)
            {
                var slopeRad = CalcSlopeRad(hit.normal);
                if (IsEnableClimbRad(slopeRad))
                {
                    return false;
                }
            }

            var newDiffX = hit.distance - rayAddDistance - kEpsilon;
            //if(newDiffX > diff.x + kEpsilon)
            //{
            //    // X+コリジョンチェックで、X+にいかないはず
            //    return false;
            //}
            diff.x = newDiffX;
            return true;
        }

        /// <summary>
        /// x軸負方向の地形判定チェックをする
        /// </summary>
        /// <param name="diff"></param>
        /// <param name="info"></param>
        /// <returns>ヒットしたか</returns>
        bool CheckLeftCollide(ref Vector2 diff, in CollideInfo info, bool isIgnoreSlope = false, bool isUseGizmos = true)
        {
            var origin = info.Origin;
            // y軸方向は補正先を使う
            origin.y += diff.y;

            var boxSize = info.HitBoxHalfSize * 0.5f;
            var rayAddDistance = info.HitBoxHalfSize.x * 0.75f;

            // 軽い段差は登れるように、足元の判定は小さくする
            var ignoreSizeY = boxSize.y * 0.3f;
            origin.y += ignoreSizeY * 0.40f; // 頭の判定もやや小さくする

            var hit = Physics2D.BoxCast(
                origin,
                new Vector2(boxSize.x, boxSize.y - ignoreSizeY),
                0.0f, // angle
                Vector2.left,
                -diff.x + rayAddDistance,
                (int)Sys.LayerMask.AllLandCollisionsExceptThrough
                );

#if UNITY_EDITOR
            if (isUseGizmos)
            {
                Debug.DrawLine(
                    origin,
                    hit ? hit.point : origin - new Vector2(info.HitBoxHalfSize.x - diff.x, 0f),
                    Color.blue
                    );
                _gizmosDataList.Add(new GizmosData()
                {
                    Pos = origin + Vector2.left * -diff.x,
                    Size = new Vector2(boxSize.x, boxSize.y - ignoreSizeY),
                });

                if (hit)
                {
                    Dbg.InstantPointDrawer.Add(hit.point);
                }
            }
#endif
            if (!hit)
            {
                return false;
            }

            if (isIgnoreSlope)
            {
                var slopeRad = CalcSlopeRad(hit.normal);
                if (IsEnableClimbRad(slopeRad))
                {
                    return false;
                }
            }

            var newDiffX = -hit.distance + rayAddDistance + kEpsilon;
            //if (newDiffX < diff.x - kEpsilon)
            //{
            //    // X-コリジョンチェックで、X-にいかないはず
            //    return false;
            //}
            diff.x = newDiffX;
            return true;
        }

        float CalcSlopeRad(in Vector2 normal)
        {
            return Mathf.Atan2(normal.y, normal.x);
        }

        bool IsEnableClimbRad(float slopeRad)
        {
            // degreeRad を [0, 2pi)にする
            if (slopeRad < 0.0f)
            {
                slopeRad += Mathf.PI * 2.0f;
            }

            var maxClimbRad = MaxClimbDegree * Mathf.Deg2Rad;

            var enableFrom = Mathf.PI * 0.5f - maxClimbRad;
            var enableTo = Mathf.PI - (Mathf.PI * 0.5f - maxClimbRad);

            if (slopeRad < enableFrom)
            {
                return false;
            }

            if (slopeRad > enableTo)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region privateフィールド
        struct CollideInfo
        {
            public Vector2 Origin;
            public Vector2 HitBoxHalfSize;
            public bool IsThroughMode;
        }

        Vector3 _beforeMovePos = Vector3.zero;
        MoveInfoCtrl _ridingMover = null; // 現在乗っている移動床
        GroundInfo _groundInfo = new GroundInfo();

        const float kEpsilon = (float)1e-3; // 誤差

        [SerializeField]
        float _nearCollideDistance = 0.1f;

#if UNITY_EDITOR
        struct GizmosData
        {
            public Vector2 Pos;
            public Vector2 Size;
        }
        List<GizmosData> _gizmosDataList = new();
#endif
        #endregion

        #region Monobehaviorの実装
        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            var hitBox = GetComponent<BoxCollider2D>();
            if (hitBox == null)
            {
                return;
            }

            var fromPos = _beforeMovePos;
            var scale = transform.localScale;
            var offset = hitBox.offset * scale;

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube((Vector2)fromPos + offset, hitBox.size);

            Gizmos.color = Color.blue;
            foreach (var data in _gizmosDataList)
            {
                Gizmos.DrawWireCube(data.Pos, data.Size);
            }
            _gizmosDataList.Clear();
#endif
        }
        #endregion
    }
}