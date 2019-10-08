using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Console2048;
using UnityEngine.EventSystems;
using MoveDirection = Console2048.MoveDirection;

public class GameController : MonoBehaviour, IPointerDownHandler, IDragHandler{

    // 游戏算法类对象
    // 用于使用 GameCore 类中的算法
    private GameCore core;
    // 2维精灵行为类数组（4*4）
    // 用于存储创建的 NumberSprite 脚本类 Component 对象的引用，在每个对象创建后立即存入
    // 方便游戏中更改其 image （2，4，8，16，……）
    private NumberSprite[,] spriteActionArray;

    // Start is called before the first frame update
    void Start()
    {
        // 创建游戏算法类对象
        core = new GameCore();
        // 创建2维精灵行为类数组（4*4）
        // 用于存储创建的 NumberSprite 脚本类 Component 对象的引用
        spriteActionArray = new NumberSprite[4, 4];

        Init();
    }

    // 初始化，创建 4*4 的 Sprite，并赋予相应名字：00，01，02，03，……，30，31，32，33
    private void Init(){

        for(int r = 0; r < 4; ++r){
            for(int c = 0; c < 4; ++c){
                CreateSprite(r, c);
            }
        }

        // 游戏开始在两个不同位置生成 2 或 4
        GenerateNewNumber();
        GenerateNewNumber();
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
        // 所以在 NumberSprite.cs 中 image = GetComponent<Image>(); 应该写在 Awake 中 而不是 Start 中。
        NumberSprite action = go.AddComponent<NumberSprite>(); 
        // 将引用存储到2维精灵行为类数组（4*4）中，
        // 存储创建的 NumberSprite 脚本类 Component 对象的引用，在每个对象创建后立即存入
        spriteActionArray[r,c] = action;
        // 设置初始图片，没有数字的图片
        action.SetImage(0);
        // 3. 设置当前 GameObject：GameController 为创建对象的父节点
        // 创建的游戏对象时 scale 默认为 1，false 表示不使用世界坐标，1 表示相对于父节点
        go.transform.SetParent(this.transform, false);
    }

    // 随机生成 2 或 4
    private void  GenerateNewNumber(){

        // 用于存储返回的位置
        Location? loc;
        // 用于存储返回的数字
        int? number;

        // 调用 GameCore 中生成 2 或 4 的方法
        core.GenerateNumber(out loc, out number);
        // 根据位置获取精灵行为脚本对象引用
        spriteActionArray[loc.Value.RIndex, loc.Value.CIndex].SetImage(number.Value);
    }

    private void Update(){

        // 如果地图有更新
        if(core.IsChange){
            // 更新界面
            UpdateMap();
            // 产生新数字
            GenerateNewNumber();
            // 判断游戏是否结束

            core.IsChange = false;
        }
    }

    // 根据游戏核心算法类中存储 Map（4*4）更新 spriteActionArray（4*4）
    // 使得玩家操作并计算后的 4*4 得以更新到游戏界面
    private void UpdateMap(){

        for (int r = 0; r < 4; ++r){
            for (int c = 0; c < 4; ++c){
                spriteActionArray[r, c].SetImage(core.Map[r, c]);
            }
        }
    }

    // 以下是输入逻辑：移动操作的代码

    // 记录按下时的位置
    private Vector3 startPoint;
    // 记录是否按下
    private bool isDown = false;

    // 当按下当前 GameObject 时执行
    public void OnPointerDown(PointerEventData eventData){
        // 记录按下时的位置
        startPoint = eventData.position;
        // 记录按下
        isDown = true;
    }

    // 当按下拖拽时每帧执行
    public void OnDrag(PointerEventData eventData){

        // 如果 isDown == false 不做以下操作
        if (isDown == false) return;

        Vector2 offset = eventData.position - (Vector2)startPoint;
        float x = Mathf.Abs(offset.x);
        float y = Mathf.Abs(offset.y);

        MoveDirection? dir = null;
        // x > y 判断为是水平方向移动
        // x >= 50 是一个阈值，如果移动的距离小于 50 像素，则判定为无效输入，不执行操作
        if(x > y && x >= 50){
            // 判断左移还是右移
            dir = offset.x > 0 ? MoveDirection.Right : MoveDirection.Left;
        }

        // x < y 判断为是垂直方向移动
        // y >= 50 是一个阈值，如果移动的距离小于 50 像素，则判定为无效输入，不执行操作
        if(x < y && y >= 50){
            // 判断上移还是下移
            dir = offset.y > 0 ? MoveDirection.Up : MoveDirection.Left;
        }

        if (dir != null){
            // 进行移动操作
            core.Move(dir.Value);
            // 将 isDown 置为 false，防止拖动的时候每帧都执行
            isDown = false;
        }
    }
}
