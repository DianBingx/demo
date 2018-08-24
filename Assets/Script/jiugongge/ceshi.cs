//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;

//public class ceshi : MonoBehaviour
//{
//    float widths, heights;
//    public float x1;
//    public float x2;
//    public float y1;
//    public float y2;

//    private int w;
//    private int h;
//    public bool stop;
//    void Start()
//    {

//    }
//    void OnEnable()
//    {
//        stop = true;
//        Vector2 v = Camera.main.WorldToScreenPoint(this.transform.position);
//        widths = Screen.width / 6;
//        heights = Screen.height / 6;
//        w = (int)(v.x / widths);
//        h = (int)(v.y / heights);
//        //初始化 人物 坐标 对应的 网格 位置，并保存到 网格内 
//        OnInstet(w, h);
//    }
//    //插入数据
//    void OnInstet(int w, int h)
//    {
//        Vector2 VectorGrid = new Vector2(w, h);
//        ArrayList arrlist = GameModel.getInstance().GridList[VectorGrid];

//        if (!arrlist.Contains(transform.name))
//        {
//            GameModel.getInstance().GridList[VectorGrid].Add(transform.name);
//        }
//        else
//        {
//            int Indexs = GameModel.getInstance().GridList[VectorGrid].IndexOf(transform.name);
//            GameModel.getInstance().GridList[VectorGrid].RemoveAt(Indexs);
//        }
//    }

//    bool sortGird(int w, int h)
//    {
//        ArrayList arr = new ArrayList();
//        if (w < 0)
//        {
//            w = 0;
//        }
//        if (h < 0)
//        {
//            h = 0;
//        }
//        if (w > 5)
//        {
//            w = 5;
//        }
//        if (h > 5)
//        {
//            h = 5;
//        }
//        Vector2 VectorGrid = new Vector2(w, h);
//        arr = GameModel.getInstance().GridList[VectorGrid];
//        if (OnSetArmy(arr))
//        {
//            return true;
//        }
//        return false;
//    }


//    bool OnSetArmy(ArrayList arr)
//    {
//        if (arr.Count > 0)
//        {
//            for (int i = 0; i < arr.Count; i++)
//            {
//                if (arr[i] != this.transform.name)//如果不是自己
//                {

//                    stop = false;//停止检测
//                    return true;
//                }
//            }
//        }
//        return false;
//    }
//    //查询数据
//    void OnFindData(int vector)
//    {
//        //根据人物 角度 选择 从 哪个开始
//        switch (vector)
//        {
//            case 0:  //中间 
//                sortGird(w, h);
//                break;
//            case 1:  //左上
//                if (sortGird(w, h))//自己
//                {
//                    break;
//                }

//                if (sortGird(w - 1, h + 1)) { break; }//左上对角 
//                if (sortGird(w - 1, h)) { break; }//左
//                if (sortGird(w, h + 1)) { break; }//上
//                //找出一个 就跳转
//                break;
//            case 2: //右上
//                if (sortGird(w, h)) { break; }//自己
//                if (sortGird(w + 1, h + 1)) { break; }//右上对角
//                if (sortGird(w + 1, h)) { break; }//右
//                if (sortGird(w, h + 1)) { break; }//上 
//                break;
//            case 3:  //左下
//                if (sortGird(w, h)) { break; }//自己
//                if (sortGird(w - 1, h - 1)) { break; }//左下对角
//                if (sortGird(w - 1, h)) { break; }//左
//                if (sortGird(w, h - 1)) { break; }//下
//                break;
//            case 4:  //右下
//                if (sortGird(w, h)) { break; }//自己
//                if (sortGird(w + 1, h - 1)) { break; }//右下对角
//                if (sortGird(w - 1, h)) { break; }//右
//                if (sortGird(w, h - 1)) { break; }//下
//                break;
//            case 5:  //上
//                if (sortGird(w, h)) { break; }//自己
//                if (sortGird(w, h + 1)) { break; }//上 
//                break;
//            case 6:  //右
//                if (sortGird(w, h)) { break; }//自己
//                if (sortGird(w + 1, h)) { break; }//右
//                break;
//            case 7: //下
//                if (sortGird(w, h)) { break; }//自己
//                if (sortGird(w, h - 1)) { break; }//下
//                break;
//            case 8: //左
//                if (sortGird(w, h)) { break; }//自己
//                if (sortGird(w - 1, h)) { break; }//左
//                break;
//            default: break;
//        }

//    }

//    private bool top;
//    private bool bottom;
//    private bool left;
//    private bool right;

//    //判断是否在临界点
//    void OnRectPostion(bool tops = true, bool bottoms = true, bool lefts = true, bool rights = true)
//    {
//        top = tops;
//        bottom = bottoms;
//        left = lefts;
//        right = rights;
//    }


//    //存储网格数据
//    void Storage()
//    {
//        Vector2 v = Camera.main.WorldToScreenPoint(this.transform.position);
//        int w_new = (int)(v.x / widths);
//        int h_new = (int)(v.y / heights);
//        int ww = w, hh = h;
//        if (w_new != w)
//        {
//            if (w_new < w)
//            {
//                int w1 = (int)(v.x) % (int)(widths);
//                if (w1 < x2)
//                {
//                    //进入了下一个格子  存储数据
//                    OnRectPostion();//判断是否在临界点
//                    ww = w_new;
//                    //    print("111");
//                }
//                else
//                {
//                    //偏左 进入临界点
//                    left = false;
//                }
//            }
//            else
//            {
//                int w1 = (int)(v.x) % (int)(widths);
//                if (w1 > x1)
//                { //进入了下一个格子
//                    OnRectPostion();
//                    ww = w_new;
//                    //     print("222");
//                }
//                else
//                {
//                    // 偏右 进入临界点
//                    right = false;
//                }
//            }
//        }
//        /////////////////////////////////////////////////////////////
//        if (h_new != h)
//        {
//            if (h_new < h)
//            {
//                int h1 = (int)(v.y) % (int)(heights);
//                if (h1 < y2)
//                {
//                    //进入了下一个格子
//                    OnRectPostion();
//                    hh = h_new;
//                    //   print("333");
//                }
//                else
//                {
//                    //偏上 进入临界点
//                    top = false;
//                }
//            }
//            else
//            {
//                int h1 = (int)(v.y) % (int)(heights);
//                if (h1 > y1)
//                { //进入了下一个格子
//                    OnRectPostion();
//                    hh = h_new;
//                    //    print("444");
//                }
//                else
//                {
//                    //偏下 进入临界点
//                    bottom = false;
//                }
//            }
//        }
//        ///////////////
//        OnInstet(w, h);//删除老数据
//        OnInstet(ww, hh);//添加新数据
//        w = ww;
//        h = hh; //换新

//    }
//    //查询人物网格内的碰撞
//    void FindGrid()
//    {
//        //如果在上面
//        if (!top)
//        {
//            if (!left)//左上  
//            {
//                OnFindData(1);
//            }
//            else if (!right)//右上
//            {
//                OnFindData(2);
//            }
//            else
//            {   //上
//                OnFindData(5);
//            }

//            return;
//        }

//        //如果在下面
//        if (!bottom)
//        {
//            if (!left)//左下
//            {
//                OnFindData(3);
//            }
//            else if (!right)//右下
//            {
//                OnFindData(4);
//            }
//            else
//            {   //下
//                OnFindData(7);
//            }

//            return;
//        }
//        //如果在左边
//        if (!left)
//        {
//            if (!top)//左上
//            {
//                OnFindData(1);
//            }
//            else if (!bottom)//左下
//            {
//                OnFindData(3);

//            }
//            else  //左边
//            {
//                OnFindData(8);
//            }
//            return;
//        }
//        //如果在右边
//        if (!right)
//        {
//            if (!top)//右上
//            {
//                OnFindData(2);
//            }
//            else if (!bottom)//右下
//            {
//                OnFindData(4);
//            }
//            else  //右边
//            {
//                OnFindData(6);
//            }
//            return;
//        }
//        //不在临界点
//        if (top && bottom && left && right)
//        {
//            OnFindData(0);
//        }
//    }
//    void Update()
//    {
//        //存储坐标 和 找出临界点
//        Storage();
//        ///////////////////////////
//        //判断临界点 的位置，找出 需要 检索的 格子。
//        if (stop)
//        {
//            FindGrid();
//        }
//    }
//}