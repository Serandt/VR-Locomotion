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
            Debug.Log($"------------------------------------------ ENEMY COUNT BEFORE {selectionTaskMeasure.enemiesCount} ---------------------------------------");
            selectionTaskMeasure.enemiesCount--;
            Debug.Log($"------------------------------------------ ENEMY COUNT AFTER {selectionTaskMeasure.enemiesCount} ---------------------------------------");
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
