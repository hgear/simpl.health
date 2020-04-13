using System;
using System.Collections.Generic;

namespace simpl.health.Universes
{
    public enum ValidationType
    {
        Field,
        Timeliness,
        Effectuation
    }

    public class Validation
    {
        public ValidationType ValidationType { get; set; }

        public bool IsValid { get; set; }

        public string Message { get; set; }

        public UniverseField Field {get;set;}

        public List<UniverseField> AdditionalFields { get; set; }

        public Validation()
        {
            IsValid = false;
            AdditionalFields = new List<UniverseField>();
        }

        public Validation(bool isValid, string message) : base()
        {
            this.IsValid = isValid;
            this.Message = message;
        }

        public Validation(bool isValid, string message, UniverseField field)
        {
            this.IsValid = IsValid;
            this.Message = message;
            this.Field = field;
        }
    }
}