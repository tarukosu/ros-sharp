using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RosSharp.RosBridgeClient
{
    [RequireComponent(typeof(RosSocket))]
    public class TFSubscriber : MonoBehaviour
    {
        private TFManager[] tfManagers;

        private RosSocket rosSocket;
        public int UpdateTime = 0;

        public float[] JointPositions; // deg
        public float[] JointVelocities; // deg/s

        //private int numberOfJoints;

        public void Start()
        {
            rosSocket = GetComponent<RosConnector>().RosSocket;
            rosSocket.Subscribe("/tf", "tf2_msgs/TFMessage", UpdateTF, UpdateTime);
            tfManagers = FindObjectsOfType<TFManager>();
        }

        private void UpdateTF(Message message)
        {
            TF2TFMessage tfMessage = (TF2TFMessage)message;

            /*
            foreach(var tf in tfMessage.transforms)
            {
                Debug.Log("tf: " + tf.child_frame_id + "->" + tf.header.frame_id + ", " + tf.transform.translation.x);
            }
            */

            foreach(var manager in tfManagers)
            {
                manager.UpdateTF(tfMessage);
            }
        }
    }

}
