using Cysharp.Threading.Tasks;
using InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System;
using Ui;
using KanKikuchi.AudioManager;

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

        [SerializeField]
        GameObject _titleMenuCanvasObject;

        [SerializeField]
        private float delaySecondsOnOpenClose = 1f;

        [SerializeField]
        StageBeginUiCtrl _stageBeginUiCtrl;
        [SerializeField]
        List<Sprite> _beginSprites;
        int _curSpriteIdx = -1;
        #endregion

        public bool MenuComplete = false;
        public CurtainState CurtainState = CurtainState.Closed;

        public void IncrementSpriteCtr()
        {
            _curSpriteIdx++;
            if (_curSpriteIdx >= _beginSprites.Count)
            {
                _curSpriteIdx--;
            }
        }

        public void OpenCurtains()
        {
            UniTask.Create(async () =>
            {
                await _stageBeginUiCtrl.AppearText(_beginSprites[_curSpriteIdx]);
                await Ui.CurtainCtrl.Instance.OpenStaging(_curtainOpenDurationSec);
                // await UniTask.WaitForSeconds(delaySecondsOnOpenClose);
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

            await UniTask.Yield();

            // wait for language choose input
            //await UniTask.WaitUntil(() => JoyconInput.Instance.GetButtonDown(ButtonCode.Jump));
            // await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            while (!InputSystem.JoyconInput.Instance.IsAnyButtonDown && !Input.GetKeyDown(KeyCode.Space))
            {
                // update
                var first = true;
                var isLeftPrev = true;
                var stickX = InputSystem.JoyconInput.Instance.GetAxis(AxisCode.Horizontal);
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

            SEManager.Instance.Play(SEPath.MENU_VALIDATION);

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
            await UniTask.WaitUntil(() => InputSystem.JoyconInput.Instance.IsAnyButtonDown || Input.GetKeyDown(KeyCode.Space));
            //await UniTask.WaitUntil(() => JoyconInput.Instance.GetButtonDown(ButtonCode.Jump));

            SEManager.Instance.Play(SEPath.GAME_START_SELECTED);

            // Close curtain
            await Ui.CurtainCtrl.Instance.CloseStaging(_curtainCloseDurationSec);

            await UniTask.WaitForSeconds(delaySecondsOnOpenClose);

            CurtainState = CurtainState.Closed;
            MenuComplete = true;

            BGMManager.Instance.Play(BGMPath.WAYIN_MUSIC, delay: 3.0f);
        }
        #endregion
    }
}