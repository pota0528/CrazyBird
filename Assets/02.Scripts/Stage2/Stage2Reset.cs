using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage2Reset : MonoBehaviour
{
    public void ResetStage2()
    {
        SceneManager.LoadScene(2);
    }
}
