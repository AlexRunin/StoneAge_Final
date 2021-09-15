using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelsButtonLoader : MonoBehaviour
{
    public void LoaderLevels(int levelsNumber)
    {
        SceneManager.LoadScene(levelsNumber);
    }


}
