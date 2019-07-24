using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class restartButton : MonoBehaviour
{
    public void OnRestartButtonClick()
    {

        Application.LoadLevel(Application.loadedLevel);
    }
}
