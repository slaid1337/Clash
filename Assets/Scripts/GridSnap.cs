using UnityEngine;

[ExecuteInEditMode]
public class GridSnap : MonoBehaviour
{
    [ExecuteAlways]
    private void Update()
    {
        Vector3Int pos = BuildingGrid.Instance.Grid.LocalToCell(transform.position);
        transform.localPosition = BuildingGrid.Instance.Grid.GetCellCenterLocal(pos);
    }
}
