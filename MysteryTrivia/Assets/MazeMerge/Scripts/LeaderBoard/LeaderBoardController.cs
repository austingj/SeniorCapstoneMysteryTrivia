using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LeaderBoardController : MonoBehaviour
{
    //This is container parent
    public Transform Parent;
    //This is leaderboard row
    public GameObject RowPrefab;

    //Set Leadeboard  when panel is turnedobn
    void OnEnable()
    {
        //Delete previous record
        for(int i=0;i<Parent.childCount;i++)
        {
            Destroy(Parent.GetChild(i).gameObject);
           // Parent.GetChild(i).gameObject.SetActive(false);
        }
        //Get Updated Leaderboard
        List<LeaderBoardModel> data = LeaderboardManager.data;

        //if(data.Count!=0)
        //{
        
        //Loop Through all Data
            for (int x = 0; x < data.Count; x++)
            {
            //Create Leaderbord Row
                GameObject q = Instantiate(RowPrefab, Parent);
            //Set The Position/Ranking starting from 1
                q.transform.GetChild(0).GetComponent<Text>().text = (data[x].rank + 1).ToString();

            //Set The  Name of the player

            if (data[x] == null)
                Debug.LogError("NAME IS NULL");

            Debug.Log("NAME:" + data[x].name.ToString());
            q.transform.GetChild(1).GetComponent<Text>().text = data[x].name.ToString();
            
            
            //Set The  Score of the player
            q.transform.GetChild(2).GetComponent<Text>().text = data[x].score.ToString();
            }
     
       //else
       // {
       //     StartCoroutine(LoadData());
       // }

        //LeaderboardManager.Instance.GetLeaderboard();


    }
  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
