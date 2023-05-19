using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answers : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isCorrect = false;
    public Quiz quiz;

    public void Answer()
    {
        if (isCorrect)
        {
            Debug.Log("Correct");
            quiz.correct();
        }
        else
        {
            Debug.Log("Wrong");
        }
    }
}
