using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections.Generic;

public class WallObjectSpawner : MonoBehaviour
{
    [Tooltip("Reference your pre-configured AnchorPrefabSpawner")]
    public AnchorPrefabSpawner spawner;
    [SerializeField] private float minWallWidth = 2f;
    [SerializeField] private float minWallHeight = 2f;
    private MRUKAnchor wallAnchor;

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

    private MRUKAnchor GetRandomWall(List<MRUKAnchor> anchors, float minWidth, float minHeight) {
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

    public void spawnObject()
    {
        if (wallAnchor == null)
        {
            Debug.LogError("No wall availale");
            return;
        }
        spawner.SpawnPrefab(wallAnchor);
    }
}