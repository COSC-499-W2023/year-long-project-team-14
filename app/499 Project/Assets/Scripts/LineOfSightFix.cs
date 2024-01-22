using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightFix : MonoBehaviour
{
    public PlayerController playerController;

    //Detects if players shoot position is inside a wall
    //Prevents line renderer from clipping through objects and bullets from spawning inside solid objects
    void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.CompareTag("Wall"))
        {
            playerController.aimingInWall = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if(collider.gameObject.CompareTag("Wall"))
        {
            playerController.aimingInWall = false;
        }
    }
}
