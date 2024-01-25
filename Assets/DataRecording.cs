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
        public float accuracy;

        public ObjectInteractionData(string round, int number, float taskTime, float accuracy)
        {
            this.round = round;
            this.number = number;
            this.taskTime = taskTime;
            this.accuracy = accuracy;
        }
    }
    
    public List<ObjectInteractionData> dataList = new List<ObjectInteractionData>();


    public void AddOneData(string round, int number, float taskTime, float accuracy)
    {
        dataList.Add(new ObjectInteractionData(round, number, taskTime, accuracy));
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
