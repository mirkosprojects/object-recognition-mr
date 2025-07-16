using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections.Generic;

public class WallObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private float minWallWidth = 2f;
    [SerializeField] private float minWallHeight = 2f;
    private MRUKAnchor wallAnchor;
    private GameObject currentObject;

    public void Start()
    {
        // Call OnSceneLoaded function when MRUK is finished loading the scene
        MRUK.Instance.SceneLoadedEvent.AddListener(OnSceneLoaded);
    }

    // Load a random suitable wall into wallAnchor
    private void OnSceneLoaded()
    {
        var currentRoom = MRUK.Instance.GetCurrentRoom();
        var wallAnchors = currentRoom.WallAnchors;
        Debug.Log($"Found {wallAnchors.Count} Wall Anchors");
        wallAnchor = GetRandomWall(wallAnchors, minWallWidth, minWallHeight);
    }

    private MRUKAnchor GetRandomWall(List<MRUKAnchor> anchors, float minWidth, float minHeight)
    {
        List<MRUKAnchor> suitableAnchors = new();

        foreach (var anchor in anchors)
        {
            var width = anchor.PlaneRect.Value.width;
            var height = anchor.PlaneRect.Value.height;

            if (width > minWidth && height > minHeight)
            {
                suitableAnchors.Add(anchor);
            }
        }

        if (suitableAnchors.Count <= 0)
        {
            Debug.LogError($"No wall found with minimum width and height ({minWidth}, {minHeight})");
            return null;
        }

        int index = Random.Range(0, suitableAnchors.Count);
        return suitableAnchors[index];
    }

    public void spawnObject(string ClassName)
    {
        if (wallAnchor == null)
        {
            Debug.LogError("No wall availale");
            return;
        }
        if (prefabToSpawn == null)
        {
            Debug.LogError("No prefab to spawn defined");
            return;
        }

        // Spawn the portal
        // TODO: Spawn different door backgrounds depending on the class
        Vector3 wallBottom = new(wallAnchor.transform.position.x, 0, wallAnchor.transform.position.z);
        Quaternion doorRotation = wallAnchor.transform.rotation * Quaternion.Euler(0, 180, 0);  // Rotate door 180 degrees
        currentObject = Instantiate(prefabToSpawn, wallBottom, doorRotation);
        currentObject.transform.SetParent(wallAnchor.transform, true);

        Debug.Log($"Spawned a portal for {ClassName}");
    }

    public void destroyCurrentObject()
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
            currentObject = null;
        }
    }
}