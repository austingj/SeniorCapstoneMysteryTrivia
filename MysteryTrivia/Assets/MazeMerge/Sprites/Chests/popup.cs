using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popup : MonoBehaviour
{
    public GameObject thisPot;
    public Animation points;
    [SerializeField]
     void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player"){
        Debug.Log("opening chest");
        points.Play();
        }
    }

    public void DelayedDestroy(){
        // Destroy(thisPot);
        thisPot.SetActive(false);
    }
}
