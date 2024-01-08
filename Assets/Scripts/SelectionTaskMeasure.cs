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

    public GameObject taskStartPanel;
    public GameObject donePanel;
    public TMP_Text startPanelText;
    public TMP_Text scoreText;
    public TMP_Text enemyCounterText;
    public int completeCount;
    public bool isTaskStart;
    public bool isTaskEnd;
    public bool isCountdown;
    public float taskTime;
    public GameObject taskUI;
    public ParkourCounter parkourCounter;
    public DataRecording dataRecording;
    private int part;
    public float partSumTime;
    public float partSumErr;

    public int enemiesCount;
    public GameObject wand;
    public GameObject particles;


    // Start is called before the first frame update
    void Start()
    {
        parkourCounter = this.GetComponent<ParkourCounter>();
        dataRecording = this.GetComponent<DataRecording>();
        part = 1;
        scoreText.text = "Part" + part.ToString();
        enemiesCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTaskStart)
        {
            // recording time
            taskTime += Time.deltaTime;
            enemyCounterText.text = $"Enemies remaining: {enemiesCount}";

            if (enemiesCount == 0)
            {
                enemyCounterText.text = "All enemies defeated!";
                Invoke("EnemyCounterTextReset", 5f);
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
    }

    public void EndOneTask()
    {
        // release
        isTaskEnd = true;
        isTaskStart = false;

        scoreText.text = scoreText.text + "Time: " + taskTime.ToString("F1") + "\n";
        partSumTime += taskTime;
        dataRecording.AddOneData(parkourCounter.locomotionTech.stage.ToString(), completeCount, taskTime);

        // Debug.Log("Time: " + taskTime.ToString("F1") + "\nPrecision: " + manipulationError.magnitude.ToString("F1"));
        Destroy(wand);
        Destroy(particles);
        StartCoroutine(Countdown(3f));
    }

    IEnumerator Countdown(float t)
    {
        taskTime = 0f;
        isCountdown = true;
        completeCount += 1;

        if (completeCount > 4)
        {
            scoreText.text = "Done Part" + part.ToString();
            part += 1;
            completeCount = 0;
        }
        else
        {
            yield return new WaitForSeconds(t);
            isCountdown = false;
        }
        isCountdown = false;
        yield return 0;
    }

    private void EnemyCounterTextReset()
    {
        enemyCounterText.text = "";
    }
}