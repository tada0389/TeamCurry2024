using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.Dbg
{
    /// <summary>
    /// 一時的なポイントデバッグ描画機能
    /// </summary>
    public class InstantPointDrawer : MonoBehaviour
    {
        #region プロパティ
        #endregion

        #region メソッド
        public static void Add(in Vector3 pos)
        {
            Add(pos, Color.blue);
        }

        public static void Add(in Vector3 pos, in Color color)
        {
            _points.Enqueue(new PointData()
            {
                Position = pos,
                Color = color,
                Timer = new Util.TimerUnscaled(_lifeTime),
            });
        }
        #endregion

        #region Monobehaviorの実装
        void Start()
        {
            _lifeTime = _lifeTimeDummy;
        }

        void Update()
        {
            while (_points.Count >= 1)
            {
                var point = _points.Peek();
                if (point.Timer.IsTimout)
                {
                    _points.Dequeue();
                    continue;
                }
                break;
            }
        }

        void OnDrawGizmos()
        {
            foreach(var point in _points)
            {
                var color = point.Color;
                color.a = (1.0f - point.Timer.TimeRate01) * 0.7f;
                Gizmos.color = color;
                Gizmos.DrawSphere(point.Position, _radius);
            }    
        }
        #endregion

        #region privateフィールド
        struct PointData
        {
            public Vector3 Position;
            public Color Color;
            public Util.TimerUnscaled Timer;
        }

        static Queue<PointData> _points = new Queue<PointData>();
        static float _lifeTime = 1.0f;

        [SerializeField]
        float _radius = 1.0f;
        [SerializeField]
        float _lifeTimeDummy = 1.0f;
        #endregion
    }
}