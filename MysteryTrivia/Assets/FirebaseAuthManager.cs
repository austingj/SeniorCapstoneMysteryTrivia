/* using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class FirebaseAuthManager : MonoBehaviour
{


    // Firebase variable
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public static FirebaseAuth auth;
    public static FirebaseUser user;
    public GameObject Guest;

    // Login Variables
    [Space]
    [Header("Login")]
    public TMP_InputField userLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text confirmText;
    public string stringConfirmText;

    //pass to next scene variables
    static string usn = "";

    // Registration Variables
    [Space]
    [Header("Registration")]
    public TMP_InputField nameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField confirmPasswordRegisterField;
    public TMP_Text warningLoginText;
    public string warningTextString;
    public TMP_Text registerText;
    public string stringRegisterText;

    private void Awake()
    {
        if (auth != null)
        {
            return;
        }
        // Check that all of the necessary dependencies for firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all firebase dependencies: " + dependencyStatus);
            }
        });
    }
    void InitializeFirebase()
    {
        //Set the default instance object
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    /*     private float timer;
        bool isMoving;
        public void Update()
        {
            if (isMoving == false) return;

            timer += Time.deltaTime;

            //Do jitter for 1.5 seconds
            if (timer > 1.5f)
            {
                //Reset position here too

                isMoving = false;
                timer = 0;
                return;
            }

            float jitter = Mathf.Sin(Time.time * 10f) * 0.01f; // Change the values as needed
            Vector3 newPosition = userLoginField.transform.position + new Vector3(jitter, 0f, 0f);
        }
     */

/*public bool isJitter;
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
}*/

/*     public void Jitter()
    {
        float jitter = Mathf.Sin(Time.time * 50f) * 0.20f; // Change the values as needed

        Vector3 newPosition = userLoginField.transform.position + new Vector3(jitter, 0f, 0f);
        userLoginField.transform.position = newPosition;

        Vector3 newPosition2 = passwordLoginField.transform.position + new Vector3(jitter, 0f, 0f);
        passwordLoginField.transform.position = newPosition2;
        
                Vector3 newPosition3 = nameRegisterField.transform.position + new Vector3(jitter, 0f, 0f);
                nameRegisterField.transform.position = newPosition3;

                Vector3 newPosition4 = emailRegisterField.transform.position + new Vector3(jitter, 0f, 0f);
                emailRegisterField.transform.position = newPosition4;

                Vector3 newPosition5 = passwordRegisterField.transform.position + new Vector3(jitter, 0f, 0f);
                passwordRegisterField.transform.position = newPosition5;

                Vector3 newPosition6 = confirmPasswordRegisterField.transform.position + new Vector3(jitter, 0f, 0f);
                confirmPasswordRegisterField.transform.position = newPosition6; */
/*  } */

/*  private void OnEnable()
 {
     StartCoroutine(JitterCheck());
 }
*/

/*     IEnumerator TimedJitter()
    {
        var time = 0.4f;
        while (time > 0)
        {
            Jitter();
            time -= Time.deltaTime;
            if (time < 0)
            {
                yield break;
            }
            yield return null;

        }
    } */


/*     public void registerSuccess()
    {
        stringRegisterText = "Registration successful!";
        registerText.text = stringRegisterText;
    }
    
    public void MoveToScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }




    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }

            user = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);


            }
        }
    }

    public void Login()
    {
        StartCoroutine(LoginAsync(userLoginField.text, passwordLoginField.text));
    }

    private IEnumerator LoginAsync(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => loginTask.IsCompleted);


        if (loginTask.Exception != null)
        {
            Debug.LogError(loginTask.Exception);

            FirebaseException firebaseException = loginTask.Exception.GetBaseException() as FirebaseException;
            AuthError authError = (AuthError)firebaseException.ErrorCode;


            string failedMessage = "Login failed, ";

            switch (authError)
            {
                case AuthError.InvalidEmail:
                    failedMessage += "email is invalid!";
                    warningLoginText.text = failedMessage;
                    isJitter = true;
                    break;
                case AuthError.WrongPassword:
                    failedMessage += "wrong password!";
                    warningLoginText.text = failedMessage;
                    isJitter = true;
                    break;
                case AuthError.MissingEmail:
                    failedMessage += "email is missing!";
                    warningLoginText.text = failedMessage;
                    isJitter = true;
                    break;
                case AuthError.MissingPassword:
                    failedMessage += "password is missing!";
                    warningLoginText.text = failedMessage;
                    isJitter = true;
                    break;
                default:
                    failedMessage = "Try again!";
                    warningLoginText.text = failedMessage;
                    isJitter = true;
                    break;
            }
            Debug.Log(failedMessage);
            warningLoginText.text = failedMessage;
        }
        else
        {
            user = loginTask.Result;
            Debug.LogFormat("{0} You are successfully logged in", user.DisplayName);
            savelistuser(user.Email, user.DisplayName);
            SceneManager.LoadScene("Categories");
            stringConfirmText = "Successfully logged in!";
            //confirmText.text = stringConfirmText;
            isJitter = false;
            //DisableText();
            //References.userName = user.DisplayName;
            //UnityEngine.SceneManagement.SceneManager.LoadScene(2);

        }
    }

    public void savelistuser(string email, string listusername)
    {
        PlayerPrefs.SetString("email", email);
        PlayerPrefs.SetString("usn", listusername);

    }
    public void listuser(string email)
    {
        usn = PlayerPrefs.GetString("usn");
        email = PlayerPrefs.GetString("email");
        Debug.Log("user is still : " + usn + " with email : " + email);
    }



    public void resetPass()
    {
        if (string.IsNullOrEmpty(emailRegisterField.text))
        {
            warningTextString = "Error, input field is empty!";
            warningLoginText.text = warningTextString;
            warningLoginText.color = Color.red;
        }



        resetPasswordSubmit(emailRegisterField.text);
    }

    public void resetPasswordSubmit(string resetPasswordEmail)
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SendPasswordResetEmailAsync(resetPasswordEmail).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SendPasswordResetEmailAsync was canceled");
                warningTextString = "Error, password sent canceled!";
                warningLoginText.text = warningTextString;
                warningLoginText.color = Color.red;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                warningTextString = "Error, check email and try again!";
                warningLoginText.text = warningTextString;
                warningLoginText.color = Color.red;
                return;
            }
            if (task.IsCompletedSuccessfully)
            {
                warningTextString = "Sent Successfully, please check your email!";
                warningLoginText.text = warningTextString;
                warningLoginText.color = Color.green;

            }
        });
    }


    public void Register()
    {
        StartCoroutine(RegisterAsync(nameRegisterField.text, emailRegisterField.text, passwordRegisterField.text, confirmPasswordRegisterField.text));
    }
    private IEnumerator RegisterAsync(string name, string email, string password, string confirmPassword)
    {
        if (name == "")
        {
            Debug.LogError("Username is empty!");
            warningTextString = ("Username is empty!");
            warningLoginText.text = warningTextString;
            //isJitter = true;
        }
        else if (email == "")
        {
            Debug.LogError("Email field is empty!");
            warningTextString = ("Email field is empty!");
            warningLoginText.text = warningTextString;
            //isJitter = true;
        }
        else if (passwordRegisterField.text != confirmPasswordRegisterField.text)
        {
            Debug.LogError("Password does not match!");
            warningTextString = ("Password does not match!");
            warningLoginText.text = warningTextString;
            //isJitter = true;
        }
        else
        {
            var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

            yield return new WaitUntil(() => registerTask.IsCompleted);


            if (registerTask.Exception != null)
            {
                Debug.LogError(registerTask.Exception);

                FirebaseException firebaseException = registerTask.Exception.GetBaseException() as FirebaseException;
                AuthError authError = (AuthError)firebaseException.ErrorCode;

                string failedMessage = "Registration failed,  ";
                switch (authError)
                {
                    case AuthError.InvalidEmail:
                        failedMessage += "email is invalid!";
                        warningLoginText.text = failedMessage;
                        //isJitter = true;
                        break;
                    case AuthError.WrongPassword:
                        failedMessage += "wrong Password!";
                        warningLoginText.text = failedMessage;
                        //isJitter = true;
                        break;
                    case AuthError.MissingEmail:
                        failedMessage += "email is missing!";
                        warningLoginText.text = failedMessage;
                        //isJitter = true;
                        break;
                    case AuthError.MissingPassword:
                        failedMessage += "password is missing!";
                        warningLoginText.text = failedMessage;
                        //isJitter = true;
                        break;
                    default:
                        failedMessage = "registration failed!";
                        //SceneManager.LoadScene("Register");
                        //isJitter = true;
                        break;
                }

                Debug.Log(failedMessage);
                warningLoginText.text = failedMessage;
                //isJitter = true;
            }
            else
            {
                // Get The User After Registration Success
                user = registerTask.Result;

                UserProfile userProfile = new UserProfile { DisplayName = name };

                var updateProfileTask = user.UpdateUserProfileAsync(userProfile);

                yield return new WaitUntil(() => updateProfileTask.IsCompleted);

                if (updateProfileTask.Exception != null)
                {
                    // Delete the user if user update failed
                    user.DeleteAsync();

                    Debug.LogError(updateProfileTask.Exception);

                    FirebaseException firebaseException = updateProfileTask.Exception.GetBaseException() as FirebaseException;
                    AuthError authError = (AuthError)firebaseException.ErrorCode;


                    string failedMessage = "Profile update failed, ";
                    switch (authError)
                    {
                        case AuthError.InvalidEmail:
                            failedMessage += "email is invalid";
                            warningLoginText.text = failedMessage;
                            break;
                        case AuthError.WrongPassword:
                            failedMessage += "wrong password";
                            warningLoginText.text = failedMessage;
                            break;
                        case AuthError.MissingEmail:
                            failedMessage += "email is missing";
                            warningLoginText.text = failedMessage;
                            break;
                        case AuthError.MissingPassword:
                            failedMessage += "password is missing";
                            warningLoginText.text = failedMessage;
                            break;
                        default:
                            failedMessage = "Profile update failed";
                            warningLoginText.text = failedMessage;
                            break;
                    }

                    Debug.Log(failedMessage);
                    warningLoginText.text = failedMessage;

                }
                else
                {
                    Debug.Log("Registration sucessful welcome " + user.DisplayName);
                    SceneManager.LoadScene("Login");
                }
            }
        }
    }
} */