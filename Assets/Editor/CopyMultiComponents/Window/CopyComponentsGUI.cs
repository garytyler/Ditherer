/// <summary>
/// Tool Name: Copy Components.
/// Version 1.0.0
/// Copyrights Denislav Georgiev - Respect Studios LTD
/// 
/// Info: 	This tool allows you to Copy Components, from one object to many other at the same time.
/// 		It support option for choosing, which component shoud be copied
/// 
/// 		It supports 3 options for copying
/// 		1: All Components - Will copy all components from the Source, to the target objects
/// 		2: Only New - Will copy only if the target object does NOT have that component
/// 		3: Copy Values - Will copy only the component values
/// </summary>

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

// Create the Menu
public class CopyComponentsMenu : Editor
{
	[MenuItem("Window/Copy Multi Components", false, 33)]
	public static void CopyComponentsGUI()
	{
		CopyComponentsGUI copyComponentsGUI = (CopyComponentsGUI) EditorWindow.GetWindow(typeof(CopyComponentsGUI));

		copyComponentsGUI.minSize = new Vector2(350, 340);

#if UNITY_5_0 || UNITY_5_1 || UNITY_4_6
		copyComponentsGUI.title = "Copy Components";
#else
		copyComponentsGUI.titleContent = new GUIContent("Copy Components");
#endif
		copyComponentsGUI.Show ();
	}
}

// Create the GUI
public class CopyComponentsGUI : EditorWindow 
{
	// Private Variables
	private GameObject sourceGO;								// The gameObject, that, the components will be copied from
	private Component[] sourceComponents = new Component[0];	// The components, that will be copied...
	
	private string[] copyOptionPop = new string[] {"All Components", "Only New", "Only Values"};
	private int copyOptionIndex = 1;
	
	private Vector2 fromScrollPos = Vector2.zero;	// ScrollView
	private Vector2 toScrollPos = Vector2.zero;
	
	private string componentName;		// Used to clear the unneeded strings, from the components names
	private List<string> componentsName = new List<string>();	// Will hold the component's names, thas are shown in the GUI
	private List<GameObject> targetGO = new List<GameObject>();	// List of objects, to paste components to
	private string removeName;			// Helper to remove part of the string of component namne
	private string removeSystem;			// Helper to remove part of the string of component namne
	
	private bool copyAllCompT = true;
	private bool[] copyCurrentCompT;		// If the current component shoud be copied
	
	void OnEnable()
	{
		sourceComponents = new Component[0];	// Reset sourceComponents at start
	}
	
	#region Drawing GUI
	void OnGUI()	// Building the GUI
	{
		EditorGUILayout.BeginVertical(GUILayout.Width(position.width), GUILayout.Height(position.height));
		{
			//	Handles.DrawLine(new Vector3(position.width / 2, 20, 0), new Vector3 (position.width / 2, 200, 0));
			
			GUILayout.Space(10);
			
			// Begin the first Horizontal, for the object, that we'll copy components from
			EditorGUILayout.BeginHorizontal();
			{
				GUILayout.Space(10);
				#if UNITY_4_6
				EditorGUILayout.BeginVertical(GUILayout.Width(position.width/2 - 10), GUILayout.Height(position.height - 70));
				EditorGUI.HelpBox(new Rect(5, 45, position.width/2 - 10, position.height - 115), "", MessageType.None);
				#else
				EditorGUILayout.BeginVertical(EditorStyles.helpBox ,GUILayout.Width(position.width/2 - 10), GUILayout.Height(position.height - 70));
				#endif
				{
					#region "Copy From" side of GUI
					GUILayout.Label("Source (Copy From):");
					
					EditorGUI.BeginChangeCheck();	// Start Checking if the ObjField have been changed
					{
						sourceGO = EditorGUILayout.ObjectField(sourceGO, typeof(GameObject), true) as GameObject ;
					}
					if (EditorGUI.EndChangeCheck())	// If there is a change, call to ExposeComponents
					{
						ExposeComponents();
					}
					
					GUILayout.Space(20);
					
					if (sourceGO != null)
					{
						GUILayout.Label ("Components To Copy: ");
						#if UNITY_4_6
						EditorGUILayout.BeginVertical(GUILayout.Width(position.width / 2 - 20), GUILayout.Height(position.height - 160));
						EditorGUI.HelpBox(new Rect(10, 83, position.width / 2 - 20, position.height - 160), "", MessageType.None);
						#else
						EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(position.width / 2 - 20), GUILayout.Height(position.height - 160));
						#endif
						{
							fromScrollPos = EditorGUILayout.BeginScrollView(fromScrollPos);	// Adding scroll view for the Components List
							
							// Check if All has Toggled. If so call the Void to change all Toggles
							EditorGUI.BeginChangeCheck();	
							copyAllCompT = GUILayout.Toggle(copyAllCompT, new GUIContent("All", "Toggle All Components"));
							
							if (EditorGUI.EndChangeCheck())
								ToggleAllChanged();
							// END check
							
							if (sourceComponents.Length > 0)
							{
								for (int i = 0; i < sourceComponents.Length; i++)	// Loop through all components
								{
									EditorGUILayout.BeginHorizontal();
									// Make Toggle for all components names
									copyCurrentCompT[i] = GUILayout.Toggle(copyCurrentCompT[i], new GUIContent(componentsName[i].ToString(), "Shoud this component be copied"));
									
									EditorGUILayout.EndHorizontal();
								}
							}
							EditorGUILayout.EndScrollView();
						}
						EditorGUILayout.EndVertical();
					}
				}
				EditorGUILayout.EndVertical();
				#endregion
				
				#region "Paste To" side of GUI
				
				// Start Second Vertical layout for the PasteTo UI Elements
				#if UNITY_4_6
				EditorGUILayout.BeginVertical(GUILayout.Width(position.width / 2 - 10), GUILayout.Height(position.height - 70));
				EditorGUI.HelpBox(new Rect(position.width / 2, 25, position.width / 2 - 10, position.height - 112), "", MessageType.None);
				#else
				EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(position.width / 2 - 10), GUILayout.Height(position.height - 70));
				#endif
				{
					GUILayout.Label("Targets (Paste To):");	// Label
					
					if (targetGO.Count > 0)
					{
						toScrollPos = EditorGUILayout.BeginScrollView(toScrollPos);	// Adding scroll view for the ObjectsList
						{
							for (int i = 0; i < targetGO.Count; i++)
							{
								EditorGUI.BeginChangeCheck();	// Check if some of the fields is changed afterwards
								// Add Field for every selected object and expose them in the GUI
								targetGO[i] = EditorGUILayout.ObjectField(targetGO[i], typeof(GameObject), true) as GameObject;
								
								if (EditorGUI.EndChangeCheck())	// If some of the fields is changed manually
								{
									ManualAddedPasteObj(targetGO[i], i);	// Call to tell that some objField is manually changed
								}
							}
						}
						EditorGUILayout.EndScrollView();
					}
					EditorGUILayout.BeginHorizontal();
					{
						if (GUILayout.Button("+", GUILayout.Width(25), GUILayout.Height(25)))	// Button to Add to the List
							AddPasteObject();
						
						if (GUILayout.Button("-", GUILayout.Width(25), GUILayout.Height(25)))	// Button to Remove from the List
							RemovePasteObject();
						
						GUILayout.Space(position.width / 4 - 80);	// Some Space before the ClearObjects bttn
						
						if (GUILayout.Button("Clear Objects", GUILayout.Height(25)))
							ClearPasteObjList();
					}
					EditorGUILayout.EndHorizontal();
				}
				EditorGUILayout.EndVertical();
				#endregion
			}
			EditorGUILayout.EndHorizontal();
			
			EditorGUIUtility.labelWidth = 100;	// The width of the PopUp Label
			EditorGUILayout.BeginHorizontal();
			{
				copyOptionIndex = EditorGUILayout.Popup("Copy Options:", copyOptionIndex, copyOptionPop, GUILayout.Width(230));
				GUILayout.Space(15);
			}
			EditorGUILayout.EndHorizontal();
			
			if (GUILayout.Button("Copy Components", GUILayout.Height(30)))
			{
				CopyComponents();	// Call to copy actual components
			}
		}
		EditorGUILayout.EndVertical();
	}
	#endregion
	
	
	#region Expose Copy Components
	// WIll be called, only when the ObjectField changes, so it doesn't execute every frame
	private void ExposeComponents()
	{
		componentsName.Clear();	// Clears the list
		
		if (sourceGO != null)	// If there is attached object in the ObjectField
		{
			sourceComponents = sourceGO.GetComponents<Component>();				// Get all components of the gameObject
			copyCurrentCompT = new bool[sourceComponents.Length];					// Set the CheckBox array size
			
			for (int i = 0; i < sourceComponents.Length; i++)						// Loop through all components
			{
				if (sourceComponents[i] != null)
				{
					copyCurrentCompT[i] = true;									// Set all Checkboxes On
					
					componentName = sourceComponents[i].ToString();					// Get the component names
					removeName = sourceGO.name + " (";							// Remove name and " (" from the name
					removeSystem =  "UnityEngine.";									// Remove UnityEngine from the name
					componentName = componentName.Remove(componentName.Length - 1);	// Remove the last character ")", from name
					
					componentName = componentName.Replace (removeName, "");			// replace the string, to remove the name of the GO
					componentName = componentName.Replace (removeSystem, "");		// Removes the UnityEngine from string
					
					componentsName.Add(componentName); 
				}
				else // If the Component Is Missing
				{
					componentsName.Add("***Missing Component***");	// Add as Missing
					copyCurrentCompT[i] = false;
					Debug.LogWarning("One of the Components is Null! Probably a Missing or Non-Compilled Script. Will be Skipped");
				}
			}
		}
		else 	// Source GO is empty
			sourceComponents = new Component[0];
	}
	#endregion
	
	#region Paste Object UI Management
	// To add PasteObjects
	private void AddPasteObject()
	{
		GameObject[] curSelection = Selection.gameObjects;
		
		if (curSelection.Length > 0)
		{
			for (int i = 0; i < curSelection.Length; i++)
			{
				if (!targetGO.Contains(curSelection[i]))
				{
					if (curSelection[i] != sourceGO)
						targetGO.Add (curSelection[i]);
					else
						Debug.LogWarning("Attempt to add Source GameObject: \"" + curSelection[i].name + "\", to Targets List. Object Removed...");
				}
				else
					Debug.LogWarning("Object \" " + targetGO[i].name + "\", is allready in the List");
			}
		}
		else
			Debug.LogWarning("Select At Least One Object");
	}
	
	// To remove the last element of the Paste Objects
	private void RemovePasteObject()
	{
		if (targetGO.Count > 0)
			targetGO.RemoveAt(targetGO.Count - 1);
		else
			Debug.LogWarning("Paste Objects List is Emptry!");
	}
	
	private void ClearPasteObjList()
	{
		targetGO.Clear();
	}
	
	// When we add manually Target Objects
	private void ManualAddedPasteObj(GameObject objChanged, int listNum)
	{
		if (targetGO.Count > 0)	// If there are any target objects
		{
			for (int i = 0; i < targetGO.Count; i++)	// Loop through them...
			{
				if (targetGO[i] != null)	// If current Target Index (Field) is Not Empty
				{
					if (i != listNum)
					{
						if (targetGO[i] == objChanged)
						{
							// Warn user, that there is same object in more than one field
							Debug.LogWarning("Object: " + objChanged.name + ", was added in 2 fields");
						}
					}
					if (targetGO[i] == sourceGO)	// If the Source is added as target
						Debug.LogWarning("Source GameObject: \"" + sourceGO.name + "\" was added in Field: " + (i + 1) + ". \nWill Copy and Paste to the same GameObject");
				}
				else
					Debug.LogWarning("Target Field was left Empty");
			}
		}
	}
	#endregion
	
	#region Copy Components
	// Copy the actual components
	private void CopyComponents()
	{
		bool componentMatch = false;
		bool nullComponentFound = false;	// Will be true, to show Warning, if there is a Null (Missing) Component
		
		if (sourceComponents.Length > 0)
		{
			if (targetGO.Count > 0)	// If there are TargetGO
			{
				for (int i = 0; i < targetGO.Count; i++)	// Loop through all targetObjects
				{
					if (targetGO[i] != null)
					{
						Component[] targetComponents = targetGO[i].GetComponents<Component>();	// Get the component of the cur object
						
						for (int a = 0; a < sourceComponents.Length; a++)	// Loop through all Source Obj's Components
						{
							if (sourceComponents[a] == null)	// If this component is Null
								continue;	// Skip the loop and continue
							
							if (copyCurrentCompT[a])	// If the current component shoud be copied
							{
								if (copyOptionIndex == 0)	// If All shoud be copied
								{
									UnityEditorInternal.ComponentUtility.CopyComponent(sourceComponents[a]);
									UnityEditorInternal.ComponentUtility.PasteComponentAsNew(targetGO[i]);
								}
								
								if (copyOptionIndex == 1 || copyOptionIndex == 2)	// If Copy only new or Copy Values
								{
									componentMatch = false;								// Set by default to false
									for (int z = 0; z < targetComponents.Length; z++)	// Loop through all destination Obj's Components
									{
										if (targetComponents[z] == null)	// If there is a Null Component, skip this loop
										{
											nullComponentFound = true;
											continue;
										}
										if (sourceComponents[a].GetType() == targetComponents[z].GetType())	// If the components match, The destination Obj have that component
										{
											componentMatch = true;	// Set to true, so we can copy that component
											
											if (copyOptionIndex == 2)	// If copyValues is On, then Copy the values of the existing Components
											{
												UnityEditorInternal.ComponentUtility.CopyComponent(sourceComponents[a]);
												UnityEditorInternal.ComponentUtility.PasteComponentValues(targetComponents[z]);
											}
										}
									}
								}
								
								if (copyOptionIndex == 1 ) // If Only New (Missing from Target) shoud be copied
								{
									if (!componentMatch && copyOptionIndex == 1)	// If this component[a], doesn't exist in the TargetGO[i], copy it
									{
										UnityEditorInternal.ComponentUtility.CopyComponent(sourceComponents[a]);
										UnityEditorInternal.ComponentUtility.PasteComponentAsNew(targetGO[i]);
									}
								}
							}
						}
					}
					else
						Debug.LogWarning("TargetField: " + i + " is Empty");
				}
				if (nullComponentFound)	// If there is a Null Component
					Debug.LogWarning("Some of the Target Objects have Missing Component, which might cause problems");
			}
			else 		// If NO TargetGO added
				Debug.LogWarning("No Target GameObjects Added");
		}
		else
			Debug.LogWarning("No Components To Copy. Add a Source Object");
	}
	#endregion
	
	// To Toggle All Components CheckBoxes
	private void ToggleAllChanged()
	{
		for (int i = 0; i < copyCurrentCompT.Length; i++)
		{
			copyCurrentCompT[i] = copyAllCompT;
		}
	}
	
	
	// Reset the GUI, when closed
	void OnDestroy()
	{
		sourceGO = null;						// Reset the source Obj field
		copyOptionIndex = 1;					// Reset Copy Popup
		sourceComponents = new Component[0];	// Reset the source Components Array
		copyCurrentCompT = new bool[0];			// Reset Components Toggle Array
		
		componentsName.Clear ();				// Clear ComponentsNames List
		targetGO.Clear ();						// Clear Target Objects List
	}


}
















