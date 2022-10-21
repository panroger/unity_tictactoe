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

    //*0 = no, 1 = player, 2 = CPU
    private int[,] mapIsOccupied;
    private gridController[,] map;

    void Awake()
    {
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);

        mapIsOccupied = new int[3,3];
        map = new gridController[3, 3];
        CreateBG();
    }

    public bool checkWin(){
        int temp;

        //簡查直線
        for(int x = 0; x < 3; ++x){
            if(mapIsOccupied[x, 0] == 0) continue;

            temp = mapIsOccupied[x, 0];
            if(temp == (mapIsOccupied[x, 0] & mapIsOccupied[x, 1] & mapIsOccupied[x, 2])) return true;
        }

        //簡查橫線
        for(int y = 0; y < 3; ++y){
            if(mapIsOccupied[0, y] == 0) continue;

            temp = mapIsOccupied[0, y];
            if(temp == (mapIsOccupied[0, y] & mapIsOccupied[1, y] & mapIsOccupied[2, y])) return true;
        }

        //檢查對角線
        if(mapIsOccupied[1, 1] == 0) return false;
        temp = mapIsOccupied[1, 1];
        if(temp == (mapIsOccupied[0, 0] & mapIsOccupied[1, 1] & mapIsOccupied[2, 2])) return true;
        else if(temp == (mapIsOccupied[0, 2] & mapIsOccupied[1, 1] & mapIsOccupied[2, 0])) return true;

        return false;
    }

    public void updateMap(float x, float y, bool isPlayer){
        Instantiate(isPlayer?cross:circle, new Vector3(x, y, 0), Quaternion.identity, p);
        mapIsOccupied[(int)x, (int)y] = isPlayer?1:2;
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
