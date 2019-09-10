using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // 初始化
    private void Init(){

        for(int r = 0; r < 4; ++r){
            for(int c = 0; c < 4; ++c){
                CreateSprite(r, c);
            }
        }
    }

    // 创建一个 Sprite
    private void CreateSprite(int r, int c){

        // UI 创建三步骤：
        // 1. 创建游戏对象，并赋予相应的名字
        GameObject go = new GameObject(r.ToString() + c.ToString());
        // 2. 添加 Image 组件
        go.AddComponent<Image>();
        // NumberSprite 脚本： Sprite 的行为
        NumberSprite action = go.AddComponent<NumberSprite>(); // Awake 立即执行（创建之后立即执行），Start 下一阵帧执行
        // 设置初始图片
        action.SetImage(0);
        // 3. 设置当前 GameObject 为创建对象的父节点
        // 创建的游戏对象时 scale 默认为 1，false 表示不使用世界坐标，1 表示相对于父节点
        go.transform.SetParent(this.transform, false);
    }
}
