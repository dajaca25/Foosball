using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball2 : MonoBehaviour
{
    Rigidbody rb;

    public float ballSpeed = 1;

    public ScreenShake ss;

    public CameraRotata camRotation;

    float redScoreVal, blueScoreVal;

    public GameObject blueScored;
    public GameObject redScored;

    
    void Start()
    {
        camRotation = FindObjectOfType<CameraRotata>();
        rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        camRotation.newRotation = Mathf.Clamp(transform.position.z * 70, -12, 12);


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


    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            float collisionForce = -collider.transform.parent.GetComponent<PlayerRodScript>().adjustment;

            if (collisionForce != 0)
            {
                rb.AddForce(collider.transform.forward * ballSpeed * collisionForce);
                rb.AddForce(collider.transform.right * Random.Range(0,10));
            }
            else
            {
                //rb.velocity = rb.velocity * -0.5f;
                rb.velocity = new Vector3 (-rb.velocity.x, 0, -rb.velocity.z);
            }
            ss.Shake();
        }

        if (collider.CompareTag("wall"))
        {
            if(transform.position.z > 0)
            {
                rb.velocity += new Vector3 (0, 0, -0.1f);
            }
            else
            {
                rb.velocity += new Vector3 (0, 0, 0.1f);
            }
        }
        
        if (collider.CompareTag("Goal"))
        {
            rb.velocity = new Vector3(0,0,0);
            ss.Shake();
            if(transform.position.z < 0)
            {
                Instantiate(redScored, transform.position + (transform.up * 0.1f), new Quaternion(0, 0, 0, 0));
                print("score for red!");
                redScoreVal += 1;
                camRotation.redScore.text = redScoreVal.ToString();
            }
            else
            {
                Instantiate(blueScored, transform.position + (transform.up * 0.1f), new Quaternion(0,0,0,0));
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
}
