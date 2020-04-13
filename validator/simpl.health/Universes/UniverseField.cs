using System;
namespace simpl.health.Universes
{
    public class UniverseField
    {
        public int FieldId { get; set; }
        public string Name {get; set;}
        public string Value { get; set; }
        public bool IsRequired { get; set; }
        public int MaxFieldLength { get; set; }
        public int FieldOrder { get; set; }
        public Type FieldType { get; set; }
        public bool WasFieldCorrected { get; set; }
        public string FieldAutoCorrectedMessage { get; set; }
        public bool IgnoreCase { get; internal set; }
        public string DefaultPossibleValues { get; set; }

        public UniverseField(string fieldName, bool isRequired,
            int maxFieldLength, int fieldOrder, Type fieldType, bool ignoreCase = false, string defaultPossibleValue = "")
        {
            Name = fieldName;
            IsRequired = isRequired;
            MaxFieldLength = maxFieldLength;
            FieldType = fieldType;
            this.IgnoreCase = ignoreCase;
            this.DefaultPossibleValues = defaultPossibleValue;
            this.FieldOrder = fieldOrder;
        }

        public UniverseField(UniverseField field)
        {
            Name = field.Name;
            FieldOrder = field.FieldOrder;
            MaxFieldLength = field.MaxFieldLength;
            FieldType = field.FieldType;
            IgnoreCase = field.IgnoreCase;
            DefaultPossibleValues = field.DefaultPossibleValues;
        }
    }
}
