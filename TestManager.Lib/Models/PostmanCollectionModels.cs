using System;
using System.Collections.Generic;

using System.Globalization;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace TestManager.Lib.ViewModels
{
    #region Enums

    public enum ListenType { Prerequest, Test };

    public enum Mode { Raw };

    public enum Method { Get, Post };

    #endregion

    /// <summary>
    /// A Postman collection object
    /// <remarks>done</remarks>
    /// </summary>
    public partial class PostmanCollection: ViewModelBase
    {
        #region Fields

        private CollectionInfo _info;
        private List<PostmanCollectionItem> _items;
        private List<Event> _events;

        #endregion

        [JsonProperty("info")]
        public CollectionInfo Info
        {
            get { return _info; }
            set
            {
                if (value == _info)
                {
                    return;
                }
                _info = value;
                RaisePropertyChanged("Info");
            }
        }

        [JsonProperty("item")]
        public List<PostmanCollectionItem> Items
        {
            get { return _items; }
            set
            {
                if (value == _items)
                {
                    return;
                }
                _items = value;
                RaisePropertyChanged("Items");
            }
        }

        [JsonProperty("event")]
        public List<Event> Events
        {
            get { return _events; }
            set
            {
                if (value == _events)
                {
                    return;
                }
                _events = value;
                RaisePropertyChanged("Events");
            }
        }
    }

    /// <summary>
    /// A Postman event
    /// <remarks>done</remarks>
    /// </summary>
    public partial class Event: ViewModelBase
    {
        #region Fields

        private ListenType _listenType;
        private Script _script;

        #endregion

        [JsonProperty("listen")]
        public ListenType ListenType
        {
            get { return _listenType; }
            set
            {
                if (value == _listenType)
                {
                    return;
                }
                _listenType = value;
                RaisePropertyChanged("ListenType");
            }
        }

        [JsonProperty("script")]
        public Script Script
        {
            get { return _script; }
            set
            {
                if (value == _script)
                {
                    return;
                }
                _script = value;
                RaisePropertyChanged("Script");
            }
        }
    }

    /// <summary>
    /// A Postman script
    /// <remarks>done</remarks>
    /// </summary>
    public partial class Script : ViewModelBase
    {
        private const string TYPE = "text/javascript";

        #region Fields

        private List<string> _execLines;

        #endregion

        [JsonProperty("exec")]
        public List<string> ExecLines
        {
            get { return _execLines; }
            set
            {
                if (value == _execLines)
                {
                    return;
                }
                _execLines = value;
                RaisePropertyChanged("ExecLines");
            }
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public Script()
        {
            this.Type = TYPE;
        }
    }

    /// <summary>
    /// A Postman collection descriptor
    /// <remarks>done</remarks>
    /// </summary>
    public partial class CollectionInfo : ViewModelBase
    {
        private const string SCHEMA = "https://schema.getpostman.com/json/collection/v2.1.0/collection.json";

        #region Fields

        private string _name;
        private string _description;

        #endregion

        [JsonProperty("name")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name)
                {
                    return;
                }
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        [JsonProperty("description")]
        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description)
                {
                    return;
                }
                _description = value;
                RaisePropertyChanged("Description");
            }
        }

        [JsonProperty("_postman_id")]
        public string PostmanId { get; set; }

        [JsonProperty("schema")]
        public string Schema { get; set; }

        public CollectionInfo()
        {
            this.Schema = SCHEMA;
        }
    }

    /// <summary>
    /// A Postman collection item
    /// <remarks>done</remarks>
    /// </summary>
    public partial class PostmanCollectionItem : ViewModelBase
    {
        #region Fields

        private string _name;
        private string _description;
        private List<Event> _events;

        #endregion

        [JsonProperty("name")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name)
                {
                    return;
                }
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        [JsonProperty("description")]
        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description)
                {
                    return;
                }
                _description = value;
                RaisePropertyChanged("Description");
            }
        }

        [JsonProperty("event")]
        public List<Event> Events
        {
            get { return _events; }
            set
            {
                if (value == _events)
                {
                    return;
                }
                _events = value;
                RaisePropertyChanged("Events");
            }
        }

        internal virtual void RefreshInformation()
        {

        }
    }

    /// <summary>
    /// A Postman folder item
    /// <remarks>done</remarks>
    /// </summary>
    public partial class FolderItem: PostmanCollectionItem
    {
        #region Fields

        private List<PostmanCollectionItem> _items;
        private bool? _isSubFolder;

        #endregion

        [JsonProperty("item")]
        public List<PostmanCollectionItem> Items
        {
            get { return _items; }
            set
            {
                if (value == _items)
                {
                    return;
                }
                _items = value;
                RaisePropertyChanged("Items");
            }
        }

        [JsonProperty("_postman_isSubFolder")]
        public bool? IsSubFolder
        {
            get { return _isSubFolder; }
            set
            {
                if (value == _isSubFolder)
                {
                    return;
                }
                _isSubFolder = value;
                RaisePropertyChanged("IsSubFolder");
            }
        }
    }

    /// <summary>
    /// A Postman request item
    /// <remarks>done</remarks>
    /// </summary>
    public partial class RequestItem : PostmanCollectionItem
    {
        #region Fields

        private Request _request;

        #endregion

        [JsonProperty("request")]
        public Request Request
        {
            get { return _request; }
            set
            {
                if (value == _request)
                {
                    return;
                }
                _request = value;
                RaisePropertyChanged("Request");
            }
        }

        [JsonProperty("response")]
        public List<object> Responses { get; set; }
    }

    /// <summary>
    /// A Postman request descriptpr
    /// <remarks>done</remarks>
    /// </summary>
    public partial class Request : ViewModelBase
    {
        #region Fields

        private Method _method;
        private List<Header> _headers;
        private RequestBody _body;
        private PostmanUrl _url;
        private string _description;

        #endregion

        [JsonProperty("method")]
        public Method Method
        {
            get { return _method; }
            set
            {
                if (value == _method)
                {
                    return;
                }
                _method = value;
                RaisePropertyChanged("Method");
            }
        }

        [JsonProperty("header")]
        public List<Header> Headers
        {
            get { return _headers; }
            set
            {
                if (value == _headers)
                {
                    return;
                }
                _headers = value;
                RaisePropertyChanged("Headers");
            }
        }

        [JsonProperty("body")]
        public RequestBody Body
        {
            get { return _body; }
            set
            {
                if (value == _body)
                {
                    return;
                }
                _body = value;
                RaisePropertyChanged("Body");
            }
        }

        [JsonProperty("url")]
        public PostmanUrl Url
        {
            get { return _url; }
            set
            {
                if (value == _url)
                {
                    return;
                }
                _url = value;
                RaisePropertyChanged("Url");
            }
        }

        [JsonProperty("description")]
        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description)
                {
                    return;
                }
                _description = value;
                RaisePropertyChanged("Description");
            }
        }
    }

    /// <summary>
    /// A Postman request body descriptpr
    /// <remarks>done</remarks>
    /// </summary>
    public partial class RequestBody : ViewModelBase
    {
        #region Fields

        private Mode _mode;
        private string _raw;

        #endregion

        [JsonProperty("mode")]
        public Mode Mode
        {
            get { return _mode; }
            set
            {
                if (value == _mode)
                {
                    return;
                }
                _mode = value;
                RaisePropertyChanged("Mode");
            }
        }

        [JsonProperty("raw")]
        public string Raw
        {
            get { return _raw; }
            set
            {
                if (value == _raw)
                {
                    return;
                }
                _raw = value;
                RaisePropertyChanged("Raw");
            }
        }
    }

    /// <summary>
    /// A Postman header descriptpr
    /// <remarks>done</remarks>
    /// </summary>
    public partial class Header : ViewModelBase
    {
        #region Fields

        private string _key;
        private string _value;

        #endregion

        [JsonProperty("key")]
        public string Key
        {
            get { return _key; }
            set
            {
                if (value == _key)
                {
                    return;
                }
                _key = value;
                RaisePropertyChanged("Key");
            }
        }

        [JsonProperty("value")]
        public string Value
        {
            get { return _value; }
            set
            {
                if (value == _value)
                {
                    return;
                }
                _value = value;
                RaisePropertyChanged("Value");
            }
        }
    }

    public partial class PostmanUrl : ViewModelBase
    {
        #region Fields

        private string _fullUrl;
        private List<string> _hosts;
        private List<string> _pathElements;
        private List<QueryParam> _queryParameters;

        #endregion

        [JsonProperty("raw")]
        public string FullUrl
        {
            get { return _fullUrl; }
            set
            {
                if (value == _fullUrl)
                {
                    return;
                }
                _fullUrl = value;
                RaisePropertyChanged("FullUrl");
            }
        }

        [JsonProperty("host")]
        public List<string> Hosts
        {
            get { return _hosts; }
            set
            {
                if (value == _hosts)
                {
                    return;
                }
                _hosts = value;
                RaisePropertyChanged("Hosts");
            }
        }

        [JsonProperty("path")]
        public List<string> PathElements
        {
            get { return _pathElements; }
            set
            {
                if (value == _pathElements)
                {
                    return;
                }
                _pathElements = value;
                RaisePropertyChanged("PathElements");
            }
        }

        [JsonProperty("query")]
        public List<QueryParam> QueryParameters
        {
            get { return _queryParameters; }
            set
            {
                if (value == _queryParameters)
                {
                    return;
                }
                _queryParameters = value;
                RaisePropertyChanged("QueryParameters");
            }
        }
    }

    public partial class QueryParam : ViewModelBase
    {
        #region Fields

        private string _key;
        private string _value;
        private bool? _disabled;

        #endregion

        [JsonProperty("key")]
        public string Key
        {
            get { return _key; }
            set
            {
                if (value == _key)
                {
                    return;
                }
                _key = value;
                RaisePropertyChanged("Key");
            }
        }

        [JsonProperty("value")]
        public string Value
        {
            get { return _value; }
            set
            {
                if (value == _value)
                {
                    return;
                }
                _value = value;
                RaisePropertyChanged("Value");
            }
        }

        [JsonProperty("equals")]
        public bool QueryEquals { get; set; }

        [JsonProperty("disabled")]
        public bool? Disabled
        {
            get { return _disabled; }
            set
            {
                if (value == _disabled)
                {
                    return;
                }
                _disabled = value;
                RaisePropertyChanged("Disabled");
            }
        }

        public QueryParam()
        {
            this.QueryEquals = true;
        }
    }

    public partial class PostmanCollection
    {
        public static PostmanCollection FromJson(string json) => JsonConvert.DeserializeObject<PostmanCollection>(json, Converter.Settings);
    }

    #region Serialze extensions

    static class ListenTypeExtensions
    {
        public static ListenType? ValueForString(string str)
        {
            switch (str)
            {
                case "prerequest": return ListenType.Prerequest;
                case "test": return ListenType.Test;
                default: return null;
            }
        }

        public static ListenType ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            var str = serializer.Deserialize<string>(reader);
            var maybeValue = ValueForString(str);
            if (maybeValue.HasValue) return maybeValue.Value;
            throw new Exception("Unknown enum case " + str);
        }

        public static void WriteJson(this ListenType value, JsonWriter writer, JsonSerializer serializer)
        {
            switch (value)
            {
                case ListenType.Prerequest: serializer.Serialize(writer, "prerequest"); break;
                case ListenType.Test: serializer.Serialize(writer, "test"); break;
            }
        }
    }

    static class ModeExtensions
    {
        public static Mode? ValueForString(string str)
        {
            switch (str)
            {
                case "raw": return Mode.Raw;
                default: return null;
            }
        }

        public static Mode ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            var str = serializer.Deserialize<string>(reader);
            var maybeValue = ValueForString(str);
            if (maybeValue.HasValue) return maybeValue.Value;
            throw new Exception("Unknown enum case " + str);
        }

        public static void WriteJson(this Mode value, JsonWriter writer, JsonSerializer serializer)
        {
            switch (value)
            {
                case Mode.Raw: serializer.Serialize(writer, "raw"); break;
            }
        }
    }

    static class MethodExtensions
    {
        public static Method? ValueForString(string str)
        {
            switch (str)
            {
                case "GET": return Method.Get;
                case "POST": return Method.Post;
                default: return null;
            }
        }

        public static Method ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            var str = serializer.Deserialize<string>(reader);
            var maybeValue = ValueForString(str);
            if (maybeValue.HasValue) return maybeValue.Value;
            throw new Exception("Unknown enum case " + str);
        }

        public static void WriteJson(this Method value, JsonWriter writer, JsonSerializer serializer)
        {
            switch (value)
            {
                case Method.Get: serializer.Serialize(writer, "GET"); break;
                case Method.Post: serializer.Serialize(writer, "POST"); break;
            }
        }
    }

    static class PostmanCollectionItemExtensions
    {
        public static PostmanCollectionItem ReadJson(JsonReader reader, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);

            PostmanCollectionItem target = null;

            JToken requestElement = jObject.SelectToken("request");
            JToken itemElement = jObject.SelectToken("item");
            if(requestElement != null)
            {
                target = new RequestItem();
            }
            else if (itemElement != null)
            {
                target = new FolderItem();
            }
            else
            {
                throw new Exception("Unknown Postman item type: " + jObject);
            }

            serializer.Populate(jObject.CreateReader(), target);
            target.RefreshInformation();
            return target;
        }

        public static void WriteJson(this PostmanCollectionItem value, JsonWriter writer, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    public static class Serialize
    {
        public static string ToJson(this PostmanCollection self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal class Converter : JsonConverter
    {
        private static List<Type> ALLOWED_TYPES = new List<Type> { typeof(ListenType), typeof(Mode), typeof(Method), typeof(PostmanCollectionItem) };

        public override bool CanConvert(Type t) => ALLOWED_TYPES.Contains(t);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (t == typeof(ListenType))
                return ListenTypeExtensions.ReadJson(reader, serializer);
            if (t == typeof(Mode))
                return ModeExtensions.ReadJson(reader, serializer);
            if (t == typeof(Method))
                return MethodExtensions.ReadJson(reader, serializer);
            if (t == typeof(ListenType?))
            {
                if (reader.TokenType == JsonToken.Null) return null;
                return ListenTypeExtensions.ReadJson(reader, serializer);
            }
            if (t == typeof(Mode?))
            {
                if (reader.TokenType == JsonToken.Null) return null;
                return ModeExtensions.ReadJson(reader, serializer);
            }
            if (t == typeof(Method?))
            {
                if (reader.TokenType == JsonToken.Null) return null;
                return MethodExtensions.ReadJson(reader, serializer);
            }
            if (t == typeof(PostmanCollectionItem))
            {
                return PostmanCollectionItemExtensions.ReadJson(reader, serializer);
            }
            throw new Exception("Unknown type");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var t = value.GetType();
            if (t == typeof(ListenType))
            {
                ((ListenType)value).WriteJson(writer, serializer);
                return;
            }
            if (t == typeof(Mode))
            {
                ((Mode)value).WriteJson(writer, serializer);
                return;
            }
            if (t == typeof(Method))
            {
                ((Method)value).WriteJson(writer, serializer);
                return;
            }
            throw new Exception("Unknown type");
        }

        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new Converter(),
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
            TypeNameHandling = TypeNameHandling.Auto
        };
    }
}