using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFlip : MonoBehaviour
{
    [System.Serializable]
    public class Card
    {
        public GameObject frontImage;
        public GameObject backImage;
    }

    public Card[] cards;  // Array of 9 cards

    private void Start()
    {
        ShowAllFront();
        StartCoroutine(FlipAllBackAfterDelay(2f));
    }

    void ShowAllFront()
    {
        foreach (Card card in cards)
        {
            card.frontImage.SetActive(true);
            card.backImage.SetActive(false);
        }
    }

    IEnumerator FlipAllBackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (Card card in cards)
        {
            card.frontImage.SetActive(false);
            card.backImage.SetActive(true);
        }
    }
}
