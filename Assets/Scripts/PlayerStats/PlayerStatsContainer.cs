using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerStatsContainer
{   
    Font arial = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
    GameObject parent;
    GameObject background;
    GameObject textObject;
    Canvas textCanvas;
    Text text;
    RectTransform rectTransform;
    RectTransform textTransform;
    private static Sprite[] statsSprites;
    private PlayerStats playerStats;
    int position;
    public PlayerStatsContainer(int position, GameObject parent, PlayerStats playerStats){
        this.position = position;
        this.parent = parent;
        this.playerStats = playerStats;
        loadSprite();
        renderObject();
    }

    private void loadSprite(){
        statsSprites = Resources.LoadAll<Sprite>("menuBlock");   
    }


    private void renderObject(){
        background = new GameObject("BackGround " + position);
        rectTransform = background.AddComponent<RectTransform>();
        rectTransform.localPosition = new Vector3((-Screen.width/2)+80, 150+(-position*100),0);
        rectTransform.sizeDelta = new Vector2(4, 4);
        rectTransform.localScale = new Vector3(20,20,1);   
        textCanvas = background.AddComponent<Canvas>();
        textCanvas.renderMode = RenderMode.WorldSpace;
        textCanvas.sortingOrder = 10;
        Image image = background.AddComponent<Image>();
        image.sprite = statsSprites[0]; 
        textObject = new GameObject("Text " + position);
        textTransform = textObject.AddComponent<RectTransform>();
        textTransform.SetParent(rectTransform, false);
        text = textObject.AddComponent<Text>();
        text.text = playerStats.getName()+ "\nScore: " + playerStats.getPoints();
        text.fontSize = 100;
        text.alignment = TextAnchor.MiddleCenter; 
        text.color = new Color(0, 0, 0);
        text.font = arial;
        textTransform.sizeDelta = new Vector3(2000,2000,0);
        textTransform.localScale = new Vector3((float)0.006, (float)0.006, (float)0.006);
        textTransform.localPosition = new Vector3(0,0,0);
    }
    public void setParent(Transform parent){
        background.transform.SetParent(parent);

    }
    public void updateScores()
    {
        text.text = playerStats.getName() + "\nScore: " + playerStats.getPoints();
    }



}
