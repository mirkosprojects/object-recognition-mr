using UnityEngine;
public class TrackedObject
{
    public string ClassName;
    public Vector3 WorldPos;
    public int FramesSinceLastSeen;
    public int FramesSeen;

    public TrackedObject(string className, Vector3 worldPos)
    {
        ClassName = className;
        WorldPos = worldPos;
        FramesSinceLastSeen = 0;
        FramesSeen = 1;
    }

    public void Update(Vector3 newPos)
    {
        WorldPos = Vector3.Lerp(WorldPos, newPos, 0.5f); // smooth update
        FramesSinceLastSeen = 0;
        FramesSeen++;
    }

    public void Miss()
    {
        FramesSinceLastSeen++;
    }
}