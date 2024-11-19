using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror
{
    public class Ball2 : NetworkBehaviour
    {
        Rigidbody rb;

        public float ballSpeed = 1;

        public ScreenShake ss;
        public CameraRotata camRotation;

        float redScoreVal, blueScoreVal;

        public GameObject blueScored;
        public GameObject redScored;
        public GameObject waitingScreen;


        [SyncVar] bool isPanelOff;

        //[SyncVar]
        //public Vector3 ballPosition;



        void Start()
        {
            camRotation = FindObjectOfType<CameraRotata>();
            rb = GetComponent<Rigidbody>();
        }



        private void Update()
        {
            camRotation.newRotation = Mathf.Clamp(transform.position.z * 70, -12, 12);

            if (isServer)
            {
                //ballPosition = transform.position;

                if (rb.velocity.x < 0.1f)
                {
                    if (transform.position.x > 0)
                    {
                        rb.velocity += new Vector3(-0.001f, 0, 0);
                    }
                    else
                    {
                        rb.velocity += new Vector3(0.001f, 0, 0);
                    }
                }
            }
            else if(Foosball_NetworkManager.Instance.isHost == false)
            {
                if (!isPanelOff && waitingScreen.activeSelf)
                {
                    StartCoroutine(TogglePanel());
                }
                //Here, get the position from Player 1.
                //transform.position = Vector3.Lerp(transform.position, ballPosition, Time.deltaTime * 20);
            }
        }


        void OnTriggerEnter(Collider collider)
        {
            if (collider.CompareTag("Player"))
            {
                float collisionForce = -collider.transform.parent.GetComponent<PlayerRodScript>().adjustment;

                if (collisionForce != 0)
                {
                    rb.AddForce(collider.transform.forward * ballSpeed * collisionForce);
                    rb.AddForce(collider.transform.right * Random.Range(0, 10));
                }
                else
                {
                    //rb.velocity = rb.velocity * -0.5f;
                    rb.velocity = new Vector3(-rb.velocity.x, 0, -rb.velocity.z);
                }
                ss.Shake(0.01f, 0.1f);
            }

            if (collider.CompareTag("wall"))
            {
                if (transform.position.z > 0)
                {
                    rb.velocity += new Vector3(0, 0, -0.1f);
                }
                else
                {
                    rb.velocity += new Vector3(0, 0, 0.1f);
                }
            }

            if (collider.CompareTag("Goal"))
            {
                rb.velocity = new Vector3(0, 0, 0);
                ss.Shake(0.1f, 0.5f);
                if (transform.position.z < 0)
                {
                    Instantiate(redScored, transform.position + (transform.up * 0.1f), Quaternion.Euler(0, 0, 0));
                    print("score for red!");
                    redScoreVal += 1;
                    camRotation.redScore.text = redScoreVal.ToString();
                }
                else
                {
                    Instantiate(blueScored, transform.position + (transform.up * 0.1f), Quaternion.Euler(0, 180, 0));
                    print("score for blue!");
                    blueScoreVal += 1;
                    camRotation.blueScore.text = blueScoreVal.ToString();
                }
                transform.position = new Vector3(0, 0.069f, 0);
            }

            /*
            if (collider.CompareTag("wall"))
            {
                rb.velocity = new Vector3(-rb.velocity.x, 0, -rb.velocity.z);

                rb.AddForce(collider.transform.forward * ballSpeed);
                rb.AddForce(collider.transform.right * Random.Range(0,10));
            }
            */
        }



        IEnumerator TogglePanel()
        {
            //if (!isServer) yield break;

            yield return new WaitForSeconds(2);
            waitingScreen.SetActive(false);
            yield return new WaitForSeconds(2);
            isPanelOff = false;
            StopCoroutine(TogglePanel());
        }
    }
}