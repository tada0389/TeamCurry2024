using System.Collections.Generic;
using UnityEngine;

namespace TadaLib.ProcSystem
{
    /// <summary>
    /// マネージャー更新用クラス
    /// このクラスは継承される想定
    /// </summary>
    public class BaseManagerProc<T> : Util.SingletonMonoBehaviour<T> where T : MonoBehaviour
    {
        [SerializeField]
        ManagerProcSection _section;

        #region MonoBehavior の実装
        void Update()
        {
            {
                if (this as IProcManagerUpdate is { } proc)
                {
                    ProcManager.Instance.AddManagerUpdateInvoke(proc, _section);
                }
            }
        }
        #endregion
    }
}