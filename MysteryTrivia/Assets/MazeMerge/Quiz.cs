using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Questions> QnA;
    public GameObject[] options;
    public int currentQuestion;

    public Text QuestionTxt;

    public void Start()
    {
        generateQuiestion();
    }
    public void correct()
    {
        QnA.RemoveAt(currentQuestion);
        generateQuiestion();
    }
    void SetAnswers()
    {
        for(int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<Answers>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = QnA[currentQuestion].Answers[i];
            if (QnA[currentQuestion].correctAnswer == i + 1)
            {
                options[i].GetComponent<Answers>().isCorrect = true;
            }
        }
    }
    void generateQuiestion()
    {
        currentQuestion = Random.Range(0, QnA.Count);
        QuestionTxt.text = QnA[currentQuestion].Question;
        SetAnswers();
       
    }
}
