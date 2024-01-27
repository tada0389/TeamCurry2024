namespace TadaLib.ProcSystem
{
    /// <summary>
    /// マネージャー処理の更新タイミング
    /// </summary>
    public enum ManagerProcSection
    {
        BeforeUpdate,
        BeforeMove,
        BeforePhysicsMove,
        BeforePostMove,
        Last,
    }
}
