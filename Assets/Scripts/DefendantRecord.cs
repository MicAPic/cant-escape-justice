using System.Collections.Generic;
using UnityEngine;

public class DefendantRecord /*: MonoBehaviour*/
{
    public Color color;

    public bool isGuilty;
    //Обвинение
    public string charge;
    public string timeOfCrime;
    public Sprite weapon;

    //Улики
    public string schedule;
    public List<Sprite> items;
    //Лицо
    public Sprite hair;
    public Sprite eye;
    public Sprite mouth;
}
