using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CameraRotata : MonoBehaviour
{
    public float newRotation = 0;

    float realRotaiton = 0;
    float realPosition = 0;

    public TMP_Text blueScore;
    public TMP_Text redScore;

    public Transform cam;

    public bool keyboardTesting = false;

    public GameObject ballObject;

    private void Start()
    {
        cam = transform.GetChild(0);
    }

    void Update()
    {
        if (!ballObject.activeSelf)
            ballObject.SetActive(true);

        cam.localPosition = new Vector3(0.55f, 1, 0);

        realRotaiton = Mathf.Lerp(realRotaiton, -newRotation, Time.deltaTime);
        realPosition = Mathf.Lerp(realPosition, newRotation / 40, 5 * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(0, realRotaiton, 0);
        transform.position = new Vector3(0, 0, realPosition);
    }
}
