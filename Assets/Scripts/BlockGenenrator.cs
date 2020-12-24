using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


[RequireComponent(typeof(GameManager))]
public class BlockGenenrator : MonoBehaviour
{
    public static BlockGenenrator instance;

    public GameObject Block_Prefab;
    public Queue<GameObject> BlockPool;

    [SerializeField] private Vector3 GeneratePosition_Left;
    [SerializeField] private Vector3 GeneratePosition_Right;
    [SerializeField] private Vector2 Y_GapRange;




    // Block Generation Util

    private float Current_Y;
    private Vector3 targetPos;




    //[SerializeField] private Vector2 GenerateSizeRange;
    //[SerializeField] private float GenerateDuration;


    private void Awake()
    {
        instance = this;


        Current_Y = 0;
        targetPos = Vector3.zero;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    #region GeneratorConstruct

    /// <summary>
    /// 오브젝트 풀 생성
    /// </summary>
    public void CreateBlockPool()
    {
        if (BlockPool == null)
        {
            BlockPool = new Queue<GameObject>();
        }



        for (int i = 0; i < GameManager.instance.GetMaxBlockPoolCount(); i++)
        {
            // 블록 스펙 결정
            bool isLeft;
            isLeft = Random.Range(0, 2) == 1 ? true : false;


            targetPos = isLeft == true ? GeneratePosition_Left : GeneratePosition_Right;
            targetPos.y = Current_Y;
            Current_Y -= Random.Range(Y_GapRange.x, Y_GapRange.y);


            if (isLeft)
                targetPos.x += Random.Range(-0.5f, 2.0f);
            else
                targetPos.x += Random.Range(-2.0f, 0.5f);


            // 블록 생성 및 스펙 반영
            GameObject block = Instantiate(Block_Prefab, targetPos, Quaternion.identity);
            block.GetComponent<Block>().Init();
            block.GetComponent<Block>().SetBlockIndex(i);

            block.GetComponent<Block>().SetBlockDirection(isLeft ? Block.BlockDirection.Left : Block.BlockDirection.Right);
            //block.GetComponent<Block>().SetBlockType(isGood ? Block.BlockType.GoodBlock : Block.BlockType.BadBlock);
            //block.GetComponent<Block>().SetBlockSize(GenerateSizeRange.x, GenerateSizeRange.y);

            block.SetActive(false);

            BlockPool.Enqueue(block);

        }

        Debug.Log("Create Block Pool");
    }

    /// <summary>
    /// 오브젝트 풀 업데이트
    /// </summary>
    public void UpdateBlockPool()
    {
        if (BlockPool.Count == 0)
        {
            Debug.LogWarning("BlockPool is empty");
            return;
        }



        // 풀 재생성
        targetPos = Vector3.zero;
        Current_Y = 0;

        for (int i = 0; i < GameManager.instance.GetMaxBlockPoolCount(); i++)
        {

            GameObject obj = BlockPool.Dequeue();

            // 블록 스펙 결정
            bool isLeft;
            isLeft = Random.Range(0, 2) == 1 ? true : false;

            targetPos = isLeft == true ? GeneratePosition_Left : GeneratePosition_Right;
            targetPos.y = Current_Y;
            Current_Y -= Random.Range(Y_GapRange.x, Y_GapRange.y);


            if (isLeft)
                targetPos.x += Random.Range(-0.5f, 2.0f);
            else
                targetPos.x += Random.Range(-2.0f, 0.5f);


            obj.transform.SetPositionAndRotation(targetPos, Quaternion.identity);
            obj.GetComponent<Block>()
                .SetBlockDirection(isLeft ? Block.BlockDirection.Left : Block.BlockDirection.Right);


            if (obj.GetComponent<Block>().GetBlockMovingOn())
                obj.GetComponent<Block>().BlockDeactivate();


            obj.SetActive(false);

            BlockPool.Enqueue(obj);


            //    // 업데이트 대상
            //    if (i < GameManager.instance.GetCurrentBlockPoolCount())
            //    {
            //        GameObject obj = BlockPool.Dequeue();

            //        // 블록 스펙 결정
            //        bool isLeft;
            //        isLeft = Random.Range(0, 2) == 1 ? true : false;

            //        targetPos = isLeft == true ? GeneratePosition_Left : GeneratePosition_Right;
            //        targetPos.y = Current_Y;
            //        Current_Y -= Random.Range(Y_GapRange.x, Y_GapRange.y);


            //        if(isLeft)
            //            targetPos.x += Random.Range(-0.5f, 2.0f);
            //        else
            //            targetPos.x += Random.Range(-2.0f, 0.5f);


            //        obj.transform.SetPositionAndRotation(targetPos, Quaternion.identity);
            //        obj.GetComponent<Block>().SetBlockDirection(isLeft ? Block.BlockDirection.Left : Block.BlockDirection.Right);


            //        if (obj.GetComponent<Block>().GetBlockMovingOn())
            //            obj.GetComponent<Block>().BlockDeactivate();


            //        obj.SetActive(false);

            //        BlockPool.Enqueue(obj);
            //    }

            //    // 업데이트 미대상
            //    else
            //    {
            //        GameObject obj = BlockPool.Dequeue();
            //        BlockPool.Enqueue(obj);
            //    }
            //}

            Debug.Log("Block Pool Update Complete!");
        }

    }



    #endregion



    #region GeneratorControl

    public void GeneratorStart()
    {
        Debug.Log("Block Generation Start!");

        // Block Generation
        for(int i = 0; i < BlockPool.Count; i++)
        {
            GameObject obj = BlockPool.Dequeue();
            obj.SetActive(true);
            obj.GetComponent<Block>().BlockActivate();

            
            BlockPool.Enqueue(obj);
        }

    }

    public void GeneratorReset()
    {
        foreach (var elem in BlockPool)
        {
            if(elem.GetComponent<Block>().GetBlockMovingOn())
                elem.GetComponent<Block>().BlockDeactivate();
            if(elem.activeSelf)
                elem.SetActive(false);
        }
    }
    


    #endregion


    public void BlockExpired(GameObject obj)
    {
        obj.GetComponent<Block>().BlockDeactivate();
        obj.SetActive(false);
        //BlockPool.Enqueue(obj);
    }


    // Debug Printing
    public void DP_BlockPool()
    {
        Debug.Log("BlockPool count  : " + BlockPool.Count);
    }

    //IEnumerator GenerationCoroutine()
    //{
    //    yield return new WaitForSecondsRealtime(GenerateDuration);

    //    if (BlockPool.Count > 0)
    //    {
    //        GameObject obj = BlockPool.Dequeue();
    //        obj.SetActive(true);

    //        obj.GetComponent<Block>().BlockActivate();
    //    }
    //}
}
