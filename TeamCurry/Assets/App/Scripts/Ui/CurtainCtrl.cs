using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Ui
{
    public class CurtainCtrl : TadaLib.Util.SingletonMonoBehaviour<CurtainCtrl>
    {
        #region method
        public async UniTask OpenStaging(float durationSec)
        {
            _leftCurtain.rectTransform.DOLocalMoveX(-1330.0f, durationSec);
            _rightCurtain.rectTransform.DOLocalMoveX(1330.0f, durationSec);

            await UniTask.Delay(System.TimeSpan.FromSeconds(durationSec));
        }

        public async UniTask CloseStaging(float durationSec)
        {
            _leftCurtain.rectTransform.DOLocalMoveX(-480.0f, durationSec);
            _rightCurtain.rectTransform.DOLocalMoveX(480.0f, durationSec);

            await UniTask.Delay(System.TimeSpan.FromSeconds(durationSec));
        }

        public void SetCloseForce()
        {
            {
                var pos = _leftCurtain.rectTransform.localPosition;
                pos.x = -480.0f;
                _leftCurtain.rectTransform.localPosition = pos;
            }
            {
                var pos = _rightCurtain.rectTransform.localPosition;
                pos.x = 480.0f;
                _rightCurtain.rectTransform.localPosition = pos;
            }
        }
        #endregion

        #region private field
        [SerializeField]
        UnityEngine.UI.Image _leftCurtain;
        [SerializeField]
        UnityEngine.UI.Image _rightCurtain;
        #endregion
    }
}