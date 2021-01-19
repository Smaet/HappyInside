using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAgitCharacter : MonoBehaviour
{
    [SerializeField]
    public string name { private set; get; }
    [SerializeField]
    public int level { private set; get; }
    [SerializeField]
    public string passiveSkill_1 { private set; get; }
    [SerializeField]
    public string passiveSkill_2 { private set; get; }
    [SerializeField]
    public string passiveSkill_3 { private set; get; }

    public virtual void Init()
    {

    }
}
