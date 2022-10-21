using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;

    void Awake()
    {
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this);
    }

    //*return score
    //*cpu turn
    private int maximizing(int[,] map, int a, int b){
        int winner = checkWin(map);
        switch (winner){
            case 0: return 0;
            case 1: return -1;
            case 2: return 1;
        }

        int score = -2;
        for(int i = 0; i < 3; ++i){
            for(int j = 0; j < 3; ++j){
                if(map[i, j] != 0) continue;

                map[i, j] = 2;
                score = Math.Max(score, minimizing(map, a, b));
                map[i, j] = 0;

                //*ab-pruning
                if(score >= b) return score;
                a = Math.Max(score, a);
            }
        }

        return score;
    }

    //*return score
    //*player turn
    private int minimizing(int[,] map, int a, int b){
        int winner = checkWin(map);
        switch (winner){
            case 0: return 0;
            case 1: return -1;
            case 2: return 1;
        }

        int score = 2;
        for(int i = 0; i < 3; ++i){
            for(int j = 0; j < 3; ++j){
                if(map[i, j] != 0) continue;

                map[i, j] = 1;
                score = Math.Min(score, maximizing(map, a, b));
                map[i, j] = 0;

                //*ab-pruning
                if(score <= a) return score;
                b = Math.Min(score, b);
            }
        }

        return score;
    }

    //*no return, just move
    //*in map, 0 = empty, 1 = player, 2 = cpu
    //*為了拿到座標, 第一層的maximizing在這做
    public void minimax(int[,] map){
        int score = -2, maxScore = -2, res_i = 0, res_j = 0, a = -2, b = 2;

        for(int i = 0; i < 3; ++i){
            for(int j = 0; j < 3; ++j){
                if(map[i, j] != 0) continue;

                map[i, j] = 2;
                score = Math.Max(score, minimizing(map, a, b));
                map[i, j] = 0;

                if(maxScore < score){
                    maxScore = score;
                    res_i = i;
                    res_j = j;
                }

                //*ab-pruning
                if(score >= b){
                    //break double loop
                    i = 3;
                    break;
                }
                a = Math.Max(score, a);
            }
        }

        //Debug.Log($"i:{res_i}, j:{res_j}");
        GameManager.instance.updateMap(res_i, res_j, false);
    }

    //*return :-1 = 遊戲繼續, 0 = 平手, 1 = 玩家贏, 2 = CPU贏
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
}
