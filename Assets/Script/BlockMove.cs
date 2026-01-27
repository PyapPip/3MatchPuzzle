using System.Collections;
using UnityEngine;

public class BlockMove : MonoBehaviour
{
    private int moveFrame = 0;                 // 이동하는 데 걸리는 프레임
    private float nowFrame = 0.0f;              // 현재 프레임
    private Vector3 startPos;
    private Vector3 targetPos;

    public BlockAnimState animState; 

    public enum BlockAnimState
    {
        Idle,
        Swaping,
        Snapback,
        Falling
        //,Destroyed  
    }

    // Update is called once per frame
    void Update()
    {
        if (animState == BlockAnimState.Idle)
            return;

        //스위치문으로 변경
        if (animState == BlockAnimState.Swaping)
        {
            MoveBlock();
        }

        else if (animState == BlockAnimState.Snapback)
        {
            Snapback();
        }

        else if (animState == BlockAnimState.Falling)
        {
            fall();
        }
    }

    /// <summary>
    /// 0 = idle 1 = matched 2 = notMatched 3 = falling
    /// </summary> 
    public void MoveStart(int _isBlockState, int[] _targetPos)
    {
        animState = (BlockAnimState)_isBlockState;

        if(_isBlockState == 0)
        { moveFrame = 24; }

        else if (_isBlockState == 1)
        { moveFrame = 16; }

        else
        { moveFrame = 0; }

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
        animState = BlockAnimState.Idle;
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
        animState = BlockAnimState.Idle;
        nowFrame = 0.0f;
        return;
    }

    private void fall()
    {
        //시작지점 -> 가속도 -> 더해졌을때 목표지점보다 클때 목표지점으로 지정
    }
}