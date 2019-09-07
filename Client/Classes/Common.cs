﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BigQ.Client.Classes
{
    /// <summary>
    /// A series of helpful methods for BigQ.
    /// </summary>
    public static class Common
    {  
        public static T DeserializeJson<T>(string json)
        {
            // Newtonsoft
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            return (T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, settings);

            // System.Web.Script.Serialization
            // JavaScriptSerializer ser = new JavaScriptSerializer();
            // ser.MaxJsonLength = Int32.MaxValue;
            // return (T)ser.Deserialize<T>(json);
        }

        public static T DeserializeJson<T>(byte[] bytes)
        {
            // Newtonsoft
            JsonSerializerSettings settings = new JsonSerializerSettings();
            string json = Encoding.UTF8.GetString(bytes);
            return (T)Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, settings);

            // System.Web.Script.Serialization
            // JavaScriptSerializer ser = new JavaScriptSerializer();
            // ser.MaxJsonLength = Int32.MaxValue;
            // return (T)ser.Deserialize<T>(Encoding.UTF8.GetString(bytes));
        }

        public static string SerializeJson(object obj)
        {
            // Newtonsoft
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            string json = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented, settings);
            return json;

            // System.Web.Script.Serialization
            // JavaScriptSerializer ser = new JavaScriptSerializer();
            // ser.MaxJsonLength = Int32.MaxValue;
            // string json = ser.Serialize(obj);
            // return json;
        } 
    }
}