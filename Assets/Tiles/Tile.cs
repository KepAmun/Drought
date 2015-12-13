using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour
{
    public enum TileType { Desert, Grass, Mud, Water}
    public TileType Type { get; protected set; }

    int level = 0;

    public GameBoard GameBoard { get; set; }
    public bool Locked { get; set; }

    public event System.Action<Tile> Activated;
    

    public virtual void Advance()
    {
    }


    void OnMouseDown()
    {
        if(!Locked)
        {
            StopAllCoroutines();
            StartCoroutine(DoDrag());
        }
    }


    System.Collections.IEnumerator DoDrag()
    {
        bool dragging = true;

        Vector3 startingPos = transform.position;

        //StopCoroutine("DoMoveTo");

        while(dragging)
        {
            if(Input.GetMouseButtonUp(0))
            {
                dragging = false;
            }
            else
            {
                RaycastHit hitInfo;
                Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(mouseRay, out hitInfo, 1000, LayerMask.GetMask("GameBoard")))
                {
                    transform.position = hitInfo.point + (Vector3.up * 0.5f);
                }

            }

            yield return null;
        }

        if(!Activate())
        {
            MoveTo(startingPos);
        }

    }


    public virtual bool Activate()
    {
        Locked = true;

        if(Activated != null)
            Activated(this);

        return GameBoard.PlaceTile(this);
    }


    public void MoveTo(Vector3 targetPosition, float delay = 0)
    {   
        StartCoroutine(DoMoveTo(targetPosition, delay));
    }


    System.Collections.IEnumerator DoMoveTo(Vector3 targetPosition, float delay)
    {
        yield return new WaitForSeconds(delay);

        while(Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            Vector3 v = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref v, 0.05f);

            yield return null;
        }

        transform.position = targetPosition;
    }
}
