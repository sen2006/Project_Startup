
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
    [SerializeField] Transform fireballSpawnPos;
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] GameObject ghost;
    void Start()
    {
        givenAnswerDisplay.text = "";
        movementRecognizer = GetComponent<MovementRecognizer>();
        movementRecognizer.gestureOutput += InputString;
        currentQuestion = questions.Keys.First();
        questionDisplay.text = "What is the translation for:\n" + currentQuestion;
    }

    /*private void Update()
    {
        GameObject fireBall = Instantiate(fireballPrefab);
        fireBall.transform.position = fireballSpawnPos.position;
        fireBall.GetComponent<Rigidbody>().velocity = fireballSpawnPos.forward * 50;
        fireBall.GetComponent<FireBall>().hitEvent += hitGhost;
        fireBall.GetComponent<FireBall>().despawndOtherOnHit = false;
    }*/

    public void InputString(string input)
    {
        input = input.ToLower();
        if (input == "back")
        {
            if (currentAnswer.Length > 0)
                currentAnswer = currentAnswer.Remove(currentAnswer.Length - 1);
            givenAnswerDisplay.text = currentAnswer;
        }
        else if (input == "send")
        {
            Debug.Log("comparing: " + questions[currentQuestion].ToLower() + " to " + currentAnswer.ToLower());
            if (questions[currentQuestion].ToLower() == currentAnswer.ToLower())
            {
                Debug.Log("the correct answer was given");
                questionDisplay.text = "";
                currentAnswer = "Correct";
                GameObject fireBall = Instantiate(fireballPrefab);
                fireBall.transform.position = fireballSpawnPos.position;
                fireBall.GetComponent<Rigidbody>().velocity = fireballSpawnPos.forward * 10;
                fireBall.GetComponent<FireBall>().hitEvent += hitGhost;
                fireBall.GetComponent<FireBall>().despawndOtherOnHit = false;
                givenAnswerDisplay.text = currentAnswer;
            }
            else
            {
                Debug.Log("the wrong answer was given");
                currentAnswer = "";
                givenAnswerDisplay.text = currentAnswer;
            }
        }
        else if (input.Length > 1) return;
        else
        {
            currentAnswer += input;
            givenAnswerDisplay.text = currentAnswer;
        }
    }

    private void hitGhost(bool uselessBool)
    {
        currentQuestionID++;
        if (currentQuestionID >= questions.Count)
        {
            questionDisplay.text = "You Defeated The Ghost";
            currentAnswer = "";
            Destroy(ghost, .5f);
            givenAnswerDisplay.text = currentAnswer;
        }
        else
        {
            currentQuestion = questions.Keys[currentQuestionID];
            questionDisplay.text = "What is the translation for:\n" + currentQuestion;
            currentAnswer = "";
            givenAnswerDisplay.text = currentAnswer;
        }
    }
}
