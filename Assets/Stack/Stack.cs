using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

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

    public Color prevColor;
    public Color nextColor;

    void Start()
    {
        if (originalBlock == null)
        {
            Debug.Log("ºí·° ¾øÀ½");
            return;
        }

        prevColor = GetRandomColor();
        nextColor = GetRandomColor();

        prevPos = Vector3.down;

        SpawnBlock();
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
            Debug.Log("ºí·° »ý¼º ¾È µÊ");
            return false;
        }

        ChangeColor(newBlock);

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

    Color GetRandomColor()
    {
        float r = Random.Range(100f, 250f) / 255f;
        float g = Random.Range(100f, 250f) / 255f;
        float b = Random.Range(100f, 250f) / 255f;

        return new Color(r, g, b);
    }

    void ChangeColor(GameObject go)
    {
        Color applyColor = Color.Lerp(prevColor, nextColor, (stackCount % 11) / 10f);

        Renderer r = go.GetComponent<Renderer>();

        if (r == null)
        {
            Debug.Log("»ö±ò º¯°æ ¾È µÊ");
            return;
        }

        r.material.color = applyColor;
        Camera.main.backgroundColor = applyColor - new Color(0.1f, 0.1f, 0.1f);

        if(applyColor.Equals(nextColor) == true)
        {
            prevColor = nextColor;
            nextColor = GetRandomColor();
        }
    }
}
