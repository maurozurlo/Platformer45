using UnityEngine;

public class globalControl : MonoBehaviour
{
    public static globalControl control;
    public bool activateSound = true;

    void Awake()
    {
        if (control == null)
            control = this;
        DontDestroyOnLoad(this.gameObject);
    }

}