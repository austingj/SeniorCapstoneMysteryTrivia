using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    //Leaderboard Name
    public string LeaderboardName = "MyLeaderboard"; // Change this to your desired leaderboard name
    //Email
    public static string playerEmail="";
    //Password
    public static  string password;
    //This list will contain all leaderboard data
    public static List<LeaderBoardModel> data;
    //Bool check is user is logged in or not
    public static bool IsLoggedIn = false;
    //Static instance
    public static LeaderboardManager Instance { get; private set; }

    //   public static LeaderboardManager instance=null;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public void UpdateScore(int score)
    {
        Debug.Log("UPDATE SCORE METHOD : " + score);
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {
                StatisticName = LeaderboardName,
                Value = score
            }
        }
        }, result => OnUpdateLeaderboardSuccess(result), OnUpdateLeaderboardError);
    }
    //Submit Score of player
    public void SubmitScore(int score)
    {
        //if player email is empty or not
        if (playerEmail.Equals(""))
        {
            return;
        }
        //Send request to update player
        //Create Credentials for loggin of user
        var emailLogin = new LoginWithEmailAddressRequest
        {
            Email = playerEmail,
            Password = password// Change this to your desired password
        };

        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new System.Collections.Generic.List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = LeaderboardName,
                    Value = score ,
                }
            }
        };
        Debug.Log("score = " + score);
        PlayerPrefs.SetInt("leaderscore", score);
        //Login the user for updating the score
        PlayFabClientAPI.LoginWithEmailAddress(emailLogin, OnLoginSuccess, OnLoginError);
    }
    void OnLoginSuccess(LoginResult result)
    {
        int score = PlayerPrefs.GetInt("leaderscore");
        
        

        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
      
            Statistics = new List<StatisticUpdate> {
        new StatisticUpdate {
              StatisticName = LeaderboardName,
            Value = score
        },
    }
        },
result => { 
    Debug.Log("User statistics updated");
    Invoke("GetLeaderboard", 2f);
},
error => { Debug.LogError(error.GenerateErrorReport()); });

    }
     
    void OnLoginError(PlayFabError error)
    {
     
    }
    /*
    {
        var updateStats = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
        new StatisticUpdate {
            StatisticName = LeaderboardName,
            Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(updateStats, response =>
        {
            //UpdateScore(score);
            Debug.Log("Score submitted successfully!");
            Invoke("GetLeaderboard", 2f);
        }, error =>
        {
            Debug.LogError("Error submitting score: " + error.ErrorMessage);
        });
    }, error =>
    {
        if (error.Error == PlayFabErrorCode.AccountNotFound)
        {
        }
        else
        {
            Debug.LogError("Error logging in: " + error.ErrorMessage);
        }
    });
    */


    //Function to get leaderboard
    public List<LeaderBoardModel> GetLeaderboard()
    {
       //Clear previous data 
        data = new List<LeaderBoardModel>();

        //Send request
        var request = new GetLeaderboardRequest
        {
            StatisticName = LeaderboardName,
            StartPosition = 0,
            MaxResultsCount = 100
        };

        //Get Leaderboard
        PlayFabClientAPI.GetLeaderboard(request, result =>
        {
            Debug.Log("Leaderboard:");
            foreach (var entry in result.Leaderboard)
            {
                //Add new player score into that list

    

                if (entry.DisplayName==null)
                {
                    data.Add(new LeaderBoardModel(entry.Position, entry.PlayFabId, entry.StatValue));
                }
             else
                    data.Add(new LeaderBoardModel(entry.Position, entry.DisplayName, entry.StatValue));

                Debug.Log($"{entry.Position}. {entry.DisplayName} ({entry.PlayFabId}): {entry.StatValue}");
            }
        }, error =>
        {
            //If some error occured
            Debug.LogError("Error getting leaderboard: " + error.ErrorMessage);
        });
        return data;
    }


   

//If Leaderboard Data is successfuly updated
    private void OnUpdateLeaderboardSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Score submitted successfully!");
        Debug.Log("Leaderboard updated successfully!");
    
        Invoke("GetLeaderboard", 2f);
    }

    //If there is an error in leadboard
    private void OnUpdateLeaderboardError(PlayFabError error)
    {
        Debug.LogError("Error updating leaderboard: " + error.ErrorMessage);
    }

}
