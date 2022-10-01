using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColidingBeam : MonoBehaviour
{
    public List<Attackable> attackables = new List<Attackable>();
 
     public void OnTriggerEnter(Collider collider)
     {
         Attackable attackable = collider.gameObject.GetComponent<Attackable>();
         if (attackable != null && !attackable.IsPlayer && !attackables.Contains(attackable))
         {
             attackables.Add(attackable);
         }
     }
 
     public void OnTriggerExit(Collider collider)
     {
         Attackable attackable = collider.gameObject.GetComponent<Attackable>();
         if(attackable != null && !attackable.IsPlayer && attackables.Contains(attackable))
         {
             attackables.Remove(attackable);
         }
     }
}
