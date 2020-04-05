using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//在资源面板右键Create，创建该类对应的Asset文件
[CreateAssetMenu(fileName = "NewBulletArray", menuName = "创建/子弹列表")]
public class BulletArray : ScriptableObject
{
  [Header("子弹列表")]
  public GameObject[] bulletArray;
}