using DG.Tweening;
using UnityEngine;
using System;
public class ObjectRotate : MonoBehaviour
{
    [SerializeField] internal Vector2 gridPos;
    [SerializeField] float rotateSpeed = 100f;
    [SerializeField] internal bool rotating;
    [SerializeField] internal bool isRotated = false;
    [SerializeField] internal bool clockRotation;
    [SerializeField] internal bool rightTurn = true;
    [SerializeField] bool startCoin;
    [SerializeField] Color darkColor;
    void Start()
    {
        gridPos = transform.localPosition;
        isRotated = Mathf.Round(transform.eulerAngles.y) == 180f;
        if (startCoin) return;
        Material[] mats = transform.GetChild(0).GetComponent<MeshRenderer>().materials;
        mats[1].color = darkColor;
    }
    void Update()
    {
        if (rotating) transform.Rotate(new Vector3(0, rotateSpeed, 0) * Time.deltaTime);
    }
    private void OnMouseDown()
    {
        if (rotating)
        {
            rotating = false;
            Vector3 rot = transform.rotation.eulerAngles;
            float fark = Mathf.Abs(180 - rot.y);
            if (fark <= 90)
            {
                rot.y = 180;
            }
            else rot.y = 0;
            SetNewRotation(rot);

        }
        else if (!clockRotation)
        {
            float targetRotationY = isRotated ? 0f : 180f;
            Vector3 newRotation = new Vector3(transform.eulerAngles.x, targetRotationY, transform.eulerAngles.z);
            isRotated = !isRotated;
            SetNewRotation(newRotation);
        }
        else
        {
            float targetRotationZ = transform.rotation.eulerAngles.z;
            if (rightTurn)
            {
                targetRotationZ -= 90;
            }
            else
            {
                targetRotationZ += 90;
            }
            Vector3 newRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, targetRotationZ);
            transform.DORotate(newRotation, .4f).OnComplete(() =>
            {
                EventManager.OnObjectRotate();
            });
        }
    }
    void SetNewRotation(Vector3 newRotation)
    {
        transform.DORotate(newRotation, .4f).OnComplete(() =>
        {
            isRotated = Mathf.Round(transform.eulerAngles.y) == 180f;
            EventManager.OnObjectRotate();
        });
    }
    public void ResetGrid()
    {
        //transform.position = new Vector3(gridPos.x, gridPos.y, 0);
        transform.localPosition = gridPos;
        if (!clockRotation)
            transform.rotation = Quaternion.Euler(Vector3.zero);
        isRotated = false;
        rotating = false;
        transform.DOScale(Vector3.one, .4f);
        Material[] mats = transform.GetChild(0).GetComponent<MeshRenderer>().materials;
        mats[1].DOColor(Color.red, .4f);
    }
}