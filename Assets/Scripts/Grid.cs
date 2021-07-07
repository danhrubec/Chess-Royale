 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using static System.Random;
using UnityEngine.AI;
using UnityEngine.UI;

public class Grid
{
    // all colors should be readonly and static to keep things consistent
    public static readonly Color TILE_GREEN = new Color(0f, 0.85f, 0f,0.9f);
    public static readonly Color TILE_RED = new Color(0.9f, 0f, 0f,0.9f);
    public static readonly Color WHITE = new Color(255f, 255f, 255f,1);
    public static readonly Color BLACK = new Color(30f, 30f, 30f,1);
    public static readonly Color RED = new Color(236f, 19f, 19f,1);
    public static readonly Color GREEN = new Color(11f, 142f, 20f,1);
    public static readonly Color BLUE = new Color(0f, 92f, 230f,1);

    //Dimensions for the map, intialized at constructor
    private int width;
    private int height;
    private float cellSize;
    public const int MAXPLAYERS = 4;
    private int activePlayer = 0;
    public int rotations = 0;
    private int completedRotations = 0;
    private int decreaseSize = 3;


    public int boardSize = 24;
    public int originalBoardSize;

    //matrix for the map, again initialized at constructor
    private int[,] gridArray;
    public LayerMask walls;
    public GameObject[,] objects;
    private GameObject invisibleWallContainer;
    private GameObject floorContainer;
    public GameObject[] player = new GameObject[MAXPLAYERS];
    private GameObject playerStats;
    private PlayerStatsHandler playerStatsHandler;
    private GameObject wallsContainer;
    public Sprite [] tileSprites;
    private Sprite [] piecesTiles;
    private Dictionary<string, int> tileColor = new Dictionary<string, int>();
    enum tileType {BLACK_TILE, WHITE_TILE};
    
    public Grid(float cs, string path)
    {
        cellSize = cs;
        decreaseSize = GoBetween.shrinkSpeed;
        //loads from the map file
        //loadMapFile(path);
        initGridArray(GoBetween.boardSize);
        //creates containers for objects
        createContainers();
        //loads sprites into game
        loadResources();
        //loads player into the game
        playerStatsHandler =  playerStats.AddComponent<PlayerStatsHandler>();
        for(int i = 0; i < MAXPLAYERS; i++)
        {
            player[i] = createPlayer(i);
            playerStatsHandler.addPlayer("Player " + (i+1), i);
        }
        //sets the board and texture objects
        initializeBoard();
        //creates invisible walls around the grid
        renderInvisibleWalls();
        Debug.Log("red"+GoBetween.redSpawnDiv);
        Debug.Log("green"+GoBetween.greenSpawnDiv);
        Debug.Log("size"+GoBetween.boardSize);
        //assign the random green spots
        generateGreenSpots(boardSize,((boardSize*boardSize)/GoBetween.greenSpawnDiv)+1);

        //assign the random red spots
        generateRedSpots(boardSize, ((boardSize*boardSize)/GoBetween.redSpawnDiv)+1);
        setCamera();

        Debug.Log("Grid initialized");
    }
    
    private void setCamera(){
        GameObject camera = GameObject.Find("Main Camera");
        camera.transform.position = new Vector3(12, 11, -10);
        Camera cam = camera.GetComponent<Camera>();
        cam.orthographicSize = 14f;
    }

    public int getMaxPlayers()
    {
        return MAXPLAYERS;
    }

    public PlayerStatsHandler getPlayerStatsHandler()
    {
        return playerStatsHandler;
    }

    private void generateRedSpots(int boardSize,int numSpots)
    {

        //repeat for a given parameter number of times, - determines how many green spots we have
        for (int i = 0; i < numSpots; i++)
        {

            int randx;
            int randy;
            bool result = true;
            //keep generating random numbers until we reach one that isnt on a player currently
            do
            {
                randx = Random.Range(0, boardSize);
                randy = Random.Range(0, boardSize);
                result = checkPlayerOnTile(randx, randy);

            } while (result == true);

            GameObject currTile = objects[randx, randy];
            //if its a black tile, switch it to white so that the color can show up
            SpriteRenderer rend = currTile.GetComponent<SpriteRenderer>();
            if (rend.sprite == (Sprite)tileSprites[0])
            {
                rend.sprite = (Sprite)tileSprites[1];
            }

            rend.color = TILE_RED;


        }
    }


    private void generateGreenSpots(int boardSize, int numSpots)
    {
        //repeat for a given parameter number of times, - determines how many green spots we have
        for(int i = 0; i < numSpots;i++ )
        {

            int randx;
            int randy;
            bool result = true;
            //keep generating random numbers until we reach one that isnt on a player currently
            do
            {
                randx = Random.Range(0, boardSize);
                randy = Random.Range(0, boardSize);
                result = checkPlayerOnTile(randx, randy);

            } while (result == true);

            GameObject currTile = objects[randx, randy];
            //if its a black tile, switch it to white so that the color can show up
            SpriteRenderer rend = currTile.GetComponent<SpriteRenderer>();
            if (rend.sprite == (Sprite)tileSprites[0])
            {
                rend.sprite = (Sprite)tileSprites[1];
            }

            rend.color = TILE_GREEN;


        }
        
    }
    private bool checkPlayerOnTile(int x, int y)
    {
        for (int i = 0; i < MAXPLAYERS; i++)
        {
            GameObject currPlayer = player[i];
            Vector3 pPosition = currPlayer.transform.position;
            Vector3 suggestedPosition = new Vector3(x, y, 0);

            if(pPosition == suggestedPosition)
            {
                return true;
            }
        }


        return false;
    }
    private GameObject createPlayer(int pIndex)
    {
        Texture2D tex = new Texture2D(100, 100);
        GameObject player = new GameObject("Player" + (pIndex + 1).ToString(), typeof(SpriteRenderer));
        Transform transform = player.transform;


        //attaching a movescript. Needs to be adjust properly, just gonna have it move with arrow keys for a while.


        //Adding a box collider to it, which i think we may need when it comes to later in the game when we interact with other pieces
        //player.AddComponent<Rigidbody2D>();
        //player.AddComponent<BoxCollider2D>();

        //add sprite renderer to player object
        //adjusting the sprite renderer to display a king piece
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteRenderer.sortingOrder = 20;

        //transform stuff that we need
        transform.SetParent(null, false);
        int rx = Random.Range(0, boardSize);
        int ry = Random.Range(0, boardSize);

        //we need to set the position depending on index
        if(pIndex == 0)
        {
            transform.localPosition = GetWorldPositions(rx, ry);
            Piece piece = player.AddComponent<Queen>();
            piece.currX = rx;
            piece.currY = ry;
            AttachPieceSprite(player, spriteRenderer, WHITE);
        }
        else if(pIndex == 1)
        {
            transform.localPosition = GetWorldPositions(18, 5);
            Piece piece = player.AddComponent<Rook>();
            piece.currX = rx;
            piece.currY = ry;
            AttachPieceSprite(player, spriteRenderer, RED);
            spriteRenderer.color = new Color(255,0,0,1);
        }
        else if (pIndex == 2)
        {
            transform.localPosition = GetWorldPositions(5, 18);
            Piece piece = player.AddComponent<King>();
            piece.currX = rx;
            piece.currY = ry;
            AttachPieceSprite(player, spriteRenderer, GREEN);
            spriteRenderer.color = new Color(0, 255, 0, 1);

        }
        else if (pIndex == 3)
        {
            transform.localPosition = GetWorldPositions(18, 18);
            Piece piece = player.AddComponent<Knight>();
            piece.currX = rx;
            piece.currY = ry;
            AttachPieceSprite(player, spriteRenderer, BLUE);
            spriteRenderer.color = new Color(0, 0, 255, 1);
        }
       
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

        
        

        return player;
    }

    private void loadResources(){
        tileSprites = Resources.LoadAll<Sprite>("chessTiles");
        piecesTiles = Resources.LoadAll<Sprite>("pieces");
    }

    private void initializeBoard(){
        tileType current = tileType.BLACK_TILE;
        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                current = tileTypeSwitch(current);
                objects[i, j] = createTileObject(gridArray[i, j], i, j, current);
            }
            if(width % 2 == 0){
                current = tileTypeSwitch(current);
            }

        }
    }
    private tileType tileTypeSwitch(tileType cur){
        if(cur == tileType.BLACK_TILE){
            return tileType.WHITE_TILE;
        }else{
            return tileType.BLACK_TILE;
        }
    }

    private void createContainers(){
        playerStats = new GameObject("PlayerStats");
        floorContainer = new GameObject("FloorContainer");
        wallsContainer = new GameObject("WallsContainer");
    }


    //using premade packages, just scales the positions of the world by the cellsize made during construction
    private Vector3 GetWorldPositions(int x, int y)
    {
        return new Vector3(x, y) * cellSize;

    }

    





    private void renderInvisibleWalls(){
        invisibleWallContainer = new GameObject("InvisibleWallContainer");
        //creates two walls per for loop, sets box one position under 0 and one at the height and width
        for(int i = 0; i < width; i++){
            GameObject invWall1 = new GameObject(i + ","+ -1,typeof(BoxCollider2D)); 
            GameObject invWall2 = new GameObject(i + "," + width, typeof(BoxCollider2D));
            Transform transform1 = invWall1.transform;
            Transform transform2 = invWall2.transform;
            transform1.SetParent(invisibleWallContainer.transform, false);
            transform2.SetParent(invisibleWallContainer.transform, false);
            transform1.localPosition = GetWorldPositions(i, -1);
            transform2.localPosition = GetWorldPositions(i, height);
        }
        for(int i = 0; i < height; i++){
            GameObject invWall1 = new GameObject(-1 + ","+ i,typeof(BoxCollider2D)); 
            GameObject invWall2 = new GameObject(height + 1 + "," + i, typeof(BoxCollider2D));
            Transform transform1 = invWall1.transform;
            Transform transform2 = invWall2.transform;
            transform1.SetParent(invisibleWallContainer.transform, false);
            transform2.SetParent(invisibleWallContainer.transform, false);
            transform1.localPosition = GetWorldPositions( -1, i);
            transform2.localPosition = GetWorldPositions( width, i);
        }

        

    }
    
    private GameObject createTileObject(int type, int x, int y, tileType color){
        //Creates texture for tile also determines size
        Texture2D tex = new Texture2D(100, 100);
        GameObject tile = new GameObject(x + "," + y,typeof(SpriteRenderer));  
        Transform transform = tile.transform;
        //Sets the position of the object
        transform.localPosition = GetWorldPositions(x, y);
        //Creates a sprite renderer to render the object from the tile
        SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(tex,new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        BoxCollider bcol = tile.AddComponent<BoxCollider>();
        
        if(type == 0){
            transform.SetParent(floorContainer.transform, false);
            if(color == tileType.BLACK_TILE){
                spriteRenderer.sprite = (Sprite)tileSprites[0];
            }else{
                spriteRenderer.sprite = (Sprite)tileSprites[1];
            }


        }
        else if(type == 1){
            transform.SetParent(wallsContainer.transform, false);
            tile.AddComponent<BoxCollider2D>();
            BoxCollider2D b2d = tile.GetComponent<BoxCollider2D>();
            b2d.size = new Vector2(1, 1);
        }
       
        return tile;

    }

    private void initGridArray(int size) { 
        boardSize = size;
        originalBoardSize = size;
        gridArray = new int[boardSize, boardSize];
        objects = new GameObject[boardSize, boardSize];
    }

    private void loadMapFile(string path){
        var reader = new StreamReader(File.OpenRead(path));
        int counter = 0;

        // While there are lines to read
        while(!reader.EndOfStream){
            var line = reader.ReadLine();
            var list = line.Split(',');
            if (counter == 0)
            {
                width = int.Parse(list[0]);
                height = int.Parse(list[1]);
                //initialize array from file width and height the first entry in the file
                gridArray = new int[width, height];
                //Giving the map some textures to be seen on the scene. This will then now allow it to be interactable. Since
                //I need to add some kind of texture to every element, we need a nested loop to modify every tile in the grid.
                
                objects = new GameObject[width, height];
            }else
            {
                //the rest of the entries are the types of tiles in the game 1 being wall 0 being walkable and the rest could be doors or items
                int x = int.Parse(list[0]);
                int y = int.Parse(list[1]);
                //checking if out of bounds from grid
                if (x < width && y < height)
                {

                    //allows different types of tiles for different types of behaviors
                    int tileType = int.Parse(list[2]);
                    gridArray[x, y] = tileType;
                    if(tileType == 0)
                    {
                        tileColor.Add(x.ToString() + y.ToString(), int.Parse(list[3]));
                    }
                }
                else
                {
                    //exit from game since map is incorrect format
                }
            }
            counter++;
        }
    }
    public int getWidth(){
        return width;
    }
    public int getHeight(){
        return height;
    }

    private void getXY(Vector3 wp, out int x, out int y)
    {
        x = Mathf.FloorToInt(wp.x / cellSize);
        y = Mathf.FloorToInt(wp.y / cellSize);
    }

    private void AttachPieceSprite(GameObject piece, SpriteRenderer spriteRenderer, Color color) {

        string name = piece.GetComponent<Piece>().GetName();

        switch (name) {
            case "king":    spriteRenderer.sprite = (Sprite) piecesTiles[0]; break;
            case "queen":   spriteRenderer.sprite = (Sprite) piecesTiles[1]; break;
            case "bishop":  spriteRenderer.sprite = (Sprite) piecesTiles[2]; break;
            case "knight":  spriteRenderer.sprite = (Sprite) piecesTiles[3]; break;
            case "rook":    spriteRenderer.sprite = (Sprite) piecesTiles[4]; break;
        }

        spriteRenderer.color = color;
        Color tmp = spriteRenderer.color;
        tmp.a = 1f;

    }

    //you can interact with the grid to set certain values to it, making for barriers or having certain values be walls/doors/items/gaurds
    public void SetValue(int x, int y, int value)
    {
        //checks the bounds of x,y sent in to make sure it is within the bounds of the grid
        if( x>=0 && y>=0 && x< width && y < height)
        {
            //now set the value to the proper grid position
            gridArray[x, y] = value;
            // debugText[x, y].text = gridArray[x, y].ToString();
        }


    }


    //different declaration of setvalue, with now taking in a world position object instead of stand alone ints x and y,
    //which in turn calls the other set value function as we are extracting the adjusted x,y values
    public void SetValue(Vector3 wp, int val)
    {
        int xPos;
        int yPos;
        getXY(wp, out xPos, out yPos);
        SetValue(xPos, yPos, val);

    }

    // is any piece on the space specified?
    public bool IsSpaceClear(Vector3 space) 
    {
        foreach (GameObject piece in player) {
            if (piece.transform.position.Equals(space)) {
                return false;
            }
        }

        return true;
    }

    public GameObject GetPlayer(int which) 
    {
        return player[which];
    }

    public int GetActivePlayer()
    {
        return activePlayer;
    }

    public void NextPlayer() 
    {
        activePlayer++;
        
       
        if (activePlayer == MAXPLAYERS)
        {
            Debug.Log("MAX REACHED");
            activePlayer = 0;
            rotations++;
        }

        if (rotations != 0 && rotations % 2 == 0) {
            rotations = 0;
            completedRotations++;
            
            Debug.Log("The map is shrinking!");
            shrinkBoard(decreaseSize);
        }

    }

    public void shrinkBoard(int shrinkBy)
    {
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                if (i >= 0  && i < completedRotations * shrinkBy)
                {
                    GameObject currTile = objects[i, j];
                    //if its a black tile, switch it to white so that the color can show up
                    SpriteRenderer rend = currTile.GetComponent<SpriteRenderer>();
                    if (rend.sprite == (Sprite)tileSprites[0])
                    {
                        rend.sprite = (Sprite)tileSprites[1];
                    }

                    rend.color = TILE_RED;
                }

                if (i >= boardSize - shrinkBy && i < boardSize)
                {
                    GameObject currTile = objects[i, j];
                    //if its a black tile, switch it to white so that the color can show up
                    SpriteRenderer rend = currTile.GetComponent<SpriteRenderer>();
                    if (rend.sprite == (Sprite)tileSprites[0])
                    {
                        rend.sprite = (Sprite)tileSprites[1];
                    }

                    rend.color = TILE_RED;
                }

                if (j >= 0  && j < completedRotations * shrinkBy)
                {
                    GameObject currTile = objects[i, j];
                    //if its a black tile, switch it to white so that the color can show up
                    SpriteRenderer rend = currTile.GetComponent<SpriteRenderer>();
                    if (rend.sprite == (Sprite)tileSprites[0])
                    {
                        rend.sprite = (Sprite)tileSprites[1];
                    }

                    rend.color = TILE_RED;
                }

                if (j >= boardSize - shrinkBy && j < boardSize)
                {
                    GameObject currTile = objects[i, j];
                    //if its a black tile, switch it to white so that the color can show up
                    SpriteRenderer rend = currTile.GetComponent<SpriteRenderer>();
                    if (rend.sprite == (Sprite)tileSprites[0])
                    {
                        rend.sprite = (Sprite)tileSprites[1];
                    }

                    rend.color = TILE_RED;
                }


            }
        }
        boardSize = boardSize - shrinkBy;
    }
    

}
