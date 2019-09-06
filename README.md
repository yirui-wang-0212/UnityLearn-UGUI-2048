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



### UGUI Draw Call 优化

#### 优化原理

- 在界面中默认一张图片一个 Draw Call（Stats 界面中的 Batches）。

- 一张 Image Source 用于不同的 Image Component （即一张图片使用多次）只调用一次 Draw Call：CPU 找图片的顶点信息、坐标和像素交给 GPU 做渲染。

#### 精灵打包

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

#### 图集

美工制作图片时，尽量将需要在同个界面显示的小图做到一张大图中。

##### 手动切割 Sprite

1. Inspector - Sprite Mode：Multiple
2. 点击 Inspector - Sprite Editor
3. 在 Sprite Editor 窗口中每切完一张图片之后点击 Apply
4. 切好的图片便会在 Assets 中原图下面

##### 自动切割 Sprite

（判断 阿尔法 通道）

Sprite Editor  - Type：Automatic

##### 按格子大小切割 Sprite

Sprite Editor  - Type：Grid By Cell Size

##### 按格子数目切割 Sprite

Sprite Editor  - Type：Grid By Cell Count