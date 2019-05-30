using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnce : MonoBehaviour
{
    public AudioClip audioLog;
    AudioSource audioSource;
    private bool hasPlayed = false;
    private BoxCollider boxCol;
    

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        boxCol = GetComponent<BoxCollider>();

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerStay(Collider col)
    {
        if (col.tag == "GameController" && Input.GetKeyDown(KeyCode.F))
        {

            audioSource.PlayOneShot(audioLog);
            boxCol.enabled =false;
            //   hasPlayed = true;
            Destroy(gameObject, audioSource.clip.length);

        }
    }
}
