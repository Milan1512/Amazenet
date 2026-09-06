using UnityEngine;

public class Timedotsetup : MonoBehaviour
{
    Sessionmode sessionmode = null;

    public void SetupSessionmode(Sessionmode temp) 
    { 
        sessionmode = temp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       sessionmode.addtime(20f);
       gameObject.SetActive(false);
        
    }
}
