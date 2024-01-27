using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib;

namespace TadaLib.Util
{
    /// <summary>
    /// シングルトン化させるスクリプト
    /// </summary>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {

        private static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    System.Type t = typeof(T);

                    instance = (T)FindObjectOfType(t);
                    Assert.IsNotNull(instance, t + " をアタッチしている GameObject はありません");
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            // 他のゲームオブジェクトにアタッチされているか調べる
            // アタッチされている場合は破棄する。
            CheckInstance();
            //DontDestroyOnLoad(this); // GlobalManager シーンは破棄されることはないので DontDestroy はしない
        }

        void OnDestroy()
        {
            instance = null;    
        }

        protected bool CheckInstance()
        {
            // 新しく来たほうを優先する
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(instance);
                return true;
            }
            else if (Instance == this)
            {
                return true;
            }
            Destroy(this);
            return false;
        }
    }
}