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
    public int[,] mapIsOccupied;
    private gridController[,] map;
    private bool playerTurn;

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
        playerTurn = true;
        CreateBG();
    }

    //*-1 = 遊戲繼續, 0 = 平手, 1 = 玩家贏, 2 = CPU贏
    private int checkWin(int[,] map){
        int temp = 0;

        //簡查直線
        for(int x = 0; x < 3; ++x){
            if(map[x, 0] == 0) continue;

            temp = map[x, 0];
            if(temp == (map[x, 0] & map[x, 1] & map[x, 2])) return temp;
        }

        //簡查橫線
        for(int y = 0; y < 3; ++y){
            if(map[0, y] == 0) continue;

            temp = map[0, y];
            if(temp == (map[0, y] & map[1, y] & map[2, y])) return temp;
        }

        //檢查對角線
        if(map[1, 1] == 0) return -1;
        temp = map[1, 1];
        if(temp == (map[0, 0] & map[1, 1] & map[2, 2])) return temp;
        else if(temp == (map[0, 2] & map[1, 1] & map[2, 0])) return temp;

        //檢查平手
        //共5個X, 4個O
        temp = 0;
        for(int x = 0; x < 3; ++x){
            for(int y = 0; y < 3; ++y){
                temp += map[x, y];
            }
        }
        if(temp == 13) return 0;

        return -1;
    }

    private void playerWin(){
        Debug.Log("player win");
    }

    private void enemyWin(){
        Debug.Log("enemy win");
    }

    private void draw(){
        Debug.Log("draw");
    }

    private void switchTurn(){
        playerTurn = !playerTurn;

        if(!playerTurn)
            EnemyController.instance.minimax(mapIsOccupied);
    }

    public void updateMap(int x, int y, bool isPlayer){
        Instantiate(isPlayer?cross:circle, new Vector3(x, y, 0), Quaternion.identity, p);
        mapIsOccupied[x, y] = isPlayer?1:2;
        map[x, y].disable();

        int winner = checkWin(mapIsOccupied);
        if(winner == 1) playerWin();
        else if(winner == 2) enemyWin();
        else if(winner == 0) draw();
        else switchTurn();
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

    public void printMap(int[,] map){
        string s = "";
        for(int y = 2; y >= 0; --y){
            for(int x = 0; x < 3; ++x){
                s += map[x, y] + " ";
            }
            s += "\n";
        }

        Debug.Log(s);
    }
}
