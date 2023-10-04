using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainProgram : MonoBehaviour
{

    public GameObject arrowPrefab;
    public GameObject negPrefab, posPrefab, neutPrefab;


    public float xStart, yStart;
    public int cols, rows;
    public float xSpace, ySpace;


    public GameObject testArrow;
    

    GameObject [,] arrowList; // vectorField 
    List<GameObject> particleList ;

    
    void Start()
    {
        arrowList = new GameObject[rows , cols];
        particleList = new List<GameObject>();
        initVectorField(); 
    }

    // Update is called once per frame
    void Update()
    {
        // only update arrow rotation if a particle moves 
        if(Input.GetKeyDown(KeyCode.N) )
        {
           
            // spawn a neg 
            Vector3 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
            mousePos.z = 10f;

            GameObject newParticle = Instantiate(negPrefab, mousePos, Quaternion.identity);
            newParticle.GetComponent<Draggable>().arrows = arrowList; 
            newParticle.GetComponent<Draggable>().particles = particleList;
            newParticle.GetComponent<Draggable>().row = rows;
            newParticle.GetComponent<Draggable>().col = cols ;
            particleList.Add( newParticle);

            
            updateVectorField(); 
        }
        if (Input.GetKeyDown(KeyCode.P))
        {

            // spawn a neg 
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 10f;

            GameObject newParticle = Instantiate(posPrefab, mousePos, Quaternion.identity);
            newParticle.GetComponent<Draggable>().arrows = arrowList;
            newParticle.GetComponent<Draggable>().particles = particleList;
            newParticle.GetComponent<Draggable>().row = rows;
            newParticle.GetComponent<Draggable>().col = cols;
            particleList.Add(newParticle);

            updateVectorField();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            deleteParticles(); 
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(); 
        }



    }

    void initVectorField()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                arrowList[i, j] = Instantiate(arrowPrefab, new Vector3(xStart + xSpace * (j), yStart - ySpace * (i)), Quaternion.identity);
            }

        }
    }

    void updateVectorField()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                arrowList[i, j].GetComponent<Arrow>().calcEField(particleList) ;
            }

        }
    }

    void deleteParticles()
    {
        for (int i = 0; i < particleList.Count; i++)
        {
            Destroy(particleList[i]);
        }

        // set all the arrows angles back to 0 and change the alpha vals back 

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                arrowList[i, j].transform.rotation = Quaternion.identity;
                arrowList[i,j].GetComponent<SpriteRenderer>().color = new Color(1,1,1, 0 ) ;
            }

        }
        particleList.Clear();
    }
}
