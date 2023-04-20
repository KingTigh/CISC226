using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class LevelSelectScript : MonoBehaviour
{
    public AudioSource click;
    public void StatLevel1()
    {
        click.Play();
        SceneManager.LoadScene(1);
    }
    public void StartLevel2()
    {
        click.Play();
        SceneManager.LoadScene(2); 
    }
}
