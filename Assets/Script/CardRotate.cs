using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardRotate : MonoBehaviour
{
    public GameObject frontImage;
    public GameObject backImage;

    private bool isFlipped = false;
    private bool isFlipping = false;
    public float flipDuration = 0.5f;

    void Start()
    {
        frontImage.SetActive(true);
        backImage.SetActive(false);

        // Auto flip after delay (optional)
        StartCoroutine(FlipAfterDelay(1f));
    }

    public void TriggerFlip()
    {
        if (!isFlipping)
        {
            StartCoroutine(Flip());
            Debug.Log("hi");
        }
    }

    IEnumerator FlipAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(Flip());
    }

    IEnumerator Flip()
    {
        isFlipping = true;

        Quaternion startRot = transform.rotation;
        Quaternion halfRot = startRot * Quaternion.Euler(0f, 90f, 0f);
        Quaternion endRot = startRot * Quaternion.Euler(0f, 180f, 0f);

        float time = 0f;

        // First half: rotate to 90°
        while (time < flipDuration / 2)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(startRot, halfRot, time / (flipDuration / 2));
            yield return null;
        }

        // Swap image
        isFlipped = !isFlipped;
        frontImage.SetActive(!isFlipped);
        backImage.SetActive(isFlipped);

        time = 0f;

        // Second half: rotate to 180°
        while (time < flipDuration / 2)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(halfRot, endRot, time / (flipDuration / 2));
            yield return null;
        }

        transform.rotation = endRot;
        isFlipping = false;
    }

}
