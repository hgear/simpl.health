using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace simpl.health.Universes
{
    public class UniverseFieldNames
    {
        public static readonly string FirstName = "Beneficiary First Name";
        public static readonly string LastName = "Beneficiary Last Name";
        public static readonly string CardholderId = "Cardholder ID";
        public static readonly string ContractId = "Contract ID";
        public static readonly string PlanId = "Plan ID";
        public static readonly string AuthOrClaimNumber = "Authorization or Claim Number";
        public static readonly string ProviderType = "Provider Type";
        public static readonly string PersonMakingRequest = "Person who made the request?";
        public static readonly string RequestReceivedDate = "Date the request was received";
        public static readonly string Diagnosis = "Diagnosis";
        public static readonly string TypeOfService = "Issue description and type of service";
        public static readonly string LevelOfService = "Level of service";
        public static readonly string TimeframeExtensionTaken = "Was a timeframe extension taken?";
        public static readonly string SponsorNotificationDelay = "If an extension was taken, did the sponsor " +
                "notify the member of the reason(s) for the delay and of their" +
                " right to file an expedited grievance?";
        public static readonly string RequestDisposition = "Request Disposition";
        public static readonly string SponsorDecisionDate = "Date of sponsor decision";
        public static readonly string RequestDeniedNecessity = "Was the request denied for lack of medical necessity?";
        public static readonly string RequestDeniedReviewd = "If denied for lack of medical necessity, was " +
                "the review completed by a physician or other appropriate" +
                " health care professional?";
        public static readonly string OralNotificationDate = "Date oral notification provided to enrollee";
        public static readonly string EnrolleeWrittenNotificationDate = "Date written notification provided to enrollee";
        public static readonly string AuthorizationEnteredDate = "Date service authorization entered/effectuated" +
                " in the sponsor's system";
        public static readonly string AORReceiptDate = "AOR Receipt date";
        public static readonly string FirstTierRelatedEntity = "First Tier, Downstream, and Related Entity";
        public static readonly string RequestReceivedTime = "Time the request as received";
        public static readonly string SubsequentRequest = "Subsequent expedited request";
        public static readonly string SponsorDecisionTime = "Time of sponsor decision";
        public static readonly string OralNotificationTime = "Time oral notification provided to enrollee";
        public static readonly string WrittenNotificationTime = "Time written notification provided to enrollee";
        public static readonly string SponsorEffectuatedTime = "Time effectuated" +
            " in the sponsor's system";
        public static readonly string ExpeditedOrStandardTimeFrame = "Was request made under the " +
        "expedited timeframe but processed by the plan under " +
        "the standard timeframe?";
        public static readonly string RequestExpedited = "Request for expedited timeframe";
        public static readonly string CleanClaim = "Is this a clean claim?";
        public static readonly string ClaimPaidDate = "Date the Claim was paid";
        public static readonly string ClaimInterestPaid = "Was interest paid on the claim?";
        public static readonly string ProviderWrittenNotifiationDate =
            "Date written notification provided to provider";
        public static readonly string ReimbursementPaidDate = "Date reimbursement paid";
        public static readonly string ReimbursementInterestPaid = "Was interest paid on the reimbursement request?";
        public static readonly string IREForwardedDate = "Date forwarded to IRE";
        public static readonly string EnrolleeRequestForwardedIREDate = "If request denied or untimely, date enrollee" +
            " notified request has been forwarded to IRE";

    }



    public abstract class Universe
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        protected readonly List<String> notApplicable =
            new List<string>() { "NA" };


        protected readonly List<String> personWhoMadeRequestPossibleValues =
            new List<string>() { "B", "BR", "CP", "NCP" };

        private readonly List<String> providerTypePossibleValues =
            new List<string>() { "CP", "NCP" };


        protected readonly List<String> yesNo =
            new List<string>() { "Y", "N" };

        protected readonly List<String> yesNoNotApplicable =
          new List<string>() { "Y", "N", "NA" };

        private readonly List<string> approvedDenied =
    new List<string>() { "APPROVED", "DENIED" };

        public UniverseFields Fields { get; protected set; }
        public string Name { get; protected set; }
       
        public Universe()
        {
            this.Fields = new UniverseFields();
            AddCommonFields();
        }

        private void AddCommonFields()
        {
            Fields.Add(UniverseFieldNames.FirstName, true, 50, 1, typeof(int));
            Fields.Add(UniverseFieldNames.LastName, true, 50, 2, typeof(int));
            Fields.Add(UniverseFieldNames.CardholderId, true, 20, 3, typeof(int));
            Fields.Add(UniverseFieldNames.ContractId, true, 5, 4, typeof(String));
            Fields.Add(UniverseFieldNames.PlanId, true, 3, 5, typeof(String));
            Fields.Add(UniverseFieldNames.AuthOrClaimNumber, true, 40, 6, typeof(String));
        }

        protected void ValidateField (string fieldName, List<string> values, Validations validations, Func<UniverseField, Validation> func)
        {
            var field = Fields[fieldName];
            if (field != null)
            {
                var newField = new UniverseField(field);
                newField.Value = values[field.FieldOrder - 1];
                validations.Add(func(newField));
            }
        }

        protected void ValidateField(string fieldName, List<string> values, Validations validations,
            Func<UniverseField, List<String>, Validation> func, List<string> possibleValues)
        {
            var field = Fields[fieldName];
            if (field != null)
            {
                var newField = new UniverseField(field);
                newField.Value = values[field.FieldOrder - 1];
                validations.Add(func(newField, possibleValues));
            }
        }


        public void ValidateOrgDeterminationFields (Validations v, List<string> values)
        {

            ValidateField(UniverseFieldNames.FirstName,values, v, ValidateFieldLength);
            ValidateField(UniverseFieldNames.LastName, values, v, ValidateFieldLength);
            ValidateField(UniverseFieldNames.CardholderId, values, v, ValidateFieldLength);
            ValidateField(UniverseFieldNames.ContractId, values, v, ValidateFieldLength);
            ValidateField(UniverseFieldNames.PlanId, values, v, ValidateFieldLength);
            ValidateField(UniverseFieldNames.AuthOrClaimNumber, values, v, ValidateFieldLength);
            ValidateField(UniverseFieldNames.ProviderType, values, v,
                ValidateFieldWithPossibleValues, providerTypePossibleValues);
            ValidateField(UniverseFieldNames.RequestReceivedDate, values, v, ValidateDate);
            ValidateField(UniverseFieldNames.Diagnosis, values, v, ValidateFieldLength);
            ValidateField(UniverseFieldNames.TypeOfService, values, v, ValidateFieldLength);
            ValidateField(UniverseFieldNames.LevelOfService, values, v, ValidateFieldLength);
            ValidateField(UniverseFieldNames.TimeframeExtensionTaken, values, v,
                ValidateFieldWithPossibleValues, yesNo);
            ValidateField(UniverseFieldNames.RequestDisposition, values, v,
                ValidateFieldWithPossibleValues, approvedDenied);
            ValidateField(UniverseFieldNames.SponsorNotificationDelay, values, v,
                ValidateFieldWithPossibleValues, yesNoNotApplicable);
            ValidateField(UniverseFieldNames.SponsorDecisionDate, values, v, ValidateDate);
            ValidateField(UniverseFieldNames.RequestDeniedNecessity, values, v,
                ValidateFieldWithPossibleValues, yesNoNotApplicable);
            ValidateField(UniverseFieldNames.RequestDeniedReviewd, values, v,
                ValidateFieldWithPossibleValues, yesNoNotApplicable);
            ValidateField(UniverseFieldNames.OralNotificationDate, values, v, ValidateDate);
            ValidateField(UniverseFieldNames.EnrolleeWrittenNotificationDate, values, v, ValidateFieldLength);
            ValidateField(UniverseFieldNames.AuthorizationEnteredDate, values, v, ValidateDate);
            ValidateField(UniverseFieldNames.AORReceiptDate, values, v, ValidateDate);
            ValidateField(UniverseFieldNames.FirstTierRelatedEntity, values, v, ValidateFieldLength);
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


        protected Validation ValidateTime (UniverseField field)
        {
            var validation = new Validation();
            validation.Field = field;
            validation.IsValid = true;
            CultureInfo enUS = new CultureInfo("en-US");

            DateTime date;
            if (string.IsNullOrWhiteSpace(field.Value))
            {
                validation.IsValid = false;
                validation.Message = "This field is required";
            }
            else if (DateTime.TryParseExact(field.Value.Trim(), "HH:mm:ss", enUS,
                DateTimeStyles.None, out date))
            {
                validation.IsValid = true;
                validation.Field.DateValue = date;
            }
            else if (DateTime.TryParse(field.Value.Trim(), out date))
            {
                validation.Field.OriginalValue = field.Value;
                validation.Field.Value = date.ToString("HH:mm:ss");
                validation.Field.WasFieldCorrected = true;
                validation.Field.DateValue = date;
            }
            else
            {
                validation.IsValid = false;
                validation.Message = "Field is not a valid time field (HH:mm:ss)";
            }

            

            return validation;
        }

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
            else if (field.DefaultPossibleValues != null
                && field.DefaultPossibleValues.Count() > 0
                && field.DefaultPossibleValues.Contains(field.Value.Trim().ToUpper())) { 

                if (!field.DefaultPossibleValues.Contains(field.Value)) {
                    field.OriginalValue = field.Value;
                    field.Value = field.Value.Trim().ToUpper();
                    field.WasFieldCorrected = true;   
                }
                
            }
            else if (Double.TryParse(field.Value, out d))
            {
                date = DateTime.FromOADate(d);
                validation.Field.Value = date.ToString("yyyy/MM/dd");
                validation.Field.DateValue = date;

            }
            else if (DateTime.TryParse(field.Value, out date))
            {
                validation.Field.Value = date.ToString("yyyy/MM/dd");
                validation.Field.DateValue = date;
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
