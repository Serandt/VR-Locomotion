using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public SelectionTaskMeasure selectionTaskMeasure;
    public GameObject wand;
    private bool catched;
    private bool inPortal;
    private float startRotationZ;
    private float startRotationX;

    void Start()
    {
        catched = false;
        inPortal = false;
        startRotationZ = 0f;
    }

    void Update()
    {
        if (catched)
        {
            transform.position = wand.transform.position;
            transform.rotation = wand.transform.rotation;
            transform.Rotate(-90, 0, 0);
        }

        if (inPortal)
        {
            //rotate enemy in portal to defeat it
            if (Mathf.Abs(startRotationZ - wand.GetComponentInParent<Transform>().rotation.z) > 0.4f
                || Mathf.Abs(startRotationX - wand.GetComponentInParent<Transform>().rotation.x) > 0.4f)
            {
                selectionTaskMeasure.enemiesCount--;
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "projectile")
        {
            if (!catched)
            {
                Destroy(other.gameObject);
                gameObject.GetComponent<Animator>().enabled = false;
                catched = true;
                transform.localScale = transform.localScale * 0.2f;
            }
        }

        if (other.gameObject.tag == "portal")
        {
            startRotationZ = wand.GetComponentInParent<Transform>().rotation.z;
            startRotationX = wand.GetComponentInParent<Transform>().rotation.x;
            inPortal = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "portal")
        {
            inPortal = false;
        }
    }
}
