using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveOnGrid : MonoBehaviour
{

    private InitGrid script;
    private GameObject player;
    GameObject popUp;
    private Sprite[] piecesTiles;
    private float speed = 10;
    private bool animating = false;
    private bool animateRequest = false;
    private bool animationComplete = false;
    private bool getNextPlayer = false;
    private Vector3 destinationAI;
    private int playerIndex;
    private RaycastHit hit;
    private PlayerStatsHandler handler;
    //for displaying moves
    private bool displayedMoves = false;
    private List<GameObject> showTiles;
   

    // Start is called before the first frame update
    void Start()
    {
        script = GameObject.FindWithTag("GameController").GetComponent<InitGrid>();
        //get first player at start
        player = script.grid.GetPlayer(0);
        showTiles = new List<GameObject>();
        piecesTiles = Resources.LoadAll<Sprite>("pieces");
        playerIndex = script.grid.GetActivePlayer();
        handler = script.grid.getPlayerStatsHandler();
        player = script.grid.player[playerIndex];



        

    }

    // Update is called once per frame
    void Update()
    {
        if (getNextPlayer && animationComplete == true)
        {
            getNextPlayer = false;
            script.grid.NextPlayer();
            playerIndex = script.grid.GetActivePlayer();
            player = script.grid.player[playerIndex];

        }
        if(animateRequest == true)
        {
            checkIfAnimate();
        }
        //if the player piece component is null then it means they where deleted so dont make a move and get the next player
        if (player.GetComponent<Piece>())
        {
            makeMove();
        }
        else
        {

            Debug.Log("Skipped Player");
            getNextPlayer = true;
                
            
           
        }
    }
    private void checkIfAnimate()
    {
        if (animateRequest == true)
        {
            if (playerIndex == 0)
            {
                StartCoroutine(animateMovement());
            }
            else
            {
                //coroutine for AI
                StartCoroutine(animateMovementAI());
            }

        }


        //coroutine
        IEnumerator animateMovement()
        {
            animating = true;
            player.transform.position = Vector3.MoveTowards(player.transform.position, hit.collider.gameObject.transform.position, speed * Time.deltaTime);

            if (player.transform.position == hit.collider.gameObject.transform.position)
            {
                animateRequest = false;
                animating = false;
                animationComplete = true;
                System.Threading.Thread.Sleep(1250);
            }
            yield return null;
        }

        IEnumerator animateMovementAI()
        {
            animating = true;
            player.transform.position = Vector3.MoveTowards(player.transform.position, destinationAI, speed * Time.deltaTime);

            if (player.transform.position == destinationAI)
            {
                animateRequest = false;
                animating = false;
                animationComplete = true;
                System.Threading.Thread.Sleep(1250);
            }
            yield return null;
        }

    }

    private void makeMove()
    {
        Piece ack = player.GetComponent<Piece>();
        //if  moveMade is false then currently handling move 
        if (ack.moveMade == false)
        {

            /*Debug.Log("Waiting...");
            Debug.Log("Currently Moving: " + player.name);*/
        }
        else
        {
            //if moveMade was true then the move was finished so proceed to the next player 
            Debug.Log("Freedom!");
            Debug.Log("Currently Moving: " + player.name);
            ack.moveMade = false;
            getNextPlayer = true;
            return;
             
        }

        
        //DISPLAYING THE VALID MOVES
        if(playerIndex == 0 && displayedMoves == false)
        {
            List<Vector3> validSqs = new List<Vector3>();
            Piece piece = player.GetComponent<Piece>();
            if (piece.GetName().Equals("king"))
            {
                King kg = player.GetComponent<King>();
                validSqs = kg.ValidSquares(script.grid, player.transform.position);

            }
            else if (piece.GetName().Equals("knight"))
            {
                Knight kn = player.GetComponent<Knight>();
                validSqs = kn.ValidSquares(script.grid, player.transform.position);
            }
            else if (piece.GetName().Equals("bishop"))
            {
                Bishop bs = player.GetComponent<Bishop>();
                validSqs = bs.ValidSquares(script.grid, player.transform.position);


            }
            else if (piece.GetName().Equals("rook"))
            {
                Rook rk = player.GetComponent<Rook>();
                validSqs = rk.ValidSquares(script.grid, player.transform.position);


            }
            else if (piece.GetName().Equals("queen"))
            {
                Queen qn = player.GetComponent<Queen>();
                validSqs = qn.ValidSquares(script.grid, player.transform.position);


            }

            for (int i = 0; i < validSqs.Count; i++)
            {
                
                Vector3 currVector = validSqs[i];
                int currX = (int)currVector.x;
                int currY = (int)currVector.y;
                Color YELLOW = new Color(150, 150, 0, 1);
                showTiles.Add(createMoveableTile(currX, currY, YELLOW));
               

            }
            displayedMoves = true;
        }



        //AI MOVEMENT COMMANDS - GENERATE RANDOM NUMBERS UNTIL WE HIT ONE THATS A VALID MOVEMENT
        if ((playerIndex == 1 || playerIndex == 2 || playerIndex == 3) && animating == false)
        {
            Piece piece = player.GetComponent<Piece>();
            //setting display moves flag back to false now that the AI is making their turn
            displayedMoves = false;
            //destory the old show tiles
            for(int i =0; i < showTiles.Count;i++)
            {
                Destroy(showTiles[i]);
            }

            int randx = Random.Range(0, 24);
            int randy = Random.Range(0, 24);
            Vector3 randomDest = new Vector3(randx, randy);

            if (piece == null)
            {
                throw new System.Exception("Player has no piece script");
            }

            if (piece.GetName().Equals("king"))
            {

                if (player.GetComponent<King>().IsMoveValid(script.grid, player.transform.position, randomDest))
                {
                    destinationAI = randomDest;
                    animateRequest = true;
                    animationComplete = false;
                    CheckDestTileAI(randomDest);
                }

            }
            else if (piece.GetName().Equals("knight"))
            {

                if (player.GetComponent<Knight>().IsMoveValid(script.grid, player.transform.position, randomDest))
                {
                    destinationAI = randomDest;
                    animateRequest = true;
                    animationComplete = false;
                    CheckDestTileAI(randomDest);
                }

            }
            else if (piece.GetName().Equals("bishop"))
            {

                if (player.GetComponent<Bishop>().IsMoveValid(script.grid, player.transform.position, randomDest))
                {
                    destinationAI = randomDest;
                    animateRequest = true;
                    animationComplete = false;
                    CheckDestTileAI(randomDest);
                }

            }
            else if (piece.GetName().Equals("rook"))
            {

                if (player.GetComponent<Rook>().IsMoveValid(script.grid, player.transform.position, randomDest))
                {
                    destinationAI = randomDest;
                    animateRequest = true;
                    animationComplete = false;
                    CheckDestTileAI(randomDest);
                }

            }
            else if (piece.GetName().Equals("queen"))
            {

                if (player.GetComponent<Queen>().IsMoveValid(script.grid, player.transform.position, randomDest))
                {
                    destinationAI = randomDest;
                    animateRequest = true;
                    animationComplete = false;
                    CheckDestTileAI(randomDest);
                }

            }
            else
            {
                throw new System.Exception("Player has unknown piece");
            }
        }
        //CHECKING FOR PLAYER MOUSE BUTTON. ONLY WORKS FOR PLAYER 1
        else if (Input.GetMouseButtonUp(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 500f))
            {

                Piece piece = player.GetComponent<Piece>();

                if (piece == null)
                {
                    throw new System.Exception("Player has no piece script");
                }

                if (piece.GetName().Equals("king"))
                {

                    if (player.GetComponent<King>().IsMoveValid(script.grid, player.transform.position, hit.collider.gameObject.transform.position))
                    {
                        animateRequest = true;
                        animationComplete = false;
                        CheckDestTile(hit);
                    }

                }
                else if (piece.GetName().Equals("knight"))
                {

                    if (player.GetComponent<Knight>().IsMoveValid(script.grid, player.transform.position, hit.collider.gameObject.transform.position))
                    {
                        animateRequest = true;
                        animationComplete = false;

                        CheckDestTile(hit);
                    }

                }
                else if (piece.GetName().Equals("bishop"))
                {

                    if (player.GetComponent<Bishop>().IsMoveValid(script.grid, player.transform.position, hit.collider.gameObject.transform.position))
                    {
                        animateRequest = true;
                        animationComplete = false;
                        CheckDestTile(hit);
                    }

                }
                else if (piece.GetName().Equals("rook"))
                {

                    if (player.GetComponent<Rook>().IsMoveValid(script.grid, player.transform.position, hit.collider.gameObject.transform.position))
                    {
                        animateRequest = true;
                        animationComplete = false;
                        CheckDestTile(hit);
                    }

                }
                else if (piece.GetName().Equals("queen"))
                {

                    if (player.GetComponent<Queen>().IsMoveValid(script.grid, player.transform.position, hit.collider.gameObject.transform.position))
                    {
                        animateRequest = true;
                        animationComplete = false;
                        CheckDestTile(hit);
                    }

                }
                else
                {
                    throw new System.Exception("Player has unknown piece");
                }

            }

        }
    }

    // all checks regarding the tile a piece wants to move on to (i.e. not the path there) should go here 
    private void CheckDestTile(RaycastHit hit) {

        //old movement without the animation, keeping it just in case needs to be referenced again in the future
        //player.transform.position = hit.collider.gameObject.transform.position; // move player's piece

        //movement with animation


      SpriteRenderer destSprite = hit.collider.gameObject.GetComponent<SpriteRenderer>();
        
        if (destSprite.sprite.name.Equals("chessTiles_1")) { // "black" tile, might be a special tile

            if (destSprite.color.Equals(Grid.TILE_RED)) { // death :(
                downGradePlayerPiece(); // maybe we can add an animation or something later?
                createPopup("Downgraded " + "\nPlayer " + (playerIndex + 1));
            } else if (destSprite.color.Equals(Grid.TILE_GREEN)) { // upgrade :)
                changePlayerPiece();
                handler.incrementPlayerPoints(playerIndex, 2);
                createPopup("Upgraded " + "\nPlayer " + (playerIndex + 1));
            } 

        }
        string[] cords = hit.collider.gameObject.name.Split(',');
        if(cords.Length == 2)
        {
            Piece tmpPlayer = player.GetComponent<Piece>();
            if (tmpPlayer)
            {
                Debug.Log("IN");
                tmpPlayer.currX = int.Parse(cords[0]);
                tmpPlayer.currY = int.Parse(cords[1]);
                checkIfOnTopOfPlayer();
            }

        }
            

    }
    private void changePlayerPiece()
    {
        
        string currPiece = player.GetComponent<Piece>().GetName();
        DestroyImmediate(player.GetComponent<Piece>());
        switch (currPiece)
        {
            case "king":
                player.AddComponent<Bishop>();
                break;
            case "bishop":
                player.AddComponent<Rook>();
                break;
            case "rook":
                player.AddComponent<Knight>();
                break;
            case "knight":
                player.AddComponent<Queen>();
                break;
            case "queen":
                player.AddComponent<Queen>();
                break;
        }
        if (player.GetComponent<Piece>())
        {
            player.GetComponent<Piece>().moveMade = true;
            AttachPieceSprite();
        }
    }
    private void downGradePlayerPiece()
    {

        string currPiece = player.GetComponent<Piece>().GetName();
        DestroyImmediate(player.GetComponent<Piece>());
        switch (currPiece)
        {
            case "king":
                DestroyImmediate(player.GetComponent<SpriteRenderer>());
                DestroyImmediate(player.GetComponent<King>());
                createPopup("Eliminated " + "\nPlayer " + (playerIndex + 1));
                break;
            case "bishop":
                player.AddComponent<King>();
                createPopup("Downgraded " + "\nPlayer " + (playerIndex + 1));
                break;
            case "rook":
                player.AddComponent<Bishop>();
                createPopup("Downgraded " + "\nPlayer " + (playerIndex + 1));
                break;
            case "knight":
                player.AddComponent<Rook>();
                createPopup("Downgraded " + "\nPlayer " + (playerIndex + 1));
                break;
            case "queen":
                player.AddComponent<Knight>();
                createPopup("Downgraded " + "\nPlayer " + (playerIndex + 1));
                break;
        }
        if (player.GetComponent<Piece>())
        {
            player.GetComponent<Piece>().moveMade = true;
            AttachPieceSprite();
        }
    }
    private void AttachPieceSprite()
    {
       
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        string name = player.GetComponent<Piece>().GetName();
 
        switch (name)
        {
            case "king": spriteRenderer.sprite = (Sprite)piecesTiles[0]; break;
            case "queen": spriteRenderer.sprite = (Sprite)piecesTiles[1];break;
            case "bishop": spriteRenderer.sprite = (Sprite)piecesTiles[2]; break;
            case "knight": spriteRenderer.sprite = (Sprite)piecesTiles[3]; break;
            case "rook": spriteRenderer.sprite = (Sprite)piecesTiles[4]; break;
        }
        

    }
    private void CheckDestTileAI(Vector3 dest)
    {
        SpriteRenderer destSprite = GameObject.Find(dest.x +"," + dest.y).GetComponent<SpriteRenderer>();

        if (destSprite.sprite.name.Equals("chessTiles_1"))
        { // "black" tile, might be a special tile

            if (destSprite.color.Equals(Grid.TILE_RED))
            { // death :(
                downGradePlayerPiece(); // maybe we can add an animation or something later?
                
            }
            else if (destSprite.color.Equals(Grid.TILE_GREEN))
            { // upgrade :)
                changePlayerPiece();
                handler.incrementPlayerPoints(playerIndex, 2);
                createPopup("Upgraded " + "\nPlayer " + (playerIndex + 1));
            }

        }
        Piece tmpPlayer = player.GetComponent<Piece>();
        if (tmpPlayer)
        {
            tmpPlayer.currX = (int)dest.x;
            tmpPlayer.currY = (int)dest.y;
            checkIfOnTopOfPlayer(); 
        }
    }
    private void checkIfOnTopOfPlayer()
    {
        Piece currPiece = player.GetComponent<Piece>();
        
        for(int i = 0; i < script.grid.getMaxPlayers(); i++)
        {
            if(playerIndex != i)
            {
                GameObject otherPlayer = script.grid.GetPlayer(i);
                Piece otherPiece = otherPlayer.GetComponent<Piece>();
                if(currPiece.currY == otherPiece.currY && currPiece.currX == otherPiece.currX)
                {
                    handler.incrementPlayerPoints(playerIndex, 10);
                    DestroyImmediate(otherPiece);
                    DestroyImmediate(otherPlayer.GetComponent<SpriteRenderer>());
                    createPopup("Eliminated Player " + i);
                }

            }
        }



    }

    private void createPopup(string message)
    {
        GameObject popup = new GameObject("popup");
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        GameObject text = new GameObject("text");
        popup.AddComponent<RectTransform>();
        Canvas canvas = popup.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        Image background = popup.AddComponent<Image>();
        text.AddComponent<RectTransform>();
        canvas.sortingOrder = 100;
        Text textParam = text.AddComponent<Text>();
        textParam.text = message;
        text.transform.SetParent(popup.transform);
        popup.transform.SetParent(camera.transform);
        popup.transform.localScale = new Vector3(0.07f, 0.07f, 0);
        popup.transform.localPosition = new Vector3(0, 0, 5);
        background.color = new Color(255, 255, 255, 0.8f);
        textParam.color = new Color(0, 0, 0);
        textParam.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        textParam.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        textParam.alignment = TextAnchor.MiddleCenter;
        Destroy(text, 0.7f);
        Destroy(popup, 0.7f);
        



    }


    private GameObject createMoveableTile(int x, int y, Color ylw)
    {
        //Creates texture for tile also determines size
        Texture2D tex = new Texture2D(50, 50);
        GameObject tile = new GameObject("Moveable Spot", typeof(SpriteRenderer));
        Transform transform = tile.transform;
        //Sets the position of the object
        transform.localPosition = new Vector3(x,y,-1);
        //Creates a sprite renderer to render the object from the tile
        SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        spriteRenderer.color = ylw;
        return tile;

    }



}
