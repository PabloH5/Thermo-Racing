using UnityEngine;

public class RotatableObject
{
    public float TotalRotation { get; private set; }

    public void UpdateRotation(float rotationDifference)
    {
        TotalRotation += rotationDifference;
    }

    public int GetCurrentFrame()
    {
        return Mathf.FloorToInt(TotalRotation / 360f);
    }
}