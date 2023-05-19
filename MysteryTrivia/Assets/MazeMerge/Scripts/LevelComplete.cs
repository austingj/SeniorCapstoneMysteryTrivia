using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class LevelComplete : MonoBehaviour
{
    public GameObject LevelCompletePanel;
    public TextMeshProUGUI ResultText;
    public void ShowLevelCompletePanel()
    {
        float score = FindObjectOfType<Player>().points;
        ResultText.text = "Total Points: " + score;
        LevelCompletePanel.SetActive(true);
        LeaderboardManager.Instance.SubmitScore((int)score);
    }
    public void NextLevel()
    {
        int currentindex = SceneManager.GetActiveScene().buildIndex;
        int totalscenes = SceneManager.sceneCountInBuildSettings;

        if(currentindex==totalscenes-1)
        {
            currentindex = 6;
        }
        else
            currentindex++;

        SceneManager.LoadScene(currentindex);
    }
}
