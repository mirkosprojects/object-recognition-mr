using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class KeyWallFrameSpawner : MonoBehaviour
{
    [Tooltip("Reference your pre-configured AnchorPrefabSpawner")]
    public AnchorPrefabSpawner spawner;
    private MRUKAnchor keyWall;
    private List<MRUKAnchor> wallAnchors;

    [SerializeField] private float minRadius = 0.5f;

    public void Start()
    {
        MRUK.Instance.SceneLoadedEvent.AddListener(OnSceneLoaded);
    }


    // Call this method from the MRUK SceneLoadedEvent
    private void OnSceneLoaded()
    {
        Vector2 wallScale;
        var currentRoom = MRUK.Instance.GetCurrentRoom();

        keyWall = currentRoom.GetKeyWall(out wallScale);

        //if (currentRoom.GenerateRandomPositionOnSurface(
        //    MRUK.SurfaceType.VERTICAL, minRadius, new LabelFilter(MRUKAnchor.SceneLabels.WALL_FACE), out Vector3 pos, out Vector3 normal))
        //{
        //    //if (currentRoom.IsPositionInRoom(spawnPosition) \&\&!room.IsPositionInSceneVolume(spawnPosition))
        //}

        spawnDoors(currentRoom.WallAnchors);

        //currentRoom.WallAnchors[0].GetAnchorCenter


        Debug.Log($"Keywall found: {keyWall}");
    }

    public void spawnObject()
    {
        if (keyWall == null)
        {
            Debug.LogError("No Keywall availale");
            return;
        }
        spawner.SpawnPrefab(keyWall);
    }

    public void spawnDoors(List<MRUKAnchor> wallAnchors)
    {
        foreach (var anchor in wallAnchors) 
        {
            spawner.SpawnPrefab(anchor);
        }


    }
}