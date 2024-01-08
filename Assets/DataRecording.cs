using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataRecording : MonoBehaviour
{
    // ParkourCounter parkourCounter;
    // SelectionTaskMeasure selectionTaskMeasure;

    public struct ObjectInteractionData
    {
        public string round;
        public int number;
        public float taskTime;

        public ObjectInteractionData(string round, int number, float taskTime)
        {
            this.round = round;
            this.number = number;
            this.taskTime = taskTime;
        }
    }
    
    public List<ObjectInteractionData> dataList = new List<ObjectInteractionData>();


    public void AddOneData(string round, int number, float taskTime)
    {
        dataList.Add(new ObjectInteractionData(round, number, taskTime));
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
