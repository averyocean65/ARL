using System;
using UnityEngine;

namespace ARL.Utils;

// don't ask me how the generic stuff works
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {
	public static T Instance = null!;

	protected virtual void Awake() {
		if (Instance) {
			Destroy(this);
			return;
		}

		Instance = (T)this;
	}
}