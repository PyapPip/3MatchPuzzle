using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector2Int boardPos;
    public int species, fall = 0;
    public bool isMoved = false;    // 블럭이 이동하여 매치 가능성을 확인했는지 여부를 나타내는 변수

    private void Update()
    {
        this.gameObject.name = $"Block_{boardPos.x}_{boardPos.y}_{fall}";
    }
}