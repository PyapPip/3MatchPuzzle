using System.Collections;
using UnityEngine;

public class BlockMove : MonoBehaviour
{
    private int moveFrame = 0;                 // 이동하는 데 걸리는 프레임
    private float nowFrame = 0.0f;              // 현재 프레임
    private Vector3 startPos;
    private Vector3 targetPos;

    public int isBlockState = -1;                    // -1 = 초기화 0 = 매치 안 됨 1 = 매치 됨 2 = 떨어지는 블럭

    // Update is called once per frame
    void Update()
    {
        if (isBlockState == -1) 
            return;

        if (isBlockState > -1)
        {
            if (isBlockState == 1)
            {
                MoveBlock();
            }

            else if(isBlockState == 2)
            {
                fall();
            }

            else
            {
                Snapback();
            }
        }
    }

    /// <summary>
    /// 0 = 매치되지 않음 1 = 매치됨 2 = 떨어지는 블록
    /// </summary> 
    public void MoveStart(int _isBlockState, int[] _targetPos)
    {
        isBlockState = _isBlockState;

        if(_isBlockState == 0)
        { moveFrame = 24; }

        else if (_isBlockState == 1)
        { moveFrame = 16; }

        startPos = transform.position;
        targetPos = new Vector3(_targetPos[0], -_targetPos[1]);
    }

    private void MoveBlock()
    {
        if (nowFrame < moveFrame)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, 1.0f / moveFrame * nowFrame);
            nowFrame++;
            return;
        }

        // 마지막 위치 보정
        transform.localPosition = targetPos;
        isBlockState = -1;
        nowFrame = 0.0f;
        return;
    }

    private void Snapback()
    {
        if (nowFrame < moveFrame)
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
        isBlockState = -1;
        nowFrame= 0.0f;
        return;
    }

    private void fall()
    {

    }
}