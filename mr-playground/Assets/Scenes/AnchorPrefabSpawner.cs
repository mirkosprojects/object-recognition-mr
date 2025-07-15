using Meta.XR.MRUtilityKit;
using UnityEngine;

public class AnchorPrefabSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void SpawnPrefab(MRUKAnchor anchor)
    {
        if (anchor == null || prefabToSpawn == null)
        {
            return;
        }

        //Vector3 wallBottom = new(anchor.transform.position.x, anchor.PlaneRect.Value.yMax, anchor.transform.position.z);
        Vector3 wallBottom = new(anchor.transform.position.x, 0, anchor.transform.position.z);

        var obj = Instantiate(prefabToSpawn, wallBottom, anchor.transform.rotation);
        obj.transform.SetParent(anchor.transform, true);
    }
}
