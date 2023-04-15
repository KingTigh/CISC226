using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GearFactory;

public class TimeController : MonoBehaviour
{   
    public static float gravity = -100;

    public struct RecordedData
    {
        public Vector2 pos;
        public Vector2 vel;
        public float animationTime;
    }

    RecordedData[,] independantRecordedData;
    RecordedData[,] recordedData; 
    int recordMax = 100000; 
    int recordCount; 
    int recordIndex; 
    bool wasRewinding = false;

    TimeControlled[] timeObjects;
    GearBase[] timeGears;
    
    private JointMotor2D hjm;

    private void Awake() 
    {
        timeGears = GameObject.FindObjectsOfType<GearBase>();
        timeObjects = GameObject.FindObjectsOfType<TimeControlled>();
        recordedData = new RecordedData[timeObjects.Length,recordMax];

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool pause = Input.GetKey(KeyCode.UpArrow);
        bool fforward = Input.GetKey(KeyCode.RightArrow);
        bool rewind = Input.GetKey(KeyCode.LeftArrow);

        if (rewind) 
        {   
            wasRewinding = true;

            if (recordIndex > 0)
            {
                recordIndex --; 

                for (int objectIndex = 0; objectIndex < timeObjects.Length; objectIndex++)
                {
                    TimeControlled timeObject = timeObjects[objectIndex];
                    RecordedData data = recordedData[objectIndex, recordIndex];
                    timeObject.transform.position = data.pos;
                    timeObject.velocity = data.vel; 
                    timeObject.animationTime = data.animationTime;
        
                    timeObject.updateAnimation();
                }
                for (int objectIndex = 0; objectIndex < timeGears.Length; objectIndex++)
                {
                    GearBase timeGear = timeGears[objectIndex];
                    hjm = timeGear.GetComponent<HingeJoint2D>().motor;
                    hjm.motorSpeed = -1 * timeGear.GetComponent<Gear>().speed;
                    timeGear.GetComponent<HingeJoint2D>().motor = hjm;
                }
            }
        }
        else if (pause && !fforward && !rewind)
        {
            for (int objectIndex = 0; objectIndex < timeGears.Length; objectIndex++)
            {
                GearBase timeGear = timeGears[objectIndex];
                hjm = timeGear.GetComponent<HingeJoint2D>().motor;
                hjm.motorSpeed = 0;
                timeGear.GetComponent<HingeJoint2D>().motor = hjm;
            }
        }
        else if (pause && fforward) 
        {
            wasRewinding = true;

            if(recordIndex < recordCount -1)
            {
                recordIndex++;

                for (int objectIndex = 0; objectIndex < timeObjects.Length; objectIndex++)
                {
                    TimeControlled timeObject = timeObjects[objectIndex];
                    RecordedData data = recordedData[objectIndex, recordIndex];
                    timeObject.transform.position = data.pos;
                    timeObject.velocity = data.vel; 
                    timeObject.animationTime = data.animationTime;

                    timeObject.updateAnimation();
                }
                for (int objectIndex = 0; objectIndex < timeGears.Length; objectIndex++)
                {
                    GearBase timeGear = timeGears[objectIndex];
                    hjm = timeGear.GetComponent<HingeJoint2D>().motor;
                    hjm.motorSpeed = timeGear.GetComponent<Gear>().speed;
                    timeGear.GetComponent<HingeJoint2D>().motor = hjm;
                }
            }
        }
        else if (!pause && fforward)
        {
            if (wasRewinding)
            {
                recordCount = recordIndex;
                wasRewinding = false;
            }

            for (int objectIndex = 0; objectIndex < timeObjects.Length; objectIndex++)
            {
                TimeControlled timeObject = timeObjects[objectIndex];
                RecordedData data = new RecordedData();
                data.pos = timeObject.transform.position;
                data.vel = timeObject.velocity;
                data.animationTime = timeObject.animationTime;
                recordedData[objectIndex, recordCount] = data;
                timeObject.speedMultiplier = 3;
            }
            for (int objectIndex = 0; objectIndex < timeGears.Length; objectIndex++)
            {
                GearBase timeGear = timeGears[objectIndex];
                hjm = timeGear.GetComponent<HingeJoint2D>().motor;
                hjm.motorSpeed = 3 * timeGear.GetComponent<Gear>().speed;
                timeGear.GetComponent<HingeJoint2D>().motor = hjm;
            }

            recordCount++;
            recordIndex = recordCount; 

            foreach(TimeControlled timeObject in timeObjects)
            {
                timeObject.TimeUpdate();
                timeObject.updateAnimation();
            }
        }
        else if (!pause && !rewind && !fforward)
        {      
            if (wasRewinding)
            {
                recordCount = recordIndex;
                wasRewinding = false;
            }

            for (int objectIndex = 0; objectIndex < timeObjects.Length; objectIndex++)
            {
                TimeControlled timeObject = timeObjects[objectIndex];
                RecordedData data = new RecordedData();
                data.pos = timeObject.transform.position;
                data.vel = timeObject.velocity;
                data.animationTime = timeObject.animationTime;
                recordedData[objectIndex, recordCount] = data;
                timeObject.speedMultiplier = 1;
            }
            for (int objectIndex = 0; objectIndex < timeGears.Length; objectIndex++)
            {
                GearBase timeGear = timeGears[objectIndex];
                hjm = timeGear.GetComponent<HingeJoint2D>().motor;
                hjm.motorSpeed = timeGear.GetComponent<Gear>().speed;
                timeGear.GetComponent<HingeJoint2D>().motor = hjm;
            }

            recordCount++;
            recordIndex = recordCount; 

            foreach(TimeControlled timeObject in timeObjects)
            {
                timeObject.TimeUpdate();
                timeObject.updateAnimation();
            }
        }

        }

    
}
