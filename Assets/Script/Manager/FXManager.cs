using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{ 
    List<GameObject> matchEffectList = new List<GameObject>(); //이펙트 오브젝트를 담을 리스트
    GameObject matchEffectPrefab; //이펙트 프리팹을 저장할 변수

    //사용할 이펙트를 미리 만듭니다. (Prefab)
    public void CreateMatchEffect()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject instans = Instantiate(matchEffectPrefab);
            instans.transform.position = new Vector2(-10, 0);
            instans.SetActive(false);
            matchEffectList.Add(instans);
        }
    }

    public void PlayMatchEffect(Vector2 _pos)
    {
        for (int i = 0; i < matchEffectList.Count; i++)
        {
            if (!matchEffectList[i].activeSelf)
            {
                matchEffectList[i].transform.position = _pos;
                matchEffectList[i].SetActive(true);
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
