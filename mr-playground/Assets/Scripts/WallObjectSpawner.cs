using UnityEngine;
using Meta.XR.MRUtilityKit;
using System.Collections.Generic;

public class WallObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private Texture[] classTextures;
    [SerializeField] private float minWallWidth = 2f;
    [SerializeField] private float minWallHeight = 2f;
    private Dictionary<string, Texture> classTextureMap = new();
    private MRUKAnchor wallAnchor;
    private GameObject currentObject;

    public void Start()
    {
        // Match texture name to class name
        foreach (var tex in classTextures)
        {
            classTextureMap[tex.name] = tex;
        }
    }

    // Load a random suitable wall into wallAnchor
    public void OnSceneLoaded()
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

    public void SpawnObject(string ClassName)
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

        // Apply background to the door depending on the class
        Transform background = currentObject.transform.Find("Canvas/BackgroundImage");
        if (background == null || !background.TryGetComponent<UnityEngine.UI.RawImage>(out var rawImage))
        {
            Debug.LogWarning("No BackgroundImage found on the spawned object");
            return;
        }
        
        if (!classTextureMap.TryGetValue(ClassName, out Texture texture))
        {
            Debug.LogWarning($"No texture found for class {ClassName}");
            return;     
        }
        rawImage.texture = texture;
    }

    public void DestroyObject()
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
            currentObject = null;
        }
    }
}