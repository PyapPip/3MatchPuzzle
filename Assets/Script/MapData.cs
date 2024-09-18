using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapData : MonoBehaviour
{
    public List<int[,]> Map = new List<int[,]>();

    public int[,] Map1 = new int[5, 5]
    {
        { 0, 1, 2, 1, 0},
        { 1, 2, 0, 1, 3},
        { 3, 0, 3, 3, 1},
        { 3, 2, 0, 2 ,1},
        { 0, 3, 2, 1, 0}
    };


    private void Start()
    {
        Map.Add(Map1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
