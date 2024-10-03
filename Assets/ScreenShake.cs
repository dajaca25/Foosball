using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public AnimationCurve curve;

    public void Shake(float intensity, float duration)
    {
        StartCoroutine(Shaking(intensity, duration));
    }

    IEnumerator Shaking(float intensity, float duration)
    {
        //Vector3 startPosition = transform.position;
        float elapsedTime = 0;

        while(elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration) * intensity;
            transform.position = transform.position + Random.insideUnitSphere * strength;
            yield return null;
        }
        //transform.position = startPosition;
    }
}
