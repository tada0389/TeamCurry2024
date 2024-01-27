using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Extension;
using UniRx;

namespace TadaLib.ActionStd
{
    /// <summary>
    /// Physics2Dの更新タイミングを制御
    /// </summary>
    public class PhysicsUpdater
        : BaseManagerProc<PhysicsUpdater>
        , IProcManagerUpdate
    {
        #region プロパティ
        #endregion

        #region メソッド
        #endregion

        #region TadaLib.ProcSystem.IProcManager の実装
        /// <summary>
        /// 更新処理
        /// </summary>
        public void OnUpdate()
        {
            // TadaRigidbody2Dの更新直前に呼ばれる想定
            Assert.IsTrue(Physics2D.simulationMode == SimulationMode2D.Script);

            Physics2D.Simulate(Time.deltaTime);
        }
        #endregion

        #region privateメソッド
        #endregion

        #region privateフィールド
        #endregion
    }
}