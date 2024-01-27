using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaLib.ProcSystem;
using TadaLib.ActionStd;
using TadaLib.Input;

namespace TadaLib.Sample.Action2d.Camera
{
    [System.Serializable]
    struct Data
    {
        public static Data CreateDefault()
        {
            return new Data()
            {
                OrthoSize = 16,
                BaseOffset = new Vector2(0.0f, 8.0f),
                FrontViewOffset = 8.0f,
                ErpRateOrthoSize = 0.2f,
                ErpRateFrontView = 0.2f,
            };
        }

        public int OrthoSize;
        public Vector2 BaseOffset;
        public float FrontViewOffset;
        public float ErpRateOrthoSize;
        public float ErpRateFrontView;
    }
}
