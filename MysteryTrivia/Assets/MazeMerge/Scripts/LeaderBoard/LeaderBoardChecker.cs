using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardChecker : MonoBehaviour
{
    public GameObject LeaderboardButton;
    void Start()
    {
        
    }
    private void OnEnable()
    {
        //If user is not logged in then do not show leaderboard
        if(LeaderboardManager.IsLoggedIn==false)
        {
            //Turn off leadeboard if user is not logged in
            LeaderboardButton.gameObject.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
