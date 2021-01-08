using UnityEngine;
using System.Collections.Generic;

// A very simple object pooling class
public class SimpleObjectPool : MonoBehaviour
{

    // 미리 생성해서 넣어줄 이름
    public string strPrefabName;

    //미리 만들어 둘 사이즈(파티클 밑,개수가 정해져 있지 않는 것들
    public int nPoolSize = 100;

    // the prefab that this object pool returns instances of
    public GameObject prefab;
    // collection of currently inactive instances of the prefab
    public Stack<GameObject> inactiveInstances = new Stack<GameObject>();

    //호출시 미리 Stack에 해당하는 prefab을 지정된 갯수 만큼 쌓아 놓는다.
    public void PreloadPool()
    {
        for(int nIndex = 0; nIndex <nPoolSize; nIndex++)
        {
            GameObject obj = Instantiate(prefab);

            PooledObject pooledObject = obj.AddComponent<PooledObject>();
            pooledObject.pool = this;

            obj.transform.SetParent(transform, false);
            obj.name = prefab.name;
            obj.SetActive(false);

            inactiveInstances.Push(obj);
        }
    }

    // Returns an instance of the prefab
    public GameObject GetObject()
    {
        GameObject spawnedGameObject;
        Debug.Log("Stack Count : " + inactiveInstances.Count);

        // if there is an inactive instance of the prefab ready to return, return that
        if (inactiveInstances.Count > 0)
        {
            // remove the instance from teh collection of inactive instances
            spawnedGameObject = inactiveInstances.Pop();
        }
        // otherwise, create a new instance
        else
        {
            spawnedGameObject = Instantiate(prefab);

            // add the PooledObject component to the prefab so we know it came from this pool
            // 이 오브젝트가 빠져나온 Pool을 참조한다.
            PooledObject pooledObject = spawnedGameObject.AddComponent<PooledObject>();
            pooledObject.pool = this;
        }

        // put the instance in the root of the scene and enable it
        spawnedGameObject.transform.SetParent(null);
        spawnedGameObject.SetActive(true);

        // return a reference to the instance
        return spawnedGameObject;
    }

    // Return an instance of the prefab to the pool
    public void ReturnObject(GameObject toReturn)
    {
        PooledObject pooledObject = toReturn.GetComponent<PooledObject>();
        //DestroyImmediate(toReturn);
        //if the instance came from this pool, return it to the pool
        if (pooledObject != null && pooledObject.pool == this)
        {
            // make the instance a child of this and disable it
            toReturn.transform.SetParent(transform, false);
            toReturn.SetActive(false);
            //Debug.Log("ReturnObj");
            // add the instance to the collection of inactive instances
          
            inactiveInstances.Push(toReturn);

            //Destroy(toReturn);
            //inactiveInstances.Pop();
        }
        // otherwise, just destroy it
        else
        {
            Debug.LogWarning(toReturn.name + " was returned to a pool it wasn't spawned from! Destroying.");
            Destroy(toReturn);
        }
    }
}

// a component that simply identifies the pool that a GameObject came from
public class PooledObject : MonoBehaviour
{
    public SimpleObjectPool pool;
}