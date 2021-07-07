using UnityEngine;
using UnityEngine.UI;

public class PlayerStats{
    private int points;
    private string name;
    private int id;
    public PlayerStats(string name, int id){
        this.name = name;
        this.id = id;
        points = 0;
    }
    public void incrementPoints(int points){
        this.points += points;
    }
    public void decrementPoints(int points){
        this.points -= points;
    }

    public int getPoints(){
        return points;
    }
    public string getName(){
        return name;
    }
    public int getId()
    {
        return id;
    }


}