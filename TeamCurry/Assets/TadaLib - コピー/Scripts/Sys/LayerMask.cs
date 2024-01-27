using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TadaLib.Sys
{
    /// <summary>
    /// Unityのレイヤーと値を一致させること
    /// </summary>
    public enum LayerMask
    {
        Player = 1 << 1,
        LandCollision = 1 << 9,
        ThroughLandCollision = 1 << 10,
        GimmickLandCollision = 1 << 11,

        TERM,

        AllLandCollisions = LandCollision | ThroughLandCollision | GimmickLandCollision,
        AllLandCollisionsExceptThrough = LandCollision | GimmickLandCollision,
        AllLandCollisionExceptGimmick = LandCollision | ThroughLandCollision,
    }
}
