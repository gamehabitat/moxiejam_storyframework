using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueArray : MonoBehaviour
{
    [SerializeField]
    string[] dialogues;

    [SerializeField]
    int currentDialogue = 0;

    public string CurrentDialogue => dialogues.Length == 0 ? string.Empty : dialogues[currentDialogue];

    void Start()
    {
    }

    public void NextDialogue()
    {
        ++currentDialogue;
        if(currentDialogue >= dialogues.Length)
        {
            currentDialogue = dialogues.Length - 1;
        }
    }
}
