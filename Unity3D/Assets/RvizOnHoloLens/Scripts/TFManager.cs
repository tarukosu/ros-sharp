using RosSharp.RosBridgeClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RosSharp
{
    public class TFManager : MonoBehaviour {
        public string BaseFrame;
        public RosTransform RosTransformPrefab;

        private bool doUpdate;
        private TF2TFMessage message;

        private Dictionary<string, RosTransform> rosTransforms = new Dictionary<string, RosTransform>();

        private TFDisplayState displayStatus = TFDisplayState.Axis;
        public enum TFDisplayState
        {
            None, Axis
        }

        public TFDisplayState DisplayStatus
        {
            set
            {
                foreach(var transform in rosTransforms)
                {
                    transform.Value.DisplayStatus = value;
                }
                displayStatus = value;
            }
            get
            {
                return displayStatus;
            }
        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            if (doUpdate)
            {
                foreach (var tf in message.transforms)
                {
                    //Debug.Log("tf: " + tf.child_frame_id + "->" + tf.header.frame_id + ", " + tf.transform.translation.x);

                    var frameId = tf.child_frame_id;
                    var parentId = tf.header.frame_id;

                    // 親のフレームを取得
                    Transform parent;
                    if (rosTransforms.ContainsKey(parentId))
                    {
                        parent = rosTransforms[parentId].transform;
                    }
                    else if(parentId == BaseFrame)
                    {
                        parent = transform;
                    }
                    else
                    {
                        //Debug.Log("error: parent frame not found");
                        break;
                    }


                    RosTransform rosTransform;
                    if (rosTransforms.ContainsKey(frameId))
                    {
                        rosTransform = rosTransforms[frameId];
                    }
                    else
                    {
                        // transform を作成
                        rosTransform = Instantiate(RosTransformPrefab, parent);
                        rosTransform.name = frameId;
                        rosTransform.DisplayStatus = displayStatus;
                        rosTransforms.Add(frameId, rosTransform);
                    }

                    // 位置を設定
                    var position = new Vector3(tf.transform.translation.x, tf.transform.translation.z, tf.transform.translation.y);
                    //var rotation = new Quaternion(tf.transform.rotation.x, tf.transform.rotation.z, -tf.transform.rotation.y, tf.transform.rotation.w);
                    //var rotation = Quaternion.AngleAxis(180, Vector3.forward) * new Quaternion(tf.transform.rotation.x, tf.transform.rotation.z, tf.transform.rotation.y, tf.transform.rotation.w);
                    //var rotation = Quaternion.AngleAxis(0, Vector3.forward) * new Quaternion(tf.transform.rotation.x, tf.transform.rotation.z, -tf.transform.rotation.y, tf.transform.rotation.w);
                    var rotation = Quaternion.AngleAxis(0, Vector3.forward) * new Quaternion(tf.transform.rotation.x, tf.transform.rotation.z, tf.transform.rotation.y, -tf.transform.rotation.w);
                    //var rotation = Quaternion.AngleAxis(90, Vector3.forward) * new Quaternion(-tf.transform.rotation.x, tf.transform.rotation.y, tf.transform.rotation.z, -tf.transform.rotation.w);
                    //var rotation = new Quaternion(-tf.transform.rotation.x, tf.transform.rotation.y, tf.transform.rotation.z, -tf.transform.rotation.w) * Quaternion.AngleAxis(90, Vector3.forward);
                    rosTransform.transform.localPosition = position;
                    rosTransform.transform.localRotation = rotation;

                }

                doUpdate = false;
            }
        }

        public void UpdateTF(TF2TFMessage tfMessage)
        {
            doUpdate = true;
            message = tfMessage;
        }
    }
}

