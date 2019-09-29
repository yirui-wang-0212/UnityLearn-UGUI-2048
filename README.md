# 2048-UGUI
Unity Learning: UGUI



###  常见 UI 元素在代码中的创建

1. 通过 GameObject 创建

   ```c#
   private void CreateSprite(int r, int c){
   
           // UI 创建三步骤：
           // 1. 创建游戏对象，并赋予相应的名字
           GameObject go = new GameObject(r.ToString() + c.ToString());
           // 2. 添加 Image 组件
           go.AddComponent<Image>();
           // 3. 设置当前 GameObject 为创建对象的父节点
           // 创建的游戏对象 scale 默认为 1，false 表示不使用世界坐标，1 表示相对于父节点
           go.transform.SetParent(this.transform, false);
   }
   ```

2. 先创建预制件，再通过预制件创建，最后```SetParent```



### Grid Layout Group (Script)

- Layout - Grid Layout Group 表格布局
- 自动排列，控制子元素的大小和位置，子元素不能更改。



### UGUI Draw Call 优化

#### 优化原理

- 在界面中默认一张图片一个 Draw Call（Stats 界面中的 Batches）。

- 一张 Image Source 用于不同的 Image Component （即一张图片使用多次）只调用一次 Draw Call：CPU 找图片的顶点信息、坐标和像素交给 GPU 做渲染。

#### 优化方法1：精灵打包

做界面时使用小图，在项目发布时引擎会根据精灵 Packing Tag 自动将小图合并在一张大图集中，从而减少 Draw Calls，减少 GPU 物体次数。

1. 选中几张小图，Inspector 中 Packing Tag：给大图取名字。（2019 版本不知道在哪里）
   - Unity会尽量打包，若选中的小图太多，会打包成多张大图。
   - 图片类型（ARGB、RGB）、压缩方式一样才能被打包到一起。
   - 一张需要在多个界面使用的图片，不要打包，否则在另外的界面使用时需要加载大图。
2. Edit - Project Settings - Editor - Sprite Package - Mode :
   - Enable For Builds：Build 打包
   - Always Enabled：每次运行打包
3. 选择 Always Enabled，运行。
4. Windows - Sprite Packer 中能看到打完包的大图。
5. 查看 Stats 界面中的 Batches，发现变小。

#### 优化方法2：图集

美工制作图片时，尽量将需要在同个界面显示的小图做到一张大图中。

- **手动切割 Sprite**
  1. Inspector - Sprite Mode：Multiple
  2. 点击 Inspector - Sprite Editor
  3. 在 Sprite Editor 窗口中每切完一张图片之后点击 Apply
  4. 切好的图片便会在 Assets 中原图下面

- **自动切割 Sprite**

  （判断 阿尔法 通道）

  Sprite Editor  - Type：Automatic

- **按格子大小切割 Sprite**

  Sprite Editor  - Type：Grid By Cell Size

- **按格子数目切割 Sprite**

  Sprite Editor  - Type：Grid By Cell Count



### 2048

#### 1 创建 4 * 4 方格

1. 创建 GameController 脚本，用于控制游戏逻辑（生成方格）。
2. 创建 NumberSprite 脚本， 用于定义 Sprite 行为（设置图片……）。
3. 创建 ResourceManager 类，用于读取资源（根据名称加载 Sprite）。

#### 2 生成新数字

1. 导入算法类：GameCore、Location、MoveDirection。

2. 修改算分类 GameCore 中生成数字方法（返回位置与数字）。

   GenerateNumber() --> GenerateNumber(out Location? loc,out int? newNumber)

3. 在 GameController 脚本中，添加生成新数字方法。

   修改创建精灵方法 CreateSripte（创建精灵时，存储精灵行为脚本的引用）。

   （存储创建的 NumberSprite 脚本类 Component 对象的引用，在每个对象创建后立即存入）

4. 在 Init() 中调用两次 GenerateNewNumber()，作为游戏开始时界面上的两个数字。

#### 3 更新界面

#### 4 获取输入，移动地图



#### 其他

- 可空值类型

```c#
private Random random;
/// <summary>
/// 生成新数字
/// </summary>
public void GenerateNumber(out Location? loc,out int? newNumber)
{
    CalculateEmpty();

    if (emptyLOC.Count > 0)
    {
        int emptyLocIndex = random.Next(0, emptyLOC.Count);//0,15

        loc = emptyLOC[emptyLocIndex];//有空位置的list  3

        newNumber = map[loc.Value.RIndex, loc.Value.CIndex] = random.Next(0, 10) == 1 ? 4 : 2;

        //将该位置清除
        emptyLOC.RemoveAt(emptyLocIndex);
    }
    else
    {
        // int? 是c#的可空值类型，可以给它赋 null
        // int? a = null;
        // int? 不再是原来的值类型， 如果需要获取值类型数据，使用 a.Value
        newNumber = null;
        loc = null;
    }
}
```