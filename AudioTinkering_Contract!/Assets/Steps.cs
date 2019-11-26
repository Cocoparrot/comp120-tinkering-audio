using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steps : MonoBehaviour
{
    public float stepCoolDown;
    public float stepRate = 0.5f;
    public AudioSource stepSound;

    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        stepCoolDown -= Time.deltaTime;

        if ((Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f) && stepCoolDown < 0f)
        {
            stepSound.pitch = Random.Range(0.6f, 1f);
            stepSound.volume = Random.Range(0.6f, 1.1f);
            stepSound.Play();
            stepCoolDown = stepRate;
        }
    }
}
