using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flask : MonoBehaviour
{
    public KeyCode _mixingKey;

    [Header("ObjectReferences")]
    public Material liquidShaderMat;

    [SerializeField] float _resetSpeed = 20f;
    [SerializeField] float _mixingSpeed = 20f;
    [SerializeField] Vector3 _experimentPosition;

    // States
    private bool _isHolding = false;
    private bool _isInUse = false;
    [HideInInspector]public bool _isFull = false;
    private bool _isMixing = false;
    [HideInInspector] public bool _isBlending = false; // Add this flag to track if blending is in progress

    [SerializeField] string sucessAnimationClip;

    [Header("DefaultChemicalsValues")]
    public float[] _minMaxFillAmount;
    public Color _initialColor;
    public Color _ResultColor;
    public float _defaultFillAmount = 1f; // Duration for color blending
    public float blendDuration = 2f; // Duration for color blending

    private Vector3 _defaultTestubePosition;
    private Quaternion _defaultTestubeRotation;
    private Vector3 _offset;

    private void Start()
    {
        _defaultTestubePosition = transform.position;
        _defaultTestubeRotation = transform.rotation;

        liquidShaderMat.SetFloat("_FillAmount", _defaultFillAmount);
        liquidShaderMat.SetColor("_Tint", _initialColor); // Set initial color

    }

    private void OnMouseDown()
    {
        _isHolding = true;
        //Debug.Log("You are holding TestTube");
    }

    private void OnMouseUp()
    {
        _isHolding = false;
        _isInUse = false;
        CheckFlaskPosition();
    }

    bool randomValuesAssigned;
    private void Update()
    {
        if (_isHolding)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            transform.position = new Vector3(transform.position.x, worldPosition.y, worldPosition.z);

            if (_isFull)
            {
                _isMixing = Input.GetKey(_mixingKey);

                if (_isMixing)
                {
                    // Setting some random rotation so it will feel like we are mixing the solution
                    if (!randomValuesAssigned)
                    {
                        float randomX = Random.Range(-20f, 20f);
                        float randomZ = Random.Range(-20f, 20f);
                        transform.rotation = Quaternion.Euler(randomX, transform.rotation.eulerAngles.y, randomZ);
                        randomValuesAssigned = true;
                    }

                    //Debug.Log("Currently Mixing The Solution");
                    transform.Rotate(0, _mixingSpeed * Time.deltaTime, 0);

                    // Start blending the color, but only if not already blending
                    if (!_isBlending)
                    {
                        StartCoroutine(BlendToResultColor(_ResultColor));
                    }
                }
                else
                {
                    randomValuesAssigned = false;
                }
            }
        }
    }

    private IEnumerator BlendToResultColor(Color targetColor)
    {
        SoundManager.instance.playSound("Stirring");
        _isBlending = true; // Indicate blending has started
        Color currentColor = liquidShaderMat.GetColor("_Tint");
        float elapsedTime = 0f;

        while (elapsedTime < blendDuration)
        {
            liquidShaderMat.SetColor("_Tint", Color.Lerp(currentColor, targetColor, elapsedTime / blendDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        liquidShaderMat.SetColor("_Tint", targetColor); // Ensure it ends exactly at the target color
        Debug.Log(sucessAnimationClip);
        AnimationManager.instance.SetToDesiredAnimation(sucessAnimationClip);
        //_isBlending = false; // Indicate blending has finished
    }

    void CheckFlaskPosition()
    {
        if (Vector3.Distance(transform.position, _experimentPosition) < 0.45f)
        {
            _isInUse = true;
            SoundManager.instance.playSound("Thud");
            transform.position = _experimentPosition;
            transform.rotation = _defaultTestubeRotation;
            GameManager.instance._currentFlaskInUse = gameObject;
        }
        else
        {
            _isInUse = false;
            GameManager.instance._currentFlaskInUse = null;
            StartCoroutine(ResetTestTubePos());
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

    public void FillChemical(float fillRate)
    {
        if (liquidShaderMat != null)
        {
            float currentFillAmount = liquidShaderMat.GetFloat("_FillAmount");

            if (currentFillAmount > _minMaxFillAmount[0])
            {
                float newFillAmount = Mathf.Clamp(currentFillAmount - fillRate, _minMaxFillAmount[0], _minMaxFillAmount[1]);
                liquidShaderMat.SetFloat("_FillAmount", newFillAmount);
            }
            else
            {
                Debug.Log("The Flask is Full");
                _isFull = true;
            }
        }
    }
}
