using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class ChangeScene : MonoBehaviour
{
    public void MoveToScene(int sceneID)
    {
        int guestlog = PlayerPrefs.GetInt("Guest");
        if (guestlog == 0)
        {
            Debug.Log("Playing as Guest");
            sceneID = 0;
        }
        SceneManager.LoadScene(sceneID);
    }
    public void MoveToScene(string sceneID)
    {
      
        SceneManager.LoadScene(sceneID);
    }
}
