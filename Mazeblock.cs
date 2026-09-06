
using UnityEngine;

public class Mazeblock : MonoBehaviour
{
    


    private bool IsVisited = false;
    [SerializeField] private GameObject ground;
    [SerializeField]private GameObject Rightwall;
    [SerializeField]private GameObject Leftwall;
    [SerializeField]private GameObject Topwall;
    [SerializeField]private GameObject Downwall;
    [SerializeField] private GameObject movableteg;


    private void Awake()
    {
        movableteg.SetActive(false);
    }



    int wallcount = 4;
    public void setendpoint(Sprite endpoint)
    {
        ground.GetComponent<SpriteRenderer>().color = Color.white;
        ground.GetComponent<SpriteRenderer>().sprite = endpoint;

    }

    public void setIsvisited() 
    {
        IsVisited = true;
    }
    public bool GetiSVisited() 
    {
        return IsVisited;
    }

    public void RemoveRightWall() {Rightwall.SetActive(false); wallcount -= 1;}
    public void RemoveLeftWall() {Leftwall.SetActive(false); wallcount -= 1;}
    public void RemoveTopWall() {Topwall.SetActive(false); wallcount -= 1;}
    public void RemoveDownWall() {Downwall.SetActive(false); wallcount -= 1;}

    public int getwallcount() 
    {
        return wallcount;
    }


    public bool ismovableon() 
    {
        return movableteg.activeSelf;    
    }
    public void Setmovabletegon() {movableteg.SetActive(true);}




}
