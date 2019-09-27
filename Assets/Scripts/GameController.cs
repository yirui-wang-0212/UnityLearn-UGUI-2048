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

    // 初始化，创建 4 * 4 的  Sprite
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
        // 在 4 * 4 的 GameObject 上添加 NumberSprite 脚本组件：用于 Sprite 的行为
        // Awake 立即执行（创建之后立即执行），Start 下一帧执行，
        // 因为在当前脚本中的 Start 会调用当前函数，并且在下一行的 action.SetImage(0); 会访问 NumberSprite 中的 image 并对其进行修改，
        // 所以在 NumberSprite.cs 中 image = GetComponent<Image>(); 应该写在 Awake 中 而不是 Starr 中。
        NumberSprite action = go.AddComponent<NumberSprite>(); 
        // 设置初始图片，没有数字的图片
        action.SetImage(0);
        // 3. 设置当前 GameObject：GameController 为创建对象的父节点
        // 创建的游戏对象时 scale 默认为 1，false 表示不使用世界坐标，1 表示相对于父节点
        go.transform.SetParent(this.transform, false);
    }
}
