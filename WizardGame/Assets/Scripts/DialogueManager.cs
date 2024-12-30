using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Queue<string> sentences;

    void Start()
    {
        sentences = new Queue<string>();

    }
    public void StartDialogue (Dialogue dialogue)
    {
        Debug.Log("starting conversation with " + dialogue.name);

        sentences.Clear();

/*        foreach (string sentence in dialogue.sentences)
        {
            sentence.Enqueue(sentence);
        }*/

    }
}
