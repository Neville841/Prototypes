using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleGrid : MonoBehaviour
{
    [SerializeField] internal Vector2 gridPos;
    [SerializeField] internal bool isRotated = false;
    private void Start()
    {
        gridPos = transform.localPosition;
    }
}
