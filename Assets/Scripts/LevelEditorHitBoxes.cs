using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelEditorHitBoxes : MonoBehaviour
{
    [SerializeField] private int randomBoxAmount;
    [SerializeField] private int obstacleAmount;
    [SerializeField] private string targetWord = "test";
    [SerializeField] private PseudoDictionary<string, string> questionAnswer = new PseudoDictionary<string, string>();
    [SerializeField] private PseudoDictionary<string, bool> wordSpawnList = new PseudoDictionary<string, bool>();
    [SerializeField] private List<string> wrongWords = new List<string>();

    private int questionIndex = 0;

    public float boxMoveSpeed;
    public float boxSpawnDelay;

    public GameObject letterBox;
    public GameObject obstacle;

    public readonly List<int> spawnQueue = new List<int>();

    public static readonly string alphabet = "abcdefghijklmnopqrstuvwxyz";

    private SpawnSignal spawnSignal;

    void Start()
    {
        spawnSignal = GameObject.Find("Spawn Handler").GetComponent<SpawnSignal>();

        for (int i = 0; i < targetWord.Length; i++)
        {
            for (int j = 0; j < alphabet.Length; j++)
            {
                if (targetWord[i] == alphabet[j])
                {
                    spawnQueue.Add(j);
                    break;
                }
            }
        }

        for (int i = 0; i < randomBoxAmount; i++)
        {
            spawnQueue.Add(UnityEngine.Random.Range(0, 26));
        }

        for (int i = 0; i < obstacleAmount; i++)
        {
            spawnQueue.Add(26);
        }
        //--------------------------------------------------------
        NextQuestion();

    }

    private void NextQuestion()
    {
        wordSpawnList.Clear();
        if (questionIndex >= questionAnswer.Count) return;
        string answer = questionAnswer.Values[questionIndex++];
        wordSpawnList.Add(answer, true);
        SaturateWrongAnswers(answer);
    }

    private void SaturateWrongAnswers(string correctAnswer)
    {
        List<string> words = new List<string>(wrongWords);
        words.Remove(correctAnswer);

        for (int i = 0; i < obstacleAmount; i++)
        {
            if (words.Count == 0) break;
            int wordIndex = UnityEngine.Random.Range(0, wrongWords.Count);
            wordSpawnList.Add(wrongWords[wordIndex], false);
            words.RemoveAt(wordIndex);
        }
    }

    public int ObjectToSpawn()
    {
        int pickNumber = UnityEngine.Random.Range(0, spawnQueue.Count);
        int returnNumber = spawnQueue[pickNumber];
        spawnQueue.RemoveAt(pickNumber);
        if (spawnQueue.Count == 0)
        {
            spawnSignal.spawnsLeft = false;
        }
        return returnNumber;
    }
}
