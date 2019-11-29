using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steps : MonoBehaviour
{


    public float stepCoolDown; //initialise the stepCooldown variable we will use to keep track of the actual steps made
    public float stepRate = 0.5f; // The variable we want to use to define the length of each footstep
    public AudioSource stepSound; //Reference to the audiosource which we want to play.

    //Declare the minimum and maximum range of the randomizer for the pitch and volume.
    public float pitchRangeMin = 0.6f;
    public float pitchRangeMax = 1.1f;
    public float volumeRangeMin = 0.6f;
    public float volumeRangeMax = 1.1f;

    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        //make the timing negative so we can reset to 0.
        stepCoolDown -= Time.deltaTime;

        /*If case to make sure the character is actually moving before playing the sound.
        *We use the cooldown to make sure the sounds are not overlapping each other.
        * We use 2 random values voor the pitch and volume so every step sounds different
        * We can use the input.GetAxis function to track movement since we have a charactercontroller on the object we are moving easier than to calculate every step
        */

        if ((Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) && stepCoolDown < 0f)
        {
            stepSound.pitch = Random.Range(pitchRangeMin, pitchRangeMax);
            stepSound.volume = Random.Range(volumeRangeMin, volumeRangeMax);
            stepSound.Play();
            stepCoolDown = stepRate; //reset the timer everytime a new step is made, so we actually have different steps.
        }
    }
}
