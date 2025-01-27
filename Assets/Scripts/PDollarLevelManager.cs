using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MovementRecognizer))]
public class PDollarLevelManager : MonoBehaviour
{
    [SerializeField] PseudoDictionary<string, string> Questions;
    [SerializeField] string currentAnswer;
    MovementRecognizer movementRecognizer;
    void Start()
    {
        movementRecognizer = GetComponent<MovementRecognizer>();
        movementRecognizer.gestureOutput += InputString;
    }

    void Update()
    {
        
    }

    public void InputString(string input)
    {
        if (input == "back")
        {
            if (currentAnswer.Length > 0)
                currentAnswer = currentAnswer.Remove(currentAnswer.Last());
            return;
        }

        currentAnswer += input;
    }
}
