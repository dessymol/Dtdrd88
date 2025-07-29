using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class CardRotate : MonoBehaviour, IPointerClickHandler
{
    public GameObject frontImage;
    public GameObject backImage;

    public int cardId; // to compare match
    private bool isFlipping = false;
    private bool isMatched = false;
    public AudioSource Playsound;
    public AudioClip FlipSound;
    public AudioClip DeleteSound;

    private CardFlip gameManager;

    public Image frontImageComponent; 

    public void SetCardImage(Sprite sprite, int id)
    {
        frontImageComponent.sprite = sprite;
        cardId = id;
    }


    private void Start()
    {
        frontImage.SetActive(true);
        backImage.SetActive(false);

        gameManager = FindObjectOfType<CardFlip>();

    
        StartCoroutine(AutoFlipToBack(1f));
    }

    IEnumerator AutoFlipToBack(float delay)
    {
        yield return new WaitForSeconds(delay);
        FlipToBack();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isFlipping && !isMatched && backImage.activeSelf)
        {
            StartCoroutine(FlipToFront());
            Playsound.PlayOneShot(FlipSound);
        }
    }

    IEnumerator FlipToFront()
    {
        isFlipping = true;

        
        yield return RotateCard(0f, 90f);
        frontImage.SetActive(true);
        backImage.SetActive(false);
        yield return RotateCard(90f, 180f);

        isFlipping = false;

        gameManager.CardRevealed(this);
    }

    public void FlipToBack()
    {
        if (!isMatched && frontImage.activeSelf)
        {
            StartCoroutine(FlipBackRoutine());
        }
    }

    IEnumerator FlipBackRoutine()
    {
        isFlipping = true;

        yield return RotateCard(180f, 90f);
        frontImage.SetActive(false);
        backImage.SetActive(true);
        yield return RotateCard(90f, 0f);

        isFlipping = false;
    }

    public bool IsMatched()
    {
        return isMatched;
    }

    public void SetMatchedInstantly()
    {
        isMatched = true;
        gameObject.SetActive(false);
    }

    IEnumerator RotateCard(float fromY, float toY)
    {
        float time = 0f;
        float duration = 0.25f;

        while (time < duration)
        {
            float yRot = Mathf.Lerp(fromY, toY, time / duration);
            transform.rotation = Quaternion.Euler(0, yRot, 0);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, toY, 0);
    }
    public void PlayDeleteAnimation()
    {
        StartCoroutine(DeleteAnimation());
        Playsound.PlayOneShot(DeleteSound);
    }

    IEnumerator DeleteAnimation()
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;

        while (elapsed < duration)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = endScale;
        frontImage.SetActive(false);
        backImage.SetActive(false);
        GetComponent<Image>().color = new Color(0, 0, 0, 0); // transparent

    }
    /* public void PlaySound(AudioClip clip)
     {
         if (audioSource != null && clip != null)
         {
             audioSource.PlayOneShot(clip);
         }
     }*/

}
