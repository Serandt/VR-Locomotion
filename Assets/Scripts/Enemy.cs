using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public SelectionTaskMeasure selectionTaskMeasure;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "projectile")
        {
            selectionTaskMeasure.enemiesCount--;
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
