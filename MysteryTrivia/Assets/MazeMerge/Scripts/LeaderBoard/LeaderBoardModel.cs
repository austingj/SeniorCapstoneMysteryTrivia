using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardModel 
{
    //This will store position
    public int rank;
    //This will store name
    public string name;
    //This will store score
    public int score;
    //Constructor will set all properties

    public LeaderBoardModel(int rank,string name,int score)
    {
        this.rank = rank;
        this.name = name;
        this.score = score;
    }
}
