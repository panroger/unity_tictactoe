using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject Grid;
    public Transform Board;
    public Color gridLight;
    public Color gridDark;  //*default color
    public Transform empty;
    private Transform p; //*empty's transform

    public Transform circle; //*CPU
    public Transform cross; //*player

    //*true = occupied
    private bool[,] mapIsOccupied;
    private gridController[,] map;

    void Awake()
    {
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);

        mapIsOccupied = new bool[3,3];
        map = new gridController[3, 3];
        CreateBG();
    }

    public void updateMap(float x, float y, bool isPlayer){
        Instantiate(isPlayer?cross:circle, new Vector3(x, y, 0), Quaternion.identity, p);
        mapIsOccupied[(int)x, (int)y] = true;
        map[(int)x, (int)y].disable();
    }

    //*create board and grid
    private void CreateBG(){
        p = Instantiate(empty, Vector3.zero, Quaternion.identity);
        Instantiate(Board, new Vector3(1, 1, 0), Quaternion.identity, p);

        for(int i = 0; i < 3; ++i){
            for(int j = 0; j < 3; ++j){
                var g = Instantiate(Grid, new Vector3(i, j, 0), Quaternion.identity, p);
                g.name = $"grid {i}, {j}";
                map[i, j] = g.GetComponentInChildren<gridController>();

                if((i+j) % 2 != 0)
                    g.GetComponent<SpriteRenderer>().color = gridLight;
            }
        }
    }
}
