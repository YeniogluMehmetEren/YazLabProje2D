using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Boss boss;
    public AnaKarakter anaKarakter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        Boss vurulanboss=collision.GetComponent<Boss>();

        if(vurulanboss!=null )
        {
            vurulanboss.DamageAl(anaKarakter.damage);
            Debug.Log("vurdu");
        }


    
    }

  


}
