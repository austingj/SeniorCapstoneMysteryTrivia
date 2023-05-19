using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
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

public class Question
{
    public string q;
    public string answer1;
    public string answer2;
    public string answer3;
    public string answer4;

    public string answer;
    public Question()
    {

    }
    public Question(string q, string a1, string a2, string a3, string a4, string answer)
    {
        this.q = q;
        this.answer = answer;
        this.answer1 = a1;
        this.answer2 = a2;
        this.answer3 = a3;
        this.answer4 = a4;
    }
  public void Output()
    {
        Debug.Log("Question: " + this.q + " answers: " + this.answer1 + " " + this.answer2 + " " + this.answer3 + " " + this.answer4 + "   CORRECT: " + this.answer);
    }
}
public class testjson : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("START");
        var request = new LoginWithCustomIDRequest() { TitleId = "47EFF", CustomId = "66E70F4C7E6A49" };
        PlayFabClientAPI.LoginWithCustomID(request, OnSucccuss, OnFailure);
        //PlayFabClientAPI.GetUserData(request){ TitleId = "47EFF", CustomId = "66E70F4C7E6A49" };
    }

    private void OnSucccuss(LoginResult obj)
    {
        Debug.Log("Logged on");
        //var request = new UpdateUserDataRequest() { Data = new Dictionary<string, string>() { { "Class", "Fighter" } }, Permission = UserDataPermission.Public };
        //PlayFabClientAPI.UpdateUserPublisherData(request, OnUpdateSuccess, OnFailure);
    }
    private void OnUpdateSuccess(UpdateUserDataResult obj)
    {
        Debug.Log("Success.");
        
    }
    private void OnFailure(PlayFabError obj)
    {
        Debug.Log(obj.GenerateErrorReport());
    }
    public void readJSON()
    {
        
        //PlayFabClientAPI.GetUserData(new GetUserDataRequest(), onDataRecieved, OnError);
        //GetUserDataRequest request = new GetUserDataRequest()

        
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            Keys = null,
            PlayFabId = "9C6D28E7F942FE1F"

        }, onDataRecieved, OnError);
        
     

    }
    void OnError(PlayFabError error)
    {
        Debug.Log("PlayFab Error," + error);
    }
    void onDataRecieved(GetUserDataResult result)
    {
        Debug.Log("Recieved data!");
        
        if (result.Data != null && result.Data.ContainsKey("Math"))
        {
            List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(result.Data["math"].Value);
            foreach (var item in questions)
            {
                item.Output();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
