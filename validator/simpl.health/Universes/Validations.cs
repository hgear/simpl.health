using System;
using System.Collections.Generic;

namespace simpl.health.Universes
{
    public class Validations : List<Validation>
    {
        public bool IsValid { get; set; }

        public Validations()
        {

        }

        public Validation Find (string FieldName)
        {
            return this.Find(f => f.Field.Name == FieldName);
        }

        public UniverseField FindField (string FieldName)
        {
            return this.Find(f => f.Field.Name == FieldName).Field;
        }
    }
}
