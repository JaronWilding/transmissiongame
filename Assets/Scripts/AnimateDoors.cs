using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateDoors : MonoBehaviour
{
    public GameObject DoorA;
    Animator animA;
    private bool doorOpen = false;
    private bool doorAnimating;

    // Start is called before the first frame update
    void Start()
    {
        animA = DoorA.GetComponent<Animator> ();
        doorOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            doorOpen = !doorOpen;
            if(!doorAnimating && doorOpen){
                animA.SetFloat("Speed", 1f);
                animA.Play("DoorA", -1, 0);
            }
            else if (!doorAnimating && !doorOpen)
            {   
                animA.SetFloat("Speed", -1f);
                animA.Play("DoorA", -1, 1);
            }
        }
        //if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("YourAnimationName")) { // Avoid any reload. }
        
        
    }


    IEnumerator OpenDoors(float Speed) 
    {
        doorAnimating = true;
        animA.SetFloat("Speed", Speed);
        animA.Play("DoorA", -1, 0);
        bool animm = true;

        while (!animm) {
            animm = animA.GetCurrentAnimatorStateInfo(0).IsName("DoorA");
            yield return null;
        }
        doorAnimating = false;
	}

    bool AnimatorIsPlaying(Animator ani){
        return ani.GetCurrentAnimatorStateInfo(0).length >
                ani.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }


}
