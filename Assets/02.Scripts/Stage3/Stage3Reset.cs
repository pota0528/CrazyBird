using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage3Reset : MonoBehaviour
{
    public void ResetStage3()
    {
        SceneManager.LoadScene(3);
    }
}
