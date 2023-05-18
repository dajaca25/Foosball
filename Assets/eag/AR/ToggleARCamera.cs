using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using Enablegames;
public class ToggleARCamera : MonoBehaviour
{

    public    Camera ARcamera;
    public RawImage ARimage;
    // Start is called before the first frame update
    void Start()
    {
    }

  
    public int ar_Mode = 1;

    public void ARCameraToggle()
    {
        ar_Mode++;
        if (ar_Mode>2)
            ar_Mode=0;

        ARcamera.enabled = ar_Mode>0;
        ARimage.enabled = ar_Mode>0;
        //if (ARimage!=null)
        if (ar_Mode>1){
            if (ARimage!=null){
               // var imageTransform = image.GetComponent<RectTransform>();
                //ARimage.GetComponent<RectTransform>().sizeDelta = new Vector2(0.3f, 0.3f);//, 0.3f, 0.3f);
                ARimage.GetComponent<RectTransform>().sizeDelta = new Vector2(0.3f*Screen.width, 0.3f*Screen.height);//, 0.3f, 0.3f);
            }else
                ARcamera.rect = new Rect(0.0f, 0.7f, 0.3f, 0.3f);
        }else{
            if (ARimage!=null)
                ARimage.GetComponent<RectTransform>().sizeDelta = new Vector2(1.0f*Screen.width, 1.0f*Screen.height);//, 0.3f, 0.3f);
//                ARimage.rectTransform.rect = new Rect(0.0f, 0.0f, 1f, 1f);
            else
                ARcamera.rect = new Rect(0.0f, 0.0f, 1f, 1f);
        }
    }
    void Update()
    {
//        if (Input.GetKeyDown(KeyCode.V)){
//                SwitchCamera();
//        }
        
    }
}
