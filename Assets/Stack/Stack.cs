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

    private Vector3 prevBlockPos;
    private Vector3 desiredPos;
    private Vector3 stackBounds = new Vector2(boundSize, boundSize);

    Transform lastBlock;
    float blockTransition = 0f;
    float secondaryPos = 0f;

    int stackCount = -1;
    public int Score { get => stackCount; }
    int comboCount = 0;
    public int Combo {  get => comboCount; }
    int maxCombo = 0;
    public int MaxCombo { get => maxCombo; }

    public Color prevColor;
    public Color nextColor;

    bool isMovingX = true;

    int bestScore;
    public int BestScore { get => bestScore; }
    int bestCombo;
    public int BestCombo { get => bestCombo; }

    const string BestScoreKey = "BestScore";
    const string BestComboKey = "BestCombo";

    bool isGameOver = false;

    void Start()
    {
        if (originalBlock == null)
        {
            Debug.Log("블럭 없음");
            return;
        }

        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        bestCombo = PlayerPrefs.GetInt(BestComboKey, 0);

        prevColor = GetRandomColor();
        nextColor = GetRandomColor();

        prevBlockPos = Vector3.down;

        SpawnBlock();
        SpawnBlock();
    }

    void Update()
    {
        if (isGameOver)
        {
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            if (PlaceBlock())
            {
                SpawnBlock();
            }
            else
            {
                Debug.Log("게임오버");
                UpdateScore();
                isGameOver = true;
                GameOverEffect();
            }
        }

        MoveBlock();

        transform.position = Vector3.Lerp(transform.position, desiredPos, stackMovingSpeed * Time.deltaTime);
    }

    bool SpawnBlock()
    {
        if(lastBlock != null)
        {
            prevBlockPos = lastBlock.localPosition;
        }

        GameObject newBlock;
        Transform newTrans;

        newBlock = Instantiate(originalBlock);

        if(newBlock == null )
        {
            Debug.Log("블럭 생성 안 됨");
            return false;
        }

        ChangeColor(newBlock);

        newTrans = newBlock.transform;
        newTrans.parent = this.transform;
        newTrans.localPosition = prevBlockPos + Vector3.up;
        newTrans.localRotation = Quaternion.identity;
        newTrans.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

        stackCount++;

        desiredPos = Vector3.down * stackCount;
        blockTransition = 0f;

        lastBlock = newTrans;

        isMovingX = !isMovingX;

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
            Debug.Log("색깔 변경 안 됨");
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

    void MoveBlock()
    {
        blockTransition += Time.deltaTime * blockMovingSpeed;

        float movePos = Mathf.PingPong(blockTransition, boundSize) - boundSize / 2;

        if (isMovingX)
        {
            lastBlock.localPosition = new Vector3(movePos * movingBoundSize, stackCount, secondaryPos);
        }
        else
        {
            lastBlock.localPosition = new Vector3(secondaryPos, stackCount, -movePos * movingBoundSize);
        }
    }

    bool PlaceBlock()
    {
        Vector3 lastPos = lastBlock.transform.localPosition;
        
        if(isMovingX)
        {
            float dX = prevBlockPos.x - lastPos.x;
            bool isNegative = dX < 0 ? true : false;

            dX = Mathf.Abs(dX);

            if (dX > ErrorMargin)
            {
                stackBounds.x -= dX;
                if(stackBounds.x <= 0)
                {
                    return false;
                }

                float mid = (prevBlockPos.x + lastPos.x) / 2f;
                lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

                Vector3 temp = lastBlock.localPosition;
                temp.x = mid;
                lastBlock.localPosition = lastPos = temp;

                float rubbleHalfSize = dX / 2f;
                CreateRubble(
                    new Vector3(
                        isNegative
                        ? lastPos.x + stackBounds.x / 2 + rubbleHalfSize
                        : lastPos.x - stackBounds.x / 2 + rubbleHalfSize
                        , lastPos.y, lastPos.z)
                    , new Vector3(dX, 1, stackBounds.y)
                    );

                comboCount = 0;
            }
            else
            {
                CheckCombo();

                lastBlock.localPosition = prevBlockPos + Vector3.up;
            }
        }
        else
        {
            float dZ = prevBlockPos.z - lastPos.z;
            bool isNegative = dZ < 0 ? true : false;

            dZ = Mathf.Abs(dZ);

            if (dZ > ErrorMargin)
            {
                stackBounds.y -= dZ;
                if (stackBounds.y <= 0)
                {
                    return false;
                }

                float mid = (prevBlockPos.z + lastPos.z) / 2f;
                lastBlock.localScale = new Vector3(stackBounds.x, 1, stackBounds.y);

                Vector3 temp = lastBlock.localPosition;
                temp.z = mid;
                lastBlock.localPosition = lastPos = temp;

                float rubbleHalfSize = dZ / 2f;
                CreateRubble(
                    new Vector3(
                        lastPos.x, lastPos.y
                        , isNegative
                        ? lastPos.z + stackBounds.y / 2 + rubbleHalfSize
                        : lastPos.z - stackBounds.y / 2 + rubbleHalfSize)
                    , new Vector3(stackBounds.x, 1, dZ)
                    );

                comboCount = 0;
            }
            else
            {
                CheckCombo();

                lastBlock.localPosition = prevBlockPos + Vector3.up;
            }
        }

        secondaryPos = isMovingX ? lastBlock.localPosition.x : lastBlock.localPosition.z;

        return true;
    }

    void CreateRubble(Vector3 pos, Vector3 scale)
    {
        GameObject go = Instantiate(lastBlock.gameObject);
        go.transform.parent = this.transform;

        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        go.transform.localRotation = Quaternion.identity;

        go.AddComponent<Rigidbody>();
        go.name = "Rubble";
    }

    void CheckCombo()
    {
        comboCount++;

        if(comboCount > maxCombo)
        {
            maxCombo = comboCount;
        }

        if(comboCount % 5 == 0)
        {
            Debug.Log("5콤보!");

            stackBounds += new Vector3(0.5f, 0.5f);
            stackBounds.x = stackBounds.x > boundSize ? boundSize : stackBounds.x;
            stackBounds.y = stackBounds.y > boundSize ? boundSize : stackBounds.y;
        }
    }

    void UpdateScore()
    {
        if(bestScore < stackCount)
        {
            Debug.Log("최고 점수 갱신!");
            bestScore = stackCount;
            bestCombo = maxCombo;

            PlayerPrefs.SetInt(BestScoreKey, bestScore);
            PlayerPrefs.SetInt(BestComboKey, bestCombo);
        }
    }

    void GameOverEffect()
    {
        int childCount = this.transform.childCount;

        for(int i = 1; i < childCount; i++)
        {
            if (childCount < i) break;

            GameObject go = transform.GetChild(childCount - i).gameObject;

            if (go.name.Equals("Rubble")) continue;

            Rigidbody rb = go.AddComponent<Rigidbody>();
            rb.AddForce(
                (Vector3.up * Random.Range(0, 10f)
                + Vector3.right * (Random.Range(0, 10f) - 5f)) * 100f);
        }
    }
}
