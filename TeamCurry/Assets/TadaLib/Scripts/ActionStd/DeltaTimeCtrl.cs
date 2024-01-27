using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TadaLib.ActionStd
{
    /// <summary>
    /// DeltaTime管理
    /// </summary>
    public class DeltaTimeCtrl : MonoBehaviour
    {
        #region プロパティ
        public float TimeScale
        {
            get
            {
                return _timeScale;
            }
            set
            {
                _timeScale = value;

                // Unity標準コンポーネントのスケールも変更する
                if(TryGetComponent<Animator>(out var animator))
                {
                    animator.speed = _timeScale;
                }
            }
        }
        #endregion

        #region メソッド
        #endregion

        #region privateメソッド
        float _timeScale = 1.0f;
        #endregion

        #region privateフィールド
        #endregion
    }
}