using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleGridContext : MonoBehaviour
{
    [SerializeField] internal List<ExampleGrid> exampleGrids;
    GridManager gridManager;
    public void OpenGrids(GridManager gridManager)
    {
        this.gridManager = gridManager;
        StartCoroutine(OpenAnimation());
    }
    IEnumerator OpenAnimation()
    {
        foreach (var exampleGrid in exampleGrids)
        {
            exampleGrid.transform.localScale = Vector3.zero;
        }
        foreach (var exampleGrid in exampleGrids)
        {
            exampleGrid.transform.DOScale(Vector3.one, .4f);
            yield return new WaitForSeconds(.1f);
        }
        gridManager.NextGrid();
    }
}
