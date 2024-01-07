using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBroom : MonoBehaviour
{
    public GameObject controller;

    void Update()
    {
        transform.position = controller.transform.position;
        transform.rotation = controller.transform.rotation;
        transform.Rotate(70, 0, 0);
    }
}
