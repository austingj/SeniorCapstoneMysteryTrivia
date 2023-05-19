using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
public class popupquestions : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject popUpBox;
    public Animator animator;
    public TMP_Text popUpText; //text for question
    public Player player1;
    public GameObject player2;
    public TMP_Text answer1;
    public TMP_Text answer2;
    public TMP_Text answer3;
    public TMP_Text answer4;


    public Button answer1button;
    public Button answer2button;
    public Button answer3button;
    public Button answer4button;
    public GameObject QuestionBox; //button object for holding question txt

    public string ChosenCategory = "";
    public GameObject answerbutton;
    public string correctAnswer = "";
    public bool unlock = false;
    private int hintCount = 0;
    private int flg = 0;
    private int total = 0;
    private int start = 0;

    public string answer;
      string[] ArrayQuestions = new string[600];
    string[] ArrayAnswers = new string[2400];
    string[] CorrectAnswers = new string[600];
    public AudioSource rightAnswer;
    public AudioSource wrongAnswer;

    void Start()
    {
        
        Debug.Log("START");
    
        
    
        }

    private void OnSucccuss(LoginResult obj)
    {
        Debug.Log("Logged on");
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
        int i = 0;
        int j = 0;
        Debug.Log("Recieved data!");
        ChosenCategory = PlayerPrefs.GetString("Category");
        if (result.Data != null && result.Data.ContainsKey("Math") && result.Data.ContainsKey("English") && result.Data.ContainsKey("History"))
        {
            List<Question> questions = JsonConvert.DeserializeObject<List<Question>>(result.Data[ChosenCategory].Value);
            foreach (var item in questions)
            {
                ArrayQuestions[i] = item.q;
                ArrayAnswers[j] = item.answer1;
                ArrayAnswers[j+1] = item.answer2;
                ArrayAnswers[j+2] = item.answer3;
                ArrayAnswers[j+3] = item.answer4;
                CorrectAnswers[i] = item.answer;
                i++;
                j = j + 4;
                //item.Output();
            }
        }
        total = i - 1;
        Debug.Log("total questions: " + total);
    }

    // Update is called once per frame
    void Update()
    {
        if (flg < 50)
        {
            flg++;
        }
        else if (flg == 50)
        {
            flg++;
            readJSON();
        }
    }
    public void PopUp(string text)
    {
        animator.SetTrigger("pop");
    }

    public void checkPoints()
    {
        string currentQuestion;
        string[] currentAnswers;

        //player1 = GameObject.Find("Player").GetComponent<Player>();
        // int points = GameObject.Find("Player").GetComponent<Player>().points;
        int playerpoints = PlayerPrefs.GetInt("playerpoints");
        int points = playerpoints;
        // int points = player1.points;
        int randomNum = Random.Range(0, 3); //rn 0 to number of questions

        var hintButton = GameObject.Find("hint").GetComponent<Button>();

        if (points < 200 || hintCount >= 3) // Disable hint button if points are less than 200 or hint button has been used 3 times
        {
            hintButton.interactable = false;
            hintButton.GetComponent<Image>().color = Color.red;
            hintCount = 0;
        }
        else
        {
            hintButton.interactable = true;
            //hintButton.GetComponent<Image>().color = Color.green;
        }

        // If the hint button is clicked, subtract 200 points and destroy one wrong answer
        if (hintButton.interactable)
        {
            currentQuestion = popUpText.text;
            currentAnswers = new string[4];
            currentAnswers[0] = answer1.text;
            currentAnswers[1] = answer2.text;
            currentAnswers[2] = answer3.text;
            currentAnswers[3] = answer4.text;
            Debug.Log(correctAnswer);
            // subtract 200 points from the player's score
            player1.points -= 200;
            player1.hintPoints.text = player1.points.ToString();
            hintCount++;

            // Loop through the answer options and destroy one wrong answer
        
            bool destroyed = false; // flag to check if an answer has been destroyed
            while (destroyed == false)
            {
                int randomNumber = Random.Range(0, 4); //rn 0 to 3 for 4 available answers
                if (answer1 != null && answer1.text != correctAnswer && !destroyed && answer1button.interactable && randomNumber == 0)
                {
                    //Destroy(answer1.gameObject);
                    answer1button.interactable = false;
                    answer1.text = "";
                    destroyed = true;
                }
                else if (answer2 != null && answer2.text != correctAnswer && answer2button.interactable && randomNumber == 1)
                {
                    //Destroy(answer2.gameObject);
                    answer2button.interactable = false;
                    answer2.text = "";
                    destroyed = true;
                }
                else if (answer3 != null && answer3.text != correctAnswer && answer3button.interactable && randomNumber == 2)
                {
                    //Destroy(answer3.gameObject);
                    answer3button.interactable = false;
                    answer3.text = "";
                    destroyed = true;
                }
                else if (answer4 != null && answer4.text != correctAnswer && answer4button.interactable && randomNumber == 3)
                {
                    //Destroy(answer4.gameObject);
                    answer4button.interactable = false;
                    answer4.text = "";
                    destroyed = true;
                }

            }
        }
    }
    public int getQuestionRange()
    {
        int range = 0;
        int difficulty = PlayerPrefs.GetInt("Difficulty");
        if(difficulty == -1)//teachers code
        {
            range = total;
        }
        else if (difficulty == 1)
        {
            range = total*1/4;
        }
        else if (difficulty == 2)
        {
            range = total*2/4;
            start = total * 1 / 4;
        }
        else if (difficulty == 3)
        {
            range = total*3/4;
            start = total * 2 / 4;
        }
        else if (difficulty == 4)
        {
            range = total;
            start = total * 3 / 4;
        }
        Debug.Log("Range: " + range);
        return range;
    }
    public void getQuestion()
    {
        
        ChosenCategory = PlayerPrefs.GetString("Category");
        btnReset();
        QuestionBox.GetComponent<Image>().color = Color.yellow;
        //get chosen category
        //get random variable from category array
        //random variable = categoryanswer[0...4]
        int limit = ArrayQuestions.Length;
        int difficulty = PlayerPrefs.GetInt("Difficulty");
        limit = getQuestionRange();
        int randomNumber = Random.Range(start, limit+1); //rn 0-9
        int index = randomNumber;
        Debug.Log("question: " + randomNumber);
        //index = 3?
        //start questions at 3 index and shows index*4 + 1,2,3 for answers
        popUpText.text = ArrayQuestions[index];
        answer1.text = ArrayAnswers[index * 4];
        answer2.text = ArrayAnswers[index * 4 + 1];
        answer3.text = ArrayAnswers[index * 4 + 2];
        answer4.text = ArrayAnswers[index * 4 + 3];
        //show available answers
        //set correct answer
        correctAnswer = CorrectAnswers[index];
        Debug.Log("answer: " + correctAnswer);
        //now check answer in getAnswer
    }
    public void getAnswer(TMP_Text answertext)
    {
  
        Debug.LogError("Error In answertext");
        Debug.Log("answer chosen: " + answertext.text);
        popUpText.text = answertext.text;
        if (correctAnswer == answertext.text)
        {
            Debug.Log("CORRECT");
            unlock = true;
            QuestionBox.GetComponent<Image>().color = Color.green;
            rightAnswer.Play();
        }
        else
        {
           
            QuestionBox.GetComponent<Image>().color = Color.red;
            int availablepoints = PlayerPrefs.GetInt("availablePoints");
            PlayerPrefs.SetInt("availablePoints", availablepoints - 10);
            wrongAnswer.Play();
            Debug.Log("WRONG!!!!!! available points: " + (availablepoints-10));
        }
    }
    public void btnClick()
    {
        var hintButton = GameObject.Find("hint").GetComponent<Button>();
        hintButton.interactable = false;
        Debug.LogError("Error In btnclick");
        answer1button.interactable = false;
        answer2button.interactable = false;
        answer3button.interactable = false;
        answer4button.interactable = false;
    }
    public void btnReset()
    {
        var hintButton = GameObject.Find("hint").GetComponent<Button>();
       
        answer1button.interactable = true;
        answer2button.interactable = true;
        answer3button.interactable = true;
        answer4button.interactable = true;
        hintCount = 0;
        hintButton.interactable = true;
        hintButton.GetComponent<Image>().color = Color.white;
    }
}
