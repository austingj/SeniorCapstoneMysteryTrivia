using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Unity.VisualScripting;

public class PlayFabTestAJ : MonoBehaviour
{
    [SerializeField] TMP_InputField codeInput;
    [SerializeField] TMP_Text messageText;
    User usertest;
    private Button LoadGame;
    private int sceneID = 0;
    private int difficulty = 1;
    private string category = "Math";
    private bool guest = true;


    // Start is called before the first frame update
    void Start()
    {
        int guestlog = PlayerPrefs.GetInt("Guest");
        if (guestlog == 0)
        {
            Debug.Log("Playing as Guest");
            guest = true;
        }
        else
        {
            guest = false;
        }
        usertest = new User();
        PlayerPrefs.SetInt("newgame", 0); //auto set newgame off
        GetData();
        GetDifficulty();
        GetCategory();
    }
    public void LoadScene()
    {
        Debug.Log("Scene ID: " + sceneID);
        SceneManager.LoadScene(sceneID);
    }
    public void CheckCode(string code)
    {
        code = codeInput.text;
        PlayerPrefs.SetString("codeCategory", code);
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = null,
            PlayFabId = "9C6D28E7F942FE1F"
        }, CheckData, OnError);
    }
    void CheckData(GetUserDataResult result)
    {
        
        string codeCategory = PlayerPrefs.GetString("codeCategory");
        Debug.Log("Checking code !" + codeCategory);
        if (result.Data != null && result.Data.ContainsKey(codeCategory))
        {
            Debug.Log("Contains code: " + codeCategory);
            PlayerPrefs.SetString("Category", codeCategory);
            SetCategory(codeCategory);
            SetNewGame();
            SetDifficulty(-1); //-1 will represent unknown difficulty as this is for custom questions, generally the teacher wont have separate levels of difficulty
            LoadScene();

        }
        else
        {
            Debug.Log("Error getting user data");
            messageText.text = "Error invalid code!";
        }
    }
    public void GetData()
    {
        if (guest == true)
        {
            return;
        }
        if (PlayFabClientAPI.IsClientLoggedIn() == false)
        {
            Debug.Log("NOT LOGGED ON");
        }
        else
        {
            Debug.Log("user is logged on");
            PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieved, OnError);
        }
        // PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieved, OnError);
    }
    void OnDataRecieved(GetUserDataResult result)
    {
        //usertest = new User();
        if (guest == true)
        {
            sceneID = 6;
            return;
        }
        if (result.Data != null && result.Data.ContainsKey("Email") && result.Data.ContainsKey("HintPoints")
            && result.Data.ContainsKey("Level") && result.Data.ContainsKey("MazeNumber") && result.Data.ContainsKey("Username") && result.Data.ContainsKey("Category"))
        {
            usertest.setUser(result.Data["Email"].Value,
                result.Data["Username"].Value,
                int.Parse(result.Data["HintPoints"].Value),
                int.Parse(result.Data["Level"].Value),
                int.Parse(result.Data["MazeNumber"].Value),
                result.Data["Category"].Value);

        }
        else
        {
            Debug.Log("Error getting user data");
        }
        if (usertest.MazeNumber == 0)
        {
            LoadGame = GameObject.FindGameObjectWithTag("LoadGame").GetComponent<Button>();
            LoadGame.interactable = false;
            usertest.MazeNumber = 6; //set to start of maze 1
        }
        sceneID = usertest.MazeNumber;
        usertest.PrintUser();
    }

    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Successfully sent user data!");
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("PlayFab Error," + error);
    }
    public void SetDifficulty(int difficult)
    {
        difficulty = difficult;
        PlayerPrefs.SetInt("Difficulty", difficulty);
        Debug.Log("Difficulty: " + difficulty);
        if(difficulty ==1)
        {
            sceneID = 6;
        }
        else if (difficulty == 2)
        {
            sceneID = 15;
        }
        else if (difficulty == 3)
        {
            sceneID = 22;
        }
        else if (difficulty == 4)
        {
            sceneID = 30;
        }
    }
    public void GetDifficulty()
    {
        if (PlayerPrefs.GetInt("Difficulty") > 0)
        {
            difficulty = PlayerPrefs.GetInt("Difficulty");
        }
    }
    public void SetCategory(string ctgry)
    {
        category = ctgry;
        PlayerPrefs.SetString("Category", category);
        Debug.Log("Category: " + category);
        //SaveData();
        sceneID = 6; //set to first scene
    }

    public void loadTeacherScene()
    {

        SceneManager.LoadScene("Teachers");
    }
    public void backCategoryScene()
    {

        SceneManager.LoadScene("Categories");
    }
    public void SetNewGame()
    {
      
        Debug.Log("USER: in savenewgame");
        //usertest = new User();
        //GetData();
        category = PlayerPrefs.GetString("Category");
        PlayerPrefs.SetInt("newgame", 1);
        usertest.Category = category;
        usertest.HintPoints = 0;
        usertest.MazeNumber = sceneID;
        usertest.Level = difficulty;
        usertest.PrintUser();

        if (guest == true)
        {
            Debug.Log("GUEST ACTIVE");
            return;
        }

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Email", usertest.Email },
                {"Username", usertest.Username },
                {"HintPoints", usertest.HintPoints.ToString() },
                {"Level", usertest.Level.ToString() },
                {"MazeNumber", usertest.MazeNumber.ToString() },
                {"Category", usertest.Category }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
        usertest.PrintUser();

    }
    public void GetCategory()
    {
        Debug.Log("User category: " + usertest.Category);
    }
    public void SaveData()
    {
        Debug.Log("USER: ");
        //usertest = new User();
        //GetData();
        category = PlayerPrefs.GetString("Category");
        usertest.Category = category;
        usertest.PrintUser();
        if (guest == true)
        {
            return;
        }
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Email", usertest.Email },
                {"Username", usertest.Username },
                {"HintPoints", usertest.HintPoints.ToString() },
                {"Level", usertest.Level.ToString() },
                {"MazeNumber", usertest.MazeNumber.ToString() },
                {"Category", usertest.Category }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
        usertest.PrintUser();
    }



    // Update is called once per frame
    void Update()
    {

    }
}
