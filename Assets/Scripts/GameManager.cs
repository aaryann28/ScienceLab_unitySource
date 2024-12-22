using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;
    public UiManager uiManager;
    [HideInInspector] public GameObject _currentFlaskInUse;
    public float _fillRate;


    [Header("HintAnnotation")]
    public Color _ActiveColor;
    public Color _InActiveColor;

    public TMP_Text _SelectFlaskTxt;
    public TMP_Text _AddTestubeSolution;
    public TMP_Text _StirTxt;
    public TMP_Text _PourFromTesttubeTxt;

    [HideInInspector]public int _TotalExpCompleted;
   
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Update()
    {
        CheckListAnnotation();
    }
    public void ChemicalPoured()
    {
        _currentFlaskInUse.gameObject.GetComponent<Flask>().FillChemical(_fillRate);
    }

    void CheckListAnnotation()
    {
       
        ActivateDeactiveTxt(_SelectFlaskTxt, _currentFlaskInUse );
        if (_currentFlaskInUse != null)
        {
            ActivateDeactiveTxt(_AddTestubeSolution, _currentFlaskInUse.GetComponent<Flask>()._isFull);
            ActivateDeactiveTxt(_StirTxt, _currentFlaskInUse.GetComponent<Flask>()._isBlending);
        }
        else
        {
            ActivateDeactiveTxt(_AddTestubeSolution, false);
            ActivateDeactiveTxt(_StirTxt, false);
        }


    }

    public void ActivateDeactiveTxt(TMP_Text textRef, bool status)
    {
        if (status)
        {
            textRef.color = _ActiveColor;
        }
        else
        {
            textRef.color = _InActiveColor;

        }
    }

    public void CheckExprerimentCompleted()
    {

        _TotalExpCompleted++;

        if (_TotalExpCompleted >= 2)
        {
            uiManager._CompletionPannel.SetActive(true);
            Invoke("DeacctivatePannel", 6.5f);
        }
    }

    void DeacctivatePannel()
    {
       uiManager._CompletionPannel.SetActive(false);

    }

}
