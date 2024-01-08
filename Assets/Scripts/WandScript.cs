using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandScript : MonoBehaviour
{
    public OVRInput.Controller controllerInput;
    public GameObject rightController;
    public GameObject enemies;
    public SelectionTaskMeasure selectionTaskMeasure;
    public int enemiesCount;

    private float triggerValue;

    public GameObject wand;
    public GameObject projectile;

    public float projectVelocity = 700f;
    public float projectileCoolDown;

    private bool canShoot;
    private bool attached;

    // Start is called before the first frame update
    void Start()
    {
        canShoot = true;
        attached = false;
    }

    // Update is called once per frame
    void Update()
    {
        triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controllerInput);

        if (attached)
        {
            if (triggerValue > 0.95f && canShoot)
            {
                GameObject attack = Instantiate(projectile, transform.position, transform.rotation);
                attack.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, projectVelocity, 0));
                canShoot = false;
                Invoke("CoolDown", 1f);
            }

            SetTransformToController();
        }
    }

    void CoolDown()
    {
        canShoot = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "rightController")
        {
            wand.GetComponent<Animator>().enabled = false;
            attached = true;
            selectionTaskMeasure.enemiesCount = enemiesCount;
            selectionTaskMeasure.wand = wand;
            selectionTaskMeasure.isTaskStart = true;
            enemies.SetActive(true);
            selectionTaskMeasure.StartOneTask();
        }
    }

    void SetTransformToController()
    {
        wand.transform.position = rightController.transform.position;
        wand.transform.rotation = rightController.transform.rotation;
        wand.transform.Rotate(40, 0, 0);
    }
}
