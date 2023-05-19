using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
//change this variable to change the timer time
    public static float timeStorage = 60;
     public float timeRemaining =timeStorage ;
     public bool timerIsRunning = false;
     public GameObject Player;
     
     public GameObject Questions;
     public GameObject Clockface;
     public CanvasGroup UI_timer;
    
     public TMP_Text Timecounter;
     public string outProcessing;
     public int decimalIndex;
     

 
    // Start is called before the first frame update
    void Start()
    {
        Timecounter.SetText(timeStorage.ToString());
        Questions = GameObject.Find("PopupQuestion");
        
    }

    // Update is called once per frame
    void Update()
    {//timer gets called if the player is frozen for a question
        
          
         
        if(Player.GetComponent<Player>().speed==0){
            timerIsRunning = true;        
             UI_timer.alpha = 1f;
             Questions.GetComponent<popupquestions>().GetComponent<CanvasGroup>().alpha=1f;
             Questions.GetComponent<popupquestions>().GetComponent<CanvasGroup>().interactable=true;
            Clockface.GetComponent<Image>().color = Color.blue;
             //Clockface.GetComponent<Image>().color=new Color(0f,75f,255f,255f); 
        }
        else
        {
            UI_timer.alpha = 0f;
            Questions.GetComponent<popupquestions>().GetComponent<CanvasGroup>().alpha=0f;
            Questions.GetComponent<popupquestions>().GetComponent<CanvasGroup>().interactable=false;
            timeRemaining = timeStorage;
            timerIsRunning = false;
        }
        
       if (timerIsRunning)
        {   //changes timer face at 5 seconds to pressure players
            if(timeRemaining<6){               
               Clockface.GetComponent<Image>().color=new Color(1f,0f,0f,1f);                    
            }
            //this increments the timer downwards 
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                //this removes all the trailing float values since this calculates on frame
               outProcessing= timeRemaining.ToString();
               decimalIndex=outProcessing.LastIndexOf(".");
               outProcessing=outProcessing.Substring(0,decimalIndex);
                Timecounter.SetText(outProcessing);
            }
            
            else if (timeRemaining>-3)
            {
                //hardcode to remove negatives
                Timecounter.SetText("0");
                //keeps the timer running
                timeRemaining -= Time.deltaTime;
                UI_timer.alpha = 0f;
                //changes question text to explanation
                Questions.GetComponent<popupquestions>().popUpText.text="Time's up!";
              }
              else
              {
                //gets rid of timer visibility and interactability 
                timerIsRunning = false;
                timeRemaining = timeStorage;
                //Get rid of question visibility and interactability 
                 Questions.GetComponent<popupquestions>().GetComponent<CanvasGroup>().alpha=0f;
                 Questions.GetComponent<popupquestions>().GetComponent<CanvasGroup>().interactable=false;
                Player.GetComponent<Player>().toggleSpeed();
                int availablepoints = PlayerPrefs.GetInt("availablePoints");
                PlayerPrefs.SetInt("availablePoints", availablepoints - 10);
                Debug.Log("Timeout available points: " + (availablepoints-10));
              }
            }
        
            }
    
}
