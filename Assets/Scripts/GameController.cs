using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init(){

        for(int i = 0; i < 4; ++i){
            for(int j = 0; j < 4; ++j){
                CreateSprite();
            }
        }
    }

    private void CreateSprite(){

    }
}
