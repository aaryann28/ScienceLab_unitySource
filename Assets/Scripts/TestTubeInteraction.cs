using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestTubeInteraction : MonoBehaviour
{
    [SerializeField] float _resetSpeed;
    [SerializeField] float _pouringSpeed;

    [SerializeField] KeyCode _pouringKey;
    public ParticleSystem _PouringEffect;
    // States
    private bool _isHolding = false;
    private bool _isPouring = false;

    private Vector3 _defaultTestubePosition;
    private Quaternion _defaultTestubeRotation;
    private Vector3 _offset;

    private void Start()
    {
        _defaultTestubePosition = transform.position;
        _defaultTestubeRotation = transform.rotation;
    }

    private void OnMouseDown()
    {
        _isHolding = true;
        Debug.Log("You are holding TestTube");

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        GameManager.instance._PourFromTesttubeTxt.gameObject.SetActive(true);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        _offset = transform.position - worldPosition;
    }

    private void OnMouseUp()
    {
        _isHolding = false;
        GameManager.instance._PourFromTesttubeTxt.gameObject.SetActive(false);
        StartCoroutine(ResetTestTubePos());
    }

    private void Update()
    {
        if (_isHolding)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition) + _offset;
            transform.position = new Vector3(transform.position.x, worldPosition.y, worldPosition.z);
           
        }

        CheckChemicalPourning();
        PlayPouringEffects();
    }

    private void CheckChemicalPourning()
    {
        if (!_isHolding)
            return;
        _isPouring = Input.GetKey(_pouringKey);

        if (_isPouring)
        {
            Quaternion targetRotation = Quaternion.Euler(_pouringSpeed, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);

        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _defaultTestubeRotation, Time.deltaTime);
        }



    }

    private void PlayPouringEffects()
    {
       
        float xRotation = transform.rotation.eulerAngles.x;
        if (xRotation > 180f) 
            xRotation -= 360f; 

        if (xRotation < -75f)
        {
            var pouringEmission = _PouringEffect.emission;
            SoundManager.instance.playSound("Pouring");
            pouringEmission.enabled = true;
        }
        else
        {
            var pouringEmission = _PouringEffect.emission;
            pouringEmission.enabled = false;
        }
    }

    private IEnumerator ResetTestTubePos()
    {
        while (Vector3.Distance(transform.position, _defaultTestubePosition) > 0.1f || Quaternion.Angle(transform.rotation, _defaultTestubeRotation) > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, _defaultTestubePosition, _resetSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, _defaultTestubeRotation, _resetSpeed * Time.deltaTime);
            yield return null;
        }
        SoundManager.instance.playSound("Thud");
        transform.position = _defaultTestubePosition;
        transform.rotation = _defaultTestubeRotation;
    }

}
