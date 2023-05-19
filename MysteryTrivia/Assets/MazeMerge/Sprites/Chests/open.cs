using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class open : MonoBehaviour

{
    public GameObject thisChest;
    public Animation chestOpen;
    [SerializeField]
     void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag=="Player"){
        Debug.Log("opening chest");
        chestOpen.Play();
        }
    }
    public void DelayedDestroy(){
        // Destroy(thisChest);
        Debug.Log("destroy chest");
        thisChest.SetActive(false);
        
    }

}
