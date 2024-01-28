using Cysharp.Threading.Tasks;
using InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System;

namespace Title
{
    public enum CurtainState
    {
        Open,
        Closed
    }

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

        [SerializeField]
        GameObject _titleMenuCanvasObject;

        [SerializeField]
        private float delaySecondsOnOpenClose = 1f;
        #endregion

        public bool MenuComplete = false;
        public CurtainState CurtainState = CurtainState.Closed;

        public void OpenCurtains()
        {            
            UniTask.Create(async () => 
            {
                await Ui.CurtainCtrl.Instance.OpenStaging(_curtainOpenDurationSec);
                await UniTask.WaitForSeconds(delaySecondsOnOpenClose);
                this.CurtainState = CurtainState.Open;
            });
        }

        public void CloseCurtains()
        {
            UniTask.Create(async () =>
            {
                await Ui.CurtainCtrl.Instance.CloseStaging(_curtainCloseDurationSec);
                await UniTask.WaitForSeconds(delaySecondsOnOpenClose);
                this.CurtainState = CurtainState.Closed;
            });
        }

        public void HideTitleScreen()
        {
            this._titleMenuCanvasObject.SetActive(false);
        }

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

            await UniTask.WaitForSeconds(delaySecondsOnOpenClose);

            CurtainState = CurtainState.Closed;
            MenuComplete = true;
        }
        #endregion
    }
}