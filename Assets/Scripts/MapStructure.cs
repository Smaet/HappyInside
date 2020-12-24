using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapStructure : MonoBehaviour
{

    public enum StructureType
    {
        UserEndPoint,
        BlockEndPoint
    }

    [SerializeField] private StructureType CurStructureType;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (CurStructureType == StructureType.UserEndPoint)
        {
            if (other.collider.CompareTag("UserCharacter"))
            {
                // GameOver 호출
                GameManager.instance.GameOver(false);
            }
        }
        else if (CurStructureType == StructureType.BlockEndPoint)
        {
            if (other.collider.CompareTag("Block"))
            {
                
            }
        }
    }
}
