using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class DialogueEntry
{
    public string dialogueText;
    public AudioClip audioClip;
}

[CreateAssetMenu(fileName = "DialogueData", menuName = "ScriptableObjects/DialogueData", order = 1)]
public class DialogueData : ScriptableObject
{
    public List<DialogueEntry> dialogueEntries;
    public List<GameObjectMovement> movements;
}

[System.Serializable]
public class GameObjectMovement
{
    public GameObject targetObject;
    public Vector3 targetPosition;
    public float duration;
    /// <summary>
    /// Ties movement to specific dialogue entry, -1 means it is unused
    /// </summary>
    public int dialogueIndex = -1; 
}

public class DialogueManager : MonoBehaviour
{
    public DialogueData dialogueData;
    public TextMeshProUGUI dialogueTextUI;
    public AudioSource audioSource;
    public KeyCode nextKey = KeyCode.Space;

    private int currentDialogueIndex = 0;
    private Coroutine typingCoroutine;

    private void Start()
    {
        StartCoroutine(ExecuteDialogueWithMovements());
    }

    private void Update()
    {
        if (Input.GetKeyDown(nextKey))
        {
            OnNextKeyPressed();
        }
    }

    private IEnumerator ExecuteDialogueWithMovements()
    {
        foreach (var movement in dialogueData.movements)
        {
            if (movement.dialogueIndex == currentDialogueIndex)
            {
                yield return StartCoroutine(MoveObject(movement));
            }
        }

        ShowNextDialogue();
    }

    private IEnumerator MoveObject(GameObjectMovement movement)
    {
        Vector3 startPosition = movement.targetObject.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < movement.duration)
        {
            movement.targetObject.transform.position = Vector3.Lerp(
                startPosition,
                movement.targetPosition,
                elapsedTime / movement.duration
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        movement.targetObject.transform.position = movement.targetPosition;
    }

    private void ShowNextDialogue()
    {
        if (currentDialogueIndex >= dialogueData.dialogueEntries.Count)
        {
            EndDialogue();
            return;
        }

        var dialogueEntry = dialogueData.dialogueEntries[currentDialogueIndex];

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeDialogue(dialogueEntry));
    }

    private IEnumerator TypeDialogue(DialogueEntry dialogueEntry)
    {
        dialogueTextUI.text = "";

        if (dialogueEntry.audioClip != null)
        {
            audioSource.clip = dialogueEntry.audioClip;
            audioSource.Play();

            float typingInterval = dialogueEntry.audioClip.length / dialogueEntry.dialogueText.Length;
            foreach (char letter in dialogueEntry.dialogueText)
            {
                dialogueTextUI.text += letter;
                yield return new WaitForSeconds(typingInterval);
            }

            while (audioSource.isPlaying)
            {
                yield return null;
            }
        }
        else
        {
            dialogueTextUI.text = dialogueEntry.dialogueText;
        }

        currentDialogueIndex++;
        StartCoroutine(ExecuteDialogueWithMovements());
    }

    private void OnNextKeyPressed()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        currentDialogueIndex++;
        StartCoroutine(ExecuteDialogueWithMovements());
    }

    private void EndDialogue()
    {
        Debug.Log("Dialogue finished.");
        dialogueTextUI.text = "";
    }
}
