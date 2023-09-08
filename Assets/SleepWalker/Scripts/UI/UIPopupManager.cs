using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopupManager : MonoBehaviour
{
    public static UIPopupManager instance;

    private void Awake()
    {
        if (instance && instance != this)
            Destroy(gameObject);
        else
            instance = this;
    }

    public void DialogueEventStarted()
    {
    }

    public void DialogueEventEnded()
    {
    }
}
