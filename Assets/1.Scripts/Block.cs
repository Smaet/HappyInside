using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Block : MonoBehaviour
{

    public enum BlockType
    {
        GoodBlock,
        BadBlock
    }

    public enum BlockDirection
    {
        Left,
        Right
    }

    [SerializeField] private BlockType Type;
    [SerializeField] private BlockDirection Dir;
    [SerializeField] private bool BlockMovingOn;
    [SerializeField] private bool isLastBlock;

    [SerializeField] private Sprite BlockTexture;

    [SerializeField] private int BlockIndex;


    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (BlockMovingOn)
        {
            this.gameObject.transform.Translate(Vector3.up * GameManager.Instance.GetBlockSpeed());
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (GetLastCheck())
        {
            if (other.collider.CompareTag("Character"))
            {
                GameManager.Instance.GameOver(true);
            }
        }
    }


    public void Init()
    {
        //SetBlockMovingOn(false);
        //SetLastCheck(false);
        GetComponent<SpriteRenderer>().sprite = BlockTexture;
    }

    public void BlockActivate()
    {
        SetBlockMovingOn(true);
        Debug.Log("BlockMovingOn" + GetBlockIndex());
    }

    public void BlockDeactivate()
    {
        SetBlockMovingOn(false);
    }


   

    #region Getter / Setter


    public Vector3 GetBlockSize()
    {
        return transform.localScale;
    }

    public BlockType GetBlockType()
    {
        return Type;
    }

    public BlockDirection GetBlockDirection()
    {
        return Dir;
    }

    public bool GetBlockMovingOn()
    {
        return BlockMovingOn;
    }

    public bool GetLastCheck()
    {
        return isLastBlock;
    }

    public int GetBlockIndex()
    {
        return BlockIndex;
    }



    public void SetBlockSize(float MinSize, float MaxSize)
    {
        transform.localScale = new Vector3(Random.Range(MinSize, MaxSize), Random.Range(MinSize, MaxSize), 1);
    }
    public void SetBlockType(BlockType newtype)
    {
        Type = newtype;
    }
    public void SetBlockDirection(BlockDirection newdirection)
    {
        Dir = newdirection;
    }
    public void SetBlockMovingOn(bool value)
    {
        BlockMovingOn = value;
    }

    public void SetLastCheck(bool value)
    {
        isLastBlock = value;
    }

    public void SetBlockIndex(int value)
    {
        BlockIndex = value;
    }

    #endregion
}
