using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PouringEffect : MonoBehaviour
{
    public Material _testTubeFillShader; 
    public float[] _minMaxFillAmount;
    public float pourRate = 0.01f; 

    private bool isFilling = false;

    private void OnParticleTrigger()
    {
        Debug.Log("Chemical Entered: Flask");
        GameManager.instance.ChemicalPoured();

        //if (_testTubeFillShader != null && !isFilling)
        //{
        //    StartCoroutine(SmoothFill());
        //}
    }

    private IEnumerator SmoothFill()
    {
        isFilling = true;

        float currentFillAmount = _testTubeFillShader.GetFloat("_FillAmount");
        float targetFillAmount = Mathf.Clamp(_minMaxFillAmount[1], _minMaxFillAmount[0], _minMaxFillAmount[1]);

        while (currentFillAmount < targetFillAmount)
        {
            currentFillAmount += pourRate;
            currentFillAmount = Mathf.Clamp(currentFillAmount, _minMaxFillAmount[0], targetFillAmount);

            _testTubeFillShader.SetFloat("_FillAmount", currentFillAmount);

            yield return null; 
        }

        isFilling = false; 
    }
}
