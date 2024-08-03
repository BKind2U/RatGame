using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageLog : MonoBehaviour
{
    // keeps track of system messages as they appear (eg, when things attack, when they deal damage, etc.)
    [SerializeField] private TMPro.TextMeshProUGUI messagebox;

    public void DisplayMessage(string message)
    {
        messagebox.text += message += "\n";
        Debug.Log(message);
    }

    
}
