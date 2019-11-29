using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour : MonoBehaviour
{
    //Initialise the audiosource we want to trigger
    public AudioClip collectSound;

   
    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.gameObject.tag != "Collectible")
        {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
            Destroy(gameObject);
        }

    }

}
