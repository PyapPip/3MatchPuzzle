using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//클래스의 이름을 조금 더 명확하게 할 필요 있음.
public class Block : MonoBehaviour
{
    public int x, y, species, fall = 0;
    public Vector2Int boardPos;
}
