using UnityEngine;

public class CameraBorders : MonoBehaviour
{
    public Transform target;
    public Transform target2;
    public Transform target3;
    public Transform target4;
    AnaKarakter anaKarakter;

    public float minX, maxX;
    public float minY, maxY;

    void LateUpdate()
    {
        if (target != null)
        {
           
            Vector3 targetPosition = transform.position;

         
            targetPosition.x = Mathf.Clamp(target.position.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(target.position.y, minY, maxY);

           
            transform.position = targetPosition;
        }

        if (target2 != null)
        {

            Vector3 targetPosition = transform.position;


            targetPosition.x = Mathf.Clamp(target2.position.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(target2.position.y, minY, maxY);


            transform.position = targetPosition;
        }
        if (target3 != null && anaKarakter.entrance==true)
        {

            Vector3 targetPosition = transform.position;


            targetPosition.x = Mathf.Clamp(target3.position.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(target3.position.y, minY, maxY);


            transform.position = targetPosition;
        }
        if (target4 != null && anaKarakter.entrance==true)
        {

            Vector3 targetPosition = transform.position;


            targetPosition.x = Mathf.Clamp(target4.position.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(target4.position.y, minY, maxY);


            transform.position = targetPosition;
        }
    }
}
