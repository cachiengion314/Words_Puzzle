/**
 * Author NBear - Nguyen Ba Hung - nbhung71711@gmail.com 
 **/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utilities.Common
{
#if UNITY_EDITOR
    public static class EditorHelper
    {
        #region File Utilities

        /// <summary>
        /// T must be Serializable
        /// </summary>
        public static void SaveJsonPanel<T>(string pMainDirectory, string defaultName, T obj)
        {
            if (string.IsNullOrEmpty(pMainDirectory))
                pMainDirectory = Application.dataPath;

            string path = EditorUtility.SaveFilePanel("Save File", pMainDirectory, defaultName, "json");
            if (!string.IsNullOrEmpty(path))
                SaveToJsonFile(path, obj);
        }

        public static void SaveToJsonFile<T>(string pPath, T pObj)
        {
            string jsonString = JsonUtility.ToJson(pObj);
            if (!string.IsNullOrEmpty(jsonString) && jsonString != "{}")
            {
                if (File.Exists(pPath))
                    File.Delete(pPath);
                File.WriteAllText(pPath, jsonString);
            }
        }

        /// <summary>
        /// T must be Serializable
        /// </summary>
        public static bool LoadJsonPanel<T>(string pMainDirectory, ref T pOutput)
        {
            if (string.IsNullOrEmpty(pMainDirectory))
                pMainDirectory = Application.dataPath;

            string path = EditorUtility.OpenFilePanel("Open File", pMainDirectory, "json");
            if (string.IsNullOrEmpty(path))
                return false;
            else
                return LoadJsonFromFile(path, ref pOutput);
        }

        public static bool LoadJsonFromFile<T>(string pPath, ref T pOutput)
        {
            if (!string.IsNullOrEmpty(pPath))
            {
                pOutput = JsonUtility.FromJson<T>(File.ReadAllText(pPath));
                return true;
            }
            return false;
        }

        public static void SaveToXMLFile<T>(string pPath, T pObj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextWriter writer = new StreamWriter(pPath))
            {
                if (File.Exists(pPath))
                    File.Delete(pPath);
                serializer.Serialize(writer, pObj);
            }
        }

        public static void LoadFromXMLFile<T>(string pPath, ref T pObj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StreamReader(pPath))
                pObj = (T)serializer.Deserialize(reader);
        }

        #endregion

        //========================================

        #region Quick Shortcut

        /// <summary>
        /// Find all scene components, active or inactive.
        /// </summary>
        public static List<T> FindAll<T>() where T : Component
        {
            T[] comps = Resources.FindObjectsOfTypeAll(typeof(T)) as T[];

            List<T> list = new List<T>();

            foreach (T comp in comps)
            {
                if (comp.gameObject.hideFlags == 0)
                {
                    string path = AssetDatabase.GetAssetPath(comp.gameObject);
                    if (string.IsNullOrEmpty(path)) list.Add(comp);
                }
            }
            return list;
        }

        public static void Save()
        {
            AssetDatabase.SaveAssets();
        }

        public static T CreateScriptableAsset<T>(string path) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
            return asset;
        }

        public static string GetObjectPath(Object pObj)
        {
            return AssetDatabase.GetAssetPath(pObj);
        }

        static public Object LoadAsset(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            return AssetDatabase.LoadMainAssetAtPath(path);
        }

        /// <summary>
        /// Convenience function to load an asset of specified type, given the full path to it.
        /// </summary>
        public static T LoadAsset<T>(string path) where T : Object
        {
            Object obj = LoadAsset(path);
            if (obj == null) return null;

            T val = obj as T;
            if (val != null) return val;

            if (typeof(T).IsSubclassOf(typeof(Component)))
            {
                if (obj.GetType() == typeof(GameObject))
                {
                    GameObject go = obj as GameObject;
                    return go.GetComponent(typeof(T)) as T;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the specified object's GUID.
        /// </summary>
        public static string ObjectToGUID(Object obj)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            return (!string.IsNullOrEmpty(path)) ? AssetDatabase.AssetPathToGUID(path) : null;
        }

        #endregion

        //=============================

        #region Tools

        /// <summary>
        /// This is useless thing, just keeping it to remember some methods 
        /// </summary>
        /// <returns></returns>
        public static SerializedProperty SerializeField(SerializedProperty field, SerializedPropertyType type)
        {
            if (type == SerializedPropertyType.Integer)
            {
                field.intValue = EditorGUILayout.IntField(field.displayName, field.intValue);
            }
            else if (type == SerializedPropertyType.Float)
            {
                field.floatValue = EditorGUILayout.FloatField(field.displayName, field.intValue);
            }
            else if (type == SerializedPropertyType.String)
            {
                field.stringValue = EditorGUILayout.TextField(field.displayName, field.stringValue);
            }
            else if (type == SerializedPropertyType.ObjectReference)
            {

            }
            else if (type == SerializedPropertyType.Enum)
            {

            }

            return field;
        }

        public static SerializedProperty SerializeFields(SerializedProperty obj, params string[] properties)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                var item = obj.FindPropertyRelative(properties[i]);
                item = SerializeField(item, item.propertyType);
            }

            return obj;
        }

        public static void Button(string label, Action doSomthing, int width = 0)
        {
            if (width > 0)
            {
                if (GUILayout.Button(label, GUILayout.Width(width)))
                    doSomthing();
            }
            else
            {
                if (GUILayout.Button(label))
                    doSomthing();
            }
        }

        public static void ButtonColor(string label, Action doSomthing, Color color = default(Color), int width = 0)
        {
            var defaultColor = GUI.backgroundColor;

            if (color != default(Color))
                GUI.backgroundColor = color;

            if (width > 0)
            {
                if (GUILayout.Button(label, GUILayout.Width(width)))
                    doSomthing();
            }
            else
            {
                if (GUILayout.Button(label))
                    doSomthing();
            }

            GUI.backgroundColor = defaultColor;
        }

        public static void BoxVertical(Action doSomthing, Color color = default(Color), bool isBox = false, float pFixedWith = 0)
        {
            var defaultColor = GUI.backgroundColor;
            if (color != default(Color))
                GUI.backgroundColor = color;

            var style = new GUIStyle(EditorStyles.helpBox);
            if (!isBox) style = new GUIStyle();
            if (pFixedWith > 0) style.fixedWidth = pFixedWith;

            EditorGUILayout.BeginVertical(style);

            doSomthing();

            EditorGUILayout.EndVertical();
            if (color != default(Color))
                GUI.backgroundColor = defaultColor;
        }

        public static void BoxVertical(string pTitle, Action doSomthing, Color color = default(Color), bool isBox = false, float pFixedWith = 0)
        {
            var defaultColor = GUI.backgroundColor;
            if (color != default(Color))
                GUI.backgroundColor = color;

            var style = new GUIStyle(EditorStyles.helpBox);
            if (!isBox) style = new GUIStyle();
            if (pFixedWith > 0) style.fixedWidth = pFixedWith;

            EditorGUILayout.BeginVertical(style);

            if (!string.IsNullOrEmpty(pTitle))
                MakeHeader(pTitle);

            doSomthing();

            EditorGUILayout.EndVertical();
            if (color != default(Color))
                GUI.backgroundColor = defaultColor;
        }

        public static void BoxHorizontal(Action doSomthing, Color color = default(Color), bool isBox = false, float pFixedWith = 0)
        {
            var defaultColor = GUI.backgroundColor;
            if (color != default(Color))
                GUI.backgroundColor = color;

            var style = new GUIStyle(EditorStyles.helpBox);
            if (!isBox) style = new GUIStyle();
            if (pFixedWith > 0) style.fixedWidth = pFixedWith;

            EditorGUILayout.BeginHorizontal(style);

            doSomthing();

            EditorGUILayout.EndHorizontal();

            if (color != default(Color))
                GUI.backgroundColor = defaultColor;
        }

        public static void BoxHorizontal(string pTitle, Action doSomthing, Color color = default(Color), bool isBox = false, float pFixedWith = 0)
        {
            var defaultColor = GUI.backgroundColor;
            if (color != default(Color))
                GUI.backgroundColor = color;

            var style = new GUIStyle(EditorStyles.helpBox);
            if (!isBox) style = new GUIStyle();
            if (pFixedWith > 0) style.fixedWidth = pFixedWith;

            if (!string.IsNullOrEmpty(pTitle))
            {
                EditorGUILayout.BeginVertical(style);
                MakeHeader(pTitle);
            }

            EditorGUILayout.BeginHorizontal();

            doSomthing();

            EditorGUILayout.EndHorizontal();

            if (!string.IsNullOrEmpty(pTitle))
                EditorGUILayout.EndVertical();

            if (color != default(Color))
                GUI.backgroundColor = defaultColor;
        }

        public static void Foldout(string label, Action DoSomething)
        {
            bool show = EditorPrefs.GetBool(label + "foldout", false);
            show = EditorGUILayout.Foldout(show, label);
            if (show)
            {
                DoSomething();
            }
            if (GUI.changed)
                EditorPrefs.SetBool(label + "foldout", show);
        }

        public static Vector2 ScrollBar(ref Vector2 scrollPos, float width, float height, string label, Action action)
        {
            EditorGUILayout.BeginVertical("box");
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(height));
            action();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            return scrollPos;
        }

        public static T DropdownList<T>(T value, string label, int labelWidth = 80, int valueWidth = 0) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enumerated type");

            EditorGUILayout.BeginHorizontal();

            if (!string.IsNullOrEmpty(label))
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            }

            var enumValues = Enum.GetValues(typeof(T));
            string[] selections = new string[enumValues.Length];

            int i = 0;
            foreach (T item in enumValues)
            {
                selections[i] = item.ToString();
                i++;
            }

            int index = 0;
            for (i = 0; i < selections.Length; i++)
            {
                if (value.ToString() == selections[i])
                {
                    index = i;
                }
            }

            if (valueWidth != 0)
                index = EditorGUILayout.Popup(index, selections, GUILayout.Width(valueWidth));
            else
                index = EditorGUILayout.Popup(index, selections);


            EditorGUILayout.EndHorizontal();

            i = 0;
            foreach (T item in enumValues)
            {
                if (i == index)
                    return item;
                i++;
            }

            return default(T);
        }

        public static void ConfimPopup(Action pOnYes, Action pOnNo = null)
        {
            if (EditorUtility.DisplayDialog("Confirm your action", "Are you sure you want to do this", "Yes", "No"))
                pOnYes();
            else
            {
                if (pOnNo != null) pOnNo();
            }
        }

        public static void ShowList<T>(ref List<T> pList, string pName, bool pShowObjectBox = true, bool pInBox = false) where T : UnityEngine.Object
        {
            var prevColor = GUI.color;
            GUI.backgroundColor = new Color(1, 1, 0.5f);

            //bool show = EditorPrefs.GetBool(pName, false);
            //GUIContent content = new GUIContent(pName);
            //GUIStyle style = new GUIStyle(EditorStyles.foldout);
            //style.margin = new RectOffset(pInBox ? 13 : 0, 0, 0, 0);
            //show = EditorGUILayout.Foldout(show, content, style);

            bool show = DrawHeader(string.Format("{0} ({1})", pName, pList.Count), pName, false);

            var list = pList;
            if (show)
            {
                int page = EditorPrefs.GetInt(pName + "_page", 0);
                int totalPages = Mathf.CeilToInt(list.Count * 1f / 20f);
                int from = page * 20;
                int to = page * 20 + 20;
                if (to >= list.Count)
                    to = list.Count - 1;

                BoxVertical(() =>
                {
                    if (totalPages > 0)
                    {
                        EditorGUILayout.BeginHorizontal();
                        Button("Prev", () =>
                        {
                            if (page > 0) page--;
                            EditorPrefs.SetInt(pName + "_page", page);
                        });
                        EditorGUILayout.LabelField(string.Format("{0}-{1} ({2})", from + 1, to, list.Count));
                        Button("Next", () =>
                        {
                            if (page < totalPages - 1) page++;
                            EditorPrefs.SetInt(pName + "_page", page);
                        });
                        EditorGUILayout.EndHorizontal();
                    }
                    for (int i = from; i <= to; i++)
                    {
                        BoxHorizontal(() =>
                        {
                            if (pShowObjectBox)
                                list[i] = (T)ObjectField<T>(list[i], "", 0, 50, true);
                            EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(25));
                            list[i] = (T)ObjectField<T>(list[i], "");
                            ButtonColor("x", () => { list.RemoveAt(i); }, Color.red, 20);
                        });
                    }
                    if (totalPages > 0)
                    {
                        EditorGUILayout.BeginHorizontal();
                        Button("Prev", () =>
                        {
                            if (page > 0) page--;
                            EditorPrefs.SetInt(pName + "_page", page);
                        });
                        EditorGUILayout.LabelField(string.Format("{0}-{1} ({2})", from + 1, to, list.Count));
                        Button("Next", () =>
                        {
                            if (page < totalPages - 1) page++;
                            EditorPrefs.SetInt(pName + "_page", page);
                        });
                        EditorGUILayout.EndHorizontal();
                    }
                    Button("Add New", () => { list.Add(null); });
                    Button("Sort By Name", () =>
                    {
                        list = list.OrderBy(m => m.name).ToList();
                    });
                    Button("Remove Duplicate", () =>
                    {
                        List<int> duplicate = new List<int>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            int count = 0;
                            for (int j = list.Count - 1; j >= 0; j--)
                            {
                                if (list[j] == list[i])
                                {
                                    count++;
                                    if (count > 1)
                                        duplicate.Add(j);
                                }
                            }
                        }
                        for (int j = list.Count - 1; j >= 0; j--)
                        {
                            if (duplicate.Contains(j))
                                list.Remove(list[j]);
                        }
                    });
                }, default(Color), true);
            }
            pList = list;

            if (GUI.changed)
                EditorPrefs.SetBool(pName, show);

            GUI.backgroundColor = prevColor;
        }

        public static void ShowListWithSearch<T>(ref List<T> pList, string pName, bool pShowBox = true, bool pInBox = false) where T : UnityEngine.Object
        {
            var prevColor = GUI.color;
            GUI.backgroundColor = new Color(1, 1, 0.5f);

            //bool show = EditorPrefs.GetBool(pName, false);
            //GUIContent content = new GUIContent(pName);
            //GUIStyle style = new GUIStyle(EditorStyles.foldout);
            //style.margin = new RectOffset(pInBox ? 13 : 0, 0, 0, 0);
            //show = EditorGUILayout.Foldout(show, content, style);

            string search = EditorPrefs.GetString(pName + "_search");
            bool show = DrawHeader(string.Format("{0} ({1})", pName, pList.Count), pName, false);

            var list = pList;
            if (show)
            {
                int page = EditorPrefs.GetInt(pName + "_page", 0);
                int totalPages = Mathf.CeilToInt(list.Count * 1f / 20f);
                int from = page * 20;
                int to = page * 20 + 20;
                if (to >= list.Count)
                    to = list.Count - 1;

                BoxVertical(() =>
                {
                    if (totalPages > 0)
                    {
                        EditorGUILayout.BeginHorizontal();
                        Button("Prev", () =>
                        {
                            if (page > 0) page--;
                            EditorPrefs.SetInt(pName + "_page", page);
                        });
                        EditorGUILayout.LabelField(string.Format("{0}-{1} ({2})", from + 1, to, list.Count));
                        Button("Next", () =>
                        {
                            if (page < totalPages - 1) page++;
                            EditorPrefs.SetInt(pName + "_page", page);
                        });
                        EditorGUILayout.EndHorizontal();
                    }

                    search = GUILayout.TextField(search);

                    bool searching = !string.IsNullOrEmpty(search);
                    for (int i = from; i <= to; i++)
                    {
                        if (searching && !list[i].name.Contains(search))
                            continue;

                        BoxHorizontal(() =>
                        {
                            if (pShowBox)
                                list[i] = (T)ObjectField<T>(list[i], "", 0, 50, true);
                            EditorGUILayout.LabelField(i.ToString(), GUILayout.Width(25));
                            list[i] = (T)ObjectField<T>(list[i], "");
                            ButtonColor("x", () => { list.RemoveAt(i); }, Color.red, 20);
                        });

                    }
                    if (totalPages > 0)
                    {
                        EditorGUILayout.BeginHorizontal();
                        Button("Prev", () =>
                        {
                            if (page > 0) page--;
                            EditorPrefs.SetInt(pName + "_page", page);
                        });
                        EditorGUILayout.LabelField(string.Format("{0}-{1} ({2})", from + 1, to, list.Count));
                        Button("Next", () =>
                        {
                            if (page < totalPages - 1) page++;
                            EditorPrefs.SetInt(pName + "_page", page);
                        });
                        EditorGUILayout.EndHorizontal();
                    }
                    Button("Add New", () => { list.Add(null); });
                    Button("Sort By Name", () =>
                    {
                        list = list.OrderBy(m => m.name).ToList();
                    });
                    Button("Remove Duplicate", () =>
                    {
                        List<int> duplicate = new List<int>();
                        for (int i = 0; i < list.Count; i++)
                        {
                            int count = 0;
                            for (int j = list.Count - 1; j >= 0; j--)
                            {
                                if (list[j] == list[i])
                                {
                                    count++;
                                    if (count > 1)
                                        duplicate.Add(j);
                                }
                            }
                        }
                        for (int j = list.Count - 1; j >= 0; j--)
                        {
                            if (duplicate.Contains(j))
                                list.Remove(list[j]);
                        }
                    });
                }, default(Color), true);
            }
            pList = list;

            if (GUI.changed)
            {
                EditorPrefs.SetBool(pName, show);
                EditorPrefs.SetString(pName + "_search", search);
            }

            GUI.backgroundColor = prevColor;
        }

        public static string CreateTabs(string pName, params string[] pTabsName)
        {
            string currentTab = EditorPrefs.GetString(string.Format("{0}_current_tab", pName), pTabsName[0]);

            BoxHorizontal(() =>
            {
                foreach (var tabName in pTabsName)
                {
                    var buttonStyle = new GUIStyle(EditorStyles.toolbarButton);
                    buttonStyle.fixedHeight = 0;
                    buttonStyle.padding = new RectOffset(8, 8, 8, 8);
                    buttonStyle.normal.textColor = Color.white;
                    buttonStyle.fontStyle = FontStyle.Bold;
                    buttonStyle.fontSize = 13;

                    var preColor = GUI.color;
                    var color = currentTab == tabName ? Color.yellow : new Color(0.5f, 0.5f, 0.5f);
                    GUI.color = color;

                    if (GUILayout.Button(tabName, buttonStyle))
                    {
                        currentTab = tabName;
                        EditorPrefs.SetString(string.Format("{0}_current_tab", pName), currentTab);
                    }
                    GUI.color = preColor;
                }
            });

            return currentTab;
        }

        private static void MakeHeader(string pHeader)
        {
            Color prevColor = GUI.color;

            var directiveLineStyle = new GUIStyle(EditorStyles.toolbar);
            directiveLineStyle.fixedHeight = 0;
            directiveLineStyle.padding = new RectOffset(8, 8, 0, 0);

            var headerStyle = new GUIStyle(EditorStyles.largeLabel);
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.normal.textColor = Color.white;
            headerStyle.alignment = TextAnchor.MiddleCenter;

            GUI.color = new Color(0.5f, 0.5f, 0.5f);
            EditorGUILayout.BeginHorizontal(directiveLineStyle, GUILayout.Height(20));
            {
                GUI.color = prevColor;
                EditorGUILayout.LabelField(pHeader, headerStyle, GUILayout.Height(20));
            }
            EditorGUILayout.EndHorizontal();
        }

        public static string ButtonFolder(string label, string pSavingKey, string mDefaultPath = null)
        {
            if (mDefaultPath == null)
                mDefaultPath = Application.dataPath;
            string savedPath = EditorPrefs.GetString(label + pSavingKey, mDefaultPath);
            TextField(savedPath, label, 80, 200, () =>
            {
                Button("...", () =>
                {
                    string path = OpenFolderPanel(savedPath);
                    if (!string.IsNullOrEmpty(path))
                    {
                        EditorPrefs.SetString(label + pSavingKey, path);
                        savedPath = path;
                    }
                }, 25);
            });
            return savedPath;
        }

        #endregion

        //=============================

        #region Input Fields

        public static string TextField(string value, string label, int labelWidth = 80, int valueWidth = 0, Action additional = null)
        {
            EditorGUILayout.BeginHorizontal();
            if (!string.IsNullOrEmpty(label))
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            }

            if (value == null)
                value = "";

            var style = new GUIStyle(EditorStyles.toolbarTextField);
            style.alignment = TextAnchor.MiddleLeft;
            style.margin = new RectOffset(0, 0, 4, 4);

            string str;
            if (valueWidth == 0)
                str = EditorGUILayout.TextField(value, style, GUILayout.Height(20), GUILayout.MinWidth(40));
            else
                str = EditorGUILayout.TextField(value, style, GUILayout.Height(20), GUILayout.MinWidth(40), GUILayout.Width(valueWidth));

            if (additional != null) additional();

            EditorGUILayout.EndHorizontal();

            return str;
        }

        public static string DropdownList(string value, string label, string[] selections, int labelWidth = 80, int valueWidth = 0)
        {
            if (selections.Length == 0)
                return "";

            EditorGUILayout.BeginHorizontal();
            if (!string.IsNullOrEmpty(label))
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            }

            int index = 0;

            for (int i = 0; i < selections.Length; i++)
            {
                if (value == selections[i])
                    index = i;
            }

            if (valueWidth != 0)
                index = EditorGUILayout.Popup(index, selections, GUILayout.Width(valueWidth));
            else
                index = EditorGUILayout.Popup(index, selections);


            EditorGUILayout.EndHorizontal();

            return selections[index] == null ? "" : selections[index];
        }

        public static int Popup(int value, string label, int[] selections, int labelWidth = 80, int valueWidth = 0)
        {
            if (selections.Length == 0)
                return -1;

            EditorGUILayout.BeginHorizontal();
            if (!string.IsNullOrEmpty(label))
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            }

            int index = 0;

            string[] selectsionsStr = new string[selections.Length];
            for (int i = 0; i < selections.Length; i++)
            {
                if (value == selections[i])
                    index = i;
                selectsionsStr[i] = selections[i].ToString();
            }

            if (valueWidth != 0)
                index = EditorGUILayout.Popup(index, selectsionsStr, GUILayout.Width(valueWidth));
            else
                index = EditorGUILayout.Popup(index, selectsionsStr);

            EditorGUILayout.EndHorizontal();

            return selections[index];
        }

        public static bool Toggle(bool value, string label, int labelWidth = 80, int valueWidth = 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (!string.IsNullOrEmpty(label))
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            }

            bool result;

            var style = new GUIStyle(EditorStyles.toggle);
            style.alignment = TextAnchor.MiddleCenter;
            style.fixedWidth = 20;
            style.fixedHeight = 20;

            if (valueWidth == 0)
                result = EditorGUILayout.Toggle(value, style, GUILayout.Height(20), GUILayout.MinWidth(40));
            else
                result = EditorGUILayout.Toggle(value, style, GUILayout.Height(20), GUILayout.MinWidth(40), GUILayout.Width(valueWidth));

            EditorGUILayout.EndHorizontal();

            return result;
        }

        public static int IntField(int value, string label, int labelWidth = 80, int valueWidth = 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (!string.IsNullOrEmpty(label))
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            }

            int result;
            if (valueWidth == 0)
                result = EditorGUILayout.IntField(value, GUILayout.Height(20), GUILayout.MinWidth(40));
            else
                result = EditorGUILayout.IntField(value, GUILayout.Height(20), GUILayout.MinWidth(40), GUILayout.Width(valueWidth));

            EditorGUILayout.EndHorizontal();

            return result;
        }

        public static float FloatField(float value, string label, int labelWidth = 80, int valueWidth = 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (!string.IsNullOrEmpty(label))
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            }

            float result;
            if (valueWidth == 0)
                result = EditorGUILayout.FloatField(value, GUILayout.Height(20));
            else
                result = EditorGUILayout.FloatField(value, GUILayout.Height(20), GUILayout.Width(valueWidth));

            EditorGUILayout.EndHorizontal();

            return result;
        }

        public static UnityEngine.Object ObjectField<T>(UnityEngine.Object value, string label, int labelWidth = 80, int valueWidth = 0, bool showAsBox = false)
        {
            EditorGUILayout.BeginHorizontal();
            if (!string.IsNullOrEmpty(label))
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            }

            UnityEngine.Object result;
            if (valueWidth == 0)
                result = EditorGUILayout.ObjectField(value, typeof(T), true);
            else if (showAsBox)
                result = EditorGUILayout.ObjectField(value, typeof(T), true, GUILayout.Width(valueWidth), GUILayout.Height(valueWidth));
            else if (!showAsBox)
                result = EditorGUILayout.ObjectField(value, typeof(T), true, GUILayout.Width(valueWidth));
            else
                result = null;


            EditorGUILayout.EndHorizontal();

            return result;
        }

        public static void LabelField(string label, int labelHeight = 20, bool isBold = true)
        {
            if (isBold)
                EditorGUILayout.LabelField(label, EditorStyles.boldLabel, GUILayout.Height(labelHeight));
            else
                EditorGUILayout.LabelField(label, GUILayout.Height(labelHeight));
        }

        public static Vector2 Vector2Field(Vector2 value, string label, int labelWidth = 80, int valueWidth = 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (!string.IsNullOrEmpty(label))
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            }

            Vector2 result;
            if (valueWidth == 0)
                result = EditorGUILayout.Vector2Field("", value, GUILayout.Height(20), GUILayout.MinWidth(40));
            else
                result = EditorGUILayout.Vector2Field("", value, GUILayout.Height(20), GUILayout.MinWidth(40), GUILayout.Width(valueWidth));

            EditorGUILayout.EndHorizontal();

            return result;
        }

        public static Vector3 Vector3Field(Vector3 value, string label, int labelWidth = 80, int valueWidth = 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (!string.IsNullOrEmpty(label))
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            }

            Vector3 result;
            if (valueWidth == 0)
                result = EditorGUILayout.Vector3Field("", value, GUILayout.Height(20), GUILayout.MinWidth(40));
            else
                result = EditorGUILayout.Vector3Field("", value, GUILayout.Height(20), GUILayout.MinWidth(40), GUILayout.Width(valueWidth));

            EditorGUILayout.EndHorizontal();

            return result;
        }

        public static float[] ArrayField(float[] values, string label, bool showHorizontal = true, int labelWidth = 80, int valueWidth = 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (!string.IsNullOrEmpty(label))
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            }

            if (showHorizontal)
                EditorGUILayout.BeginHorizontal();
            else
                EditorGUILayout.BeginVertical();
            float[] results = new float[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                float result;
                if (valueWidth == 0)
                    result = EditorGUILayout.FloatField(values[i], GUILayout.Height(20));
                else
                    result = EditorGUILayout.FloatField(values[i], GUILayout.Height(20), GUILayout.Width(valueWidth));

                results[i] = result;
            }
            if (showHorizontal)
                EditorGUILayout.EndHorizontal();
            else
                EditorGUILayout.EndVertical();


            EditorGUILayout.EndHorizontal();

            return results;
        }

        public static SerializedProperty SerializeField(SerializedObject pObject, string pPropertyName, string pDisplayName = null, bool isArray = false)
        {
            SerializedProperty serializedProperty = pObject.FindProperty(pPropertyName);
            if (serializedProperty == null)
            {
                Debug.Log("Not found property " + pPropertyName);
                return null;
            }

            if (!isArray)
            {
                EditorGUILayout.PropertyField(serializedProperty, new GUIContent(string.IsNullOrEmpty(pDisplayName) ? serializedProperty.displayName : pDisplayName));
                return serializedProperty;
            }
            else
            {
                if (serializedProperty.isExpanded)
                    EditorGUILayout.PropertyField(serializedProperty, true);
                else
                    EditorGUILayout.PropertyField(serializedProperty, new GUIContent(serializedProperty.displayName));
                return serializedProperty;
            }
        }

        /// <summary>
        /// Draw a distinctly different looking header label
        /// </summary>
        public static bool DrawHeader(string text, string key, bool minimalistic = false)
        {
            bool state = EditorPrefs.GetBool(key, true);

            if (!minimalistic) GUILayout.Space(3f);
            if (!state) GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
            GUILayout.BeginHorizontal();
            GUI.changed = false;

            if (minimalistic)
            {
                if (state) text = "\u25BC" + (char)0x200a + text;
                else text = "\u25BA" + (char)0x200a + text;

                GUILayout.BeginHorizontal();
                GUI.contentColor = EditorGUIUtility.isProSkin ? new Color(1f, 1f, 1f, 0.7f) : new Color(0f, 0f, 0f, 0.7f);
                if (!GUILayout.Toggle(true, text, "PreToolbar2", GUILayout.MinWidth(20f)))
                    state = !state;
                GUI.contentColor = Color.white;
                GUILayout.EndHorizontal();
            }
            else
            {
                text = "<b><size=11>" + text + "</size></b>";
                if (state) text = "\u25BC " + text;
                else text = "\u25BA " + text;
                if (!GUILayout.Toggle(true, text, "dragtab", GUILayout.MinWidth(20f)))
                    state = !state;
            }

            if (GUI.changed)
                EditorPrefs.SetBool(key, state);

            if (!minimalistic) GUILayout.Space(2f);
            GUILayout.EndHorizontal();
            GUI.backgroundColor = Color.white;
            if (!state) GUILayout.Space(3f);
            return state;
        }

        /// <summary>
        /// Draw a visible separator in addition to adding some padding.
        /// </summary>
        static public void DrawSeparator()
        {
            GUILayout.Space(12f);

            if (Event.current.type == EventType.Repaint)
            {
                Texture2D tex = EditorGUIUtility.whiteTexture;
                Rect rect = GUILayoutUtility.GetLastRect();
                GUI.color = new Color(0f, 0f, 0f, 0.25f);
                GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 4f), tex);
                GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 1f), tex);
                GUI.DrawTexture(new Rect(0f, rect.yMin + 9f, Screen.width, 1f), tex);
                GUI.color = Color.white;
            }
        }

        #endregion

        //=============================

        #region Get / Load Assets

        public static string OpenFolderPanel(string pFolderPath = null)
        {
            if (pFolderPath == null)
                pFolderPath = Application.dataPath;
            string path = EditorUtility.OpenFolderPanel("Select Folder", pFolderPath, "");
            return path;
        }

        private static string FormatPathToUnityPath(string path)
        {
            string[] paths = path.Split('/');

            int startJoint = -1;
            string realPath = "";

            for (int i = 0; i < paths.Length; i++)
            {
                if (paths[i] == "Assets")
                {
                    startJoint = i;
                }
                if (startJoint != -1 && i >= startJoint)
                {
                    if (i == paths.Length - 1)
                        realPath += paths[i];
                    else
                        realPath += paths[i] + "/";
                }
            }
            return realPath;
        }

        public static string[] GetDirectories(string path)
        {
            var directories = Directory.GetDirectories(path);

            for (int i = 0; i < directories.Length; i++)
                directories[i] = FormatPathToUnityPath(directories[i]);

            return directories;
        }

        private static T Assign<T>(string pPath) where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath(pPath, typeof(T)) as T;
        }

        /// <summary>
        /// Example: GetObjects<AudioClip>(@"Assets\Game\Sounds\Musics", "t:AudioClip")
        /// </summary>
        /// <returns></returns>
        public static List<T> GetObjects<T>(string pPath, string filter, bool getChild = true) where T : UnityEngine.Object
        {
            var directories = EditorHelper.GetDirectories(pPath);

            List<T> list = new List<T>();

            var resources = AssetDatabase.FindAssets(filter, directories);

            for (int i = 0; i < resources.Length; i++)
            {
                if (getChild)
                {
                    var childAssets = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GUIDToAssetPath(resources[i]));
                    for (int j = 0; j < childAssets.Length; j++)
                    {
                        if (childAssets[j] is T)
                        {
                            list.Add(childAssets[j] as T);
                        }
                    }
                }
                else
                {
                    list.Add(Assign<T>(AssetDatabase.GUIDToAssetPath(resources[i])));
                }
            }

            return list;
        }

        #endregion

        //===============

        #region Build

        public static void RemoveDirective(string pSymbol)
        {
            var taget = EditorUserBuildSettings.selectedBuildTargetGroup;
            string directives = PlayerSettings.GetScriptingDefineSymbolsForGroup(taget);
            directives = directives.Replace(pSymbol, "");
            if (directives[directives.Length - 1] == ';')
                directives = directives.Remove(directives.Length - 1, 1);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(taget, directives);
        }

        public static void AddDirective(string pSymbol)
        {
            var taget = EditorUserBuildSettings.selectedBuildTargetGroup;
            string directives = PlayerSettings.GetScriptingDefineSymbolsForGroup(taget);
            if (string.IsNullOrEmpty(directives))
                directives += pSymbol;
            else
                directives += ";" + pSymbol;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(taget, directives);
        }

        public static string[] GetCurrentDirectives()
        {
            string defineStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            string[] currentDefines = defineStr.Split(';');
            for (int i = 0; i < currentDefines.Length; i++)
                currentDefines[i] = currentDefines[i].Trim();
            return currentDefines;
        }

        public static bool ContainDirective(string pSymbol)
        {
            var directives = GetCurrentDirectives();
            foreach (var d in directives)
                if (d == pSymbol)
                    return true;
            return false;
        }

        #endregion

        //===============
    }
#endif
}