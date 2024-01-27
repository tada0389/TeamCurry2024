using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.ActionStd
{
    // ステートマシンのクラス
    public class StateMachine
        : BaseProc
        , IProcUpdate
    {
        #region 定義
        // 各ステートのベースとなる基底クラス
        public abstract class StateBase
        {
            // このステートが所属するステートマシン
            [System.NonSerialized]
            public StateMachine _prvStatemachine; // これもっと隠したいな

            [System.NonSerialized]
            public GameObject obj;

            public int AnimStateHashId
            {
                get
                {
                    if(_animStateHashId is null)
                    {
                        var animStateName = this.GetType().Name;
                        //var animStateName = this.GetType().Name.Replace("State", "");
                        _animStateHashId = Animator.StringToHash(animStateName);
                    }

                    return (int)_animStateHashId;
                }
            }
            int? _animStateHashId = null;
            public string Name => this.GetType().Name;

            // 前回のステート
            protected int PrevStateId { get { return _prvStatemachine.PrevStateId; } }

            // このステートが開始してからの時間
            protected float TimerSec { private set; get; }

            // ステートが開始したときに呼ばれるメソッド
            public virtual void OnStart()
            {

            }

            // ステートが終了したときに呼ばれるメソッド
            public virtual void OnEnd()
            {

            }

            // 毎フレーム呼ばれる関数
            public virtual void OnUpdate()
            {

            }

            // ステートが開始する前に初期化する (ステートを登録した時点で呼び出す)
            public virtual void OnInit()
            {

            }

            //// ステートが完全に終了したときに呼ばれるメソッド (StateMachineが破棄されたときに呼ばれる)
            //public virtual void OnFinish()
            //{

            //}

            // ステートを変更する 第一引数に変更後のステート
            protected void ChangeState(System.Type newStateType) =>
                _prvStatemachine._stateQueue.Enqueue(newStateType.GetHashCode());

            // ステートフラグのセット
            protected void SetStateFlag(StateInfoKind kind) =>
                _prvStatemachine.StateInfo.SetFlag(kind, true);


            // ステート開始のデバッグ出力をする これ蛇足・・・？
            protected void DumpStartMsg(StateBase state) =>
                UnityEngine.Debug.Log(state.GetType().Name + "開始");

            // ステート終了のデバッグ出力をする
            protected void DumpEndMsg(StateBase state) =>
                UnityEngine.Debug.Log(state.GetType().Name + "終了");

            //=== 隠したい ========================================-
            public void PrvTimerReset() => TimerSec = 0.0f;
            public void PrvTimerUpdate(float delta_time) => TimerSec += delta_time;
        }
        #endregion

        #region プロパティ
        // 前回のステート
        public int PrevStateId { private set; get; }
        // 現在のステート
        public int CurrentStateId { private set; get; }
        public string CurrentStateName => _factory[CurrentStateId].GetType().Name;
        public StateInfoCtrl StateInfo { private set; get; } = new StateInfoCtrl();
        #endregion

        #region メソッド
        // ステートを登録
        public void AddState(StateBase state)
        {
            var key = ConvertType2Key(state.GetType());
            _factory.Add(key, state);
            state._prvStatemachine = this; // ステートにマシンのデータを渡す ここ天才
            state.obj = gameObject;
            state.OnInit();
        }

        public void SetInitialState(System.Type stateType)
        {
            var key = ConvertType2Key(stateType);
            Assert.IsTrue(_factory.ContainsKey(key), "登録されていないステートです");
            Assert.IsFalse(_stateQueue.Count >= 1, "すでに初期のステートが設定されています");
            _stateQueue.Enqueue(key);
            _state = _factory[_stateQueue.Peek()]; // 新しいステートに変更
            StateInfo.OnStateChanged(); // ステートフラグ情報の更新
            _stateStartCallback?.Invoke(); // ステート開始時のコールバック
            _state.OnStart(); // 新しいステートの初期化
            ActorUtil.TryToPlayAnim(gameObject, _state.Name); // 開始アニメの自動呼び
            _state.PrvTimerReset(); // タイマーを再設定
            CurrentStateId = key;
        }

        // ステートを変更する(外部から) 強制的に選択したステートにするため，Queueは空にする
        public void ChangeState(System.Type stateType)
        {
            var key = ConvertType2Key(stateType);
            Assert.IsTrue(_factory.ContainsKey(key), "登録されていないステートです");
            while (_stateQueue.Count > 1)
            {
                ChangeStateImpl(_stateQueue.Dequeue(), _stateQueue.Peek());
            }
            _stateQueue.Enqueue(key);
            ChangeStateImpl(_stateQueue.Dequeue(), _stateQueue.Peek());
        }

        /// <summary>
        /// ステート開始時に呼ばれるコールバックを登録する
        /// </summary>
        /// <param name="callback"></param>
        public void AddStateStartCallback(System.Action callback)
        {
            _stateStartCallback += callback;
        }

        /// <summary>
        /// ステート開始時に呼ばれるコールバックを登録する
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="callback"></param>
        public static void AddStateStartCallback(GameObject obj, System.Action callback)
        {
            var stateMachine = obj.GetComponent<StateMachine>();
            Assert.IsTrue(stateMachine != null);
            stateMachine.AddStateStartCallback(callback);
        }

        public T GetStateInstance<T>() where T : StateBase
        {
            var key = ConvertType2Key(typeof(T));
            Assert.IsTrue(_factory.ContainsKey(key), "登録されていないステートです");
            return _factory[key] as T;
        }

        // 現在のステート名を取得する
        //public override string ToString() => _state.ToString(_state);
        #endregion

        #region TadaLib.ProcSystem.IProcUpdate の実装
        /// <summary>
        /// 移動前の更新処理
        /// </summary>
        public void OnUpdate()
        {
            Assert.IsTrue(_stateQueue.Count >= 1, "初期のステートが登録されていません");

            _state.OnUpdate(); // ステートの状態を更新
            _state.PrvTimerUpdate(gameObject.DeltaTime()); // ステート経過時間を更新

            CheckState(); // ステートの変更要求があるか確かめる
        }
        #endregion

        #region privateメソッド
        int ConvertType2Key(System.Type type)
        {
            return type.GetHashCode();
        }
        // ステートの変更要求があるか確かめる
        void CheckState()
        {
            while (_stateQueue.Count > 1)
            {
                Assert.IsTrue(_factory.ContainsKey(_stateQueue.Peek()), "登録されていないステートです");
                ChangeStateImpl(_stateQueue.Dequeue(), _stateQueue.Peek());
            }
        }

        /// <summary>
        /// ステート変更
        /// </summary>
        void ChangeStateImpl(int prevStateId, int nextStateId)
        {
            PrevStateId = prevStateId; // 現在のステートを保持
            _state.OnEnd(); // 現在のステートの終了処理
            _state = _factory[nextStateId]; // 新しいステートに変更
            StateInfo.OnStateChanged(); // ステートフラグ情報の更新
            _stateStartCallback?.Invoke(); // ステート開始時のコールバック
            _state.OnStart(); // 新しいステートの初期化
            ActorUtil.TryToPlayAnim(gameObject, _state.Name); // 開始アニメの自動呼び
            _state.PrvTimerReset(); // タイマーを再設定
            CurrentStateId = nextStateId;
        }
        #endregion

        #region privateフィールド
        // 現在のステート
        StateBase _state;

        // ステートIDとそれに対応するステートクラスの辞書
        readonly Dictionary<int, StateBase> _factory = new Dictionary<int, StateBase>();

        // ステートを変更したいときに，要求する列挙型を入れる
        readonly Queue<int> _stateQueue = new Queue<int>();

        System.Action _stateStartCallback;
    }
    #endregion
}