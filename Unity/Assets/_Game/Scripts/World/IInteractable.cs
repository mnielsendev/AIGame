using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
	public List<InteractBehavior> behaviors { get; set; }

	public void AddBehavior (InteractBehavior behavior);
	public void RemoveBehavior (InteractBehavior behavior);
	public void DoBehavior (InteractBehavior behavior);
}

public abstract class InteractBehavior
{
	public virtual void OnInteract ()
	{
		Debug.LogError("The InteractBehavior does not override OnInteract.");
		throw new System.NotImplementedException();
	}
}