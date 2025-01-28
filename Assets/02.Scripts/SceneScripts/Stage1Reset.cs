using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage1Reset : MonoBehaviour
{
    public void ResetStage1()
    {
        SceneManager.LoadScene(1);
    }
}
