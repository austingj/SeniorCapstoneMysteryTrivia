using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class audioController : MonoBehaviour
{
    public Button MuteButton;
    public Sprite MuteImage;
    public Sprite FullImage;
    public int muteState=0;
    public float storevolume;
    [SerializeField] Slider volumeSlider;
    void Update()
    {
       if (Input.GetKey(KeyCode.Comma))
        {
            volumeSlider.value=volumeSlider.value-0.001f;
           ChangeVolume();
            Debug.Log("<");
        }if (Input.GetKey(KeyCode.Period))
        {
            volumeSlider.value=volumeSlider.value+0.001f;
            ChangeVolume();
            Debug.Log(">");
        }
        if(muteState==1){
             volumeSlider.value=0;
            AudioListener.volume = volumeSlider.value;
            
        }

    }
    void Start(){
       volumeSlider.value= PlayerPrefs.GetFloat("preferedVolume");
       storevolume=PlayerPrefs.GetFloat("FrozenVolume");
       AudioListener.volume = volumeSlider.value;
        muteState=  PlayerPrefs.GetInt("volumeMute");
        if(muteState==1){
            MuteButton.GetComponent<Image>().sprite=MuteImage;
            
            }
    }

   public void ChangeVolume()
    {
       AudioListener.volume = volumeSlider.value;
       PlayerPrefs.SetFloat("preferedVolume",AudioListener.volume);
    }
    public void MuteToggle(){
        if(muteState==0){
            storevolume=volumeSlider.value;
            PlayerPrefs.SetFloat("FrozenVolume",AudioListener.volume);
            volumeSlider.value=0;
            AudioListener.volume = volumeSlider.value;
            muteState=1;
            MuteButton.GetComponent<Image>().sprite=MuteImage;
            PlayerPrefs.SetInt("volumeMute",1);
    }
    else if(muteState==1){
        volumeSlider.value=storevolume;
            AudioListener.volume = volumeSlider.value;
            muteState=0;
             MuteButton.GetComponent<Image>().sprite=FullImage;
            PlayerPrefs.SetInt("volumeMute",0);
    }
    }

}