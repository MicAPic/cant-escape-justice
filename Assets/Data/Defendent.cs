using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defendent : MonoBehaviour
{
    public StringsRandomData charges;
    public StringsRandomData shedule;
    public StringsRandomData sheduleTime;
    public ImageRandomData items;          

    public string Charge { get; set; }
    public string Shedule { get; set; }
    public List<Sprite> Items { get; set; }

    public void Awake()
    {
        //setting
    }
}
