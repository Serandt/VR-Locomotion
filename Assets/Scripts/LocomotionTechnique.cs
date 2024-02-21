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

    private bool moving;
    private bool grounded;
    private bool locomotionEnabled;

    public GameObject player;
    private Rigidbody playerRB;
    private float leaningThreshold;

    //Leaning
    private float hmdStartPositionY;
    private float leaningDistance;
    private float movementSpeed;
    private float maxSpeed;

    //Broom
    private float broomControllerStartY;
    private float broomControllerStartX;
    private float currentPositionY;
    private float currentPositionX;
    private float currentControllerDistanceY;
    private float startControllerDistanceY;
    private float rotationThreshold;
    private float rotationSpeed;
    private float elevationThreshold;
    private float elevationSpeed;

    //after fix
    private bool PositionSetted;

    /////////////////////////////////////////////////////////
    // These are for the game mechanism.
    public ParkourCounter parkourCounter;
    public string stage;
    public SelectionTaskMeasure selectionTaskMeasure;
    
    void Start()
    {
        hmdStartPositionY = hmd.transform.localPosition.y;
        leaningThreshold = 0.15f;
        movementSpeed = 1f;
        maxSpeed = 5f;
        rotationThreshold = 0.05f;
        rotationSpeed = 75f;
        elevationThreshold = 0.05f;
        elevationSpeed = 2f;
        playerRB = player.GetComponent<Rigidbody>();
        moving = false;
        PositionSetted = false;
    }

    private void FixedUpdate()
    {
        MovePlayerForward();
        MovePlayerVertically();
        MovePlayerHorizontally();
    }

    void Update()
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        // Please implement your LOCOMOTION TECHNIQUE in this script :D.

        //Set start position for hmd and broom
        if (!PositionSetted && hmd.transform.localPosition.y > 0.5f)
        {
            PositionSetted = true;
            SetMovementVariableValues();
        }

        //Forward movement
        leaningDistance = Mathf.Abs(hmdStartPositionY - hmd.transform.localPosition.y);

        //Left and Right movement
        currentPositionX = OVRInput.GetLocalControllerPosition(leftController).x;

        //Up and Down movement
        currentPositionY = OVRInput.GetLocalControllerPosition(leftController).y;
        currentControllerDistanceY = Mathf.Abs(hmd.transform.localPosition.y - currentPositionY);

        ////////////////////////////////////////////////////////////////////////////////
        // These are for the game mechanism.
        if (OVRInput.Get(OVRInput.Button.Two) || OVRInput.Get(OVRInput.Button.Four))
        {
            if (parkourCounter.parkourStart)
            {
                player.transform.position = parkourCounter.currentRespawnPos;
            }
        }
    }

    private void SetMovementVariableValues()
    {
        hmdStartPositionY = hmd.transform.localPosition.y;
        broomControllerStartX = OVRInput.GetLocalControllerPosition(leftController).x;
        broomControllerStartY = OVRInput.GetLocalControllerPosition(leftController).y;
        startControllerDistanceY = Mathf.Abs(hmdStartPositionY - broomControllerStartY);
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
            selectionTaskMeasure.partSumErr = 0f;
            selectionTaskMeasure.partSumTime = 0f;
            float tempValueY = other.transform.position.y > 0 ? 12 : 0;
            Vector3 tmpTarget = new Vector3(hmd.transform.position.x, tempValueY, hmd.transform.position.z);
            selectionTaskMeasure.taskUI.transform.LookAt(tmpTarget);
            selectionTaskMeasure.taskUI.transform.Rotate(new Vector3(0, 180f, 0));
        }
        else if (other.CompareTag("coin"))
        {
            parkourCounter.coinCount += 1;
            this.GetComponent<AudioSource>().Play();
            other.gameObject.SetActive(false);
        }
        
        if (other.CompareTag("ground"))
        {
            grounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ground"))
        {
            grounded = false;
        }
    }

    void MovePlayerForward()
    {
        if (leaningDistance > leaningThreshold)
        {
            playerRB.AddForce(transform.forward.normalized * movementSpeed, ForceMode.VelocityChange);

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

    void MovePlayerHorizontally()
    {
        if (currentPositionX > (broomControllerStartX + rotationThreshold))
        {
            //turn left
            player.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        else if (currentPositionX < (broomControllerStartX - rotationThreshold))
        {
            //turn right
            player.transform.Rotate(-Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }

    void MovePlayerVertically()
    {
        if (currentControllerDistanceY < (startControllerDistanceY - elevationThreshold) || grounded)
        {
            //up
            player.transform.position += transform.up * Time.deltaTime * elevationSpeed;
        }
        else if (currentControllerDistanceY > (startControllerDistanceY + elevationThreshold) && !grounded)
        {
            //down
            player.transform.position -= transform.up * Time.deltaTime * elevationSpeed;
        }
    }
}