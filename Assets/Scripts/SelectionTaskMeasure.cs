using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SelectionTaskMeasure : MonoBehaviour
{
    public GameObject targerT;
    public GameObject targerTPrefab;
    Vector3 targetTStartingPos;
    public GameObject objectT;
    public GameObject objectTPrefab;
    Vector3 objectTStartingPos;
    public GameObject portal;

    public GameObject taskStartPanel;
    public GameObject donePanel;
    public TMP_Text startPanelText;
    public TMP_Text scoreText;
    public TMP_Text enemyCounterText;
    public int completeCount;
    public bool isTaskStart;
    public bool isTaskEnd;
    public bool isCountdown;
    public bool isWandAttached;
    public float taskTime;
    public float defeateEnemeyTime;
    public GameObject taskUI;
    public ParkourCounter parkourCounter;
    public DataRecording dataRecording;
    public int part;
    public float partSumTime;
    public float partSumErr;

    public int enemiesCount;
    private int enemiesInRound;
    public GameObject wand;
    public GameObject particles;

    private float accuracy;
    public int projectilesCount;
    public int projectilesUsedForPreviousEnemies;

    // Start is called before the first frame update
    void Start()
    {
        parkourCounter = this.GetComponent<ParkourCounter>();
        dataRecording = this.GetComponent<DataRecording>();
        part = 1;
        scoreText.text = "Part " + part.ToString();
        enemiesCount = 10;
        accuracy = 0;
        projectilesCount = 0;
        projectilesUsedForPreviousEnemies = 0;
        isWandAttached = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTaskStart)
        {
            // recording time
            taskTime += Time.deltaTime;
            defeateEnemeyTime += Time.deltaTime;
            enemyCounterText.text = $"Enemies remaining: {enemiesCount}";

            if (enemiesCount == 0 && isWandAttached)
            {
                // release
                isTaskEnd = true;
                isTaskStart = false;

                EndOneTask();
            }
        }

        if (isCountdown)
        {
            taskTime += Time.deltaTime;
        }
    }

    public void StartOneTask()
    {
        taskTime = 0f;
        defeateEnemeyTime = 0f;
        projectilesCount = 0;
        portal.SetActive(true);
        enemiesInRound = enemiesCount;
    }

    public void EndOneTask()
    {
        accuracy = (enemiesInRound * 100 / projectilesCount);
        scoreText.text = scoreText.text + "Done Part " + part.ToString() + "! Time: " + taskTime.ToString("F1") + ", Accuracy: " + $"Enemies={enemiesInRound}/Projectiles={projectilesCount} ({accuracy}%)" + "\n";
        parkourCounter.accuracy = accuracy;
        partSumTime += taskTime;
        dataRecording.AddOneData(parkourCounter.locomotionTech.stage.ToString(), completeCount, taskTime, accuracy);

        // Debug.Log("Time: " + taskTime.ToString("F1") + "\nPrecision: " + manipulationError.magnitude.ToString("F1"));
        portal.SetActive(false);
        Destroy(wand);
        isWandAttached = false;
        Destroy(particles);

        completeCount = 0;

        defeateEnemeyTime = 0f;
        projectilesUsedForPreviousEnemies = 0;

        enemyCounterText.text = "All enemies defeated!";
        Invoke("EnemyCounterTextReset", 3f);
    }

    public void EnemyDefeated()
    {
        scoreText.text = scoreText.text + "Time: " + defeateEnemeyTime.ToString("F1") + $", Projectiles used: {projectilesCount - projectilesUsedForPreviousEnemies}" + "\n";
        defeateEnemeyTime = 0f;
        projectilesUsedForPreviousEnemies = projectilesCount;
    }

    private void EnemyCounterTextReset()
    {
        enemyCounterText.text = "";
    }
}