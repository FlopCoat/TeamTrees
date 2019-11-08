using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class TreesController : MonoBehaviour
{
	public Mesh mesh;
	public Transform ground;
	public Material[] treeMaterials;
	public float spacing;
	public float sideLength;
	private EntityManager entityManager;

	private void Awake()
	{
		GameManager.trees = this;
	}

	public void PlantTrees(int amount)
	{
		entityManager = World.Active.EntityManager;

		NativeArray<Entity> entityArray = new NativeArray<Entity>(amount, Allocator.Temp);
		EntityArchetype entityArchetype = entityManager.CreateArchetype(
			typeof(RenderMesh),
			typeof(LocalToWorld),
			typeof(Translation),
			typeof(Rotation),
			typeof(Scale)
		);
		entityManager.CreateEntity(entityArchetype, entityArray);

		sideLength = Mathf.CeilToInt(Mathf.Sqrt(amount));
		List<float3> availablePositions = new List<float3>();

		var spacingHalf = spacing / 2f;
		for (int i = 0; i < sideLength; i++)
		{
			for (int u = 0; u < sideLength; u++)
			{
				availablePositions.Add(new float3(spacingHalf + spacing * i, 3.5f, spacingHalf + spacing * u));
			}
		}

		int entityIndex = 0;
		var treeMaterialsLength = treeMaterials.Length;
		foreach(Entity entity in entityArray)
		{
			entityManager.SetSharedComponentData(entity, new RenderMesh
			{
				mesh = mesh,
				material = treeMaterials[Random.Range(0, treeMaterialsLength)],
			});

			entityManager.SetComponentData(entity, new Translation
			{
				Value = availablePositions[entityIndex],
			});

			entityManager.SetComponentData(entity, new Rotation
			{
				Value = quaternion.Euler(new float3 (0, Random.Range(0, 360f), 0)),
			});

			entityManager.SetComponentData(entity, new Scale
			{
				Value = 10f,
			});

			entityIndex++;
		}

		entityArray.Dispose();

		var groundSize = sideLength * spacing;
		ground.localScale = new Vector3(groundSize, groundSize, groundSize);
		ground.localPosition = new Vector3(sideLength * spacingHalf - spacing / spacing, -sideLength / 2f * spacing, sideLength * spacingHalf - spacing / spacing);
		ground.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(groundSize, groundSize) / 10f;
	}

	private void OnDestroy()
	{
		if (World.Active != null)
		{
			var entityManager = World.Active.EntityManager;
			foreach (var e in entityManager.GetAllEntities())
				entityManager.DestroyEntity(e);
		}		
	}
}