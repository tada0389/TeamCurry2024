using Cysharp.Threading.Tasks;
using InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Title
{
    public class TitleStaging : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            StartStaging().Forget();
        }

        #region private field
        [SerializeField]
        UnityEngine.CanvasGroup _languageCanvas;
        [SerializeField]
        UnityEngine.CanvasGroup _titleMenuCanvas;
        [SerializeField]
        UnityEngine.UI.Image _titleImage;
        [SerializeField]
        float _fadeOutLanguageCanvasDurationSec = 0.25f;
        [SerializeField]
        float _curtainOpenDurationSec = 0.5f;
        [SerializeField]
        float _curtainCloseDurationSec = 0.5f;
        #endregion

        #region private method
        async UniTask StartStaging()
        {
            Ui.CurtainCtrl.Instance.SetCloseForce();

            // wait for language choose input
            //await UniTask.WaitUntil(() => JoyconInput.Instance.GetButtonDown(ButtonCode.Jump));
            await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            _languageCanvas.DOFade(0.0f, _fadeOutLanguageCanvasDurationSec);

            await UniTask.WaitForSeconds(_fadeOutLanguageCanvasDurationSec);

            // Open curtain
            Ui.CurtainCtrl.Instance.OpenStaging(_curtainOpenDurationSec).Forget();

            await UniTask.WaitForSeconds(_curtainOpenDurationSec * 0.2f);

            // appear title text
            _titleImage.rectTransform.DOScale(Vector3.one, _curtainOpenDurationSec * 0.8f).SetEase(Ease.InCirc);

            await UniTask.WaitForSeconds(_curtainOpenDurationSec * 0.8f);

            await UniTask.WaitForSeconds(0.3f);

            _titleMenuCanvas.DOFade(1.0f, 0.3f);

            // wait for language choose input
            await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            //await UniTask.WaitUntil(() => JoyconInput.Instance.GetButtonDown(ButtonCode.Jump));

            // Close curtain
            await Ui.CurtainCtrl.Instance.CloseStaging(_curtainCloseDurationSec);
        }
        #endregion
    }
}