using System.Collections.Generic;
using UnityEngine;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;
using System;
using System.Linq;

public class MovementRecognizer : MonoBehaviour
{
    [SerializeField] float newPositionThreshholdDistance = 0.05f;
    [SerializeField] GameObject drawObject;
    [SerializeField] float recognitionThreshold;
    /// <summary>
    /// the timer the scripts waits untill it has detirmined that you have finished a gesture
    /// </summary>
    [SerializeField] float gestureFinishTimer = 1.5f;
    [SerializeField] bool creationMode = false;
    [SerializeField] bool creationIsLetter = false;
    /// <summary>
    /// all letters need to start with the letter used.
    /// for example "a" and the next will be "a2"
    /// </summary>
    [SerializeField] string newGestureName = "";
    
    [SerializeField] GameObject gestureDisplayParent;


    public delegate void GestureOutput(string gesture);
    public event GestureOutput gestureOutput;

    private bool isButtonDown = false;
    private bool isMoving = false;
    private bool gestureIsBusy = false;
    private int gestureStroke = 0;
    private float gestureTimer = 0;

    private Transform movementSource;

    private List<Gesture> trainingSet = new List<Gesture>();
    private List<Vector3> positionList = new List<Vector3>();
    private List<Tuple<Vector3, int>> gesturePositions = new List<Tuple<Vector3, int>>();

    private string persistantGestureFilePath;
    private string localGestureFilePath;

    // Start is called before the first frame update
    public void Start()
    {
        persistantGestureFilePath = Application.persistentDataPath + "/gestures";
        localGestureFilePath = Application.streamingAssetsPath + "/gestures";

        // check if Persistent file paths are present
        if (!Directory.Exists(persistantGestureFilePath))
        {
            Debug.Log(persistantGestureFilePath + " not found, creating new folder");
            Directory.CreateDirectory(persistantGestureFilePath);
        }
        if (!Directory.Exists(persistantGestureFilePath + "/other"))
        {
            Debug.Log(persistantGestureFilePath + "/other not found, creating new folder");
            Directory.CreateDirectory(persistantGestureFilePath + "/other");
        }
        if (!Directory.Exists(persistantGestureFilePath + "/letters"))
        {
            Debug.Log(persistantGestureFilePath + "/letters not found, creating new folder");
            Directory.CreateDirectory(persistantGestureFilePath + "/letters");
        }

        Debug.Log("Loading Gesture Trainingset from (" + persistantGestureFilePath + ") and (" + localGestureFilePath + ")");

        //read all file names in persistent and local folders
        string[] otherGestureFiles = Directory.GetFiles(persistantGestureFilePath + "/other/", "*.xml");
        string[] letterGestureFiles = Directory.GetFiles(persistantGestureFilePath + "/letters/", "*.xml");
        string[] localOtherGestureFiles = null;
        string[] localLetterGestureFiles = null;
        if (Directory.Exists(localGestureFilePath + "/other"))
            localOtherGestureFiles = Directory.GetFiles(localGestureFilePath + "/other/", "*.xml");
        if (Directory.Exists(localGestureFilePath + "/letters"))
            localLetterGestureFiles = Directory.GetFiles(localGestureFilePath + "/letters/", "*.xml");


        // read all files to the trainingset
        foreach (var item in otherGestureFiles)
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));

        foreach (var item in letterGestureFiles)
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));

        if (localOtherGestureFiles != null) foreach (var item in localOtherGestureFiles)
                trainingSet.Add(GestureIO.ReadGestureFromFile(item));

        if (localLetterGestureFiles != null) foreach (var item in localLetterGestureFiles)
                trainingSet.Add(GestureIO.ReadGestureFromFile(item));

        Debug.Log("Loaded " + trainingSet.Count + " Gestures to the training set");
    }

    // Update is called once per frame
    void Update()
    {
        // start the movement
        if (!isMoving && isButtonDown)
        {
            StartMovement();
            // also start the gesture
            if (!gestureIsBusy)
            {
                StartGesture();
            }
        }
        // stop the movement
        else if (isMoving && !isButtonDown)
        {
            EndMovement();
        }
        // updating the movement
        else if (isMoving && isButtonDown)
        {
            UpdateMovement();
        }
        // update the gesture timer that ends the gesture
        else if (gestureIsBusy && !isMoving && !isButtonDown)
        {
            UpdateGestureTimer();
        }
    }

    public void SetButtonDown(bool value) { isButtonDown = value; }
    public void SetMovementSource(Transform source) { movementSource = source; }
    public void RemoveMovementSource() { movementSource = null; }

    void StartGesture()
    {
        Debug.Log("Start Gesture");
        gestureIsBusy = true;
        gestureStroke = 0;
        gesturePositions.Clear();
    }

    void EndGesture()
    {
        Debug.Log("End Gesture");
        gestureIsBusy = false;

        Point[] pointArray = new Point[gesturePositions.Count];
        float totalDist = 0;
        int lenthCount = 0;
        // read all positions from the position list to a point array
        // converts from 3d space to 2d screenspace
        for (int i = 0; i < pointArray.Length; i++)
        {
            Vector3 position = gesturePositions[i].Item1;
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(new Vector3(position.x, position.y, position.z));
            pointArray[i] = new Point(screenPosition.x, screenPosition.y, gesturePositions[i].Item2);

            
            if (i > 0)
            {
                if (gesturePositions[i - 1].Item2 == gesturePositions[i].Item2)
                {
                    totalDist += (position - gesturePositions[i - 1].Item1).magnitude;
                    lenthCount++;
                }
            }
            if (gestureDisplayParent != null)
            {
                foreach (Transform child in gestureDisplayParent.transform)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        float avarageDist = totalDist / lenthCount;

        Debug.Log("total Point Count: "+gesturePositions.Count+", strokes: "+gesturePositions.Last().Item2+", avarage DistanceBetween Points: "+avarageDist);

        Gesture newGesture = new Gesture(pointArray);

        // add new gesture to trainingSet if in creation mode
        if (creationMode)
        {
            newGesture.Name = creationIsLetter ? newGestureName.ToCharArray()[0].ToString() : newGestureName;
            trainingSet.Add(newGesture);

            string fileName = persistantGestureFilePath + "/" + (creationIsLetter ? "letters/" : "other/") + newGesture.Name + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);
            Debug.Log("saved as: " + fileName);
        }
        // recognize gesture
        else
        {
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
            Debug.Log("found gesture: " + result.GestureClass + " - " + result.Score);
            if (result.Score > recognitionThreshold)
            {
                gestureOutput(result.GestureClass);
            }
        }
    }

    void UpdateGestureTimer()
    {
        //Debug.Log("Updating Gesture Timer");
        gestureTimer -= Time.deltaTime;

        // if the timer reaches 0 end the gesture
        if (gestureTimer <= 0)
        {
            EndGesture();
        }
    }
    void StartMovement()
    {
        Debug.Log("Start Movement");
        isMoving = true;
        positionList.Clear();
    }

    void EndMovement()
    {
        Debug.Log("End Movement");
        isMoving = false;

        foreach (Vector3 position in positionList)
        {
            gesturePositions.Add(new Tuple<Vector3, int>(position, gestureStroke));
        }

        gestureStroke++;
        gestureTimer = gestureFinishTimer;
    }

    void UpdateMovement()
    {
        //Debug.Log("Update Movement");
        if (movementSource != null)
        {
            if (positionList.Count > 0)
            {
                Vector3 lastPosition = positionList[positionList.Count - 1];
                if (Vector3.Distance(lastPosition, movementSource.position) >= newPositionThreshholdDistance)
                {
                    positionList.Add(movementSource.position);
                    if (drawObject != null)
                    {
                        GameObject displayObject = Instantiate(drawObject, movementSource.position, Quaternion.identity);
                        displayObject.transform.SetParent(gestureDisplayParent.transform);
                    }
                }
            }
            else
            {
                positionList.Add(movementSource.position);
                if (drawObject != null)
                {
                    GameObject displayObject = Instantiate(drawObject, movementSource.position, Quaternion.identity);
                    displayObject.transform.SetParent(gestureDisplayParent.transform);

                }
            }
        }
    }
}
