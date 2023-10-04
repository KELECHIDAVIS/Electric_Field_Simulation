using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    Vector2 diff = Vector2.zero;
    public GameObject[,] arrows;
    public List<GameObject> particles;
    public int row, col;  
    // Start is called before the first frame update
    private void OnMouseDown()
    {
        diff = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)(transform.position); 
        
    }

    private void OnMouseDrag()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - diff;
        updateField(); 
    }

    // update vectorField
    private void updateField()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                arrows[i, j].GetComponent<Arrow>().calcEField(particles);
            }

        }
    }
}
