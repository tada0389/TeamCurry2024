using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JoyconTest
{

    public class JoyconDemo : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            // Synchronize Joycon tilt

            Vector3 orientation = InputSystem.JoyconInput.Instance.GetJoyconVector();
            Vector3 angles = transform.localEulerAngles;
            angles.z = orientation.y + 180;

            foreach (Transform child in transform)
            {
                child.transform.localEulerAngles = angles;
            }
        }
    }
}