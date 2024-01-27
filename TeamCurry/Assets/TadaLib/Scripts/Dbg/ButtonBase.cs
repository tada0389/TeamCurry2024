using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

#if UNITY_EDITOR
namespace TadaLib.Dbg
{

    [RequireComponent(typeof(RectTransform))]
    public class ButtonBase : ProcSystem.BaseProc
    {
        #region protected関数
        /// <summary>
        /// 生成時の処理
        /// </summary>
        protected void Init()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        protected void Proc()
        {
            //var mousePos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
            //_mousePos = new Vector3(mousePos.x, mousePos.y, 0.0f);
            //_mousePosOnCanvas = _mousePos * 1920 / Screen.width;
            //float scale = Screen.width / 1920f;
            //Vector2 pos = rectTransform.position;
            //Rect rect = rectTransform.rect;
            ////ピボットをずらすとずれる?
            //if (_mousePos.x > pos.x - rect.width / 2 * scale && _mousePos.x < pos.x + rect.width / 2 * scale && _mousePos.y > pos.y - rect.height * scale && mousePos.y < pos.y)
            //{
            //    if (isTouching)
            //    {
            //        onTouchStay?.Invoke();
            //    }
            //    else
            //    {
            //        onTouchEnter?.Invoke();
            //    }

                //if (UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
                //{
                //    onClick?.Invoke();
                //}

                //isTouching = true;
            //}
            //else
            //{
            //    if (isTouching)
            //    {
            //        onTouchExit?.Invoke();
            //    }

            //    isTouching = false;
            //}
        }
        #endregion

        #region protectedフィールド
        protected RectTransform rectTransform;
        protected Action onClick;
        protected Action onTouchEnter;
        protected Action onTouchStay;
        protected Action onTouchExit;
        protected bool isTouching;
        #endregion

        #region privateフィールド
        Vector3 _mousePos;
        Vector3 _mousePosOnCanvas;
        #endregion
    }
}
#endif