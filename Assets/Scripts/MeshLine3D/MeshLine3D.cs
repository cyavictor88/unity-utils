using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// g2w = grid to world
// gr = grid space
// ws = world space
// ips = individual point space
public class MeshLine3D
{
    public GameObject thisgo;
    MeshFilter mf;
    public Material linemeshMaterial;
    MeshRenderer mr;
    GameObject gparent;
    public Color color;
    public string name;
    public MeshCollider mc;
    Vector3[] major_points_gr = new Vector3[] { Vector3.zero, Vector3.up };
    Matrix4x4 g2w = Matrix4x4.identity;
    int nside_for_circle;

    private Vector3[] verts_ws;

    float lineThickness;

    public MeshLine3D(GameObject gparent, Vector3[] major_points_gr, Matrix4x4 g2w) : this(gparent, major_points_gr, g2w, "lineMesh", Color.white, 8, 0.03f) { }
    public MeshLine3D(GameObject gparent, Vector3[] major_points_gr, Matrix4x4 g2w, Color color) : this(gparent, major_points_gr, g2w, "lineMesh", color, 8, 0.03f) { }
    public MeshLine3D(GameObject gparent, Vector3[] major_points_gr, Matrix4x4 g2w, string name) : this(gparent, major_points_gr, g2w, name, Color.white, 8, 0.03f) { }
    public MeshLine3D(GameObject gparent, Vector3[] major_points_gr, Matrix4x4 g2w, string name, Color color, int nside_for_circle) : this(gparent, major_points_gr, g2w, name, Color.white, nside_for_circle, 0.03f) { }
    public MeshLine3D(GameObject gparent, Vector3[] major_points_gr, Matrix4x4 g2w, string name, Color color, int nside_for_circle, float lineThickness)
    {
        this.lineThickness = lineThickness;
        this.color = color;
        this.name = name;
        this.nside_for_circle = nside_for_circle;
        this.major_points_gr = major_points_gr;
        this.gparent = gparent;
        linemeshMaterial = new Material(Shader.Find("Sprites/Default"));
        linemeshMaterial.SetColor("_Color", color);

        thisgo = new GameObject { name = name };
        thisgo.name = name;
        thisgo.transform.parent = gparent.transform;

        mf = thisgo.AddComponent<MeshFilter>();
        mr = thisgo.AddComponent<MeshRenderer>();
        mr.material = linemeshMaterial;

        this.g2w = g2w;
        makeMesh();
        showMesh(true);


    }
    public void activateCollider()
    {
        mc = thisgo.AddComponent<MeshCollider>();
        mc.convex = false;
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
        bool isMeshCurrentlyShowing = mr.enabled;
        showMesh(false);
        this.major_points_gr = new_major_points_gr;
        makeMesh();
        showMesh(isMeshCurrentlyShowing);
    }

    // given a list of points, the line mesh is made by expanding each point in the list to a proper rotated circle, 
    // and then from all the cicrles of points, a long curvy cylinder mesh is made.
    void makeMesh()
    {
        // expanding each point in the list to a proper rotated circle
        List<List<Vector3>> verts_after_expansion_gr = expand_each_point_in_major_points_gr_to_nside_ploygon(major_points_gr, nside_for_circle, lineThickness);

        // now with all the cicrles points, a long curvy cylinder mesh is made by creating the correct triangles.
        int[] alltriangles = get_triangles(verts_after_expansion_gr, nside_for_circle);

        List<Vector3> list_verts_ws = new List<Vector3>();
        foreach (List<Vector3> vl in verts_after_expansion_gr)
        {
            foreach (Vector3 v in vl)
            {
                list_verts_ws.Add(g2w.MultiplyPoint3x4(v));
            }
        }

        verts_ws = list_verts_ws.ToArray();
        mf.mesh.vertices = verts_ws;
        mf.mesh.triangles = alltriangles;

    }

    //  a long curvy cylinder mesh (our line mesh) is made by creating the correct triangles.
    private int[] get_triangles(List<List<Vector3>> verts_after_expansion_gr, int nside)
    {
        List<int> allTriangles = new List<int>();

        // draw the very first circle to "cover" the start of the linemesh
        for (int j = 0; j < nside - 2; j++)
        {
            allTriangles.Add(0);
            allTriangles.Add(2 + j);
            allTriangles.Add(1 + j);
        }

        // draw the "walls" from one point to the next point
        for (int i = 0; i < verts_after_expansion_gr.Count - 1; i++)
        {
            int idx_supplement = i * nside;
            int[] coners = new int[4];

            for (int j = 0; j < nside - 1; j++)
            {
                coners[0] = j + idx_supplement;
                coners[3] = j + 1 + idx_supplement;
                coners[1] = coners[0] + nside;
                coners[2] = coners[3] + nside;

                //define the order in which the vertices in the VerteicesArray shoudl be used to draw the triangle
                allTriangles.Add(coners[0]);
                allTriangles.Add(coners[1]);
                allTriangles.Add(coners[2]);
                allTriangles.Add(coners[2]);
                allTriangles.Add(coners[3]);
                allTriangles.Add(coners[0]);
            }

            coners[0] = 0 + idx_supplement;
            coners[3] = nside - 1 + idx_supplement;
            coners[1] = coners[0] + nside;
            coners[2] = coners[3] + nside;

            //define the order in which the vertices in the VerteicesArray shoudl be used to draw the triangle
            allTriangles.Add(coners[0]);
            allTriangles.Add(coners[1]);
            allTriangles.Add(coners[2]);
            allTriangles.Add(coners[2]);
            allTriangles.Add(coners[3]);
            allTriangles.Add(coners[0]);
        }

        // draw the very last circle to "cover" the end of the linemesh
        for (int j = 0; j < nside - 2; j++)
        {
            allTriangles.Add(verts_after_expansion_gr[verts_after_expansion_gr.Count - 1].Count - nside);
            allTriangles.Add(verts_after_expansion_gr[verts_after_expansion_gr.Count - 1].Count - nside + 2 + j);
            allTriangles.Add(verts_after_expansion_gr[verts_after_expansion_gr.Count - 1].Count - nside + 1 + j);
        }


        return allTriangles.ToArray();
    }

    // for each point in the linemesh, using the point as the center, expand to a nside circle, with radius=lineThickness
    // each circle "faces"(its normal vector points to) the next point in the linemesh
    List<List<Vector3>> expand_each_point_in_major_points_gr_to_nside_ploygon(Vector3[] major_points_gr, int nside, float radius)
    {
        List<List<Vector3>> verts_after_expansion_gr = new List<List<Vector3>>();
        for (int i = 0; i < major_points_gr.Length; i++)
        {
            verts_after_expansion_gr.Add(new List<Vector3>());
            Vector3 normal_gr = Vector3.zero;
            if (i == major_points_gr.Length - 1) normal_gr = major_points_gr[i] - major_points_gr[i - 1];
            else normal_gr = major_points_gr[i + 1] - major_points_gr[i];

            Vector3 normal_ips = new Vector3(0, 0, 1);
            Vector3 gridcenter_gr = major_points_gr[i];
            Matrix4x4 rotM_ips_to_gr = get_rotationM_ips_to_gr(normal_ips, normal_gr, gridcenter_gr);
            float theta = 360 / nside;
            Vector3[] vertsArr = new Vector3[nside + 1];
            for (int j = 0; j < nside; j++)
            {
                Vector3 point_on_circle = new Vector3(radius, 0, 0);
                point_on_circle = Quaternion.AngleAxis(theta * j, normal_ips) * point_on_circle;
                verts_after_expansion_gr[i].Add((rotM_ips_to_gr.MultiplyPoint3x4(point_on_circle)));
            }
        }
        return verts_after_expansion_gr;
    }

    // some tricky math to get the rotation matrix that rotates each circle to point the next point  
    public Matrix4x4 get_rotationM_ips_to_gr(Vector3 normal_ips, Vector3 normal_gr, Vector3 gridcenter_gr)
    {
        //https://math.stackexchange.com/questions/180418/calculate-rotation-matrix-to-align-vector-a-to-vector-b-in-3d
        //https://www.home.uni-osnabrueck.de/mfrankland/Math416/Math416_SimilarMatrices.pdf // for our case here, bring vec to non-e base first and trans
        Matrix4x4 translationM = Matrix4x4.identity;

        translationM.SetColumn(3, new Vector4(gridcenter_gr.x, gridcenter_gr.y, gridcenter_gr.z, 1));

        // for our case here, bring vec to non-elementary base first and then trans
        Vector3 A = normal_ips.normalized;
        Vector3 B = normal_gr.normalized;

        if (A == B || A == -B) return translationM * Matrix4x4.identity;

        float A_dot_B = Vector3.Dot(A, B);
        Vector3 A_x_B = Vector3.Cross(A, B);

        Vector3 u = A;
        Vector3 v = (B - (A * (A_dot_B))) / (B - (A * A_dot_B)).magnitude; 
        Vector3 w = Vector3.Cross(B, A);
        Matrix4x4 G = Matrix4x4.identity;
        G.SetRow(0, new Vector4(A_dot_B, -(A_x_B.magnitude), 0, 0));
        G.SetRow(1, new Vector4((A_x_B.magnitude), A_dot_B, 0, 0));
        G.SetRow(2, new Vector4(0, 0, 1, 0));

        Matrix4x4 F = Matrix4x4.identity;
        F.SetColumn(0, u);
        F.SetColumn(1, v);
        F.SetColumn(2, w);
        F = F.inverse;
        Matrix4x4 U = (F.inverse) * G * F;

        Matrix4x4 rotationM_ips_to_gr = translationM * U;
        return rotationM_ips_to_gr;
    }
}
