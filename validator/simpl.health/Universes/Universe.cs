using System;
using System.Collections.Generic;
using System.Linq;

namespace simpl.health.Universes
{
    public abstract class Universe
    {
        public Dictionary<string,UniverseField> Fields { get; protected set; }

       
        public Universe()
        {
            this.Fields = new Dictionary<string, UniverseField>();
        }

        public Validation ValidateFieldNames(List<string> columns)
        {
            if (columns != null)
            {
                int i = 1;
                columns.ForEach(c =>
                {
                var field = Fields.Where(t => t.Key.ToLower() == c.Trim().ToLower()).First().Value;
                    if (field != null)
                    {
                        field.FieldOrder = i;
                        i++;
                    }
                });
            }

            var requiredFields = this.Fields.Where(f => f.Value.IsRequired && f.Value.FieldOrder == 0);

            if (requiredFields != null && requiredFields.Count() > 0)
                return new Validation(false, "Missing fields");

            return new Validation(true, string.Empty);
        }

        public abstract Validations ValidateFields(List<string> values);


        protected Validation ValidateDate (UniverseField field)
        {
            var validation = new Validation();
            validation.Field = field;
            validation.IsValid = true;

            double d;
            DateTime date;
            if (string.IsNullOrWhiteSpace(field.Value))
            {
                validation.IsValid = false;
                validation.Message = "This field is required";
            }
            else if (!string.IsNullOrWhiteSpace(field.DefaultPossibleValues) &&
                field.DefaultPossibleValues.ToUpper().Equals(field.Value.ToUpper())) {
                field.Value = field.Value.ToUpper();
            }
            else if (Double.TryParse(field.Value, out d))
            {
                date = DateTime.FromOADate(d);
                field.Value = date.ToString("yyyy/MM/dd");
            }
            else if (DateTime.TryParse(field.Value, out date))
            {
                field.Value = date.ToString("yyyy/MM/dd");
            }
            else
            {
                validation.Message = "This is not a valid Date";
                validation.IsValid = false;
            }
            return validation;
        }

        protected Validation ValidateFieldLength (UniverseField field)
        {
            var validation = new Validation();
            validation.Field = field;
            validation.IsValid = true;

            if (string.IsNullOrWhiteSpace(field.Value))
            {
                validation.IsValid = false;
                validation.Message = "This field is required";
            }
            else if (field.Value.Length > field.MaxFieldLength && field.FieldType != typeof(DateTime))
            {
                validation.IsValid = false;
                validation.Message = String.Format("Field value cannot be longer than {0} characters", field.MaxFieldLength);
            }

            return validation;
        }

        protected Validation ValidateFieldWithPossibleValues(UniverseField field, List<String> possibleValues)
        {
            var validation = new Validation();
            validation.Field = field;
            validation.IsValid = true;

            var v = ValidateFieldLength(field);
            if (!v.IsValid)
            {
                validation = v;
            }
            else if (!possibleValues.Exists
                (v => v.ToUpper().Equals(field.Value.ToUpper())))
            {
                validation.IsValid = false;
                validation.Message = String.Format("Invalid Value. Valid values are {0}",
                    string.Join(",", possibleValues));
            }
            else if (!field.IgnoreCase && !field.Value.ToUpper().Equals(field.Value.ToUpper()))
            {
                validation.Field.Value = field.Value.ToUpper();
                validation.Field.WasFieldCorrected = true;
                validation.Field.FieldAutoCorrectedMessage = "Value was converted to Upper Case";
            }
            return validation;
        }
    }
}
