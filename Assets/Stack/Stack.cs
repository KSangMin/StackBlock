using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    private const float boundSize = 3.5f;
    private const float movingBoundSize = 3f;
    private const float stackMovingSpeed = 5f;
    private const float blockMovingSpeed = 3.5f;
    private const float ErrorMargin = 0.1f;

    public GameObject originalBlock;

    private Vector3 prevPos;
    private Vector3 targetPos;
    private Vector3 stackBounds = new Vector2(boundSize, boundSize);

    Transform lastBlock;
    float blockTransition = 0f;
    float secondaryPos = 0f;

    int stackCount = -1;
    int comboCount = 0;

    void Start()
    {
        if (originalBlock == null)
        {
            Debug.Log("블럭 없음");
            return;
        }

        prevPos = Vector3.down;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SpawnBlock();
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, stackMovingSpeed * Time.deltaTime);
    }

    bool SpawnBlock()
    {
        if(lastBlock != null)
        {
            prevPos = lastBlock.localPosition;
        }

        GameObject newBlock;
        Transform newTrans;

        newBlock = Instantiate(originalBlock);

        if(newBlock == null )
        {
            Debug.Log("블럭 생성 안 됨");
            return false;
        }

        newTrans = newBlock.transform;
        newTrans.parent = this.transform;
        newTrans.localPosition = prevPos + Vector3.up;
        newTrans.localRotation = Quaternion.identity;
        newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

        stackCount++;

        targetPos = Vector3.down * stackCount;
        blockTransition = 0f;

        lastBlock = newTrans;

        return true;
    }
}
