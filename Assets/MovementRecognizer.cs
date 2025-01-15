using System.Collections.Generic;
using UnityEngine;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;

public class MovementRecognizer : MonoBehaviour
{
    [SerializeField] float newPositionThreshholdDistance = 0.05f;
    [SerializeField] GameObject drawObject;
    [SerializeField] float recognitionThreshold;
    [SerializeField] bool creationMode = false;
    [SerializeField] bool creationIsLetter = false;
    /// <summary>
    /// all letters need to start with the letter used.
    /// for example "a" and the next will be "a2"
    /// </summary>
    [SerializeField] string newGestureName = "";

    [System.Serializable]
    public class UnityStringEvent : UnityEvent<string> { }
    [SerializeField] UnityStringEvent OnRecognized;

    private List<Gesture> trainingSet = new List<Gesture>();

    private bool isButtonDown = false;
    private bool isMoving = false;
    private List<Vector3> positionList = new List<Vector3>();
    private Transform movementSource;

    // Start is called before the first frame update
    public void Start()
    {
        string[] otherGestureFiles = Directory.GetFiles(Application.persistentDataPath+ "/gestures/other/", " *.xml");
        string[] letterGestureFiles = Directory.GetFiles(Application.persistentDataPath+ "/gestures/letters/", " *.xml");
        foreach (var item in otherGestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));
        }
        foreach (var item in letterGestureFiles)
        {
            trainingSet.Add(GestureIO.ReadGestureFromFile(item));
        }
        Debug.Log("Loaded "+trainingSet.Count+" Gestures to the training set");
    }

    // Update is called once per frame
    void Update()
    {
        // start the movement
        if (!isMoving && isButtonDown)
        {
            StartMovement();
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
    }

    public void SetButtonDown(bool value)
    {
        isButtonDown = value;
    }

    public void SetMovementSource(Transform source)
    {
        movementSource = source;
    }

    public void RemoveMovementSource()
    {
        movementSource = null;
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

        // create gesture from possition list
        Point[] pointArray = new Point[positionList.Count];

        for (int i = 0; i < positionList.Count; i++)
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(positionList[i]);
            pointArray[i] = new Point(screenPosition.x, screenPosition.y,0);
        }

        Gesture newGesture = new Gesture(pointArray);

        // add new gesture to trainingSet
        if (creationMode)
        {
            newGesture.Name = creationIsLetter ? newGestureName.ToCharArray()[0].ToString() : newGestureName;
            trainingSet.Add(newGesture);

            string fileName = Application.persistentDataPath + "/gestures/"+ (creationIsLetter? "letters/" : "other/") + newGesture.Name + ".xml";
            GestureIO.WriteGesture(pointArray,newGestureName,fileName);
        }
        // recognize gesture
        else
        {
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());
            Debug.Log(result.GestureClass + " - " + result.Score);
            if (result.Score > recognitionThreshold)
            {
                OnRecognized.Invoke(result.GestureClass);
            }
        }
    }

    void UpdateMovement()
    {
        Debug.Log("Update Movement");
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
                        Destroy(Instantiate(drawObject, movementSource.position, Quaternion.identity), 10);
                    }
                }
            }
            else
            {
                positionList.Add(movementSource.position);
                if (drawObject != null)
                {
                    Destroy(Instantiate(drawObject, movementSource.position, Quaternion.identity), 10);
                }
            }
        }
    }
}
