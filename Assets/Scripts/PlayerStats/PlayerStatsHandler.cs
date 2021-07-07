using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class PlayerStatsHandler : MonoBehaviour
{
    Dictionary<int,PlayerStats> players = new Dictionary<int , PlayerStats>();
    Dictionary<int,PlayerStatsContainer> playerStatsContainers = new Dictionary<int, PlayerStatsContainer>();
    GameObject playerStatsHandler;
    private Canvas canvas;
    int totalPlayers = 0;
    // Start is called before the first frame update
    void Awake(){
        playerStatsHandler = new GameObject("playerStatsHandler");
        
        canvas = playerStatsHandler.AddComponent<Canvas>();
    }
    private void Update()
    {
        foreach(PlayerStats player in players.Values){
            playerStatsContainers[player.getId()].updateScores();
        }
    }
    public void incrementPlayerPoints(int id, int points)
    {
        players[id].incrementPoints(points);
    }
     public void decrementPlayerPoints(int id, int points)
    {
        players[id].decrementPoints(points);
    }




    void Start()
    {
        
        playerStatsHandler.AddComponent<CanvasScaler>();
        playerStatsHandler.AddComponent<GraphicRaycaster>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        canvas.sortingOrder = 20;
        canvas.pixelPerfect = true;
    }
    public void addPlayer(string name, int id){
        
        PlayerStats newPlayer =  new PlayerStats(name, id); 
        players.Add(id , newPlayer);
        PlayerStatsContainer playerStatsContainer = new PlayerStatsContainer(totalPlayers, playerStatsHandler, players[id]);
        playerStatsContainer.setParent(canvas.transform);
        playerStatsContainers.Add(id, playerStatsContainer);
        totalPlayers++;
    }

}
