using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    [SerializeField] List<ObjectRotate> objectRotates;
    [SerializeField] List<ExampleGrid> exampleObjects;
    [SerializeField] ExampleGridContext[] exampleContext;
    [SerializeField] Grid[] grids;
    int curCompletedGridCount;
    private void OnEnable()
    {
        EventManager.ObjectRotate += CheckGrid;
    }
    private void OnDisable()
    {
        EventManager.ObjectRotate -= CheckGrid;
    }
    void CheckGrid()
    {
        bool isComplete = true;
        for (int i = 0; i < exampleObjects.Count; i++)
        {
            ExampleGrid exampleGrid = exampleObjects[i];
            ObjectRotate objectRotate = objectRotates[i];
            if (objectRotate.clockRotation)
            {
                if (!AreRotationsEqual(exampleGrid.transform.rotation, objectRotate.transform.rotation, 0.5f))
                {
                    isComplete = false;
                    Debug.Log("oturmayan grid" + objectRotate.gridPos);
                }
            }
            else
            {
                if (exampleGrid.isRotated != objectRotate.isRotated || !AreRotationsEqual(exampleGrid.transform.rotation, objectRotate.transform.rotation, 0.5f) || objectRotate.rotating)
                {
                    isComplete = false;
                    Debug.Log("oturmayan grid" + objectRotate.gridPos);
                }
            }
        }
        if (isComplete)
        {
            Debug.Log("GridCompleted");
            StartCoroutine(SetComplete());
        }
        else Debug.Log("GridFailed");
    }
    IEnumerator SetComplete()
    {
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < exampleObjects.Count; i++)
        {
            ExampleGrid exampleGrid = exampleObjects[i];
            ObjectRotate objectRotate = objectRotates[i];
            objectRotate.transform.DOScale(Vector3.zero, 1f);
            Vector3 targetPos = exampleGrid.transform.position;
            targetPos.z = -1;
            objectRotate.transform.DOJump(targetPos, 1, 1, 1);
            yield return new WaitForSeconds(.1f);
        }
        yield return new WaitForSeconds(.3f);
        Destroy(objectRotates[0].transform.parent.gameObject);

        yield return new WaitForSeconds(.2f);
        for (int i = 0; i < exampleObjects.Count; i++)
        {
            ExampleGrid exampleGrid = exampleObjects[i];
            exampleGrid.transform.DOScale(Vector3.zero, 1f);
            yield return new WaitForSeconds(.1f);
        }
        yield return new WaitForSeconds(.3f);
        Destroy(exampleObjects[0].transform.parent.gameObject);
        curCompletedGridCount++;
        SpawnNextGrid();
    }
    void SpawnNextGrid()
    {
        GameObject newGrid = Instantiate(exampleContext[curCompletedGridCount].gameObject);
        exampleObjects.Clear();
        ExampleGridContext nextGrid = newGrid.GetComponent<ExampleGridContext>();
        exampleObjects = nextGrid.exampleGrids;
        nextGrid.OpenGrids(this);
    }
    public void NextGrid()
    {
        objectRotates.Clear();
        Grid nextGrid = grids[curCompletedGridCount].GetComponent<Grid>();
        Vector3 nextGridPos = nextGrid.transform.position;
        nextGridPos.z = 0;
        nextGrid.transform.position = nextGridPos;
        objectRotates = nextGrid.objectRotates;
        StartCoroutine(ResetGrids());
    }
    public IEnumerator ResetGrids()
    {
        foreach (ObjectRotate item in objectRotates)
        {
            item.ResetGrid();
            yield return new WaitForSeconds(.1f);
        }
    }
    bool AreRotationsEqual(Quaternion rot1, Quaternion rot2, float tolerance)
    {
        // Euler açýlarýyla karþýlaþtýrma
        Vector3 euler1 = rot1.eulerAngles;
        Vector3 euler2 = rot2.eulerAngles;

        // Her bir eksendeki farký kontrol et
        return Mathf.Abs(euler1.x - euler2.x) <= tolerance &&
               Mathf.Abs(euler1.y - euler2.y) <= tolerance &&
               Mathf.Abs(euler1.z - euler2.z) <= tolerance;
    }

}
