using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    #region Singleton
    public static Animal instance;

    void Awake()
    {
        instance = this;
    }
    #endregion
}
