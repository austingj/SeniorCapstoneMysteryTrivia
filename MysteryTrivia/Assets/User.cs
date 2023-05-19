using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    // Start is called before the first frame update
    public string Email;
    public string Username;
    public int HintPoints;
    public int Level;
    public int MazeNumber;
    public string Category;
    public User(string email, string username, int hintPoints, int level, int mazeNumber, string category)
    {
        Email = email;
        Username = username;
        HintPoints = hintPoints;
        Level = level;
        MazeNumber = mazeNumber;
        Category = category;
    }
    public User()
    {
        Email = "";
        Username = "";
        HintPoints = 0;
        Level = 0;
        MazeNumber = 0;
        Category = "Math";

    }

    public User setUser(string email, string username, int hintpointss, int level, int mazenumber, string category)
    {
        this.Email = email;
        this.Username = username;
        this.HintPoints = hintpointss;
        this.Level = level;
        this.MazeNumber = mazenumber;
        this.Category = category;
        return this;
    }
    public void PrintUser()
    {
        Debug.LogError("email : " + this.Email + " usn: " + this.Username +
           " hintpoints: " + this.HintPoints + " level: " + this.Level + " mazenumber:" + this.MazeNumber + " Category: " + this.Category);

    }
}