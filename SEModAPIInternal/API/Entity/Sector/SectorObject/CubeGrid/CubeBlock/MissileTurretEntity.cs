﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Sandbox.Common.ObjectBuilders;

using SEModAPIInternal.API.Common;
using SEModAPIInternal.Support;

namespace SEModAPIInternal.API.Entity.Sector.SectorObject.CubeGrid.CubeBlock
{
	public class MissileTurretEntity : FunctionalBlockEntity
	{
		#region "Attributes"

		public static string MissileTurretNamespace = "";
		public static string MissileTurretClass = "";

		#endregion

		#region "Constructors and Intializers"

		public MissileTurretEntity(CubeGridEntity parent, MyObjectBuilder_LargeMissileTurret definition)
			: base(parent, definition)
		{
		}

		public MissileTurretEntity(CubeGridEntity parent, MyObjectBuilder_LargeMissileTurret definition, Object backingObject)
			: base(parent, definition, backingObject)
		{
		}

		#endregion

		#region "Properties"

		#endregion

		#region "Methods"

		/// <summary>
		/// Method to get the casted instance from parent signature
		/// </summary>
		/// <returns>The casted instance into the class type</returns>
		new internal MyObjectBuilder_LargeMissileTurret GetSubTypeEntity()
		{
			return (MyObjectBuilder_LargeMissileTurret)ObjectBuilder;
		}

		#endregion
	}
}
