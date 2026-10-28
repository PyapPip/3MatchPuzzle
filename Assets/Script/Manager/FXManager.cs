using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject matchEffectPrefab;      //이펙트 프리팹을 저장할 변수
    List<GameObject> matchEffectList = new List<GameObject>();  //이펙트 오브젝트를 담을 리스트

    public int countActiveEffect = 0; //활성화된 이펙트의 개수를 세는 변수

    //사용할 이펙트를 미리 만듭니다. (Prefab)
    public GameObject CreateMatchEffect()
    {
        GameObject instans = Instantiate(matchEffectPrefab, this.transform);
        instans.transform.position = new Vector2(-10, 0);
        instans.SetActive(false);
        matchEffectList.Add(instans);
        return instans;
    }

    public void PlayMatchEffect(Vector2 _pos)
    {
        countActiveEffect++;

        if (countActiveEffect > matchEffectList.Count)
        {
            Debug.Log(countActiveEffect);
            GameObject effect = CreateMatchEffect();
            effect.transform.position = _pos;
            effect.SetActive(true);
            effect.GetComponent<Animator>().Play("Match_Pop_Effect");
            return;
        }

        else
        {
            for (int i = 0; i < matchEffectList.Count; i++)
            {
                if (!matchEffectList[i].activeSelf)
                {
                    Debug.Log(countActiveEffect);
                    matchEffectList[i].transform.position = _pos;
                    matchEffectList[i].SetActive(true);
                    matchEffectList[i].GetComponent<Animator>().Play("Match_Pop_Effect");
                    return;
                }
            }
        }
    }
     
    public void FXEnd()
    {
        countActiveEffect--;

        if(countActiveEffect < 0)
         {
            gameManager.ChangeGameState(GameState.settle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
