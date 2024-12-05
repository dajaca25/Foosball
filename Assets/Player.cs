using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Enablegames;
using Enablegames.Suki;
using UnityEngine.Networking;

namespace Mirror
{
    public class Player : NetworkBehaviour
    {
        [SyncVar]
        public float horizontalInputBase;
        [SyncVar]
        public float verticalInputBase;
        [SyncVar]
        public float horizontalInput;
        [SyncVar]
        public float verticalInput;

        [SyncVar]
        public bool playerPaused;

        //New stuff:
        public string controlDirection = "";

        public bool levelFinish = false;

        public float inputResistanceX;
        public float inputResistanceY;
        public float maxRotationX = 10;
        public float maxRotationY = 10;

        private Enablegames.Suki.SukiInput suki = null; //maps avatar body data to game input

        private SkeletonData Skeleton;
        //private NetworkSkeleton netskeleton;
        //private RoboticData roboticData;





        private void Awake()
        {
            Application.targetFrameRate = 200;

            PlayerPrefs.SetString(egParameterStrings.LAUNCHER_ADDRESS, "localhost");
            print("egAwake");
            // initialize SUKI

            suki = SukiInput.Instance;
            //suki.Skeleton = Skeleton;
            //Debug.Log("suki.skel = " + suki.Skeleton);

            Tracker.Instance.BeginTracking();
            Tracker.Instance.Message("Game Begin, userid: " + SerialHandler.userid);
        }

        void Start()
        {
            suki = SukiInput.Instance;
            print("controlDirection: " + controlDirection);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            horizontalInput = 0;
            verticalInput = 0;
        }

        // need to use FixedUpdate for rigidbody
        void FixedUpdate()
        {
            horizontalInputBase = Input.GetAxis("Horizontal") * 10;
            verticalInputBase = Input.GetAxis("Vertical") * 10;

            if (suki.RangeExists("placement"))
            {
                horizontalInputBase = (horizontalInputBase + (((suki.GetRange("placement") * 2) - 1) * 10)) * 0.5f;
                verticalInputBase = (horizontalInputBase + (((suki.GetRange("placement") * 2) - 1) * 10)) * 0.5f;
                Debug.Log("suki value: " + horizontalInputBase);
            }

            print("range exists? " + suki.RangeExists("placement") + " horizInput: " + suki.GetRange("placement"));

            // only let the local player control the racket.
            // don't control other player's rackets
            if (isLocalPlayer)
            {
                //rigidbody2d.velocity = new Vector2(0, Input.GetAxisRaw("Vertical")) * speed * Time.fixedDeltaTime;
            }

            if (isLocalPlayer)
            {
                if (isServer)
                {
                    horizontalInput = horizontalInputBase / inputResistanceX * maxRotationX * Time.fixedDeltaTime;
                    verticalInput = verticalInputBase / inputResistanceY * maxRotationY * Time.fixedDeltaTime;
                }
                else
                {
                    CmdUpdateVariables(horizontalInputBase / inputResistanceX * maxRotationX * Time.fixedDeltaTime, verticalInputBase / inputResistanceY * maxRotationY * Time.fixedDeltaTime);
                }
            }
        }

        [Command]
        void CmdUpdateVariables(float newHorizontal, float newVertical)
        {
            verticalInput = newVertical;
            horizontalInput = newHorizontal;
        }
    }
}
