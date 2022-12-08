using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldObject : MonoBehaviour, IInteractable
{
	public string ID;
	public string displayName;
	public List<InteractBehavior> behaviors { get; set; }

	public void AddBehavior (InteractBehavior behavior)
	{
		behaviors.Add(behavior);
	}
	public void RemoveBehavior (InteractBehavior behavior)
	{
		behaviors.Remove(behavior);
	}
	public void DoBehavior(InteractBehavior behavior)
	{
		if (!behaviors.Contains(behavior))
		{
			Debug.LogError("The IInteractable does not contain " + behavior);
			throw new System.Exception("The IInteractable does not contain " + behavior);
		}
	}

}

public class ObjectBuilder
{

}