using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject Grid;
    public Transform Board;
    public Color gridLight;
    public Color gridDark;  //default color
    public Transform empty;

    void Awake()
    {
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);

        CreateBG();
    }

    //create board and grid
    private void CreateBG(){
        var p = Instantiate(empty, Vector3.zero, Quaternion.identity);
        Instantiate(Board, new Vector3(1, 1, 0), Quaternion.identity, p);

        for(int i = 0; i < 3; ++i){
            for(int j = 0; j < 3; ++j){
                var g = Instantiate(Grid, new Vector3(i, j, 0), Quaternion.identity, p);
                g.name = $"grid {i}, {j}";

                if((i+j) % 2 != 0)
                    g.GetComponent<SpriteRenderer>().color = gridLight;
            }
        }
    }
}
