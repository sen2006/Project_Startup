using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MovementRecognizer))]
public class PDollarLevelManager : MonoBehaviour
{
    [SerializeField] PseudoDictionary<string, string> questions;
    string currentQuestion;
    string correctAnswer;
    [SerializeField] string currentAnswer;
    MovementRecognizer movementRecognizer;
    void Start()
    {
        movementRecognizer = GetComponent<MovementRecognizer>();
        movementRecognizer.gestureOutput += InputString;
        currentQuestion = questions.Keys.First();
    }

    public void InputString(string input)
    {
        input = input.ToLower();
        if (input == "back")
        {
            if (currentAnswer.Length > 0)
                currentAnswer = currentAnswer.Remove(currentAnswer.Last());
        }
        else if (input == "send")
        {
            // TODO input code for confirming the answer
            if (questions[currentQuestion] == currentAnswer)
            {
                Debug.Log("the correct answer was given");
            } else
            {
                Debug.Log("the wrong answer was given");
            }
        }
        else if (input.Length>1) return;
        currentAnswer += input;
    }
}
