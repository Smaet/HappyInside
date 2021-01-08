/*------------------------------------------------------------------------------------------
 * Copyright (C) 2018 Version R Inc
 * SimpleSingleton.cs
 * Author : mwkang@ver-r.com
 *----------------------------------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T _instance = null;

    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject obj = new GameObject(typeof(T).ToString());
                _instance = obj.AddComponent<T>();
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        _instance = this as T;
    }
}
