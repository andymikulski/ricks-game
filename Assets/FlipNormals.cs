using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class FlipNormals : MonoBehaviour
{
	void Start()
	{
		Mesh mesh = GetComponent<MeshFilter> ().sharedMesh;
		mesh.uv = mesh.uv.Select(FlipUV).ToArray();
		mesh.triangles = mesh.triangles.Reverse().ToArray();
		mesh.normals = mesh.normals.Select(FlipNormal).ToArray();
	}

	private Vector2 FlipUV(Vector2 uv)
	{
		return new Vector2(1 - uv.x, uv.y);
	}

	private Vector3 FlipNormal(Vector3 normal)
	{
		return -normal;
	}
}