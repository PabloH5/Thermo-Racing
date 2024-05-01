using UnityEngine;


[CreateAssetMenu]
public class TiresDataBase : ScriptableObject
{
    public Tires[] tires;


    public int TiresCount
    {
        get
        {
            return tires.Length;
        }
    }

    public Tires GetTires(int index)
    {
        return tires[index];
    }
}


