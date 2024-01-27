using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Ui
{
    public class CurtainCtrl : MonoBehaviour
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
        #endregion

        #region private field
        [SerializeField]
        UnityEngine.UI.Image _leftCurtain;
        [SerializeField]
        UnityEngine.UI.Image _rightCurtain;
        #endregion
    }
}