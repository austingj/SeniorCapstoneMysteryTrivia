using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
//using Firebase.Database;
public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    //    DatabaseReference reference;
    public Animator playeranimator;
    [SerializeField]
    public float speed = 3.0f;
    [SerializeField]
    private float rotationSpeed;

    private PolygonCollider2D polycolider;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public int points = 0;
    public int level = 0;
    public int mazenumber = 0;
    public int flag = 0;
    public int flagBoost = 0;

    private bool facingRight = true;
    private bool froze = false;
    public TMP_Text hintPoints;
    public TMP_Text win;
    public string popUp = "popUp";
    public bool isGameWon = false;
    public TMP_Text check;
    popupquestions popq;
    User user;

    GameObject newLock;

    private bool guest = true;

    public AudioSource explosion;
    public AudioSource goodPickup;
    public AudioSource whoosh;
    void Start()
    {
        PlayerPrefs.SetInt("availablePoints", 50);
        int guestlog = PlayerPrefs.GetInt("Guest");
        if (guestlog == 0)
        {
            Debug.Log("Playing as Guest");
            guest = true;
            PlayerPrefs.SetInt("Guest", 0); //0 set to as logged on as guest
            var request = new LoginWithCustomIDRequest() { TitleId = "47EFF", CustomId = "714738762171981" };
            PlayFabClientAPI.LoginWithCustomID(request, OnGuestLoginSuccess, OnError);
        }
        else
        {
            guest = false;
        }
        //reference = FirebaseDatabase.DefaultInstance.RootReference;
        rb = GetComponent<Rigidbody2D>();
        polycolider = GetComponent<PolygonCollider2D>();
        popq = GameObject.Find("PopupQuestion").GetComponent<popupquestions>();
        // Animator animator = popq.animator;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (!spriteRenderer) Debug.LogError("Can't Find the Sprite Renderer");
        //StartCoroutine(doWait());
        //toggleSpeed();
        user = new User();
        if (flag == 0)
        {
            Debug.LogError("Flag = 0");
            int newgameflag = PlayerPrefs.GetInt("newgame");
            if (newgameflag == 1)//==1 means it is new game
            {
                Debug.LogError("user newgameflag ==1");
                user.PrintUser();
                PlayerPrefs.SetInt("newgame", 0); //0 set to not new game
                Read_Data();
                user.HintPoints = 0;
                Debug.LogError("user after newgameflag==1");
                user.PrintUser();
            }
            else
            {
                Debug.LogError("ELSE read_data");
                Read_Data();//this is NOT a new game so read data normally
                user.PrintUser();
            }

            flag = flag + 1;
        }

    }
    void OnGuestLoginSuccess(LoginResult result)
    {
        Debug.Log("Guest Login Success");
        
 
    }
    // Update is called once per frame
    void Update()
    {
        if (flag <10)
        {
            Debug.LogError("Flag <3");
            Read_Data();
            user.PrintUser();

            flag = flag + 1;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if(froze == true)
        {
            horizontalInput = 0;
            verticalInput = 0;
        }

        if(horizontalInput < 0)
        {
            if (facingRight == true) //currently facing right and moving left
            {
                //Debug.Log("FLIP");
                //spriteRenderer.flipX = false;
                Vector3 theFlip = transform.localScale;
                float x = theFlip.x;
                float y = theFlip.y;
                float z = theFlip.z;
                transform.eulerAngles = new Vector3(-x, y, z);
                transform.localScale = new Vector3(-x, y, z);
                facingRight = false;
                //Flip();
            }
        }
        if (horizontalInput > 0)
        {
            //Debug.Log("DONT FLIP");
            if (facingRight == false)
            {
                Debug.Log("FLIP");
                Vector3 theFlip = transform.localScale;
                float x = theFlip.x;
                float y = theFlip.y;
                float z = theFlip.z;
                transform.eulerAngles = new Vector3(-x, y, z);
                transform.localScale = new Vector3(-x, y, z);
                facingRight = true;
                //Flip();
            }
        }
        Vector2 movementDirection = new Vector2(horizontalInput, verticalInput);
        float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);
        movementDirection.Normalize();
        transform.Translate(movementDirection * speed * inputMagnitude * Time.deltaTime, Space.Self);

        if (playeranimator != null)
        {
            if (horizontalInput != 0 || verticalInput != 0)
            {
                playeranimator.SetFloat("Speed", 3); //set animation to go faster
            }
            else
            {
                playeranimator.SetFloat("Speed", 0);
            }
        }
        /*
        if(movementDirection != Vector2.zero)
        {
           // Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, movementDirection);
            //float angle = Mathf.Atan2(toRotation.y, toRotation.x)* Mathf.Rad2Deg;
            //Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
           // transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
        */
        //int temppoints = points;
       // int playerpoints = PlayerPrefs.GetInt("playerpoints");
        PlayerPrefs.SetInt("playerpoints", points);
        hintPoints.text = "Hint Points: " + points;
    }
    public void waitToggleSpeed()
    {
        Invoke("toggleSpeed", 1.5f);
    }
    public void toggleSpeed()
    {
        if (speed > 0)
        {
            speed = 0;
        }
        else
        {
            //set to normal speed
            speed = 3.0f;
            if (flagBoost > 0)
            { //if already had boost, change to 7
                speed += 4.0f;
            }
        }
    }
    public void Read_Data()
    {
        if(guest == true)
        {
            int guestpoints = PlayerPrefs.GetInt("guestpoints");
            Debug.Log("Guest points: " + guestpoints);
            points = guestpoints;
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
    }
    void OnDataRecieved(GetUserDataResult result)
    {
        if(guest == true)
        {
            
        }
        if (result.Data != null && result.Data.ContainsKey("Email") && result.Data.ContainsKey("HintPoints")
            && result.Data.ContainsKey("Level") && result.Data.ContainsKey("MazeNumber") && result.Data.ContainsKey("Username") && result.Data.ContainsKey("Category"))
        {
            user.setUser(result.Data["Email"].Value,
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
        if (user.MazeNumber == 0)
        {
            user.MazeNumber = 6; //set to start of maze 1 if this is the first game
        }
        calculateDifficulty();
        PlayerPrefs.SetString("Category", user.Category);
        points = user.HintPoints;
        mazenumber = user.MazeNumber;
        user.PrintUser();
    }
    void calculateDifficulty()
    {
        

        int difficulty = PlayerPrefs.GetInt("Difficulty");
        if (difficulty == -1)
        {
            Debug.Log("difficulty: " + difficulty);
            return; //will always stay -1 for teachers code
        }
        difficulty = user.Level;
        Debug.Log("user difficulty: " + difficulty);
        if (difficulty == -1)
        {
            return; //will always stay -1 for teachers code
        }
        
        else if (difficulty ==1 && user.MazeNumber == 8)
        {
            difficulty = 2;
            user.Level = 2;
            PlayerPrefs.SetInt("Difficulty", difficulty);
        }
        else if (difficulty == 2 && user.MazeNumber == 22)
        {
            difficulty = 3;
            user.Level = 3;
            PlayerPrefs.SetInt("Difficulty", difficulty);
        }
        else if (difficulty == 3 && user.MazeNumber == 30)
        {
            difficulty = 4;
            user.Level = 4;
            PlayerPrefs.SetInt("Difficulty", difficulty);
        }
    }
    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Successfully sent user data!");
    }
    void OnError(PlayFabError error)
    {
        Debug.Log("PlayFab Error," + error);
    }
   
    public void SaveData()
    {
        if (guest == true)
        {
            Debug.Log("GUEST: ");
            PlayerPrefs.SetInt("guestpoints", points); //save guest points for next level
            Debug.Log("Guest points: at save" + points);
            return;
        }
        Debug.Log("USER: ");
        user.HintPoints = points;
        user.MazeNumber = SceneManager.GetActiveScene().buildIndex + 1;
        user.Level = PlayerPrefs.GetInt("Difficulty");
        user.PrintUser();
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Email", user.Email },
                {"Username", user.Username },
                {"HintPoints", points.ToString() },
                {"Level", user.Level.ToString() },
                {"MazeNumber", user.MazeNumber.ToString() },
                {"Category", user.Category }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemies")
        {
            Vector3 theFlip = transform.localScale;
            rb.freezeRotation = true;
            froze = true;
            polycolider.enabled = false;
            float x = theFlip.x;
            float y = theFlip.y;
            float z = theFlip.z;
            transform.eulerAngles = new Vector3(-x, y, z);
            transform.localScale = new Vector3(-x * 4, y * 4, z * 4);
            playeranimator.SetTrigger("death");
            explosion.Play();

        }
        if (collision.gameObject.tag == "Chest")
        {
            Debug.Log("CHEST");
            points += 150;
            hintPoints.text = "Hint Points: " + points;
            goodPickup.Play();
        }
        if (collision.gameObject.tag == "Boost")
        {
            flagBoost += 1;
            speed += 2.0f;
            Destroy(collision.gameObject);
            whoosh.Play();
        }
        if (collision.gameObject.tag == "Finish")
        {
            hintPoints.gameObject.SetActive(false);
            isGameWon = true;
            if (FindObjectOfType<LevelComplete>() != null)
                FindObjectOfType<LevelComplete>().ShowLevelCompletePanel();
            else
                win.text = "YOU WIN!!!!";
            SaveData();
        }
        if (collision.gameObject.tag == "Lock" && popq.unlock == false)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.Translate(18 * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {

                transform.Translate(-18 * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {

                transform.Translate(0, -18 * Time.deltaTime, 0);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {

                transform.Translate(0, 18 * Time.deltaTime, 0);
            }
        }

        if (collision.gameObject.tag == "Lock")
        {
            newLock = collision.gameObject;


            popq = GameObject.Find("PopupQuestion").GetComponent<popupquestions>();
            if(popq == null)
            {
                popq = new popupquestions();
            }
            if (popq.unlock == true)
            {
                //Destroy(collision.gameObject);
                newLock = collision.gameObject;
                newLock.GetComponent<Animator>().SetTrigger("Unlock");
                popq.unlock = false;
                int availablepoints = PlayerPrefs.GetInt("availablePoints");
                if(availablepoints <= 0)
                {
                    availablepoints = 0;
                }
                points = points + availablepoints;
                PlayerPrefs.SetInt("availablePoints", 50);
            }
            else
            {
                popq.PopUp(popUp);
                toggleSpeed();//once question appears, change player speed to 0
                              //enable popup then get the question
                popq.getQuestion();
            }
        }
       
        
    }
}
