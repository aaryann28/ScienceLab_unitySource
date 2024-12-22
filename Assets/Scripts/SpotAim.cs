using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotAim : MonoBehaviour
{
    public Transform headTarget;
    public Transform lightTarget;

    [SerializeField] float followSpeed;

    private void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z;

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        headTarget.position = new Vector3(headTarget.position.x, worldPosition.y, worldPosition.z);
        lightTarget.position = Vector3.Lerp(lightTarget.position, new Vector3(lightTarget.transform.position.x, lightTarget.transform.position.y, worldPosition.z), followSpeed * Time.deltaTime);

    }

}
