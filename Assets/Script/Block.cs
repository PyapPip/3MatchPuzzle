using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int x, y, species;
    public int isMatchChack = -1;   //-1.unconfirmed 0.not match 1.width 2.length 3.both
}
