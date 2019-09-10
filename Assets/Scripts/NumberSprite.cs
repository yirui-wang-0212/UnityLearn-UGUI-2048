using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 附加到每个方格中，用于定义方格行为
public class NumberSprite : MonoBehaviour
{

    private Image image;

    // 找 Image 组件，以供更改 Sprite 的引用
    // 不是 Start，Start 报错，在 SetImage(0) 之后执行
    private void Awake(){

        image = GetComponent<Image>();
    }

    // 根据传入数值（2，4，8，……）设置对应的 Sprite 引用
    public void SetImage(int number){
        // 2 ==> Sprite ==> 将 Sprite 引用到 Image 中
        // 调用 ResourceManager 的 LoadSprite 方法
        image.sprite = ResourceManager.LoadSprite(number);
    }

    //移动 合并 生成效果
}
