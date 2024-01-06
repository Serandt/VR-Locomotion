using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class LocomotionTechnique : MonoBehaviour
{
    // Please implement your locomotion technique in this script. 
    public OVRInput.Controller leftController;
    public OVRInput.Controller rightController;
    [Range(0, 10)] public float translationGain = 0.5f;
    public GameObject hmd;
    [SerializeField] private float leftTriggerValue;    
    [SerializeField] private float rightTriggerValue;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool isIndexTriggerDown;


    public GameObject player;
    private Rigidbody playerRB;
    private float leaningThreshold;

    //Leaning
    private float hmdStartPositionY;
    private float leaningDistance;
    private float movementSpeed;
    private float maxSpeed;

    //Rotation
    private Quaternion broomStartRotation;
    private float currentRotationY;
    private float currentRotationX;
    private float rotationThreshold;
    private float rotationSpeed;


    /////////////////////////////////////////////////////////
    // These are for the game mechanism.
    public ParkourCounter parkourCounter;
    public string stage;
    public SelectionTaskMeasure selectionTaskMeasure;
    
    void Start()
    {
        hmdStartPositionY = hmd.transform.localPosition.y;
        leaningThreshold = 0.15f;
        movementSpeed = 5f;
        maxSpeed = 20f;
        broomStartRotation = OVRInput.GetLocalControllerRotation(leftController);
        rotationThreshold = 2f;
        rotationSpeed = 1f;
        playerRB = player.GetComponent<Rigidbody>();
        Debug.Log($"--------------------------- START POSITION {hmd.transform.localPosition.y} ------------------------------------");
    }

    private void FixedUpdate()
    {
        MovePlayerForward();
    }

    void Update()
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // Please implement your LOCOMOTION TECHNIQUE in this script :D.
        leftTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, leftController); 
        rightTriggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, rightController);

        //Set start position if zero
        if (hmdStartPositionY == 0)
        {
            hmdStartPositionY = hmd.transform.localPosition.y;
        }

        //Forward movement
        leaningDistance = Mathf.Abs(hmdStartPositionY - hmd.transform.localPosition.y);
        Debug.Log($"--------------------------- DISTANCE {leaningDistance} ------------------------------------");
        Debug.Log($"--------------------------- POSITION {hmd.transform.localPosition.y} ------------------------------------");


        //Left and Right roation
        Debug.Log($"----------------------- Start rotation y={broomStartRotation.y} x={broomStartRotation.x} ------------------");
        currentRotationY = OVRInput.GetLocalControllerRotation(leftController).y;
        Debug.Log($"----------------------- Current rotation y={currentRotationY} ------------------");

        if (currentRotationY > (broomStartRotation.y + rotationThreshold))
        {
            //turn left
            //transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
        }
        else if (currentRotationY < (broomStartRotation.y - rotationThreshold) )
        {
            //turn right
            //transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }

        //Up and Down movement
        currentRotationX = OVRInput.GetLocalControllerRotation(leftController).x;
        Debug.Log($"----------------------- Current rotation x={currentRotationX} ------------------");

        if (currentRotationX > (broomStartRotation.y + rotationThreshold))
        {
            //transform.position += transform.up * Time.deltaTime * movementSpeed;
        }
        else if (currentRotationX < (broomStartRotation.x - rotationThreshold))
        {
            //transform.position += transform.down * Time.deltaTime * movementSpeed;
        }

        ////////////////////////////////////////////////////////////////////////////////
        // These are for the game mechanism.
        if (OVRInput.Get(OVRInput.Button.Two) || OVRInput.Get(OVRInput.Button.Four))
        {
            if (parkourCounter.parkourStart)
            {
                this.transform.position = parkourCounter.currentRespawnPos;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {

        // These are for the game mechanism.
        if (other.CompareTag("banner"))
        {
            stage = other.gameObject.name;
            parkourCounter.isStageChange = true;
        }
        else if (other.CompareTag("objectInteractionTask"))
        {
            selectionTaskMeasure.isTaskStart = true;
            selectionTaskMeasure.scoreText.text = "";
            selectionTaskMeasure.partSumErr = 0f;
            selectionTaskMeasure.partSumTime = 0f;
            // rotation: facing the user's entering direction
            float tempValueY = other.transform.position.y > 0 ? 12 : 0;
            Vector3 tmpTarget = new Vector3(hmd.transform.position.x, tempValueY, hmd.transform.position.z);
            selectionTaskMeasure.taskUI.transform.LookAt(tmpTarget);
            selectionTaskMeasure.taskUI.transform.Rotate(new Vector3(0, 180f, 0));
            selectionTaskMeasure.taskStartPanel.SetActive(true);
        }
        else if (other.CompareTag("coin"))
        {
            parkourCounter.coinCount += 1;
            this.GetComponent<AudioSource>().Play();
            other.gameObject.SetActive(false);
        }
        // These are for the game mechanism.
    }

    void MovePlayerForward()
    {
        if (leaningDistance > leaningThreshold)
        {
            playerRB.AddForce(transform.forward.normalized * movementSpeed, ForceMode.Force);

            //Aus https://discussions.unity.com/t/addforce-and-maximum-speed/27860
            if (playerRB.velocity.magnitude > maxSpeed)
            {
                playerRB.velocity = playerRB.velocity.normalized * maxSpeed;
            }
        }
        else
        {
            playerRB.velocity = playerRB.velocity * 0.97f;
        }
    }
}