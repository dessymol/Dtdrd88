using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFlip : MonoBehaviour
{
    private CardRotate firstCard = null;
    private CardRotate secondCard = null;
    private bool isBusy = false;

    public AudioSource backgroundMusic;
    public AudioSource sfxAudioSource;
    public AudioClip winSound;
    public GameObject winPanel;

    private int totalCards;
    private int matchedCards = 0;


    public int cardId; // Set this when instantiating cards

    void Start()
    {
        totalCards = GameObject.FindGameObjectsWithTag("Card").Length;
        winPanel.SetActive(false);
    }

    public bool IsBusy()
    {
        return isBusy;
    }

    public void CardRevealed(CardRotate card)
    {
        if (firstCard == null)
        {
            firstCard = card;
        }
        else if (secondCard == null)
        {
            secondCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        isBusy = true;

        yield return new WaitForSeconds(0.5f); // Wait to let player see the second card

        if (firstCard.cardId == secondCard.cardId)
        {
            firstCard.PlayDeleteAnimation();
            secondCard.PlayDeleteAnimation();

            matchedCards += 2; //  Only increase when match is correct

            if (matchedCards >= totalCards)
            {
                GameWin(); //  Show panel only after all matched
            }
        }
        else
        {
            firstCard.FlipToBack();
            secondCard.FlipToBack();
        }

        // Reset cards and state
        firstCard = null;
        secondCard = null;
        isBusy = false;

    }

    void GameWin()
    {
        backgroundMusic.Stop();
        sfxAudioSource.PlayOneShot(winSound);
        winPanel.SetActive(true);
    }

    public void exit()
    {
        Application.Quit();
    }
}
