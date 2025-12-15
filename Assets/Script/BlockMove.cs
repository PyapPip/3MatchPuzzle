using System.Collections;
using UnityEngine;

public class BlockMove : MonoBehaviour
{
    private int moveFrame = 24;                 // 이동하는 데 걸리는 프레임
    private bool isMovingStart = false;         // true = 이동 시작
    private int[] targetLocation;

    public int isMatch = -1;                    // -1 = 미확인 0 = 매치 안 됨 1 = 매치 됨

    void Start()
    {
        UpdatePosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMovingStart) 
            return;

        targetLocation = new int[2] { this.gameObject.GetComponent<Block>().x, this.gameObject.GetComponent<Block>().y };
        if (isMatch == 1)
        {
            MoveBlock();
        }
        else
        {
            Snapback();
        }
    }

    public void MoveStart(bool _matchResult, int _dir)
    {
        isMovingStart = true;
        if (_matchResult)
            isMatch = 1;
        else
            isMatch = 0;
    }

    private void UpdatePosition()
    {
        // 격자 좌표 → 실제 월드 좌표로 변환
        transform.position = new Vector3(targetLocation[0], targetLocation[1], 0);
    }

    void MoveBlock()
    {
        float nowFrame = 0.0f;

        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(targetLocation[0], targetLocation[1], 0);

        while (nowFrame < moveFrame)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, 1.0f / moveFrame * nowFrame);

            nowFrame++;
        }

        // 마지막 위치 보정
        UpdatePosition();

        isMatch = -1;
    }

    void Snapback()
    {
        float nowFrame = 0.0f;

        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(targetLocation[0], targetLocation[1], 0);

        while (nowFrame < moveFrame)
        {
            if (nowFrame * 2 <= moveFrame)
            {
                transform.position = Vector3.Lerp(startPos, targetPos, 2.0f / moveFrame * nowFrame);
            }
            else
            {
                transform.position = Vector3.Lerp(targetPos, startPos, 2.0f / moveFrame * (nowFrame - moveFrame / 2));
            }

            nowFrame++;
            return;
        }

        transform.position = startPos;
        isMatch = -1;
    }
}