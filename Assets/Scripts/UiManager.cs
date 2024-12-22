using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class UiManager : MonoBehaviour
{

    public GameObject _CompletionPannel;

    public void OnSceneChangebtnClick(int Sceneid)
    {
        SceneManager.LoadScene(Sceneid);
    }

    public void OnExitBtnClick()
    {
        Application.Quit();
    }


}
