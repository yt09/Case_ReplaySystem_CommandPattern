

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Replay
{
    /// <summary>
    /// 序列化工具接口
    /// </summary>
    public abstract class AbsSerialize
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract bool IsTypeSupport(Type type);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract string Serialize(object value);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract object Deserialize(string value);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public abstract object Deserialize(string value, Type type);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract T Deserialize<T>(string value);
    }

    /// <summary>
    /// 
    /// </summary>
    public class RSerialize
    {
        /// <summary>
        /// 当前序列化工具
        /// </summary>
        public static AbsSerialize serializeTool;

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Serialize(object value)
        {
            
            if (!serializeTool.IsTypeSupport(value.GetType()))
            {
                Debug.LogWarning("当前序列化工具不支持此类型的序列化");
                return null;
            }
            return serializeTool.Serialize(value);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object Deserialize(string value)
        {
            if (!serializeTool.IsTypeSupport(value.GetType()))
            {
                Debug.LogWarning("当前序列化工具不支持此类型的序列化");
                return null;
            }
            return serializeTool.Deserialize(value);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string value)
        {
            if (!serializeTool.IsTypeSupport(value.GetType()))
            {
                Debug.LogWarning("当前序列化工具不支持此类型的序列化");
                return default(T);
            }
            Type t = typeof(T);
            return serializeTool.Deserialize<T>(value);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Deserialize(string value, Type type)
        {
            if (!serializeTool.IsTypeSupport(value.GetType()))
            {
                Debug.LogWarning("当前序列化工具不支持此类型的序列化");
                return null;
            }
            return serializeTool.Deserialize(value, type);
        }


    }

}

namespace Replay
{

    /// <summary>
    /// Unity自带序列化工具
    /// </summary>
    public class JsonUtilitySerializeTool : AbsSerialize
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name = "value" ></ param >
        /// < returns ></ returns >
        public override object Deserialize(string value)
        {
            return JsonUtility.FromJson<object>(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name = "value" ></ param >
        /// < param name="type"></param>
        /// <returns></returns>
        public override object Deserialize(string value, Type type)
        {
            return JsonUtility.FromJson(value, type);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name = "T" ></ typeparam >
        /// < param name="value"></param>
        /// <returns></returns>
        public override T Deserialize<T>(string value)
        {
            return JsonUtility.FromJson<T>(value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name = "type" ></ param >
        /// < returns ></ returns >
        public override bool IsTypeSupport(Type type)
        {
            return type.IsSerializable;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name = "value" ></ param >
        /// < returns ></ returns >
        public override string Serialize(object value)
        {
            return JsonUtility.ToJson(value);
        }
    }

    /// <summary>
    /// JsonDotNet插件序列化工具
    /// </summary>
    public class NewtonsoftSerializeTool : AbsSerialize
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object Deserialize(string value)
        {
            return JsonConvert.DeserializeObject(value, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override object Deserialize(string value, Type type)
        {
            return JsonConvert.DeserializeObject(value, type, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public override T Deserialize<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public override bool IsTypeSupport(Type type)
        {
            return type.IsSerializable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override string Serialize(object value)
        {
            return JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
        }
    }

    /// <summary>
    /// 通用序列化工具
    /// </summary>
    public class CommonSerializeTool : AbsSerialize
    {
        public List<Component> components = new List<Component>();
        public List<GameObject> gos = new List<GameObject>();
        NewtonsoftSerializeTool jsonTool = new NewtonsoftSerializeTool();

        public CommonSerializeTool()
        {
            Init();
        }

        public void Init()
        {
            RefreshCom();
            RefreshGo();
        }

        private void RefreshGo()
        {
            gos.Clear();
            gos.AddRange(Array.ConvertAll(UnityEngine.Object.FindObjectsOfType<Transform>(), p => p.gameObject));
        }

        private void RefreshCom()
        {
            components.Clear();
            components.AddRange(UnityEngine.Object.FindObjectsOfType<Component>());
            components.Sort((p, q) => q.GetInstanceID().CompareTo(p.GetInstanceID()));
        }

        public override object Deserialize(string value)
        {
            object r = jsonTool.Deserialize(value);

            //if (r is ComponentID)
            //{
            //    r = components[(ComponentID)r];
            //    Debug.Log(r);
            //}
            //if (r is GameObjectID)
            //{
            //    r = gos[(GameObjectID)r];
            //}
            return r;
        }

        public override object Deserialize(string value, Type type)
        {
            Type realType = type;
            if (type == typeof(Component) || type == typeof(GameObject))
            {
                realType = typeof(int);
            }

            object result = jsonTool.Deserialize(value, realType);
            if (type == typeof(Component))
            {
                result = components[(int)result];
            }
            else if (type == typeof(GameObject))
            {
                result = gos[(int)result];
            }
            return result;
        }

        public override T Deserialize<T>(string value)
        {
            Type realType = typeof(T);
            if (realType == typeof(Component))
            {
                return (T)(object)components[jsonTool.Deserialize<int>(value)];
            }
            else if (realType == typeof(GameObject))
            {
                return (T)(object)gos[jsonTool.Deserialize<int>(value)];
            }

            return jsonTool.Deserialize<T>(value);
        }

        public override bool IsTypeSupport(Type type)
        {
            return true;
        }

        public override string Serialize(object value)
        {
            object realValue = value;

            if (value.GetType().IsSubclassOf(typeof(BaseAopMonoBehaviour)))
            {
                realValue = new AopMonoBehaviourID(((BaseAopMonoBehaviour)value).UUID, components.FindIndex(p => p == value as Component).ToString(), value.GetType().FullName);
            }
            else if (value.GetType().IsSubclassOf(typeof(BaseAopClass)))
            {
                realValue = new AopClassID(((BaseAopClass)value).UUID, value.GetType().FullName);
            }

            //if (value is Component)
            //{
            //    //RefreshCom();
            //    realValue = (ComponentID)components.FindIndex(p => p == value as Component);
            //}
            //else if (value is GameObject)
            //{
            //    //RefreshGo();
            //    realValue = (GameObjectID)gos.FindIndex(p => p == value as GameObject);
            //}
            return jsonTool.Serialize(realValue);
        }

        struct AopMonoBehaviourID
        {
            public string id;
            public string monoId;
            public string typeName;

            public AopMonoBehaviourID(string id, string monoId, string typeName)
            {
                this.id = id;
                this.monoId = monoId;
                this.typeName = typeName;
            }
        }

        struct AopClassID
        {
            public string id;
            public string typeName;

            public AopClassID(string id, string typeName)
            {
                this.id = id;
                this.typeName = typeName;
            }
        }

        struct ComponentID
        {
            public int id;
            public static implicit operator int(ComponentID id)
            {
                return id.id;
            }
            public static implicit operator ComponentID(int i)
            {
                return new ComponentID() { id = i };
            }
        }

        struct GameObjectID
        {
            public int id;
            public static implicit operator int(GameObjectID id)
            {
                return id.id;
            }
            public static implicit operator GameObjectID(int i)
            {
                return new GameObjectID() { id = i };
            }
        }
    }

}