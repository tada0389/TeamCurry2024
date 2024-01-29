using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.Ui
{
    public class LanguageSelect : MonoBehaviour
    {

        [SerializeField] private Sprite _jpImage;
        [SerializeField] private Sprite _enImage;

        // Start is called before the first frame update
        void Update()
        {
            // �y�����邽�߂� update �ł��

            var sprite = _jpImage;
            if (StageManager.IsEnglish)
            {
                sprite = _enImage;

            }
            if (TryGetComponent<UnityEngine.UI.Image>(out var image))
            {
                image.sprite = sprite;
            }

            if (TryGetComponent<UnityEngine.SpriteRenderer>(out var spRenderer))
            {
                spRenderer.sprite = sprite;
            }
        }
    }
}