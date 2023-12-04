using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineOfSightFix : MonoBehaviour
{
    public PlayerController playerController;

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
