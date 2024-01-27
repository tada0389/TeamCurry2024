using UnityEngine;
using UnityEditor;

namespace TadaLib.Editor
{
	/// <summary>
	/// コンパイルロック
	/// </summary>
	public class CompileLock : MonoBehaviour
	{
		[MenuItem("Rainier/Compile/Lock", false)]
		static void Lock()
		{
			Debug.Log("コンパイルしなくなります");
			EditorApplication.LockReloadAssemblies();
		}

		[MenuItem("Rainier/Compile/UnLock", false)]
		static void Unlock()
		{
			Debug.Log("コンパイルするようになります");
			EditorApplication.UnlockReloadAssemblies();
		}

		[MenuItem("Rainier/Compile/Force Compile", false)]
		static void Compile()
		{
			AssetDatabase.Refresh();
			Unlock();
			Debug.Log("コンパイルします");
			UnityEditor.Compilation.CompilationPipeline.RequestScriptCompilation();
			Lock();
		}
	}
}