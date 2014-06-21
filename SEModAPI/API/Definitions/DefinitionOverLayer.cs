using Microsoft.Xml.Serialization.GeneratedAssembly;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml;
using System.Reflection;
using ProtoBuf;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;

using SEModAPI.Support;

namespace SEModAPI.API.Definitions
{
	/// <summary>
	/// This class is only intended for easy data access and management. It is a wrapper around all MyObjectBuilder_DefinitionsBase children sub type
	/// </summary>
	public abstract class DefinitionOverLayer
	{
		#region "Attributes"

		protected MyObjectBuilder_DefinitionBase m_baseDefinition;

		#endregion

		#region "Constructors and Initializers"

		protected DefinitionOverLayer(MyObjectBuilder_DefinitionBase baseDefinition)
		{
			m_baseDefinition = baseDefinition;
		}

		#endregion

		#region "Properties"

		/// <summary>
		/// Gets the changed status of the object
		/// </summary>
		[Browsable(true)]
		[Description("Represent the state of changes in the object")]
		public virtual bool Changed { get; protected set; }

		/// <summary>
		/// Obtain the API formated name of the object
		/// </summary>
		[Browsable(false)]
		[ReadOnly(true)]
		[Description("The formatted name of the object")]
		public abstract string Name { get; }

		/// <summary>
		/// Gets the Internal data of the object
		/// </summary>
		[Browsable(false)]
		[ReadOnly(true)]
		[Description("Internal data of the object")]
		protected MyObjectBuilder_DefinitionBase BaseDefinition
		{
			get { return m_baseDefinition; }
		}

		/// <summary>
		/// Gets the internal data of the object
		/// </summary>
		[Browsable(true)]
		[ReadOnly(true)]
		[Description("The value ID representing the type of the object")]
		public MyObjectBuilderTypeEnum TypeId
		{
			get { return m_baseDefinition.TypeId; }
		}

		/// <summary>
		/// Gets the internal Id of the instance
		/// </summary>
		[Browsable(true)]
		[Description("The Id as SerializableDefinitionId that represent the Object representation")]
		public SerializableDefinitionId Id
		{
			get { return m_baseDefinition.Id; }
			set
			{
				if (m_baseDefinition.Id.ToString() == value.ToString()) return;
				m_baseDefinition.Id = value;
				Changed = true;
			}
		}

		/// <summary>
		/// Get the description of the object
		/// </summary>
		[Browsable(true)]
		[Description("Represents the description of the object")]
		public string Description
		{
			get { return m_baseDefinition.Description; }
			set
			{
				if (m_baseDefinition.Description == value) return;
				m_baseDefinition.Description = value;
				Changed = true;
			}
		}

		#endregion

		#region "Methods"



		/// <summary>
		/// Serialization of the current instance
		/// </summary>
		/// <param name="xmlWriter">An initialized XmlWriter instance where to append the current object</param>
		public void Serialize(XmlTextWriter xmlWriter)
		{
			xmlWriter.Formatting = Formatting.Indented;
			xmlWriter.Indentation = 2;
			xmlWriter.IndentChar = ' ';
			SerializeSubType(xmlWriter);
		}

		/// <summary>
		/// This metod serialize a sub-type into the given XMLTextWriter.
		/// </summary>
		/// <returns></returns>
		protected abstract void SerializeSubType(XmlTextWriter xmlWriter);

		/// <summary>
		/// Serialization without exception.
		/// </summary>
		/// <param name="writer">An initialized XmlWriter instance where to append the current object</param>
		/// <returns></returns>
		public virtual bool TrySerialize(XmlTextWriter writer)
		{
			bool serialized = false;
			try
			{
				Serialize(writer);
				serialized = true;
			}
			catch (Exception)
			{}
			return serialized;
		}

		#endregion
	}


////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	public class DefinitionsOverLayerManager
	{
		#region "Attributes"

		private bool m_isMutable = true;
		private bool m_changed = false;

		//Use Long (key) as Id and OverLayerDefinition sub type (value) as Name
		//For entity objects (saved game data) we use EntityId as the long key
		private Dictionary<long, DefinitionOverLayer> m_definitions = new Dictionary<long, DefinitionOverLayer>();

		#endregion

		#region "Constructors and Initializers"

		protected DefinitionsOverLayerManager()
		{
			m_changed = false;
			m_isMutable = true;
		}

		public DefinitionsOverLayerManager(MyObjectBuilder_DefinitionBase[] baseDefinitions)
		{
			m_changed = false;
			m_isMutable = true;

			foreach (var definition in baseDefinitions)
			{
				AddDefinition(definition);
			}
		}

		#endregion

		#region "Properties"

		/// <summary>
		/// Define if the object can be changed
		/// </summary>
		[Browsable(true)]
		[Description("Define if the object can be changed")]
		public bool IsMutable
		{
			get { return m_isMutable; }
			set { m_isMutable = value; }
		}

		/// <summary>
		/// Defines if the object has changed
		/// </summary>
		[Browsable(true)]
		[Description("Defines if the object has changed")]
		protected bool Changed
		{
			get
			{
				if (m_changed) return true;
				foreach (var def in Definitions)
				{
					if (def.Changed)
						return true;
				}
				return false;
			}
		}

		public DefinitionOverLayer[] Definitions
		{
			get
			{
				return GetInternalData().Values.ToArray();
			}
		}

		#endregion

		#region "Methods"

		protected virtual Dictionary<long, DefinitionOverLayer> GetInternalData()
		{
			return m_definitions;
		}

		/// <summary>
		/// This method is used to extract all instances of MyObjectBuilder_Definitions sub type encapsulated in the manager
		/// This method is slow and should only be used to extract the underlying type
		/// </summary>
		/// <returns>All instances of the MyObjectBuilder_Definitions sub type sub type in the manager</returns>
		public List<T> ExtractBaseDefinitions()
		{
			List<T> list = new List<T>();
			foreach (var def in GetInternalData().Values)
			{
				list.Add(GetBaseTypeOf(def));
			}
			return list;
		}

		private bool IsIdValid(long id)
		{
			return GetInternalData().ContainsKey(id);
		}

		private bool IsIndexValid(int index)
		{
			return (index < GetInternalData().Keys.Count && index >= 0);
		}

		public U DefinitionOf(long id)
		{
			U result = default(U);
			if (IsIdValid(id))
				GetInternalData().TryGetValue(id, out result);

			return result;
		}

		public U DefinitionOf(int index)
		{
			return IsIndexValid(index) ? GetInternalData().Values.ToArray()[index] : default(U);
		}

		#region "Abstract Methods"

		/// <summary>
		/// Template method that create the instance of the children of OverLayerDefinition sub type
		/// </summary>
		/// <param name="definition">MyObjectBuilder_Definitions object</param>
		/// <returns>An instance representing OverLayerDefinition sub type</returns>
		protected abstract U CreateOverLayerSubTypeInstance(T definition);

		/// <summary>
		/// This template method is intended to extact the BaseObject inside the overlayer
		/// </summary>
		/// <param name="overLayer">the over layer from which to extract the base object</param>
		/// <returns>MyObjectBuilder_Definitions Sub Type</returns>
		protected abstract T GetBaseTypeOf(U overLayer);

		/// <summary>
		/// This template method is intended to know if the state of the object insate the overlayer has changed
		/// </summary>
		/// <param name="overLayer">the overlayer from which to know if the base type has changed</param>
		/// <returns>if the underlying object has changed</returns>
		protected abstract bool GetChangedState(U overLayer);

		#endregion

		public bool AddDefinition()
		{
			if (!IsMutable) return default(U);

			var newEntry = CreateOverLayerSubTypeInstance(source);
			GetInternalData().Add(m_definitions.Count, newEntry);
			m_changed = true;

			return newEntry;
		}

		public U NewEntry(long id)
		{
			if (!IsMutable) return default(U);

			var newEntry = CreateOverLayerSubTypeInstance((T)Activator.CreateInstance(typeof(T), new object[] { }));
			GetInternalData().Add(id, newEntry);
			m_changed = true;

			return newEntry;
		}

		public U NewEntry(T source)
		{
			if (!IsMutable) return default(U);

			var newEntry = CreateOverLayerSubTypeInstance(source);
			GetInternalData().Add(m_definitions.Count, newEntry);
			m_changed = true;

			return newEntry;
		}

		public U NewEntry(U source)
		{
			if (!IsMutable) return default(U);

			//Create the new object
			Type entryType = typeof(T);
			var newEntry = (T)Activator.CreateInstance(entryType, new object[] { });

			//Copy the field data
			//TODO - Find a way to fully copy complex data structures in fields instead of just copying reference
			foreach (FieldInfo field in entryType.GetFields())
			{
				field.SetValue(newEntry, field.GetValue(source.BaseDefinition));
			}

			//Add the new object to the manager as a new entry
			return NewEntry(newEntry);
		}

		public bool DeleteEntry(long id)
		{
			if (!IsMutable) return false;

			if (GetInternalData().ContainsKey(id))
			{
				GetInternalData().Remove(id);
				m_changed = true;
				return true;
			}

			return false;
		}

		public bool DeleteEntry(T entry)
		{
			if (!IsMutable) return false;

			foreach (var def in m_definitions)
			{
				if (def.Value.BaseDefinition.Equals(entry))
				{
					GetInternalData().Remove(def.Key);
					m_changed = true;
					return true;
				}
			}

			return false;
		}

		public bool DeleteEntry(U entry)
		{
			if (!IsMutable) return false;

			foreach (var def in m_definitions)
			{
				if (def.Value.Equals(entry))
				{
					GetInternalData().Remove(def.Key);
					m_changed = true;
					return true;
				}
			}

			return false;
		}

		#endregion
	}

	public class SerializableDefinitionsManager<T, U> : OverLayerDefinitionsManager<T, U> where U : OverLayerDefinition<T>
	{
		#region "Attributes"

		private FileInfo m_fileInfo;
		private readonly GameInstallationInfo m_gameInstallation;
		private readonly FieldInfo m_definitionsContainerField;

		#endregion

		#region "Constructors and Initializers"

		protected SerializableDefinitionsManager()
		{
			m_fileInfo = null;
			m_gameInstallation = new GameInstallationInfo();

			m_definitionsContainerField = GetMatchingDefinitionsContainerField();
		}

		#endregion

		#region "Properties"

		public FileInfo FileInfo
		{
			get { return m_fileInfo; }
			set { m_fileInfo = value; }
		}

		#endregion

		#region "Methods"

		#region "Static"

		public static FileInfo GetContentDataFile(string configName)
		{
			string filePath = Path.Combine(Path.Combine(GameInstallationInfo.GetGameRegistryPath(), @"Content\Data"), configName);
			FileInfo saveFileInfo = new FileInfo(filePath);

			return saveFileInfo;
		}

		#endregion

		#region "Serializers"

		protected T LoadContentFile<T, TS>(FileInfo fileInfo) where TS : XmlSerializer1
		{
			object fileContent = null;

			string filePath = fileInfo.FullName;

			if (!File.Exists(filePath))
			{
				throw new AutoException(new GameInstallationInfoException(GameInstallationInfoExceptionState.ConfigFileMissing), filePath);
			}

			try
			{
				fileContent = ReadSpaceEngineersFile<T, TS>(filePath);
			}
			catch
			{
				throw new AutoException(new GameInstallationInfoException(GameInstallationInfoExceptionState.ConfigFileCorrupted), filePath);
			}

			if (fileContent == null)
			{
				throw new AutoException(new GameInstallationInfoException(GameInstallationInfoExceptionState.ConfigFileEmpty), filePath);
			}

			// TODO: set a file watch to reload the files, incase modding is occuring at the same time this is open.
			//     Lock the load during this time, in case it happens multiple times.
			// Report a friendly error if this load fails.

			return (T)fileContent;
		}

		protected void SaveContentFile<T, TS>(T fileContent, FileInfo fileInfo) where TS : XmlSerializer1
		{

			string filePath = fileInfo.FullName;

			if (!File.Exists(filePath))
			{
				throw new AutoException(new GameInstallationInfoException(GameInstallationInfoExceptionState.ConfigFileMissing), filePath);
			}

			try
			{
				WriteSpaceEngineersFile<T, TS>(fileContent, filePath);
			}
			catch
			{
				throw new AutoException(new GameInstallationInfoException(GameInstallationInfoExceptionState.ConfigFileCorrupted), filePath);
			}

			if (fileContent == null)
			{
				throw new AutoException(new GameInstallationInfoException(GameInstallationInfoExceptionState.ConfigFileEmpty), filePath);
			}

			// TODO: set a file watch to reload the files, incase modding is occuring at the same time this is open.
			//     Lock the load during this time, in case it happens multiple times.
			// Report a friendly error if this load fails.
		}

		protected T ReadSpaceEngineersFile<T, TS>(Stream stream)
			where TS : XmlSerializer1
		{
			var settings = new XmlReaderSettings
			{
				IgnoreComments = true,
				IgnoreWhitespace = true,
			};

			object obj;

			using (var xmlReader = XmlReader.Create(stream, settings))
			{

				var serializer = (TS)Activator.CreateInstance(typeof(TS));
				//serializer.UnknownAttribute += serializer_UnknownAttribute;
				//serializer.UnknownElement += serializer_UnknownElement;
				//serializer.UnknownNode += serializer_UnknownNode;
				obj = serializer.Deserialize(xmlReader);
			}

			return (T)obj;
		}

		protected bool TryReadSpaceEngineersFile<T, TS>(string filename, out T entity)
			 where TS : XmlSerializer1
		{
			try
			{
				entity = ReadSpaceEngineersFile<T, TS>(filename);
				return true;
			}
			catch
			{
				entity = default(T);
				return false;
			}
		}

		protected T ReadSpaceEngineersFile<T, TS>(string filename)
			where TS : XmlSerializer1
		{
			var settings = new XmlReaderSettings
			{
				IgnoreComments = true,
				IgnoreWhitespace = true,
			};

			object obj = null;

			if (File.Exists(filename))
			{
				using (var xmlReader = XmlReader.Create(filename, settings))
				{
					var serializer = (TS)Activator.CreateInstance(typeof(TS));
					obj = serializer.Deserialize(xmlReader);
				}
			}

			return (T)obj;
		}

		protected T Deserialize<T>(string xml)
		{
			using (var textReader = new StringReader(xml))
			{
				return (T)(new XmlSerializerContract().GetSerializer(typeof(T)).Deserialize(textReader));
			}
		}

		protected string Serialize<T>(object item)
		{
			using (var textWriter = new StringWriter())
			{
				new XmlSerializerContract().GetSerializer(typeof(T)).Serialize(textWriter, item);
				return textWriter.ToString();
			}
		}

		protected bool WriteSpaceEngineersFile<T, TS>(T sector, string filename)
			where TS : XmlSerializer1
		{
			// How they appear to be writing the files currently.
			try
			{
				using (var xmlTextWriter = new XmlTextWriter(filename, null))
				{
					xmlTextWriter.Formatting = Formatting.Indented;
					xmlTextWriter.Indentation = 2;
					xmlTextWriter.IndentChar = ' ';
					TS serializer = (TS)Activator.CreateInstance(typeof(TS));
					serializer.Serialize(xmlTextWriter, sector);
				}
			}
			catch
			{
				return false;
			}

			//// How they should be doing it to support Unicode.
			//var settingsDestination = new XmlWriterSettings()
			//{
			//    Indent = true, // Set indent to false to compress.
			//    Encoding = new UTF8Encoding(false)   // codepage 65001 without signature. Removes the Byte Order Mark from the start of the file.
			//};

			//try
			//{
			//    using (var xmlWriter = XmlWriter.Create(filename, settingsDestination))
			//    {
			//        S serializer = (S)Activator.CreateInstance(typeof(S));
			//        serializer.Serialize(xmlWriter, sector);
			//    }
			//}
			//catch (Exception ex)
			//{
			//    return false;
			//}

			return true;
		}

		#endregion

		protected override U CreateOverLayerSubTypeInstance(T definition)
		{
			return (U)Activator.CreateInstance(typeof(U), new object[] { definition });
		}

		protected override T GetBaseTypeOf(U overLayer)
		{
			return overLayer.BaseDefinition;
		}

		protected override bool GetChangedState(U overLayer)
		{
			return overLayer.Changed;
		}

		private FieldInfo GetMatchingDefinitionsContainerField()
		{
			//Find the the matching field in the container
			Type thisType = typeof(T[]);
			Type defType = typeof(MyObjectBuilder_Definitions);
			FieldInfo matchingField = null;
			foreach (FieldInfo field in defType.GetFields())
			{
				Type fieldType = field.FieldType;
				if (thisType.FullName == fieldType.FullName)
				{
					matchingField = field;
					break;
				}
			}

			return matchingField;
		}

		public void Load(FileInfo sourceFile)
		{
			m_fileInfo = sourceFile;

			//Get the definitions content from the file
			MyObjectBuilder_Definitions definitionsContainer = LoadContentFile<MyObjectBuilder_Definitions, MyObjectBuilder_DefinitionsSerializer>(m_fileInfo);

			if (m_definitionsContainerField == null)
				throw new AutoException(new GameInstallationInfoException(GameInstallationInfoExceptionState.Invalid), "Failed to find matching definitions field");

			//Get the data from the definitions container
			T[] baseDefinitions = (T[])m_definitionsContainerField.GetValue(definitionsContainer);

			//Copy the data into the manager
			GetInternalData().Clear();
			foreach (var definition in baseDefinitions)
			{
				NewEntry(definition);
			}
		}

		public void Load(T[] source)
		{
			//Copy the data into the manager
			GetInternalData().Clear();
			foreach (var definition in source)
			{
				NewEntry(definition);
			}
		}

		public void Load(U[] source)
		{
			//Copy the data into the manager
			GetInternalData().Clear();
			foreach (var definition in source)
			{
				NewEntry(definition.BaseDefinition);
			}
		}

		public bool Save()
		{
			if (!this.Changed) return false;
			if (!this.IsMutable) return false;
			if (this.FileInfo == null) return false;

			MyObjectBuilder_Definitions definitionsContainer = new MyObjectBuilder_Definitions();

			if (m_definitionsContainerField == null)
				throw new AutoException(new GameInstallationInfoException(GameInstallationInfoExceptionState.Invalid), "Failed to find matching definitions field");

			//Save the source data into the definitions container
			m_definitionsContainerField.SetValue(definitionsContainer, ExtractBaseDefinitions().ToArray());

			//Save the definitions container out to the file
			SaveContentFile<MyObjectBuilder_Definitions, MyObjectBuilder_DefinitionsSerializer>(definitionsContainer, m_fileInfo);

			return true;
		}

		#endregion
	}
}
