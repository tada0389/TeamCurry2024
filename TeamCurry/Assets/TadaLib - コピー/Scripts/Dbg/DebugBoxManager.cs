using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

#if UNITY_EDITOR
namespace TadaLib.Dbg
{
    public class DebugBoxManager : MonoBehaviour
    {
        #region static関数
        public static DebugBox Display(MonoBehaviour mono)
        {
            _mainCameraStatic = UnityEngine.Camera.main;
            Vector3 pos = _mainCameraStatic.WorldToScreenPoint(mono.transform.position);
            var boxObj = Instantiate(_textBoxPrefabStatic, pos, Quaternion.identity, _debugCanvasStatic);
            return boxObj.SetBox(mono.ToString, mono.transform);
        }
        #endregion

        #region MonoBehaviorの実装
        void Awake()
        {
            _textBoxPrefabStatic = _textBoxPrefab;
            _debugCanvasStatic = _debugCanvas;
            _mainCameraStatic = UnityEngine.Camera.main;
        }
        #endregion

        #region private staticフィールド
        static DebugBox _textBoxPrefabStatic;
        static Transform _debugCanvasStatic;
        static UnityEngine.Camera _mainCameraStatic;
        #endregion

        #region privateフィールド
        [SerializeField]
        DebugBox _textBoxPrefab;
        [SerializeField]
        Transform _debugCanvas;
        #endregion
    }
}
#endif