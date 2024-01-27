using UnityEngine;

namespace TadaLib.ProcSystem
{
    /// <summary>
    /// 更新用クラス
    /// このクラスは継承される想定
    /// </summary>
    public class BaseProc : MonoBehaviour
    {
        #region MonoBehavior の実装
        void Update()
        {
            ProcManager.Instance.AddUpdateInvoke(this);
        }
        #endregion
    }
}