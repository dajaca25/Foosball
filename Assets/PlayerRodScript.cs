using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRodScript : MonoBehaviour
{
    [SerializeField] float speed;

    [SerializeField] string inputAxis;
    [SerializeField] string inputButton;

    public float adjustment;

    void FixedUpdate()
    {
        adjustment = (Input.GetAxis(inputAxis)) * speed;

        if(Input.GetButton(inputButton))
        {
            transform.Translate(0.001f * adjustment, 0, 0);
        }
        else
        {
            transform.Rotate(adjustment, 0, 0);
        }
    }
}
