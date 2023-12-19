using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class VagueGenerator : MonoBehaviour
{
    Mesh myMesh;
    MeshFilter myFilter;

    [SerializeField] Vector2 planSize = new Vector2(1,1);
    [SerializeField] int planeResolution = 1;

    List<Vector3> vertices;
    List<int> triangles;

    private void Awake()
    {
        myMesh = new Mesh();
        myFilter= GetComponent<MeshFilter>();
        myFilter.mesh= myMesh;
    }

    // Update is called once per frame
    void Update()
    {
        planeResolution = Mathf.Clamp(planeResolution, 1, 50);
        GenerateTriangle(planSize, planeResolution);
        MouvementGaucheDroite(Time.timeSinceLevelLoad);
        AssignMesh();
    }
    void MouvementGaucheDroite(float time)
    {
        // Je définie les valeurs y de mes sommets comme étant le sinus de l'heure actuelle plus la position x 
        for (int i = 0; i<vertices.Count; i++)
        {
            Vector3 vertex = vertices[i];
            vertex.y = Mathf.Sin(time+vertex.x);
            vertices[i] = vertex;
        }
    }

    void GenerateTriangle(Vector2 planSize, int planeResolution)
    {
        // Crée une liste de d'index(triangles) qui indiquent à notre mesh dans quel ordre les points seront connectés
        vertices = new List<Vector3>();
        float x = planSize.x/planeResolution;
        float y = planSize.y/planeResolution;
        //Ajoute des somments selon la grosseur demander
        for (int i = 0; i<planeResolution+1; i++)
        {
            for(int j = 0; j<planeResolution+1; j++) 
            { 
                vertices.Add(new Vector3(j*x,0,i*y));
            }
        }
        triangles = new List<int>();
        // J'ajoute les triangles dans l'ordre horaire sans qu'il se repasse dessus en faisant
        // Une boucle qui parcour tous les points à l'exception de la dernière ligne et du dernier point de chaque ligne
        for (int row = 0; row<planeResolution; row++)
        {
            for(int colomn = 0; colomn<planeResolution; colomn++)
            {
                int i = (row*planeResolution) + row + colomn;
                triangles.Add(i);
                triangles.Add(i + (planeResolution) + 1);
                triangles.Add(i + (planeResolution) + 2);

                triangles.Add(i);
                triangles.Add(i + planeResolution + 2);
                triangles.Add(i+1);
            }
        }
    }
    void AssignMesh()
    {
        // Assignation des triangles à mon mesh
        myMesh.Clear();
        myMesh.vertices = vertices.ToArray();
        myMesh.triangles = triangles.ToArray();
    }
}
    