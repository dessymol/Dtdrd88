using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardFlip : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource sfxAudioSource;
    public AudioClip winSound;
    public AudioClip mismatchSound;
    public GameObject winPanel;

    private List<CardRotate> flippedCards = new List<CardRotate>();
    private int totalCards;
    private int matchedCards = 0;

    public TextMeshProUGUI scoreText;

    private int score = 0;

    void Start()
    {
        totalCards = FindObjectsOfType<CardRotate>().Length;
        winPanel.SetActive(false);
    }

    public void CardRevealed(CardRotate card)
    {
        // Add to flipped cards list
        flippedCards.Add(card);

        // Only check when 2 cards flipped
        if (flippedCards.Count >= 2)
        {
            StartCoroutine(CheckMatch(flippedCards[flippedCards.Count - 2], flippedCards[flippedCards.Count - 1]));
        }
    }

    IEnumerator CheckMatch(CardRotate firstCard, CardRotate secondCard)
    {
        // Wait so player can see both
        yield return new WaitForSeconds(0.5f);

        if (firstCard.cardId == secondCard.cardId)
        {
            firstCard.PlayDeleteAnimation();
            secondCard.PlayDeleteAnimation();

            matchedCards += 2;

            AddScore(1);

            if (matchedCards >= totalCards)
            {
                GameWin();
            }
        }
        else
        {
            // Mismatch: flip back
            firstCard.FlipToBack();
            secondCard.FlipToBack();
            sfxAudioSource.PlayOneShot(mismatchSound);
        }

        // Clean up: remove both from flipped list
        flippedCards.Remove(firstCard);
        flippedCards.Remove(secondCard);
    }

    void GameWin()
    {
        backgroundMusic.Stop();
        sfxAudioSource.PlayOneShot(winSound);
        StartCoroutine(ShowWinPanelWithDelay());
    }

    IEnumerator ShowWinPanelWithDelay()
    {
        yield return new WaitForSeconds(1f); 
        winPanel.SetActive(true);
    }

    public int rows = 2;
    public int cols = 2;
    public List<CardRotate> allCards = new List<CardRotate>();

    public void SaveGame()
    {
        PlayerPrefs.SetInt("Rows", rows);
        PlayerPrefs.SetInt("Cols", cols);
        PlayerPrefs.SetInt("Score", score);

        // Save matched cards
        List<int> matchedIds = new List<int>();
        foreach (var card in allCards)
        {
            if (card.IsMatched())
                matchedIds.Add(card.cardId);
        }

        // Convert list to string (e.g., "1,2,3")
        string matchedString = string.Join(",", matchedIds);
        PlayerPrefs.SetString("MatchedCards", matchedString);

        PlayerPrefs.Save();
        Debug.Log("Game Saved!");
    }


    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("Score")) // Or "MatchedCards"
        {
            Debug.Log("No saved game!");
            return;
        }

        rows = PlayerPrefs.GetInt("Rows");
        cols = PlayerPrefs.GetInt("Cols");
        score = PlayerPrefs.GetInt("Score");

        if (scoreText != null)
            scoreText.text = "Score: " + score;

        string matchedString = PlayerPrefs.GetString("MatchedCards");
        List<int> matchedIds = matchedString.Split(',').Select(int.Parse).ToList();

        foreach (var card in allCards)
        {
            if (matchedIds.Contains(card.cardId))
            {
                card.SetMatchedInstantly();
            }
        }

        Debug.Log("Game Loaded!");
    }

    [System.Serializable]
    public class SaveData
    {
        public int rows;
        public int cols;
        public List<int> matchedCardIds = new List<int>();
        public int score;
    }
    public void AddScore(int amount)
    {
        score += amount;
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void exit()
    {
        Application.Quit();
    }
}
