using System.Collections.Generic;
using System.Net;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Mazegenerator : MonoBehaviour
{

    [Header("Mazegenration")]
    [SerializeField] Dictionary<Vector2,Mazeblock> Mazeblocklist = new Dictionary<Vector2, Mazeblock>();
    [SerializeField] int mazelength = 0;
    [SerializeField] GameObject Mazebolck = null;
    [SerializeField] Contarollermovement contaroller = null;
    [SerializeField] Sprite endpoint = null;

    


    int mazeX = 0;
    int mazeY = 0;

    bool trygenratemaze = false;

    Mazeblock Curentmazeblock = null;
    Vector2 curentid = Vector2.zero;
    Vector2 StartBlockid = Vector2.zero;
    Vector2 Endblockid= Vector2.zero;
    List<Vector2> deadendblock = new List<Vector2>();


    Stack<Vector2> MazeBlockstack = new Stack<Vector2>(); 
    List<GameObject> Mazeblockgameobjects = new List<GameObject>();


    Vector2[] directions = new Vector2[]
{
    new Vector2(0, 1),   // up
    new Vector2(0, -1),  // down
    new Vector2(-1, 0),  // left
    new Vector2(1, 0)    // right
};
    private void Awake()
    {
        genrategried();
        genratemaze();
    }
    private void Start()
    {
    }


    private void Update()
    {

        if (trygenratemaze) 
        {
            resetgried();
            genrategried();
            genratemaze();
            trygenratemaze = false ;
        }
    }

    void resetgried()
    {

        foreach (GameObject block in Mazeblockgameobjects)
        {
            Destroy(block);
        }
        Mazeblocklist.Clear();
        MazeBlockstack.Clear();
        Mazeblockgameobjects.Clear();
        deadendblock.Clear();
        Curentmazeblock = null;
        curentid = Vector2.zero;
        StartBlockid = Vector2.zero;
        Endblockid = Vector2.zero;


    }

    void genrategried() 
    {   mazeX = mazelength;
        mazeY = mazelength;

        for (int i = 0; i < mazeX; i++)
        {
            for (int j = 0; j < mazeY; j++)
            {
                Vector2 newlocation = new Vector2(i, j);
                GameObject tempmazeblock = Instantiate(Mazebolck, newlocation, Quaternion.identity);
                Mazeblockgameobjects.Add(tempmazeblock);
                Mazeblocklist.Add(new Vector2(i, j), tempmazeblock.GetComponent<Mazeblock>());
            }
        }
        curentid = new Vector2(Random.Range(1, mazeX), Random.Range(1, mazeY));
        StartBlockid = curentid;
        MazeBlockstack.Push(curentid);
        Curentmazeblock = Mazeblocklist[curentid];
        Curentmazeblock.Setmovabletegon(); 
        Curentmazeblock.setIsvisited();
    }
    void genratemaze() 
    {

        int blockcount = 0;
        int blockdistence = 0;

        Dictionary <int, Vector2> endblocklist = new Dictionary<int, Vector2>();
        Vector2 lastdir = new Vector2();    
        while (blockcount < Mazeblocklist.Count - 1)
        {
            List<Vector2> nextblocklist = new List<Vector2>();
            foreach (Vector2 dir in directions)
            {
                Vector2 chakenextblockid = curentid + dir;
                if (Mazeblocklist.ContainsKey(chakenextblockid) && !Mazeblocklist[chakenextblockid].GetiSVisited())
                {
                    nextblocklist.Add(chakenextblockid);
                }
            }
            if (nextblocklist.Count > 0)
            {
                Vector2 nextblockdir = nextblocklist[Random.Range(0, nextblocklist.Count)] - curentid;
                Vector2 nextblockid = curentid + nextblockdir;
                Mazeblock NextMazeblock = Mazeblocklist[nextblockid];

                if (nextblockdir == directions[0])
                {
                    Curentmazeblock.RemoveTopWall();
                    NextMazeblock.RemoveDownWall();
                }
                if (nextblockdir == directions[1])
                {
                    Curentmazeblock.RemoveDownWall();
                    NextMazeblock.RemoveTopWall();
                }
                if (nextblockdir == directions[2])
                {
                    Curentmazeblock.RemoveLeftWall();
                    NextMazeblock.RemoveRightWall();
                }
                if (nextblockdir == directions[3])
                {
                    Curentmazeblock.RemoveRightWall();
                    NextMazeblock.RemoveLeftWall();
                }

                if(lastdir != nextblockdir) 
                { Curentmazeblock.Setmovabletegon();
                      lastdir = nextblockdir; 
                }

                NextMazeblock.setIsvisited();
                MazeBlockstack.Push(nextblockid);
                curentid = nextblockid;
                Curentmazeblock = NextMazeblock;
                blockcount++;
                blockdistence++;
            }
            else
            {
               
                if (Mazeblocklist[curentid].getwallcount() == 3) {
                    if (!deadendblock.Contains(curentid)) // add this check
                    {
                        deadendblock.Add(curentid);
                    }
                    if (!endblocklist.ContainsKey(blockdistence))
                    {   
                        endblocklist.Add(blockdistence, curentid);
                    }
                }
                MazeBlockstack.Pop();
                if (MazeBlockstack.Count == 0) return; 
                curentid = MazeBlockstack.Peek();
                Curentmazeblock = Mazeblocklist[curentid];
                blockdistence--;
                lastdir = Vector2.zero;
            }
        }

        int mostdis = 0;
        Vector2 mostdisid = Vector2.zero;

        
        foreach(Vector2 id in Mazeblocklist.Keys) 
        {
            if (Mazeblocklist[id].getwallcount() == 3) 
            {
                Mazeblocklist[id].Setmovabletegon();
            }
        }
        foreach (int id in endblocklist.Keys)
        {
            if (id > mostdis)
            {
                mostdis = id;
                mostdisid = endblocklist[mostdis];
            }
        }

        Endblockid = mostdisid;
        Mazeblocklist[Endblockid].setendpoint(endpoint);
        deadendblock.Remove(Endblockid);
        deadendblock.Remove(StartBlockid);
        endblocklist.Clear();
        contaroller.setlocationtostartingpoint(StartBlockid,Endblockid);

    }

    public void Setmazesize(int mazesize){ mazelength = mazesize;}
    public int Getmazesize() { return mazelength;}

    public void  settrygenratemaze() 
    {
        if (!trygenratemaze) 
        {
            trygenratemaze = true;
        }
    }

    public Vector2 getdeadend() 
    {
        if (deadendblock.Count >= 5)
        {
            int randomindex = Random.Range(0, deadendblock.Count);
            Vector2 result = deadendblock[randomindex];
            deadendblock.Remove(result);
            return result;
        }
        else { return Vector2.negativeInfinity;};
    }


}

