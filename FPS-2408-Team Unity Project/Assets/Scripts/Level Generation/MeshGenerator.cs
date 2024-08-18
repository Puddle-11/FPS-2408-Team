using Palmmedia.ReportGenerator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.AI;
using static UnityEngine.GraphicsBuffer;
using System.Reflection;


[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    private ChunkGrid chunkRef;
    private Vector3Int GridSize;

    public MeshCell[,,] Cells;
    public List<Vector3> Verticies;
    public List<int> Triangles;
    public List<Vector2> UVs;
    private MeshFilter mainMesh;
    
    [SerializeField] private MeshCollider colliderMesh;
    private float Scale;
    public float terrainScale;
    private Vector2 WorldCenter;
    [HideInInspector] public WorldBounds chunkBounds;

    public struct WorldBounds
    {

        public WorldBounds(Vector3 max, Vector3 min)
        {
            maxBounds = max;
            minBounds = min;
        }
        public Vector3 maxBounds;
        public Vector3 minBounds;
    }

    //add world to cell function for break and place functions
    public Vector3[] VertexPos = new Vector3[]
    {
        //Top Vertex
        new Vector3(-1, 1, -1),new Vector3(-1, 1, 1),
        new Vector3(1, 1, 1),new Vector3(1, 1, -1),
        //Bottom Vertex
        new Vector3(-1, -1, -1),new Vector3(-1, -1, 1),
        new Vector3(1, -1, 1),new Vector3(1, -1, -1)
    };
    public void SetChunkRef(ChunkGrid _val)
    {
        chunkRef = _val;
    }
   
    public Vector3Int GetCenter()
    {
        return new Vector3Int(GridSize.x / 2, GridSize.y / 2, GridSize.z / 2);
    }
    public void updateGridCell(Vector3Int _pos, int _newID)
    {
        Cells[_pos.x, _pos.y, _pos.z].ID = _newID;
        UpdateShape();
    }
    public void UpdateShape()
    {
        UVs = new List<Vector2>();
        Verticies = new List<Vector3>();
        Triangles = new List<int>();
        GenerateFaces();
    }
    public void ClearShape()
    {
        Triangles = new List<int>();
        Verticies = new List<Vector3>();
        UVs = new List<Vector2>();
        for (int x = 0; x < Cells.GetLength(0); x++)
        {
            for (int y = 0; y < Cells.GetLength(1); y++)
            {
                for (int z = 0; z < Cells.GetLength(2); z++)
                {
                    Cells[x, y, z].ID = 0;
                }
            }
        }
        GenerateFaces();

    }
    public void CreateShape()
    {
        Scale = chunkRef.VoxelSize;
        GridSize = chunkRef.ChunkSize;
        WorldCenter = new Vector2(GetWorldPos(GetCenter()).x, GetWorldPos(GetCenter()).z);
        mainMesh = GetComponent<MeshFilter>();
        Triangles = new List<int>();
        Verticies = new List<Vector3>();
        UVs = new List<Vector2>();

        GenerateGrid();
        GenerateFaces();
    }
    public void GenerateFaces()
    {
        
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                for (int z = 0; z < GridSize.z; z++)
                {

                    if (Cells[x, y, z].ID != 0)
                    {
                        int filteredID = Cells[x, y, z].ID  -1;

                        
                        #region chunkEdge Draws
                        if (y == 0)
                        {
                            //Render BottomFace
                            AddQuad(7, 5, 6, 4, new Vector3(x, y, z) * Scale, Verticies.Count, 1, filteredID);
                        }
                        if (y == Cells.GetLength(1) - 1)
                        {
                            AddQuad(0, 1, 2, 3, new Vector3(x, y, z) * Scale, Verticies.Count, 0, filteredID);
                            //Render TopFace

                        }
                        if (x == 0)
                        {
                            AddQuad(1, 0, 5, 4, new Vector3(x, y, z) * Scale, Verticies.Count, 2, filteredID);

                        }
                        if (x == Cells.GetLength(0) - 1)
                        {
                            AddQuad(6, 2, 7, 3, new Vector3(x, y, z) * Scale, Verticies.Count, 4, filteredID);

                        }
                        if (z == 0)
                        {

                            AddQuad(4, 0, 6, 2, new Vector3(x, y, z) * Scale, Verticies.Count, 3, filteredID);
                        }
                        if (z == Cells.GetLength(2) - 1)
                        {
                            AddQuad(1, 5, 3, 7, new Vector3(x, y, z) * Scale, Verticies.Count, 5, filteredID);
                        }
                        #endregion

                        //------------------------------------------
                        //X Side 
                        if (x < Cells.GetLength(0) - 1)
                        {
                            if (Cells[x + 1, y, z].ID == 0)
                            {
                                AddQuad(6, 2, 7, 3, new Vector3(x, y, z) * Scale, Verticies.Count, 4, filteredID);
                            }
                        }
                        if (x > 0)
                        {
                            if (Cells[x - 1, y, z].ID == 0)
                            {
                                AddQuad(1, 0, 5, 4, new Vector3(x, y, z) * Scale, Verticies.Count, 2, filteredID);

                            }
                        }
                        //------------------------------------------


                        //------------------------------------------
                        //Y Side 
                        if (y < Cells.GetLength(1) - 1)
                        {
                            if (Cells[x, y + 1, z].ID == 0)
                            {
                                AddQuad(0, 1, 2, 3, new Vector3(x, y, z) * Scale, Verticies.Count, 0, filteredID);

                            }
                        }
                        if (y > 0)
                        {
                            if (Cells[x, y - 1, z].ID == 0)
                            {
                                AddQuad(7, 5, 6, 4, new Vector3(x, y, z) * Scale, Verticies.Count, 1, filteredID);

                            }
                        }
                        //------------------------------------------


                        //------------------------------------------
                        //Z Side 
                        if (z < Cells.GetLength(2) - 1)
                        {
                            if (Cells[x, y, z + 1].ID == 0)
                            {
                                AddQuad(1, 5, 3, 7, new Vector3(x, y, z) * Scale, Verticies.Count, 5, filteredID);

                            }
                        }
                        if (z > 0)
                        {
                            if (Cells[x, y, z - 1].ID == 0)
                            {
                                AddQuad(4, 0, 6, 2, new Vector3(x, y, z) * Scale, Verticies.Count, 3, filteredID);

                            }
                        }
                        //------------------------------------------
                    }
                }
            }
        }
        UpdateMesh();
    }
    public void GenerateGrid()
    {
        Cells = new MeshCell[GridSize.x, GridSize.y, GridSize.z];
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                for (int z = 0; z < GridSize.z; z++)
                {
                    if (Cells[x,y,z] == null)
                    {
                        Cells[x, y, z] = new MeshCell();
                    }
                   // Cells[x, y, z].worldPosition = GetWorldPos(x,y,z);
                
                    Cells[x, y, z].ID = chunkRef.PlaceTile(new Vector3Int(x, y, z), gameObject);
                }
            }
        }
    }
    public Vector3 GetWorldPos(Vector3Int _cellPos)
    {
        return ((Vector3)_cellPos * Scale) + transform.position;
    }
    public Vector3 GetWorldPos(int _x, int _y, int _z)
    {
        return new Vector3(_x, _y, _z) * Scale;
    }
    public Vector3Int GetCellPos(Vector3 _worldPos)
    {
        _worldPos -= transform.position - new Vector3(Scale/2, Scale/2, Scale/2);
        _worldPos = _worldPos / Scale;
        int x = (int)_worldPos.x;
        int y = (int)_worldPos.y;
        int z = (int)_worldPos.z;
        return new Vector3Int(x, y, z);
    }


    #region Unused terrain code - KEEP 
    //public float Sample3DPerlin(Vector3 pos)
    //{
    //    return noise.snoise(new Vector3((pos.x + 0.1f) / terrainScale, (pos.y + 0.1f) / terrainScale, (pos.z + 0.1f) / terrainScale)) - (pos.y * chunkRef.Compression) + chunkRef.Density;
    //}
    // public float SampleHeight(Vector3 pos)
    //{
    //    //2D noise
    //    float Sample = (Mathf.PerlinNoise((pos.x + 0.1f) / (terrainScale), (pos.z + 0.1f) / (terrainScale)) * chunkRef.MountainHeight - chunkRef.GroundHeight);
    //    return Sample;
    //}
    //public float SampleMultiplierMap(Vector3 pos)
    //{
    //    float Sample = (Mathf.PerlinNoise((pos.x + 0.1f) / (chunkRef.M_scale), (pos.z + 0.1f) / chunkRef.M_scale) * chunkRef.M_MountainHeight + chunkRef.M_BaseHeight);
    //    return Sample;
    //}
    //public float SampleMulti(Vector3 pos)
    //{
    //    float RawNoise = noise.snoise(new Vector3((pos.x + 0.1f) / terrainScale, (pos.y + 0.1f) / terrainScale, (pos.z + 0.1f) / terrainScale));
    //    float PowNoise = 0;
    //    if (RawNoise > 0)
    //    {

    //     PowNoise = Mathf.Abs(Mathf.Pow(RawNoise, chunkRef.perlinPower));
    //    }
    //    else
    //    {
    //        PowNoise = Mathf.Abs(Mathf.Pow(RawNoise, chunkRef.perlinPower)) * -1;

    //    }
    //    return PowNoise * (chunkRef.PerlinNoiseInfluence * SampleMultiplierMap(pos)) - ((pos.y + SampleHeight(pos)) * chunkRef.Compression) + chunkRef.Density;

    //}
    #endregion

    public void AddQuad(int _ip1, int _ip2, int _ip4, int _ip3, Vector3 _origin, int _startIndex, int _side, int _ID)
    {
        int fx = _ID % chunkRef.textureAtlasSize;
        int fy = Mathf.FloorToInt(_ID / (float)chunkRef.textureAtlasSize);
        Vector2 UVAnchor = new Vector2(fx, fy) / chunkRef.textureAtlasSize;
        Debug.Log("ID: " + _ID + " Anchor: " + UVAnchor);


        _side = Mathf.Clamp(_side, 0, 5);
        Vector3[] tempVertex = new Vector3[4];
        Vector2[] tempUV = new Vector2[4]; 
        switch (_side)
        {
            case 0:
                tempUV = new Vector2[] 
                { 
                 UVAnchor + new Vector2(0,0) / (chunkRef.textureAtlasSize),
                 UVAnchor + new Vector2(0,0.5f) / (chunkRef.textureAtlasSize),
                 UVAnchor + new Vector2(0.5f,0.5f) / (chunkRef.textureAtlasSize),
                 UVAnchor + new Vector2(0.5f,0) / (chunkRef.textureAtlasSize)
                };

                break;
            case 1:
                tempUV = new Vector2[]
                     {
                 UVAnchor + new Vector2(0.5f,0.5f) / (chunkRef.textureAtlasSize),
                 UVAnchor + new Vector2(0.5f,1) / (chunkRef.textureAtlasSize),
                UVAnchor + new Vector2(1,1) / (chunkRef.textureAtlasSize),
                 UVAnchor + new Vector2(1,0.5f) / (chunkRef.textureAtlasSize),
                     };
                break;
            case 2:
                tempUV = new Vector2[]
                       {
                 UVAnchor + new Vector2(0,1) / (chunkRef.textureAtlasSize),
                 UVAnchor + new Vector2(0.5f,1) / (chunkRef.textureAtlasSize),
                 UVAnchor + new Vector2(0.5f,0.5f) / (chunkRef.textureAtlasSize),
                 UVAnchor + new Vector2(0,0.5f) / (chunkRef.textureAtlasSize),
                };
                break;
            case 3:
            case 4:
                tempUV = new Vector2[]
                         {
                 UVAnchor + new Vector2(0,0.5f) / (chunkRef.textureAtlasSize),
                 UVAnchor + new Vector2(0,1) / (chunkRef.textureAtlasSize),
                 UVAnchor + new Vector2(0.5f,1) / (chunkRef.textureAtlasSize),
                 UVAnchor + new Vector2(0.5f,0.5f) / (chunkRef.textureAtlasSize),
                         };
                break;
            case 5:
                tempUV = new Vector2[]
                       {
                       UVAnchor + new Vector2(0.5f,1) / (chunkRef.textureAtlasSize),
                       UVAnchor + new Vector2(0.5f,0.5f) / (chunkRef.textureAtlasSize),
                       UVAnchor + new Vector2(0,0.5f) / (chunkRef.textureAtlasSize),
                       UVAnchor + new Vector2(0,1) / (chunkRef.textureAtlasSize),
                       };
                break;

        }
        tempVertex[0] = _origin + (VertexPos[_ip1] * (Scale/2));
        tempVertex[1] = _origin + (VertexPos[_ip2] * (Scale / 2));
        tempVertex[2] = _origin + (VertexPos[_ip3] * (Scale / 2));
        tempVertex[3] = _origin + (VertexPos[_ip4] * (Scale / 2));
        int[] tempTriangles = new int[] 
        { 
        _startIndex, _startIndex + 1, _startIndex + 3,
        _startIndex + 1, _startIndex + 2, _startIndex + 3,
        };
        Triangles.AddRange(tempTriangles);
        Verticies.AddRange(tempVertex);
        UVs.AddRange(tempUV);
    }
    public void UpdateMesh()
    {
        Vector2[] tempUV = new Vector2[Verticies.Count];
        for (int i = 0; i < tempUV.Length; i++)
        {
            if(i >= UVs.Count)
            {
                break;
            }
            else
            {
                tempUV[i] = UVs[i];
            }
        }
        
        mainMesh.mesh.vertices = Verticies.ToArray();
        mainMesh.mesh.triangles = Triangles.ToArray();
        mainMesh.mesh.uv = UVs.ToArray();
        mainMesh.mesh.RecalculateNormals();

        colliderMesh.sharedMesh = mainMesh.mesh;

    }
  
}
