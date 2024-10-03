using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enablegames.Suki;

public class PlayerRodScript : MonoBehaviour
{
    [SerializeField] float speed;

    [SerializeField] string inputAxis;
    [SerializeField] string inputButton;

    public float adjustment;

    ConfigurableJoint joint;

    Vector3 defaultPos;

    public bool keyboardTest = false;
    public bool isLocalPlayer = true;


    private void Start()
    {
        joint = GetComponent<ConfigurableJoint>();
        defaultPos = transform.position;

        keyboardTest = FindObjectOfType<CameraRotata>().keyboardTesting;

        //Set isLocalPlayer to whatever Mirror is telling you.
    }


    void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            if(keyboardTest)
            {
                adjustment = (Input.GetAxis(inputAxis)) * speed;
            
                if (Input.GetButton(inputButton))
                {
                    transform.Translate(0.001f * adjustment, 0, 0);
                }
                else
                {
                    transform.Rotate(adjustment, 0, 0);
                }
            }
            else
            {
                if (SukiInput.Instance.RangeExists("joystick"))
                {
                    float xPosition;
                    adjustment = SukiInput.Instance.GetRange("joystick");

                    if (Input.GetButton(inputButton))
                    {
                        transform.Translate(0.001f * adjustment, 0, 0);
                    }
                    else
                    {
                        transform.Rotate(adjustment, 0, 0);
                    }
                }

                if (SukiInput.Instance.RangeExists("placement"))
                {
                    print("placement: " + SukiInput.Instance.GetRange("placement"));

                    float xPosition;
                    adjustment = (SukiInput.Instance.GetRange("placement") * 2) - 1;

                    if (Input.GetButton(inputButton))
                    {
                        transform.position = Vector3.Lerp(transform.position, new Vector3(joint.linearLimit.limit * adjustment, defaultPos.y, defaultPos.z), Time.deltaTime * 20);
                    }
                    else
                    {
                        transform.localRotation = Quaternion.Euler(adjustment * 90, 0, 0);
                    }
                }
            }
        }
        else
        {
            //Get the input float from player 2's game.  Should also send back my position to player 2 as part of an array called player2Positions, and correct position every few frames.
        }
    }
}
