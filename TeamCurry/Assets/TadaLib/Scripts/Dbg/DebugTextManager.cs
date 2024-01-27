using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System;

namespace TadaLib.Dbg
{
    public class DebugTextManager : MonoBehaviour
    {
        #region MoveBehaviorの実装

        void Start()
        {
            _debugElements = new List<DebugElement>();
            _debugText.text = string.Empty;
            //DontDestroyOnLoad(_debugCanvas);

#if UNITY_EDITOR
            _debugCanvas.SetActive(false);
#else
        _debugCanvas.SetActive(false);
#endif
        }

        // Update is called once per frame
        void Update()
        {
            //// Qが押されたら。ほかに良い書き方ありそう
            //if (UnityEngine.InputSystem.Keyboard.current.allKeys.
            //    Where(x => x.keyCode == UnityEngine.InputSystem.Key.Q).
            //    Where(x => x.wasPressedThisFrame).Count() == 1)
            //{
            //    _debugCanvas.SetActive(!_debugCanvas.activeSelf);

            //}

            _debugText.text = string.Empty;
            for (int i = _debugElements.Count - 1; i >= 0; i--)
            {
                //Triggerで削除
                if (_debugElements[i].removeTrigger())
                {
                    _debugElements.RemoveAt(i);
                }
                else
                {
                    _debugText.text += _debugElements[i].message();
                }
            }

        }
        #endregion

        public static DebugElement Display(Func<string> message, int priority = 0)
        {
            DebugElement elem = new DebugElement(message, priority);
            _debugElements.Add(elem);
            _debugElements.Sort((a, b) => b.priority - a.priority);
            return elem;
        }

        public static DebugElement Display(object obj, int priority = 0)
        {
            return Display(obj.ToString, priority);
        }

        public class DebugElement
        {
            public int priority { get; private set; }
            int indent;
            public Func<string> message { get; private set; }
            public List<DebugElement> elements = new List<DebugElement>();
            public bool isOpen { get; set; }
            public Func<bool> removeTrigger { get; private set; }

            public DebugElement(Func<string> s, int p = 0)
            {
                message = s;
                priority = p;
                removeTrigger = () => false;
            }

            public DebugElement AddRemoveTrigger(MonoBehaviour mono)
            {
                removeTrigger = () => mono != null ? false : true;
                return this;
            }
        }

        public void ResolutionChange(int resolution)
        {
            Screen.SetResolution(resolution / 10000, resolution % 10000, Screen.fullScreen);
        }

        public void FullScreenChange()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }

        #region privateフィールド
        [SerializeField]
        TextMeshProUGUI _debugText;
        [SerializeField]
        GameObject _debugCanvas;
        static List<DebugElement> _debugElements = new List<DebugElement>();
        #endregion
    }
}