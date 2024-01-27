using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 入力情報を管理するクラス
/// 先行入力などに対応している
/// 先行入力は，ジャンプとダッシュのみ対応
/// 
/// 先行入力に関して
/// ・タイムスケールが通常とは異なった時はどうなるか
/// -> タイムスケールを考慮せず，常にタイムスケールが1のときと同じ挙動をする
/// 
/// </summary>

namespace TadaLib.Input
{

    public interface IInput
    {
        public bool ActionEnabled { get; set; }

        // 入力状態をリセットする
        public void ResetInput();

        /// <summary>
        /// 指定したボタンが入力されたかを取得する
        /// </summary>
        /// <param name="code">ボタン</param>
        /// <param name="precedeSec">先行入力時間</param>
        /// <returns></returns>
        public bool GetButtonDown(ButtonCode code, float precedeSec);
        /// <summary>
        /// 指定したボタンが入力されているかを取得する
        /// </summary>
        /// <param name="code">ボタン</param>
        /// <param name="precedeSec">先行入力時間</param>
        /// <returns></returns>
        public bool GetButton(ButtonCode code, float precedeSec);

        /// <summary>
        /// 指定したボタンの入力が離されたかを取得する
        /// </summary>
        /// <param name="code">ボタン</param>
        /// <param name="precedeSec">先行入力時間</param>
        /// <returns></returns>
        public bool GetButtonUp(ButtonCode code, float precedeSec);

        /// <summary>
        /// 過去の入力フラグを全て立てる
        /// </summary>
        /// <param name="code"></param>
        public void ForceFlagOnHistory(ButtonCode code);

        /// <summary>
        /// 過去の入力フラグを全て降ろす
        /// </summary>
        /// <param name="code"></param>
        public void ForceFlagOffHistory(ButtonCode code);

        /// <summary>
        /// 指定したボタンが入力されているかを取得する
        /// </summary>
        /// <param name="code">ボタン</param>
        /// <returns></returns>
        public float GetAxis(AxisCode code);
    }
}