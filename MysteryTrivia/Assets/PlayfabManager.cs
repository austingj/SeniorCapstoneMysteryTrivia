using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;
using Newtonsoft.Json;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;

public class PlayfabManager : MonoBehaviour
{
    public string TitleId = "47EFF";
    [SerializeField]
    private string sceneNameToLoad;
    private float timeElapsed;

    [Header("Login")]
    [SerializeField] TMP_Text messageText;
    [SerializeField] TMP_InputField emailLoginInput;
    [SerializeField] TMP_InputField passwordLoginInput;


    [Header("Register")]

    [SerializeField] TMP_InputField usernameRegisterInput;
    [SerializeField] TMP_InputField emailRegisterInput;
    [SerializeField] TMP_InputField passwordRegisterInput;
    [SerializeField] TMP_InputField confirmPasswordRegisterInput;

    [Header("Register")]

    [SerializeField] TMP_InputField emailRecoveryInput;

    // Start is called before the first frame update
    void Start()
    {
        TitleId = "47EFF";
        PlayFabSettings.TitleId = TitleId;
    }
    void Update()
    {

    }
    /*                                  ----Time delay----
    [SerializeField]
    private float delayBeforeLoading = 10f;
    [SerializeField]
    private string sceneNameToLoad;
    private float timeElapsed;
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > delayBeforeLoading)
        {
            SceneManager.LoadScene(sceneNameToLoad);
        }
    } */

    public bool isJitter;
    public float duration = 5f;
    public IEnumerator JitterCheck()
    {
        while (true)
        {
            for (float i = 0; i < duration && isJitter; i += Time.deltaTime)
            {
                Jitter();
                yield return null;
            }
            isJitter = false;
            yield return null;
        }
    }

    public void Jitter()
    {
        float jitter = Mathf.Sin(Time.time * 50f) * 0.20f; // Change the values as needed

        Vector3 newPosition = emailLoginInput.transform.position + new Vector3(jitter, 0f, 0f);
        emailLoginInput.transform.position = newPosition;

        Vector3 newPosition2 = passwordLoginInput.transform.position + new Vector3(jitter, 0f, 0f);
        passwordLoginInput.transform.position = newPosition2;
        /* 
                Vector3 newPosition3 = nameRegisterField.transform.position + new Vector3(jitter, 0f, 0f);
                nameRegisterField.transform.position = newPosition3;
                Vector3 newPosition4 = emailRegisterField.transform.position + new Vector3(jitter, 0f, 0f);
                emailRegisterField.transform.position = newPosition4;
                Vector3 newPosition5 = passwordRegisterField.transform.position + new Vector3(jitter, 0f, 0f);
                passwordRegisterField.transform.position = newPosition5;
                Vector3 newPosition6 = confirmPasswordRegisterField.transform.position + new Vector3(jitter, 0f, 0f);
                confirmPasswordRegisterField.transform.position = newPosition6; */
    }
    private void OnEnable()
    {
        StartCoroutine(JitterCheck());
    }

    public void LoginButton()
    {

        var request = new LoginWithEmailAddressRequest
        {
            TitleId = "47EFF",
            Email = emailLoginInput.text,
            Password = passwordLoginInput.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    public void RegisterButton()
    {
        if (usernameRegisterInput.text == "")
        {
            messageText.text = "Username is empty!";
            return;
        }
        else if (emailRegisterInput.text == "")
        {
            messageText.text = "Email is empty!";
            return;
        }
        else if (passwordRegisterInput.text.Length < 6 || confirmPasswordRegisterInput.text.Length < 6)
        {
            messageText.text = "Password length too short!";
            return;
        }
        else if (passwordRegisterInput.text != confirmPasswordRegisterInput.text)
        {
            messageText.text = "Password does not match!";
            return;
        }
        else if (!Regex.IsMatch(emailRegisterInput.text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            messageText.text = "Email not valid!";
            return;
        }
        else
        {
            messageText.text = "Registering...";
        }
        var request = new RegisterPlayFabUserRequest
        {
            Email = emailRegisterInput.text,
            Password = passwordRegisterInput.text,
            DisplayName = usernameRegisterInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnRegisterError);
    }
    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        CreateUserInPlayFab();

        LeaderboardManager.playerEmail = emailRegisterInput.text;

        LeaderboardManager.password = passwordRegisterInput.text;

        LeaderboardManager.Instance.SubmitScore(0);

        SceneManager.LoadScene("Login");

    }


    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = emailLoginInput.text,
            TitleId = "47EFF",
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, forgotPasswordError);
    }

    public void OnPasswordReset(SendAccountRecoveryEmailResult result)
    {
        messageText.color = Color.green;
        messageText.text = "Sent Successfully, please check registered email!";

    }

    void OnLoginSuccess(LoginResult result)
    {
        PlayerPrefs.SetInt("Guest", 1); //0 set to as logged on as guest
        Debug.Log("Login Success");
        messageText.text = "You have successfully logged in!";

        LeaderboardManager.playerEmail = emailLoginInput.text;

        LeaderboardManager.password = passwordLoginInput.text;

        LeaderboardManager.IsLoggedIn = true;

        LeaderboardManager.Instance.GetLeaderboard();

        messageText.text = "Loading Data! Please Wait";
        messageText.color = Color.green;
        Invoke("DelayLoadScene", 2f);
    }
    public void DelayLoadScene()
    {
        messageText.text = "You have successfully logged in!";
        SceneManager.LoadScene("PlayMenu");
    }
    void OnGuestLoginSuccess(LoginResult result)
    {
        Debug.Log("Login Success");
        PlayerPrefs.SetInt("guestpoints", 0);
        messageText.text = "You have successfully logged in!";
        SceneManager.LoadScene("Categories");

    }

    void OnError(PlayFabError error)
    {
        messageText.text = "Error, incorrect email or password!";
        isJitter = true;
        Debug.Log(error.GenerateErrorReport());
    }
    void OnRegisterError(PlayFabError error)
    {
        messageText.text = "Error, please try again";
    }
    void forgotPasswordError(PlayFabError error)
    {
        messageText.color = Color.green;
        messageText.text = "Sent Successfully, please check registered email!";
        Debug.Log(error.GenerateErrorReport());
    }
    public void PlayAsGuest()
    {
        PlayerPrefs.SetInt("Guest", 0); //0 set to as logged on as guest
        var request = new LoginWithCustomIDRequest() { TitleId = "47EFF", CustomId = "714738762171981" };
        PlayFabClientAPI.LoginWithCustomID(request, OnGuestLoginSuccess, OnError);
    }
    public void CreateUserInPlayFab()
    {
        //Create user in database first time they register
        User usertest = new User();
        usertest.Email = emailRegisterInput.text;
        usertest.Username = usernameRegisterInput.text;
        usertest.Level = 0;
        usertest.HintPoints = 0;
        usertest.MazeNumber = 0;
        usertest.Category = "Math"; //default
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {"Email", usertest.Email },
                {"Username", usertest.Username },
                {"HintPoints", usertest.HintPoints.ToString() },
                {"Level", usertest.Level.ToString() },
                {"MazeNumber", usertest.MazeNumber.ToString() },
                {"Category", usertest.Category }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
        usertest.PrintUser();
    }
    void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Successfully sent user data!");
    }
    public void MoveToScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}