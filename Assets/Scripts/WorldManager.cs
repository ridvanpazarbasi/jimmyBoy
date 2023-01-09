using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour {
    int[,] tiles;
    float[] depth;
    Hashtable tileObjects;
    public float xNoiseMult = .15f;
    public float yNoiseMult = .15f;
    public float EnemyFrequency = 50;
    public int TransitionDistance = 100;
    float WallTightness = 7;
    public GameObject Walls;
    public GameObject Enemies;
    public GameObject player;
    public GameObject camera;
    public GameObject background;
    public GameObject projectiles;
    public float fallSpeed = 1;
    public float Difficulty = 1;
    public string Tileset = "Cave";
    float currentDifficulty;
    float enemyPoints = 0;
    bool transitioning = false;
    bool lastandfirst = false;
    float transitionDirection = 0;
    float transVal = 0;
    float transYVal = 0;
    string useTileset1;
    string useTileset2;
    int xMax = 40;
    int yMax = 20;
    int xMin = 0;
    
    float playerXPos = 0;
    float wallXPos = 0;
    float wallYPos = 0;
    float worldSeed = 0;
    GameObject TransitionObj;
    //bool ready = false;
    // Use this for initialization

    GameObject[] enemyList;
    GameObject[] dropList;

    List<GameObject> availableEnemies;

    int[,] transition = new int[89,62];

    void Start () {
        worldSeed = Random.Range(0, 10000);
        wallXPos = playerXPos - 5;
        enemyList = Resources.LoadAll<GameObject>("Enemies");
        dropList = Resources.LoadAll<GameObject>("Drops");

        CreateTransitionArray();

        SetTileset(Tileset);

        tiles = new int[xMax, yMax];
        depth = new float[yMax];
        tileObjects = new Hashtable();
        GenerateTiles(wallXPos, 0);
	}
	
	// Update is called once per frame
	void Update () {
        wallXPos = playerXPos - 5;
        currentDifficulty += Difficulty * Time.deltaTime;
        enemyPoints += Difficulty * 10 * Time.deltaTime;
       
        float prevPos = player.transform.position.x;
        player.transform.position = new Vector3(prevPos + Time.deltaTime * fallSpeed, player.transform.position.y, player.transform.position.z);
        camera.transform.position = new Vector3(camera.transform.position.x + Time.deltaTime * fallSpeed, camera.transform.position.y, camera.transform.position.z);
        background.transform.position = new Vector3(background.transform.position.x + Time.deltaTime * fallSpeed, background.transform.position.y, background.transform.position.z);
        projectiles.transform.position = new Vector3(projectiles.transform.position.x + Time.deltaTime * fallSpeed, projectiles.transform.position.y, projectiles.transform.position.z);


        if (Mathf.Floor(prevPos) != Mathf.Floor(player.transform.position.x))
            {
                updateWalls();
                playerXPos = Mathf.Floor(player.transform.position.x);
            }

        if (transitioning)
        {
            //print(TransitionObj.transform.position.x + ", " + (transVal-70));
            if (playerXPos > transVal + 84)
            {
                wallYPos += 20 * transitionDirection ;
                transitionDirection = 0;
                transitioning = false;
                lastandfirst = true;
            }
            else if (playerXPos > transVal + 51)
            {
                float py = GameObject.Find("Player").transform.position.y;

                if (py > 9.5f + wallYPos && transitionDirection == 0)
                {
                    transitionDirection = 1;
                    this.Tileset = useTileset1;
                }
                else if (transitionDirection == 0)
                {
                    transitionDirection = -1;
                    this.Tileset = useTileset2;
                }

                if (Mathf.Abs(transYVal) < 20)
                {

                    transYVal += 4 * Time.deltaTime * transitionDirection;
                    transYVal = Mathf.Clamp(transYVal ,- 20,20);
                    camera.transform.position = new Vector3(camera.transform.position.x, wallYPos + transYVal + 9.41f, camera.transform.position.z);
                    background.transform.position = new Vector3(background.transform.position.x, wallYPos + transYVal + 9.63f, background.transform.position.z);
                    projectiles.transform.position = new Vector3(projectiles.transform.position.x, wallYPos + transYVal, projectiles.transform.position.z);

                }
            } 

         }

        if (TransitionObj != null)
        {            
            if (playerXPos > transVal + 130)
            {
                Destroy(TransitionObj);
            }
        }

        if ((int)playerXPos % TransitionDistance == 0 && !transitioning && TransitionObj == null)
        {
            transitioning = true;
            lastandfirst = true;
            CreateTransitionObject();
            transYVal = 0;
            transVal = playerXPos;
        }

       if(Time.timeScale > 0) CheckForSpawn();
	}

    void CreateTransitionArray()
    {

        for (int y = 0; y < 62; y++)
            for (int x = 0; x < 88; x++)
            {
                transition[x, y] = 0;
            }
        GameObject transitionObj = Resources.Load<GameObject>("Tilesets/Transition");
        foreach(Transform obj in transitionObj.transform)
        {
            transition[(int)obj.localPosition.x, (int)obj.localPosition.y + 21] = 1;
        }

        this.fixTiles(transition, 0, 89, 62,-2);

        //CreateTransitionObject();

    }

    string GetTilesetByIndex(int index)
    {
        switch (index)
        {
            case 0: return "Example";
            case 1: return "Red"; 
            case 2: return "Cave"; 
        }
        return "Cave";
    }

    void CreateTransitionObject()
    {
        int rndTileset = (int)Random.Range(0, 3);
        useTileset1 = GetTilesetByIndex(rndTileset);

        rndTileset = (int)Random.Range(0, 3);
        useTileset2 = GetTilesetByIndex(rndTileset);

        string currentTileset = Tileset;


        TransitionObj = new GameObject();
        TransitionObj.transform.parent = Walls.transform;
        TransitionObj.transform.localPosition = new Vector3(38, -21, 0);
        bool collision_off = false;
        for (int y = 0; y < 62; y++)
            for (int x = 0; x < 88; x++)
            {
                collision_off = false;
                int rnd = Random.Range(0, 60 - x);
                if (rnd < 1 && transition[x, y] > 0)
                {
                    if (transition[x, y] == 5)
                        collision_off = true;
                    transition[x, y] = 100;
                    if (y >= 31)
                        Tileset = useTileset1;
                    else
                        Tileset = useTileset2;
                }
                else
                {
                    Tileset = currentTileset;
                }

                // if (transition[x, y] != 5 && transition[x, y] != 0)
                this.updateSceneTile(transition, x, y, TransitionObj.transform, collision_off, false);
            }
      
    }


    void updateWalls()
    {
        //clear left column
        for (int y = 0; y < yMax; y++)
        {
            string index = new Vector2(wallXPos, y+wallYPos).ToString();
            GameObject to = tileObjects[index] as GameObject;
            tileObjects.Remove(index);
            Destroy(to);
        }



       
            int x = 2;
            for (int y = 0; y < yMax; y++)
            {
                float val = Mathf.PerlinNoise((xMax - wallXPos) * xNoiseMult, y * yNoiseMult + worldSeed);

                float CDist = Mathf.Abs((yMax / 2) - y);
                if (CDist < WallTightness)
                    val += -.1f * (WallTightness - CDist);

                depth[y] = Mathf.PerlinNoise((xMax - wallXPos - 1) * xNoiseMult, y * yNoiseMult + worldSeed);


                if (val > 0.5f)
                {
                    tiles[x, y] = 100;
                }
                else {
                    tiles[x, y] = 0;
                }


                if (y <= 1 || y >= yMax - 2)
                {
                    tiles[x, y] = 100;
                }






                //updateSceneTile(x-1, y,  xMax-2);

            }

            /* x = 1;
                 for (int y = 0; y < yMax; y++)
                 {

                     if (tiles[x, y] > 0)
                     {
                         bool[] b = evalTile(x, y,3);

                         if ((!b[1] && b[4] && !b[7]) || (!b[3] && b[4] && !b[5]))
                             tiles[x, y] = 0;


                     }
                 }
                 */
            fixTiles(tiles, 1, 3, yMax, 1, xMax - 2);


            //generate new right column
            reorderTileArray();
        
    }

    void reorderTileArray()
    {

        int[,] tilestemp = new int[3, yMax];
        for (int x = 0; x < 2 ; x++)
            for (int y = 0; y < yMax; y++)
            {
                tilestemp[x, y] = tiles[x+1, y];                
            }

        tiles = tilestemp;

    }


    void GenerateTiles(float xpos, float ypos)
    {

        for (int x = xMin; x < xMax; x++)
           // for (int y = 0; y < yMax; y++)
            {
              /*  float val = Mathf.PerlinNoise(x * xNoiseMult, y * yNoiseMult);

                float CDist = Mathf.Abs((yMax / 2) - y);
                if (CDist < WallTightness)
                    val += -.1f * (WallTightness - CDist);


                if (val > 0.5f)
                {
                    tiles[x, y] = 5;
                }
                else {
                    tiles[x, y] = 0;
                }
                */

               // if (y <= 1 || y >= yMax - 2)
                //{
                    tiles[x, 0] = 5;
                    tiles[x, 1] = 5;
                    tiles[x, yMax-1] = 5;
                    tiles[x, yMax - 2] = 5;
            //}



        }

        /*for (int x = xMin; x < xMax - 1; x++)
            for (int y = 0; y < yMax; y++)
            {

                if (tiles[x, y] > 0)
                {
                    bool[] b = evalTile(x, y,xMax);

                    if ((!b[1] && b[4] && !b[7]) || (!b[3] && b[4] && !b[5]))
                        tiles[x, y] = 0;


                }
            }*/
        fixTiles(tiles,0,xMax,yMax,-1);


        //Transform wallT = Walls.transform;
        //wallT.position = new Vector3(0, 0, 0);
        // wallT.localScale = new Vector3(0.53f, 0.53f, 0.53f);

        int[,] tilestemp = new int[3, yMax];
        for (int x = 0; x < 3 - 1; x++)
            for (int y = 0; y < yMax; y++)
            {
                tilestemp[x, y] = tiles[xMax - 3 + x, y];
            }

        tiles = tilestemp;


        //ready = true;
    }



    void fixTiles(int[,] ttiles, int xMin, int txMax, int tyMax, int column, float xOffSet = 0)
    {

       

        for (int x = xMin; x < txMax-1; x++)
            for (int y = 0; y < tyMax; y++)
            {

                if (ttiles[x, y] > 0)
                {
                    bool[] b = evalTile(ttiles,x, y,txMax,tyMax);





                    if ( !b[1]
                        && b[3] && b[4] && b[5]
                        && b[7] && b[7] && b[8])
                        ttiles[x, y] = 2;


                   


                    if (b[1] && b[2]
                     && !b[3] && b[4] && b[5]
                              && b[7] && b[8])
                        ttiles[x, y] = 4;


                    if (b[0] && b[1]
                     && b[3] && b[4] && !b[5]
                     && b[6] && b[7])
                        ttiles[x, y] = 6;


                    if (b[0] && b[1] && b[2]
                      && b[3] && b[4] && b[5]
                             && !b[7])
                        ttiles[x, y] = 8;


                    if (!b[0] && !b[1] 
                     && !b[3] && b[5] && b[5]
                              && b[7] && b[8])
                        ttiles[x, y] = 1;

                    if (      !b[1] && !b[2]
                    && b[3] && b[4] && !b[5]
                    && b[6] && b[7] )
                        ttiles[x, y] = 3;

                    if (b[1] && b[2]
                    && !b[3] && b[4] && b[5]
                    && !b[6] && !b[7])
                        ttiles[x, y] = 7;

                    if (b[0] && b[1] 
                    && b[3] && b[4] && !b[5]
                     && !b[7] && !b[8])
                        ttiles[x, y] = 9;


                    if (b[0] && b[1] && !b[2]
                      && b[3] && b[4] && b[5]
                      && b[6] && b[7] && b[8])
                       ttiles[x, y] = 13;

                    if (!b[0] && b[1] && b[2]
                      && b[3] && b[4] && b[5]
                      && b[6] && b[7] && b[8])
                        ttiles[x, y] = 12;

                    if (b[0] && b[1] && b[2]
                      && b[3] && b[4] && b[5]
                      && b[6] && b[7] && !b[8])
                        ttiles[x, y] = 11;

                    if (b[0] && b[1] && b[2]
                      && b[3] && b[4] && b[5]
                      && !b[6] && b[7] && b[8])
                        ttiles[x, y] = 10;


                    if ( b[0] && b[1] && b[2]
                      && b[3] && b[4] && b[5]
                      && b[6] && b[7] && b[8])
                        ttiles[x, y] = 5;



                }               
                if(column ==-1)
                        updateSceneTile(ttiles,x, y,Walls.transform);
                else if((column >=0 &&  !transitioning) ||(column >= 0 && transitioning && lastandfirst)) {
                    //print(transitioning);
                    if(lastandfirst && ttiles[x, y] >0) ttiles[x, y] = 100;
                    updateSceneTile(ttiles, column, y,Walls.transform,false,true,xOffSet);
                    }
             
            }
        if (lastandfirst) lastandfirst = false;
    }





    bool[] evalTile(int[,] ttiles, int x, int y, int txMax,int tyMax)
    {
        bool[] eval = new bool[9];
        int index = 0;
        for (int ey = 1; ey > -2; ey--)
            for (int ex = -1; ex < 2; ex++)
            {
                if (x + ex >= 0 && x + ex < txMax && y + ey >= 0 && y + ey < tyMax)
                {
                    if(ttiles[x+ex,y+ey]>0)
                        eval[index] = true;
                    else
                        eval[index] = false;
                }
                else
                    eval[index] = true;

                
                index++;
            }


        return eval;
    }




    void updateSceneTile(int[,] ttiles, int x, int y, Transform Parent, bool disableCollision = false, bool VaryDepth = true, float xOffset = 0)
    {
        string set = Tileset;
        string type = "Center";

        switch(ttiles[x, y]){
            case 1: type = "OCTopLeft"; break;
            case 2: if (Random.Range(1, 5) > 3) type = "Top2"; else type = "Top"; break;
            case 3: type = "OCTopRight"; break;
            case 4: type = "Left"; break;
            case 5: if(Random.Range(1,5) >3) type = "Center2"; break;
            case 6: type = "Right"; break;
            case 7: type = "OCBottomLeft"; break;
            case 8: if (Random.Range(1, 5) > 3) type = "Bottom2"; else type = "Bottom"; break;
            case 9: type = "OCBottomRight"; break;
            case 10: type = "UCBottomLeft"; break;
            case 11: type = "UCBottomRight"; break;
            case 12: type = "UCTopLeft"; break;
            case 13: type = "UCTopRight"; break;
            default: type = "Default"; break;


        }

        if (ttiles[x, y] > 0)
        {
            GameObject to = Instantiate(Resources.Load("Tilesets/" + set + "/" + type)) as GameObject;

            if (disableCollision)
                to.GetComponent<EdgeCollider2D>().enabled = false;


            Transform wallT = Parent;
            to.transform.SetParent(wallT);
            float cVal = 1;
            if (VaryDepth)
            {
               // cVal = Mathf.Clamp(depth[y], .7f, 1);
                //to.GetComponent<SpriteRenderer>().color = new Color(cVal, cVal, cVal, 1);
            }
            to.transform.localPosition = new Vector3(Mathf.Floor(x+wallXPos + xOffset), y+ wallYPos, -2);
           
            Vector2 index = new Vector2(Mathf.Floor(to.transform.localPosition.x),to.transform.localPosition.y);
            if(VaryDepth)
                tileObjects.Add(index.ToString(), to);

            if (Random.Range(0, 50) < 1)
            {
                int rndD = Random.Range(1, 5);
                GameObject tod = Instantiate(Resources.Load("Tilesets/" + "Cave" + "/Debris" + rndD)) as GameObject;
                tod.transform.SetParent(to.transform);
                tod.transform.localPosition = new Vector3(0,0,-2);
            }
         }

       
    }


    void CheckForSpawn()
    {

        float val = Random.Range(0, EnemyFrequency);

        if(val > EnemyFrequency-2)
        {
            SpawnRandomEnemy();
        }
    }

    void SpawnRandomEnemy()
    {

        int rndMax = availableEnemies.ToArray().Length;
        float pick = Random.Range(0, rndMax);

        GameObject tEnemy = availableEnemies.ToArray()[(int)pick];
        if (this.enemyPoints > tEnemy.GetComponent<EnemyBase>().PointCost)
        {

            enemyPoints -= tEnemy.GetComponent<EnemyBase>().PointCost;

            float yPos = Random.Range(4, yMax - 5);

            GameObject to = Instantiate(tEnemy) as GameObject;
            to.transform.position = new Vector3(40 + playerXPos, yPos + wallYPos, -3);
            to.transform.SetParent(Enemies.transform);
        }
    }

    public void SetPause(bool val)
    {
        if (val)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void Quit()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    public void SetTileset(string newTileset)
    {
        Tileset = newTileset;
        availableEnemies = new List<GameObject>();
        
        for (int x = 0; x < enemyList.Length; x++)
        {
            if(enemyList[x].GetComponent<EnemyBase>().Tileset.Equals(Tileset) || enemyList[x].GetComponent<EnemyBase>().Tileset.Equals("Any"))
                availableEnemies.Add(enemyList[x]);
        }
    }

    public void GenerateDrop(Transform origin)
    {
        string type = "BaseDrop";

        GameObject[] objs = FindDropsOfRarity(Random.Range(0,100));

        int rndTop = objs.Length;

        type = objs[(int)Random.Range(0,rndTop)].name;

        GameObject to = Instantiate(Resources.Load("Drops/" + type)) as GameObject;
        to.transform.position = origin.position;
        to.transform.parent = origin.parent;
    }

    GameObject[] FindDropsOfRarity(float rarity)
    {
        List<GameObject> objs = new List<GameObject>();
        foreach(GameObject obj in dropList){
            if(obj.GetComponent<DropBase>().HowCommon >= rarity)                
                objs.Add(obj);
        }
        return objs.ToArray();
    }
}
