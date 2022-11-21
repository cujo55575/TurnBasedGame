using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DottedTrailController : MonoBehaviour
{
    [SerializeField] private Transform from;
    [SerializeField] private Transform lineInstance;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane + 89.7f;

        Vector3 lineVector = Camera.main.ScreenToWorldPoint(mousePos) - from.position;

        lineInstance.position = from.position + (lineVector / 2f);
    }
}
