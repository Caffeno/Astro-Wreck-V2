using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    private string scoreTemplate = "Score: ";
    public static int scoreValue = 0;

    private Text screenText;
    private LootTracker playerLoot;

    // Start is called before the first frame update
    void Start()
    {
        screenText = GetComponent<Text>();

        playerLoot = FindObjectOfType<Player>().GetComponent<LootTracker>();
        GameEvents.instance.updateScore += UpdateScoreTotal;
        UpdateScoreTotal();
    }



    // Update is called once per frame
    void Update()
    {
    }

    private void UpdateScoreTotal()
    {
        scoreValue = playerLoot.GetScore();
        screenText.text = scoreTemplate + scoreValue;

    }

    public void ResetScore()
    {
        scoreValue = 0;
    }
}
