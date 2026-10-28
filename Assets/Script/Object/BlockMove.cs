using UnityEngine;

public class BlockMove : MonoBehaviour
{
    private int   moveFrame = 0;                 // 이동하는 데 걸리는 프레임
    private float nowFrame = 0.0f;              // 현재 프레임
    private float moveSpeed = 0.0f;
    private Vector3 startPos;
    private Vector3 targetPos;

    private BlockAnimState animState = BlockAnimState.Idle; 

    public enum BlockAnimState
    {
        Idle,
        Swaping,
        Snapback,
        Fall,
        Destroy
    }

    // Update is called once per frame
    void Update()
    {
        if (animState == BlockAnimState.Idle) 
            return;

        switch (animState)
        {
            case BlockAnimState.Swaping:
                {
                    Swaping();
                    return;
                }
            case BlockAnimState.Snapback:
                {
                    Snapback();
                    return;
                }
            case BlockAnimState.Fall:
                {
                    Fall();
                    return;
                }
            case BlockAnimState.Destroy:
                {
                    Destroy();
                    return;
                }
        }  
    }

    /// <summary>
    /// 0 = idle 1 = matched 2 = notMatched 3 = falling
    /// </summary> 
    public void MovePlay(BlockAnimState _moveType, Vector2 _targetPos)
    {
        if(_moveType == BlockAnimState.Swaping)
        { moveFrame = 16; }

        else if (_moveType == BlockAnimState.Snapback)
        { moveFrame = 24; }

        else
        { moveFrame = 0; }

        startPos = transform.position;
        targetPos = new Vector3(_targetPos[0], _targetPos[1]);
        animState = _moveType;
    }

    private void Swaping()
    {
        if (nowFrame < moveFrame)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, 1.0f / moveFrame * nowFrame);
            nowFrame++;
            return;
        }

        //Debug.Log(nowFrame);

        // 마지막 위치 보정
        transform.position = targetPos;
        animState = BlockAnimState.Idle;
        nowFrame = 0.0f;
        callMoveEnd();
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
        
        //Debug.Log(nowFrame);

        transform.position = startPos;
        animState = BlockAnimState.Idle;
        nowFrame = 0.0f;
        callMoveEnd();
        return;
    }

    private void Fall()
    {
        if(this.transform.position != targetPos)
        {
            moveSpeed += 1;
              
            if (moveSpeed > 15)
            {
                moveSpeed = 15;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeed);
            return;
        }

        animState = BlockAnimState.Idle; //check 만들어서 적용 필요
        moveSpeed = 0;
        GetComponent<Block>().fall = 0;
        callMoveEnd();
        return;
    }

    private void Destroy()
    {
        this.GetComponent<Animator>().Play("Block_Destroy");
    }

    //블럭 애니매이션이 끝났을때 호출하기 위한 함수. 애니메이션 이벤트에도 사용.
    private void callMoveEnd()
    {
        if (GetComponentInParent<BlockManager>().moveBlockCount < 0)
        {
            Debug.Log("moveBlockCount < 0");
            return;
        }
        GetComponentInParent<BlockManager>().BlockMoveEnd();
    }
}