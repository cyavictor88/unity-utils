using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VicMeshLine
{
    public GameObject thisgo;
    MeshFilter mf;
    Material lm;
    MeshRenderer mr;
    GameObject gparent;
    public Color color;
    public string name;
    public MeshCollider mc;
    Vector3[] major_points_gr = new Vector3[] { Vector3.zero, Vector3.up };
    Matrix4x4 g2w = Matrix4x4.identity;
    int nside_for_circle = 8;

    private Vector3[] verts_ws_small_r;
    private Vector3[] verts_ws_big_r;

    public VicMeshLine(GameObject gparent, Vector3[] major_points_gr, Matrix4x4 g2w) : this(gparent, major_points_gr, g2w, "lineMesh", Color.white, 8) { }
    public VicMeshLine(GameObject gparent, Vector3[] major_points_gr, Matrix4x4 g2w, Color color) : this(gparent, major_points_gr, g2w, "lineMesh", color, 8) { }
    public VicMeshLine(GameObject gparent, Vector3[] major_points_gr, Matrix4x4 g2w, string name) : this(gparent, major_points_gr, g2w, name, Color.white, 8) { }
    public VicMeshLine(GameObject gparent, Vector3[] major_points_gr, Matrix4x4 g2w, string name, Color color, int nside_for_circle)
    {
        this.color = color;
        this.name = name;
        this.nside_for_circle = nside_for_circle;
        this.major_points_gr = major_points_gr;
        this.gparent = gparent;
        lm = new Material(Shader.Find("Sprites/Default"));
        lm.SetColor("_Color", color);

        //gobj = GameObject.Instantiate(vicplaneprefab, new Vector3(0, 0, 0), Quaternion.identity);
        thisgo = new GameObject { name = name };
        thisgo.name = name;
        thisgo.transform.parent = gparent.transform;


        mf = thisgo.AddComponent<MeshFilter>();
        mr = thisgo.AddComponent<MeshRenderer>();
        mr.material = lm;



        this.g2w = g2w;
        makeMesh();






    }
    public void activateCollider()
    {
        mc = thisgo.AddComponent<MeshCollider>();
        mc.convex = true;
        mc.sharedMesh = mf.mesh;
        mc.enabled = true;
    }

    public void showMesh(bool yesno)
    {
        mr.enabled = yesno;
        if (mc != null) mc.enabled = yesno;
    }

    public void updateMesh(Vector3[] new_major_points_gr)
    {
        bool meshshowingornot = mr.enabled;
        showMesh(false);
        this.major_points_gr = new_major_points_gr;
        makeMesh();
        showMesh(meshshowingornot);

    }

    public void showBigR(bool yesno)
    {
        showMesh(false);
        if (!yesno) mf.mesh.vertices = verts_ws_small_r;
        else mf.mesh.vertices = verts_ws_big_r;
        showMesh(true);
    }

    void makeMesh()
    {

        List<List<Vector3>> verts_after_expansion_gr_small_r = expand_each_point_in_major_points_gr_to_nside_ploygon(major_points_gr, nside_for_circle, 0.03f);
        List<List<Vector3>> verts_after_expansion_gr_big_r = expand_each_point_in_major_points_gr_to_nside_ploygon(major_points_gr, nside_for_circle, 0.1f);
        int[] alltriangles = get_triangles(verts_after_expansion_gr_small_r, nside_for_circle);


        List<Vector3> list_verts_ws_small_r = new List<Vector3>();
        foreach (List<Vector3> vl in verts_after_expansion_gr_small_r)
        {
            foreach (Vector3 v in vl)
            {
                list_verts_ws_small_r.Add(g2w.MultiplyPoint3x4(v));

            }
        }
        List<Vector3> list_verts_ws_big_r = new List<Vector3>();
        foreach (List<Vector3> vl in verts_after_expansion_gr_big_r)
        {
            foreach (Vector3 v in vl)
            {
                list_verts_ws_big_r.Add(g2w.MultiplyPoint3x4(v));
            }
        }

        verts_ws_big_r = list_verts_ws_big_r.ToArray();
        verts_ws_small_r = list_verts_ws_small_r.ToArray();


        mf.mesh.vertices = verts_ws_small_r;



        mf.mesh.triangles = alltriangles;


        //Vector3[] VerteicesArray = new Vector3[4];
        //int[] trianglesArray = new int[6];
        ////lets add 3 vertices in the 3d space


        //VerteicesArray[0] = g2w.MultiplyPoint3x4(corner_gr[0]);
        //VerteicesArray[1] = g2w.MultiplyPoint3x4(corner_gr[1]);
        //VerteicesArray[2] = g2w.MultiplyPoint3x4(corner_gr[2]);
        //VerteicesArray[3] = g2w.MultiplyPoint3x4(corner_gr[3]);
        ////define the order in which the vertices in the VerteicesArray shoudl be used to draw the triangle
        //trianglesArray[0] = 0;
        //trianglesArray[1] = 1;
        //trianglesArray[2] = 2;
        //trianglesArray[3] = 2;
        //trianglesArray[4] = 3;
        //trianglesArray[5] = 0;
        ////add these two triangles to the mesh
        //mf.mesh.vertices = VerteicesArray;
        //mf.mesh.triangles = trianglesArray;
        //mr.enabled = false;


    }

    private int[] get_triangles(List<List<Vector3>> verts_after_expansion_gr, int nside)
    {
        List<int> alltris = new List<int>();
        // for (int i = 0; i < verts_after_expansion_gr.Count; i++)
        // {
        //     int idx_supplement = i * nside;
        //     for (int j = 0; j < nside - 2; j++)
        //     {
        //         alltris.Add(0 + idx_supplement);
        //         alltris.Add(2 + j + idx_supplement);
        //         alltris.Add(1 + j + idx_supplement);
        //     }
        // }
        for (int i = 0; i < verts_after_expansion_gr.Count - 1; i++)
        {
            int idx_supplement = i * nside;
            //int j = 0;
            int[] corners = new int[4];

            for (int j = 0; j < nside - 1; j++)
            {
                corners[0] = j + idx_supplement;
                corners[1] = corners[0] + nside;
                corners[2] = corners[1] + 1;
                corners[3] = corners[0] + 1;

                //define the order in which the vertices in the VerteicesArray shoudl be used to draw the triangle
                alltris.Add(corners[0]);
                alltris.Add(corners[1]);
                alltris.Add(corners[2]);
                alltris.Add(corners[2]);
                alltris.Add(corners[3]);
                alltris.Add(corners[0]);

            }
            corners[0] = 0 + idx_supplement;
            corners[3] = nside - 1 + idx_supplement;
            corners[1] = corners[0] + nside;
            corners[2] = corners[3] + nside;

            //define the order in which the vertices in the VerteicesArray shoudl be used to draw the triangle
            alltris.Add(corners[0]);
            alltris.Add(corners[1]);
            alltris.Add(corners[2]);
            alltris.Add(corners[2]);
            alltris.Add(corners[3]);
            alltris.Add(corners[0]);



        }
        return alltris.ToArray();
    }

    List<List<Vector3>> expand_each_point_in_major_points_gr_to_nside_ploygon(Vector3[] major_points_gr, int nside, float radius)
    {
        List<List<Vector3>> verts_after_expansion_gr = new List<List<Vector3>>();
        for (int i = 0; i < major_points_gr.Length; i++)
        {
            verts_after_expansion_gr.Add(new List<Vector3>());
            Vector3 normal_gr = Vector3.zero;
            if (i == major_points_gr.Length - 1) normal_gr = major_points_gr[i] - major_points_gr[i - 1];
            else normal_gr = major_points_gr[i + 1] - major_points_gr[i];

            Vector3 normal_ds = new Vector3(0, 0, 1);
            Vector3 gridcenter_gr = major_points_gr[i];
            Matrix4x4 rotM_ds_to_gr = get_rotationM_ds_to_gr(normal_ds, normal_gr, gridcenter_gr);
            float theta = 360 / nside;
            Vector3[] vertsArr = new Vector3[nside + 1];
            for (int j = 0; j < nside; j++)
            {
                Vector3 point_on_circle = new Vector3(radius, 0, 0);
                point_on_circle = Quaternion.AngleAxis(theta * j, normal_ds) * point_on_circle;
                verts_after_expansion_gr[i].Add((rotM_ds_to_gr.MultiplyPoint3x4(point_on_circle)));


            }
        }
        return verts_after_expansion_gr;
    }

    public  Matrix4x4 get_rotationM_ds_to_gr(Vector3 normal_ds, Vector3 normal_gr,Vector3 gridcenter_gr )
    {
        //https://math.stackexchange.com/questions/180418/calculate-rotation-matrix-to-align-vector-a-to-vector-b-in-3d
        //https://www.home.uni-osnabrueck.de/mfrankland/Math416/Math416_SimilarMatrices.pdf // for our case here, bring vec to non-e base first and trans
        Matrix4x4 translationM = Matrix4x4.identity;

        translationM.SetColumn(3, new Vector4(gridcenter_gr.x, gridcenter_gr.y, gridcenter_gr.z, 1));

        // for our case here, bring vec to non-elementary base first and then trans
        Vector3 A = normal_ds.normalized;
        Vector3 B = normal_gr.normalized;

        if (A == B || A==-B) return translationM * Matrix4x4.identity;

        float A_dot_B = Vector3.Dot(A, B);
        Vector3 A_x_B = Vector3.Cross(A, B);

        Vector3 u = A;
        Vector3 v = (B - (A * (A_dot_B))) / (B - (A * A_dot_B)).magnitude; //B - (A_dot_B * A) / ((A_dot_B * A).magnitude);
        Vector3 w = Vector3.Cross(B, A);
        Matrix4x4 G = Matrix4x4.identity;
        G.SetRow(0, new Vector4(A_dot_B, -(A_x_B.magnitude), 0, 0));
        G.SetRow(1, new Vector4((A_x_B.magnitude), A_dot_B, 0, 0));
        G.SetRow(2, new Vector4(0, 0, 1, 0));

        Matrix4x4 F = Matrix4x4.identity;
        F.SetColumn(0, u);
        F.SetColumn(1, v);
        F.SetColumn(2, w);
        //F.SetColumn(3, new Vector4(gridcenter_gr.x, gridcenter_gr.y, gridcenter_gr.z, 1));
        F = F.inverse;
        Matrix4x4 U = (F.inverse) * G * F;


        Matrix4x4 rotationM_ds_to_gr = translationM* U;
        return rotationM_ds_to_gr;
    }

}
