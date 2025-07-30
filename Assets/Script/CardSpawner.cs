using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardSpawner : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardImages; 
    public Transform cardContainer;
    public int rows ;
    public int columns ;

    private List<int> cardIds = new List<int>();

    void Start()
    {
        GenerateGrid(rows, columns);
    }

    public void GenerateGrid(int rows, int columns)
    {
        // Clean existing cards
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        cardIds.Clear();

        int totalCards = rows * columns;
        if (totalCards % 2 != 0)
        {
            Debug.LogError("Card count must be even!");
            return;
        }

        // Create card ID pairs
        for (int i = 0; i < totalCards / 2; i++)
        {
            cardIds.Add(i);
            cardIds.Add(i);
        }

        // Shuffle cardIds
        Shuffle(cardIds);

        // Spawn cards
        for (int i = 0; i < totalCards; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardContainer);
            CardRotate card = cardObj.GetComponent<CardRotate>();

            int cardId = cardIds[i];
            Sprite assignedSprite = cardImages[cardId]; // use card ID to select the right image

            card.SetCardImage(assignedSprite, cardId);
        }


        // Set Grid Layout Group constraints
        GridLayoutGroup grid = cardContainer.GetComponent<GridLayoutGroup>();
        if (grid != null)
        {
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = columns;

            float spacing = grid.spacing.x;
            RectTransform rt = cardContainer.GetComponent<RectTransform>();

            float totalWidth = rt.rect.width - (columns - 1) * spacing;
            float totalHeight = rt.rect.height - (rows - 1) * spacing;

            float cellWidth = totalWidth / columns;
            float cellHeight = totalHeight / rows;

            grid.cellSize = new Vector2(cellWidth, cellHeight);
        }
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int rand = Random.Range(i, list.Count);
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}
