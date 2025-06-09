using UnityEngine;
using System.Collections;

public class csShowAllEffect : MonoBehaviour
{
    public string[] EffectNames;
    public string[] Effect2Names;
    public Transform[] Effect;
    //int a = 0;


    void Start()
    {
        Instantiate(Effect[0], new Vector3(0, 5, 0), Quaternion.identity);
    }
}
