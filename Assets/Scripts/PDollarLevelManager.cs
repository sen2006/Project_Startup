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
    [SerializeField] TMPro.TextMeshPro givenAnswerDisplay;
    [SerializeField] TMPro.TextMeshPro questionDisplay;
    int currentQuestionID = 0;
    void Start()
    {
        givenAnswerDisplay.text = "";
        movementRecognizer = GetComponent<MovementRecognizer>();
        movementRecognizer.gestureOutput += InputString;
        currentQuestion = questions.Keys.First();
        questionDisplay.text = "What is the translation for:\n" + currentQuestion;
    }

    public void InputString(string input)
    {
        input = input.ToLower();
        if (input == "back")
        {
            if (currentAnswer.Length > 0)
                currentAnswer = currentAnswer.Remove(currentAnswer.Length-1);
        }
        else if (input == "send")
        {
            Debug.Log("comparing: "+ questions[currentQuestion].ToLower() +" to "+ currentAnswer.ToLower());
            if (questions[currentQuestion].ToLower() == currentAnswer.ToLower())
            {
                Debug.Log("the correct answer was given");
                currentQuestion = questions.Keys[++currentQuestionID];
                questionDisplay.text = "What is the translation for:\n" + currentQuestion;
                currentAnswer = "";
            } else
            {
                Debug.Log("the wrong answer was given");
                currentAnswer = "Try again";
            }
        }
        else if (input.Length>1) return;
        else currentAnswer += input;
        givenAnswerDisplay.text = currentAnswer;
    }
}
