using System;
using System.Collections.Generic;

namespace simpl.health.Universes
{
    public class UniverseFields : Dictionary<string, UniverseField>
    {
        public UniverseFields()
        {
        }

        public void Add (string fieldName, bool isRequired,
            int maxFieldLength, int fieldOrder, Type fieldType, bool ignoreCase = false, List<string> defaultPossibleValue = null)
        {
            this.Add(fieldName,
                new UniverseField(fieldName, isRequired, maxFieldLength,
                fieldOrder, fieldType, ignoreCase, defaultPossibleValue));
        }
    }
}
