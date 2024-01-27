using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.Input;

namespace TadaLib.Sample.Action2d.Actor.Chara
{
    /// <summary>
    /// 
    /// </summary>
    public class ColorManager : BaseManagerProc<ColorManager>
    {
        public enum ColorKind
        {
            PlayerBody,
            PlayerOutline,
            StageBody,
            StageOutline,
            TERM,
        }

        #region メソッド
        public Color GetColor(ColorKind kind)
        {
            return kind switch
            {
                ColorKind.PlayerBody => _playerBodyColor,
                ColorKind.PlayerOutline => _playerOutlineColor,
                ColorKind.StageBody => _stageBodyColor,
                ColorKind.StageOutline => _stageOutlineColor,
                _ => _playerBodyColor,
            };
        }
        #endregion

        #region private フィールド
        [SerializeField]
        Color _playerBodyColor = Color.white;
        [SerializeField]
        Color _playerOutlineColor = Color.green;
        [SerializeField]
        Color _stageBodyColor = Color.gray;
        [SerializeField]
        Color _stageOutlineColor = Color.black;
        #endregion
    }
}