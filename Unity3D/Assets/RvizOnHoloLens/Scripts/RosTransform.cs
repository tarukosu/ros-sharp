using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp
{
    public class RosTransform : MonoBehaviour
    {
        public string FrameId;

        private GameObject axes;

        public TFManager.TFDisplayState DisplayStatus
        {
            set
            {
                switch (value)
                {
                    case TFManager.TFDisplayState.Axis:
                        axes.SetActive(true);
                        break;
                    case TFManager.TFDisplayState.None:
                        axes.SetActive(false);
                        break;
                }
            }
        }

        private void Awake()
        {
            axes = transform.Find("coordinate").gameObject;
        }
    }
}