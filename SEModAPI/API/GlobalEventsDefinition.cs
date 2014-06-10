﻿using System;
using System.Collections.Generic;
using Sandbox.Common.ObjectBuilders.Definitions;
using SEModAPI.Support;

namespace SEModAPI.API
{
	public class GlobalEventsDefinition
	{
		#region "Attributes"

		private MyObjectBuilder_GlobalEventDefinition _definition;

		#endregion

		#region "Constructors and Initializers"

		public GlobalEventsDefinition(MyObjectBuilder_GlobalEventDefinition definition)
		{
			_definition = definition;
			Changed = false;
		}

		#endregion

		#region "Properties"

        public bool Changed { get; private set; }

        public MyObjectBuilder_GlobalEventDefinition Definition
        {
            get { return _definition; }
            set
            {
                if (_definition == value) return;
                _definition = value;
                Changed = true;
            }
        }

		public string Id
		{
			get { return _definition.Id.SubtypeId; }
		}

		public string Name
		{
			get { return _definition.DisplayName; }
            set
            {
                if (_definition.DisplayName == value) return;
                _definition.DisplayName = value;
                Changed = true;
            }
		}

		public string Description
		{
			get { return _definition.Description; }
            set
            {
                if (_definition.Description == value) return;
                _definition.Description = value;
                Changed = true;
            }
		}

		public long MinActivation
		{
			get { return _definition.MinActivationTimeMs.Value; }
            set
            {
                if (_definition.MinActivationTimeMs == value) return;
                _definition.MinActivationTimeMs = value;
                Changed = true;
            }
		}

		public long MaxActivation
		{
			get { return _definition.MaxActivationTimeMs.Value; }
            set
            {
                if (_definition.MaxActivationTimeMs == value) return;
                _definition.MaxActivationTimeMs = value;
                Changed = true;
            }
		}

		public long FirstActivation
		{
			get { return _definition.FirstActivationTimeMs.Value; }
            set
            {
                if (_definition.FirstActivationTimeMs == value) return;
                _definition.FirstActivationTimeMs = value;
                Changed = true;
            }
		}

		#endregion

	}

	//////////////////////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////////////////////
	//////////////////////////////////////////////////////////////////////////////////////////////////////

	public class GlobalEventsDefinitionsWrapper
	{
		#region "Attributes"

		private MyObjectBuilder_GlobalEventDefinition[] _definitions;
		private Dictionary<KeyValuePair<string, string>, int> _nameIndexes = new Dictionary<KeyValuePair<string, string>, int>();

		#endregion

		#region "Constructors and Initializers"

		public GlobalEventsDefinitionsWrapper(MyObjectBuilder_GlobalEventDefinition[] definitions)
		{
			_definitions = definitions;
			Changed = false;
			int index = 0;

			foreach (var definition in _definitions)
			{
				_nameIndexes.Add(new KeyValuePair<string, string>(definition.DisplayName, definition.Id.SubtypeId), index);
				++index;
			}
		}

		#endregion

        #region "Properties"

        public bool Changed { get; private set; }

        public MyObjectBuilder_GlobalEventDefinition[] Definitions
        {
            get { return _definitions; }
            set
            {
                if (_definitions == value) return;
                _definitions = value;
                Changed = true;
            }
        }

        #endregion

        #region "Getters"

        public string IdOf(int index)
		{
			return IsIndexValid(index) ? _definitions[index].Id.SubtypeId : null;
		}

		public string NameOf(int index)
		{
			return IsIndexValid(index) ? _definitions[index].DisplayName : null;
		}

		public string DescriptionOf(int index)
		{
			return IsIndexValid(index) ? _definitions[index].Description : null;
		}

		public string TypeOf(int index)
		{
			return IsIndexValid(index) ? _definitions[index].EventType.ToString() : null;
		}

		public long MinActivationOf(int index)
		{
			return IsIndexValid(index) ? _definitions[index].MinActivationTimeMs.Value : -1;
		}

		public long MaxActivationOf(int index)
		{
			return IsIndexValid(index) ? _definitions[index].MaxActivationTimeMs.Value : -1;
		}

		public long FirstActivationOf(int index)
		{
			return IsIndexValid(index) ? _definitions[index].FirstActivationTimeMs.Value : -1;
		}

		#endregion

		#region "Setters"

		public bool SetNameOf(int index, string name)
		{
			if (!IsIndexValid(index) || _definitions[index].DisplayName == name) return false;
			_definitions[index].DisplayName = name;
			Changed = true;
			return true;
		}

		public bool SetDescriptionOf(int index, string description)
		{
			if (!IsIndexValid(index) || _definitions[index].Description == description) return false;
			_definitions[index].Description = description;
			Changed = true;
			return true;
		}

		public bool SetMinActivationOf(int index, long minActivation)
		{
			if (!IsIndexValid(index) || _definitions[index].MinActivationTimeMs.Value == minActivation) return false;
			_definitions[index].MinActivationTimeMs = minActivation;
			Changed = true;
			return true;
		}

		public bool SetMaxActivationOf(int index, long maxActivation)
		{
			if (!IsIndexValid(index) || _definitions[index].MaxActivationTimeMs.Value == maxActivation) return false;
			_definitions[index].MaxActivationTimeMs = maxActivation;
			Changed = true;
			return true;
		}

		public bool SetFirstActivationOf(int index, long firstActivation)
		{
			if (!IsIndexValid(index) || _definitions[index].FirstActivationTimeMs.Value == firstActivation) return false;
			_definitions[index].FirstActivationTimeMs = firstActivation;
			Changed = true;
			return true;
		}

		#endregion

		#region "Methods"

		public GlobalEventsDefinition GetDefinitionOf(int index)
		{
			if (IsIndexValid(index))
			{
				return new GlobalEventsDefinition(_definitions[index]);
			}
			return null;
		}

		public int IndexOf(string name, string subtype)
		{
			int index = -1;
			_nameIndexes.TryGetValue(new KeyValuePair<string, string>(name, subtype), out index);
			return index;
		}

		private bool IsIndexValid(int index)
		{
			return (index < _definitions.Length && index >= 0);
		}

		#endregion
	}
}
