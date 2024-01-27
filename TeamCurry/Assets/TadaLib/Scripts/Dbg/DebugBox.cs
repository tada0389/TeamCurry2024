using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

#if UNITY_EDITOR
namespace TadaLib.Dbg
{
    public class DebugBox : ButtonBase, ProcSystem.IProcPostMove
    {

        #region コンストラクタ
        public DebugBox(Func<string> m, Transform t)
        {
            Message = m;
            Parent = t;
        }
        #endregion

        #region プロパティ
        public Transform Parent { get; private set; }
        public Vector2 Size { get; private set; } = new Vector2(600, 400);
        public Func<string> Message { get; private set; }
        #endregion

        #region 関数
        public DebugBox SetBox(Func<string> m, Transform t)
        {
            Message = m;
            Parent = t;
            _text = GetComponentInChildren<TextMeshProUGUI>();
            return this;
        }

        public DebugBox SetSize(Vector2 s)
        {
            Size = s;
            return this;
        }

        public DebugBox SetSize(float width, float height)
        {
            return SetSize(new Vector2(width, height));
        }

        public DebugBox SetOffset(Vector2 o)
        {
            _offset = o;
            return this;
        }

        public DebugBox SetAlignment(TextAlignmentOptions o)
        {
            _text.alignment = o;
            return this;
        }

        public DebugBox SetDefaultOpen()
        {
            return this;
        }
        #endregion

        #region MonoBehavior の実装
        void Start()
        {
            base.Init();

            _boxes = new List<DebugBox>();
            _isTracing = true;
            _parentObj = Parent.gameObject;
            //offset = new Vector3(0, -100);
            rectTransform.sizeDelta = new Vector2(100, 100);
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _text.rectTransform.sizeDelta = Size;
            _image = GetComponent<Image>();
            _text.gameObject.SetActive(false);
            rectTransform.position = _offset;
            onTouchEnter = () =>
            {
                if (!_isAlwaysOpen)
                {
                    seq.Kill();
                    _openState = OpenState.Opening;
                    seq = DOTween.Sequence()
                    .Append(rectTransform.DOSizeDelta(Size, 0.2f))
                    //.Join(DOTween.To(() => offset, (t) => offset = t, new Vector3(0, -250), 0.2f))
                    .OnComplete(() =>
                    {
                        _text.gameObject.SetActive(true);
                        _openState = OpenState.Opened;
                    });
                }
            };

            onTouchExit = () =>
            {
                if (!_isAlwaysOpen)
                {
                    seq.Kill();
                    _text.gameObject.SetActive(false);
                    _openState = OpenState.Closing;
                    seq = DOTween.Sequence()
                    .Append(rectTransform.DOSizeDelta(new Vector2(100, 100), 0.2f))
                    //.Join(DOTween.To(() => offset, (t) => offset = t, new Vector3(0, -100), 0.2f))
                    .OnComplete(() =>
                    {
                        _openState = OpenState.Closed;
                    });
                }
            };

            onClick = () =>
            {
                if (_openState == OpenState.Opened)
                {
                    seq.Kill();
                    rectTransform.localScale = Vector3.one;
                    _isAlwaysOpen = !_isAlwaysOpen;
                    seq = DOTween.Sequence()
                    .Append(rectTransform.DOPunchScale(Vector3.one * 0.1f, 0.2f))
                    .Join(_image.DOColor(_isAlwaysOpen ? Color.black : Color.white, 0.2f));
                }
            };
        }
        #endregion

        #region TadaLib.ProcSystem.IProcPostMove の実装
        public void OnPostMove()
        {
            base.Proc();

            if (Parent == null)
            {
                Destroy(gameObject);
                return;
            }

            if (_isTracing)
            {
                Vector3 pos = UnityEngine.Camera.main.WorldToScreenPoint(Parent.position);
                transform.position = pos;
                rectTransform.transform.localPosition += _offset;
            }
            else
            {
                //実装がやばい
                float xsum = 0;
                float xmax = 0;
                float ysum = 0;
                for (int i = 0; i < id; i++)
                {
                    xmax = Mathf.Max(xmax, _boxes[i].rectTransform.sizeDelta.x);
                    ysum += _boxes[i].rectTransform.sizeDelta.y;
                    if (ysum + Size.y > 1080)
                    {
                        ysum = 0;
                        xsum += xmax;
                        xmax = 0;
                    }
                }
                rectTransform.transform.localPosition = new Vector2(960 - rectTransform.sizeDelta.x / 2 - xsum, 540 - ysum);
            }

            if (_openState == OpenState.Opened)
            {
                _text.text = Message();
            }

            //if (isTouching && UnityEngine.InputSystem.Mouse.current.rightButton.wasPressedThisFrame)
            //{
            //    if (_isTracing)
            //    {
            //        _isTracing = false;
            //        id = _boxes.Count;
            //        _boxes.Add(this);
            //    }
            //    else
            //    {
            //        _isTracing = true;
            //        _boxes.RemoveAt(id);
            //        for (int i = id; id < _boxes.Count; i++)
            //        {
            //            _boxes[i].id--;
            //        }
            //    }
            //}
        }
        #endregion

        #region 定義
        enum OpenState
        {
            Closed,
            Opening,
            Opened,
            Closing,

            TERM
        }
        #endregion

        #region privateフィールド
        GameObject _parentObj;
        TextMeshProUGUI _text;
        Image _image;
        Vector3 _offset;
        Vector3 _posPrev;
        Sequence seq = DOTween.Sequence();
        OpenState _openState = OpenState.Closed;
        bool _isAlwaysOpen = false;
        bool _isTracing;
        List<DebugBox> _boxes = new List<DebugBox>();
        public int id;
        #endregion
    }
}
#endif