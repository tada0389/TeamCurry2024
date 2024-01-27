namespace TadaLib.ProcSystem
{
    /// <summary>
    /// 移動前の更新用インターフェース
    /// </summary>
    public interface IProcUpdate
    {
        /// <summary>
        /// 移動前の更新処理
        /// </summary>
        void OnUpdate();
    }
}