using System;
using System.Collections;
using System.Reflection;
using UnityEditor;

namespace UnityToolbag
{
	public static class MyPreferenceClass
	{
		[PreferenceItem("A Custom Preference Drawer")]
		public static void PreferencesGUI()
		{
			EditorGUILayout.Toggle("Bool Preference", false);
		}

		[MenuItem("Tools/Custom Prefrences")]
		private static void OpenMenu()
		{
			PreferenceWindowUtils.OpenPreference(typeof(MyPreferenceClass));
		}
	}

	public static class PreferenceWindowUtils
	{
		/// <summary>
		/// Open Unity preferences window to a class decorated with PreferenceItem attribute
		/// </summary>
		/// <param name="targetClass"></param>
		public static void OpenPreference(Type targetClass)
		{
			// Find the assemblies needed to access internal Unity classes
			Type tEditorAssembly = null;
			Type tAssemblyHelper = null;
			foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				if (tEditorAssembly == null)
				{
					var tempEditorAssembly = assembly.GetType("UnityEditor.EditorAssemblies");
					if (tempEditorAssembly != null)
						tEditorAssembly = tempEditorAssembly;
				}
				if (tAssemblyHelper == null)
				{
					var tempAssemblyHelper = assembly.GetType("UnityEditor.AssemblyHelper");
					if (tempAssemblyHelper != null)
						tAssemblyHelper = tempAssemblyHelper;
				}
				if (tEditorAssembly != null && tAssemblyHelper != null)
					break;
			}

			if (tEditorAssembly == null || tAssemblyHelper == null)
				return;

			var nonPublicStatic = BindingFlags.Static | BindingFlags.NonPublic;

			var loadedAssemblyProp = tEditorAssembly.GetProperty("loadedAssemblies", nonPublicStatic);

			IList loadedAssemblies = loadedAssemblyProp.GetValue(null, null) as IList;
			var methodGetTypes = tAssemblyHelper.GetMethod("GetTypesFromAssembly", nonPublicStatic);

			if (loadedAssemblies == null || methodGetTypes == null)
				return;

			int targetIndex = -1;
			int totalCustomSections = 0;
			// This reconstructs PreferenceWindow.AddCustomSections() as reflection calls
			foreach (object loadedAssemblyObj in loadedAssemblies)
			{
				Assembly assembly = loadedAssemblyObj as Assembly;
				if (assembly == null)
					continue;
				IList types = methodGetTypes.Invoke(null, new object[] { assembly }) as IList;
				if (types == null)
					continue;

				foreach (object typeObj in types)
				{
					Type type = typeObj as Type;
					if (type == null)
						continue;
					foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
					{
						PreferenceItem preferenceItem = Attribute.GetCustomAttribute((MemberInfo)method, typeof(PreferenceItem)) as PreferenceItem;
						if (preferenceItem != null)
						{
							if (type == targetClass)
								targetIndex = totalCustomSections;
							totalCustomSections++;
						}
					}
				}
			}

			if (targetIndex < 0)
				return;

			// Opening preference window, taken from here
			// http://answers.unity3d.com/questions/473949/open-editpreferences-from-code.html
			var asm = Assembly.GetAssembly(typeof(EditorWindow));
			var tPrefsWindow = asm.GetType("UnityEditor.PreferencesWindow");
			var methodShow = tPrefsWindow.GetMethod("ShowPreferencesWindow", nonPublicStatic);
			methodShow.Invoke(null, null);

			// Need to wait a few frames to let preference window build itself
			int frameWait = 3;
			EditorApplication.CallbackFunction waitCallback = null;
			waitCallback = () =>
			{
				frameWait--;
				if (frameWait > 0)
					return;
				EditorApplication.update -= waitCallback;

				var prefWindow = EditorWindow.GetWindow(tPrefsWindow);

				var nonPublicInstance = BindingFlags.NonPublic | BindingFlags.Instance;

				var fieldSections = tPrefsWindow.GetField("m_Sections", nonPublicInstance);
				int sectionCount = (fieldSections.GetValue(prefWindow) as IList).Count;
				int startIdx = sectionCount - totalCustomSections;

				var propSectionIndex = tPrefsWindow.GetProperty("selectedSectionIndex", nonPublicInstance);
				propSectionIndex.SetValue(prefWindow, startIdx + targetIndex, null);
			};

			EditorApplication.update += waitCallback;
		}
	}
}