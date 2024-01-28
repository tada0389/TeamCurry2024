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
        private UnityEngine.UI.Image _englishLanguageImage;
        [SerializeField]
        private UnityEngine.UI.Image _japaneseLanguageImage;
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
            // await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            while (!Input.GetKeyDown(KeyCode.Space))
            {
                // update
                var first = true;
                var isLeftPrev = true;
                // var stickX = InputSystem.JoyconInput.Instance.GetAxis(AxisCode.Horizontal);
                var stickX = Input.GetAxis("Horizontal");
                Debug.Log(stickX);
                switch (stickX)
                {
                    case < 0:
                        // _japaneseLanguageImage.DOKill(true);
                        if (first || !isLeftPrev)
                        {
                            first = false;
                            isLeftPrev = true;
                            _japaneseLanguageImage.rectTransform.DOScale(1.0f, 0.1f);
                            _englishLanguageImage.rectTransform.DOScale(2.0f, 0.1f);
                        }
                        
                        break;
                    case > 0:
                        // _englishLanguageImage.DOKill(true);
                        if (first || isLeftPrev)
                        {
                            first = false;
                            isLeftPrev = false;
                            _englishLanguageImage.rectTransform.DOScale(1.0f, 0.1f);
                            _japaneseLanguageImage.rectTransform.DOScale(2.0f, 0.1f);
                        }
                        
                        break;
                }

                await UniTask.Yield();
            }
            
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