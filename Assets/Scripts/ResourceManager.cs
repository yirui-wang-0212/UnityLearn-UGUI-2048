using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 资源管理类，负责管理加载资源
public class ResourceManager : MonoBehaviour
{
    // 存储 Sprite 图集
    // private static Sprite[] spriteArray;（效率太低，弃用）
    // 字典，键：图片上的数字，值：Sprite
    private static Dictionary<int, Sprite> spriteDic;

    // 类被加载时调用
    static ResourceManager(){

        // 备注：读取单个精灵使用 Load，读取精灵图集使用 LoadAll
        // 只需读取一次
        var spriteArray = Resources.LoadAll<Sprite>("2048Atlas");
        // 将 spriteArray 的数据放入 spriteDic 中
        spriteDic = new Dictionary<int, Sprite>();
        foreach (var item in spriteArray){
            int intSpriteName = int.Parse(item.name);
            spriteDic.Add(intSpriteName, item);
        }
    }

    // 根据数字读取相应的 Sprite
    // 参数：Sprite 表示的数字
    // return：数字对应的 Sprite
    public static Sprite LoadSprite(int number){
        
        // （效率太低，弃用）
        // 根据传入数值（2，4，8，……）设置对应的 Sprite 引用
        // foreach(var item in spriteArray){
        //     if(item.name == number.ToString()){
        //         return item;
        //     }
        // }
        // return null;

        return spriteDic[number];
    }
}
