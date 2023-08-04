using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class BestScore
{
    string name;
    int score;

    public string Name { get => name; set => name = value; }
    public int Score { get => score; set => score = value; }
}
