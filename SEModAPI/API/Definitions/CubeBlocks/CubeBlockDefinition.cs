using System;
using System.Xml;
using Microsoft.Xml.Serialization.GeneratedAssembly;
using Sandbox.Common.ObjectBuilders.Definitions;
using System.IO;

namespace SEModAPI.API.Definitions.CubeBlocks
{
	public class CubeBlockDefinition : BlockDefinition
    {
		#region "Constructors and Initializers"

		public CubeBlockDefinition(MyObjectBuilder_CubeBlockDefinition definition): base(definition)
		{}

		#endregion

        #region "Methods"

		/// <summary>
		/// Cast the base definition into the sub-type type representing a CubeBlock
		/// </summary>
		/// <returns>The base definition casted into the sub-type type representing a CubeBlock</returns>
		protected override MyObjectBuilder_CubeBlockDefinition GetSubTypeDefinition()
		{
			return (MyObjectBuilder_CubeBlockDefinition)m_baseDefinition;
		}

		/// <summary>
		/// Serialize a CubeBlock in the xml recieved as parameter
		/// </summary>
		/// <param name="xmlWriter"></param>
		protected override void SerializeSubType(XmlTextWriter xmlWriter)
		{
			var serializer = (MyObjectBuilder_CubeBlockDefinitionSerializer)Activator.CreateInstance(typeof(MyObjectBuilder_CubeBlockDefinitionSerializer));
			serializer.Serialize(xmlWriter, GetSubTypeDefinition());
		}

        #endregion	
    }
}