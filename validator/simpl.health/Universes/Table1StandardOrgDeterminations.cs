using System;
using System.Collections.Generic;

namespace simpl.health.Universes
{
    public static class Table1FieldNames {

        public static readonly string FirstName = "Beneficiary First Name";
        public static readonly string LastName = "Beneficiary Last Name";
        public static readonly string CardholderId = "Cardholder ID";
        public static readonly string ContractId = "Contract ID";
        public static readonly string PlanId = "Plan ID";
        public static readonly string AuthOrClaimNumber = "Authorization or Claim Number";
        public static readonly string PersonMakingRequest = "Person who made the request?";
        public static readonly string ProviderType = "Provider Type";
        public static readonly string DateRequestReceived = "Date the request was received";
        public static readonly string Diagnosis = "Diagnosis";
        public static readonly string TypeOfService = "Issue description and type of service";
        public static readonly string LevelOfService = "Level of service";
        public static readonly string ExpeditedOrStandardTimeFrame = "Was request made under the " +
                "expedited timeframe but processed by the plan under " +
                "the standard timeframe?";
        public static readonly string RequestExpedited = "Request for expedited timeframe";
        public static readonly string TimeframeExtensionTaken = "Was a timeframe extension taken?";
        public static readonly string SponsorNotificationDelay = "If an extension was taken, did the sponsor " +
                "notify the member of the reason(s) for the delay and of their" +
                " right to file an expedited grievance?";
        public static readonly string RequestDisposition = "Request Disposition";
        public static readonly string SponsorDecisionDate = "Date of sponsor decision";
        public static readonly string RequestDeniedNecessity = "Was the request denied for lack of medical necessity?";
        public static readonly string RequestReviewedNecessity = "If denied for lack of medical necessity, was " +
                "the review completed by a physician or other appropriate" +
                " health care professional?";
        public static readonly string OralNotificationDate = "Date oral notification provided to enrollee";
        public static readonly string WrittenNotificationDate = "Date written notification provided to enrollee";
        public static readonly string AuthorizationEnteredDate = "Date service authorization entered/effectuated" +
                " in the sponsor's system";
        public static readonly string AORReceiptDate = "AOR Receipt date";
        public static readonly string FirstTierRelatedEntity = "First Tier, Downstream, and Related Entity";



    }

    public class Table1StandardOrgDeterminations : Universe
    {
        private readonly List<String> personWhoMadeRequestPossibleValues =
            new List<string>() { "B", "BR", "CP", "NCP" };

        private readonly List<String> providerTypePossibleValues =
            new List<string>() { "CP", "NCP" };

        private readonly List<String> yesNo =
            new List<string>() { "Y", "N" };

        private readonly List<String> yesNoNotApplicable =
          new List<string>() { "Y", "N", "NA" };


        private readonly List<String> requestExpeditedTimeFrame =
            new List<string>() {"NCP", "B", "BR", "NA"};

        private readonly List<string> approvedDenied =
            new List<string>() { "APPROVED", "DENIED" };



        public Table1StandardOrgDeterminations() :base()
        {
            Fields.Add(Table1FieldNames.FirstName, new UniverseField(Table1FieldNames.FirstName, true, 50, 1, typeof(int)));
            Fields.Add(Table1FieldNames.LastName, new UniverseField(Table1FieldNames.LastName, true, 50, 2, typeof(int)));
            Fields.Add(Table1FieldNames.CardholderId, new UniverseField(Table1FieldNames.CardholderId, true, 20, 3, typeof(int)));
            Fields.Add(Table1FieldNames.ContractId, new UniverseField(Table1FieldNames.ContractId, true, 5, 4, typeof(String)));
            Fields.Add(Table1FieldNames.PlanId, new UniverseField(Table1FieldNames.PlanId, true, 3, 5, typeof(String)));
            Fields.Add(Table1FieldNames.AuthOrClaimNumber, new UniverseField(Table1FieldNames.AuthOrClaimNumber, true, 40, 6, typeof(String)));
            Fields.Add(Table1FieldNames.PersonMakingRequest, new UniverseField(Table1FieldNames.PersonMakingRequest, true, 3, 7, typeof(String)));
            Fields.Add(Table1FieldNames.ProviderType, new UniverseField(Table1FieldNames.ProviderType, true, 3, 8, typeof(String)));
            Fields.Add(Table1FieldNames.DateRequestReceived, new UniverseField(Table1FieldNames.DateRequestReceived, true, 10, 9, typeof(DateTime)));
            Fields.Add(Table1FieldNames.Diagnosis, new UniverseField(Table1FieldNames.Diagnosis, true, 100, 10, typeof(String)));
            Fields.Add(Table1FieldNames.TypeOfService, new UniverseField(Table1FieldNames.TypeOfService, true, 2000, 11, typeof(string)));
            Fields.Add(Table1FieldNames.LevelOfService, new UniverseField(Table1FieldNames.LevelOfService, true, 50, 12, typeof(String)));
            Fields.Add(Table1FieldNames.ExpeditedOrStandardTimeFrame, new UniverseField(Table1FieldNames.ExpeditedOrStandardTimeFrame, true, 1, 13, typeof(String)));
            Fields.Add(Table1FieldNames.RequestExpedited, new UniverseField(Table1FieldNames.RequestExpedited, true, 3, 14, typeof(String)));
            Fields.Add(Table1FieldNames.TimeframeExtensionTaken, new UniverseField(Table1FieldNames.TimeframeExtensionTaken, true, 1, 15, typeof(String)));
            Fields.Add(Table1FieldNames.SponsorNotificationDelay, new UniverseField(Table1FieldNames.SponsorNotificationDelay, true, 2, 16, typeof(String)));
            Fields.Add(Table1FieldNames.RequestDisposition, new UniverseField(Table1FieldNames.RequestDisposition, true, 8, 17, typeof(String), true));
            Fields.Add(Table1FieldNames.SponsorDecisionDate, new UniverseField(Table1FieldNames.SponsorDecisionDate, true, 10, 18, typeof(DateTime), ignoreCase: false, "NA"));
            Fields.Add(Table1FieldNames.RequestDeniedNecessity, new UniverseField(Table1FieldNames.RequestDeniedNecessity, true, 2, 19, typeof(String), false, "NA"));
            Fields.Add(Table1FieldNames.RequestReviewedNecessity, new UniverseField(Table1FieldNames.RequestReviewedNecessity, true, 2, 20, typeof(String)));
            Fields.Add(Table1FieldNames.OralNotificationDate, new UniverseField(Table1FieldNames.OralNotificationDate, true, 10, 21, typeof(DateTime), false, "NA"));
            Fields.Add(Table1FieldNames.WrittenNotificationDate, new UniverseField(Table1FieldNames.WrittenNotificationDate, true, 10, 22, typeof(DateTime), false, "NA"));
            Fields.Add(Table1FieldNames.AuthorizationEnteredDate, new UniverseField(Table1FieldNames.AuthorizationEnteredDate, true, 10, 23, typeof(DateTime), false, "NA"));
            Fields.Add(Table1FieldNames.ProviderType, new UniverseField(Table1FieldNames.AORReceiptDate, true, 10, 24, typeof(DateTime), false, "NA"));
            Fields.Add(Table1FieldNames.FirstTierRelatedEntity, new UniverseField(Table1FieldNames.FirstTierRelatedEntity, true, 70, 25, typeof(String)));
        }

        /// <summary>
        // 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public override Validations ValidateFields(List<string> values)
        {
            var validations = new Validations();
 
            //first name
            var field = new UniverseField(Fields[Table1FieldNames.FirstName]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldLength(field));

            //last name
            field = new UniverseField(Fields[Table1FieldNames.LastName]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldLength(field));

            //cardholder id
            field = new UniverseField(Fields[Table1FieldNames.CardholderId]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldLength(field));

            //contract id
            field = new UniverseField(Fields[Table1FieldNames.ContractId]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldLength(field));

            //plan id
            field = new UniverseField(Fields[Table1FieldNames.PlanId]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldLength(field));

            //authorization claim number
            field = new UniverseField(Fields[Table1FieldNames.AuthOrClaimNumber]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldLength(field));

            //person who made the request
            field = new UniverseField(Fields[Table1FieldNames.PersonMakingRequest]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldWithPossibleValues(field, this.personWhoMadeRequestPossibleValues));

            //provider type
            field = new UniverseField(Fields[Table1FieldNames.ProviderType]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldWithPossibleValues(field, this.providerTypePossibleValues));

            //Date the request was received
            field = new UniverseField(Fields[Table1FieldNames.DateRequestReceived]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateDate(field));

            //Diagnosis
            field = new UniverseField(Fields[Table1FieldNames.Diagnosis]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldLength(field));

            //issue description and type of service
            field = new UniverseField(Fields[Table1FieldNames.TypeOfService]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldLength(field));

            //level of service
            field = new UniverseField(Fields[Table1FieldNames.LevelOfService]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldLength(field));

            //Expedited timeframe ?
            field = new UniverseField(Fields[Table1FieldNames.ExpeditedOrStandardTimeFrame]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldWithPossibleValues(field, this.yesNo));

            //Request for expedited timeframe
            field = new UniverseField(Fields[Table1FieldNames.RequestExpedited]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldWithPossibleValues(field, this.requestExpeditedTimeFrame));

            //Was time frame extension taken
            field = new UniverseField(Fields[Table1FieldNames.TimeframeExtensionTaken]) ;
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldWithPossibleValues(field, this.yesNo));

            //Extension taken did sponsor notify the member
            field = new UniverseField(Fields[Table1FieldNames.SponsorNotificationDelay]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldWithPossibleValues(field, this.yesNoNotApplicable));

            //Request Disposition
            field = new UniverseField(Fields[Table1FieldNames.RequestDisposition]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldWithPossibleValues(field, this.approvedDenied));

            //Date of sponsor decision
            field = new UniverseField(Fields[Table1FieldNames.SponsorDecisionDate]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateDate(field));

            //Was request denied
            field = new UniverseField(Fields[Table1FieldNames.RequestDeniedNecessity]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldWithPossibleValues(field, this.yesNoNotApplicable));

            //if denied for lack of medical necessity was review completed by physician
            field = new UniverseField(Fields[Table1FieldNames.RequestReviewedNecessity]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldWithPossibleValues(field, this.yesNoNotApplicable));

            //Date of oral notification to enrollee
            field = new UniverseField(Fields[Table1FieldNames.OralNotificationDate]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateDate(field));

            //Date written notification provided
            field = new UniverseField(Fields[Table1FieldNames.WrittenNotificationDate]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateDate(field));

            //Date Service authorization entered
            field = new UniverseField(Fields[Table1FieldNames.AuthorizationEnteredDate]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateDate(field));

            //AOR Receipt Date
            field = new UniverseField(Fields[Table1FieldNames.AORReceiptDate]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateDate(field));

            //First Tier, Downstream, and Realted Entity
            field = new UniverseField(Fields[Table1FieldNames.FirstTierRelatedEntity]);
            field.Value = values[field.FieldOrder - 1];
            validations.Add(ValidateFieldLength(field));

            var errors = validations.FindAll(v => !v.IsValid);

            //Logical Errors
            //If this is BR then column X should have a date that is not more than a year from column I
            var tempValidation =
                validations.Find(f => f.Field.Name == Table1FieldNames.PersonMakingRequest);
            if (tempValidation.IsValid
                && field.Value.Equals("BR"))
            {
                var temprequestReceivedValidation =
                    validations.Find(f => f.Field.Name == Table1FieldNames.DateRequestReceived);
                var temprequestAORValidation =
                    validations.Find(f => f.Field.Name == Table1FieldNames.AORReceiptDate);

                if (temprequestReceivedValidation.IsValid &&
                    temprequestAORValidation.IsValid)
                {
                    DateTime requesteReceived =
                    Convert.ToDateTime(temprequestReceivedValidation.Field.Value);
                    DateTime AORRequest =
                        Convert.ToDateTime(temprequestAORValidation.Field.Value);

                    if (AORRequest.Subtract(requesteReceived).TotalDays > 365)
                    {
                        var v = new Validation();
                        v.Field = tempValidation.Field;
                        v.IsValid = false;
                        v.AdditionalFields.Add(temprequestReceivedValidation.Field);
                        v.AdditionalFields.Add(temprequestAORValidation.Field);
                        v.Message = String.Format("Fields {0} and {1} have to be 1 year apart",
                            temprequestAORValidation.Field.Name,
                            temprequestReceivedValidation.Field.Name);
                        v.ValidationType = ValidationType.Field;
                        validations.Add(v);
                    }
                }
            }

            DateTime tempDate;
            //Request for Expedited time frame
            //If value is BR then AORDate should be a date.
            tempValidation = validations.Find(f => f.Field.Name == Table1FieldNames.RequestExpedited);
            if (tempValidation.IsValid && tempValidation.Field.Value.Equals("BR"))
            {
                var temprequestAORValidation =
                    validations.Find(f => f.Field.Name == Table1FieldNames.AORReceiptDate);
                
                if (!DateTime.TryParse(temprequestAORValidation.Field.Value,
                    out tempDate))
                {

                    var v = new Validation();
                    v.Field = tempValidation.Field;
                    v.IsValid = false;
                    v.AdditionalFields.Add(temprequestAORValidation.Field);
                    v.Message = string.Format("{0} needs to be a valid date", temprequestAORValidation.Field.Name);
                    validations.Add(v);
                }
            }

            //If column O is Y then this cannot be NA.
            tempValidation = validations.Find(f => f.Field.Name == Table1FieldNames.TimeframeExtensionTaken);
            if (tempValidation.IsValid && tempValidation.Field.Value.Equals("Y"))
            {
                var temprequestSponsorNotificationDelay = validations.Find(f => f.Field.Name == Table1FieldNames.SponsorNotificationDelay);
                
                if (!DateTime.TryParse(temprequestSponsorNotificationDelay.Field.Value, out tempDate))
                {

                    var v = new Validation();
                    v.Field = tempValidation.Field;
                    v.IsValid = false;
                    v.AdditionalFields.Add(temprequestSponsorNotificationDelay.Field);
                    v.Message = string.Format("{0} needs to be a valid date", temprequestSponsorNotificationDelay.Field.Name);
                    validations.Add(v);
                }
            }

            //if request disposition is approved
            //request denied for lack of necessity should be NA
            //review completed for denial should be NA
            tempValidation = validations.Find(f => f.Field.Name == Table1FieldNames.RequestDisposition);
            var tempRequestDenied = validations.Find(f => f.Field.Name == Table1FieldNames.RequestDeniedNecessity);
            var tempRequestReviewed = validations.Find(f => f.Field.Name == Table1FieldNames.RequestReviewedNecessity);

            if (tempValidation.IsValid && tempValidation.Field.Value.ToLower().Equals("approved"))
            {
                if (!tempRequestDenied.Field.Value.ToUpper().Equals("NA") ||
                    !tempRequestReviewed.Field.Value.ToUpper().Equals("NA"))
                {
                    var v = new Validation();
                    v.Field = tempValidation.Field;
                    v.IsValid = false;
                    v.AdditionalFields.Add(tempRequestDenied.Field);
                    v.AdditionalFields.Add(tempRequestReviewed.Field);
                    v.Message = string.Format("{0} and {1} fields both need to be NA",
                        tempRequestDenied.Field.Name, tempRequestReviewed.Field.Name);
                    validations.Add(v);
                }

            }

            if (tempRequestReviewed.IsValid &&
                (tempRequestReviewed.Field.Value.Equals("Y") ||
                tempRequestReviewed.Field.Value.Equals("N")) &&
                tempRequestDenied.IsValid &&
                tempRequestDenied.Field.Value.Equals("NA"))
            {
                var v = new Validation();
                v.Field = tempRequestDenied.Field;
                v.IsValid = false;
                v.AdditionalFields.Add(tempRequestReviewed.Field);
                v.Message = string.Format("{0} cannot be NA", tempRequestDenied.Field.Name);
                validations.Add(v);
                
            }

            if (tempRequestReviewed.IsValid &&
                tempRequestReviewed.Field.Value.Equals("NA") &&
                tempRequestDenied.IsValid &&
                tempRequestDenied.Field.Value.Equals("Y"))
            {
                var v = new Validation();
                v.Field = tempRequestReviewed.Field;
                v.IsValid = false;
                v.AdditionalFields.Add(tempRequestReviewed.Field);
                v.Message = string.Format("{0} cannot be NA", tempRequestReviewed.Field.Name);
                validations.Add(v);
            }

            var tempOralNotification = validations.Find(f => f.Field.Name == Table1FieldNames.OralNotificationDate);
            var tempWrittenNotfication = validations.Find(f => f.Field.Name == Table1FieldNames.WrittenNotificationDate);
            if (tempOralNotification.IsValid && tempWrittenNotfication.IsValid)
            {
                DateTime dateOralNotification;

                DateTime dateWrittenNotification;

                if (DateTime.TryParse(tempOralNotification.Field.Value, out dateOralNotification))
                {
                    if (DateTime.TryParse(tempWrittenNotfication.Field.Value, out dateWrittenNotification))
                    {
                        var timeSpan = dateWrittenNotification.Subtract(dateOralNotification);
                        if (timeSpan.TotalHours > 72)
                        {
                            var v = new Validation();
                            v.Field = tempOralNotification.Field;
                            String.Format("{0} field must be within 72 hours of {1}",
                                tempOralNotification.Field.Name, tempWrittenNotfication.Field.Name);
                            v.IsValid = false;
                            validations.Add(v);
                        }
                       
                    }
                    else
                    {
                        var v = new Validation();
                        v.Field = tempOralNotification.Field;
                        v.AdditionalFields.Add(tempWrittenNotfication.Field);
                        v.IsValid = false;
                        v.Message = string.Format("{0} field must be a valid date",
                            tempOralNotification.Field.Value);
                        validations.Add(v);
                    }
                }
            }


            var tempAORReceiptDate = validations.Find(f => f.Field.Name == Table1FieldNames.AORReceiptDate);
            var tempPersonMadeRequest = validations.Find(f => f.Field.Name == Table1FieldNames.PersonMakingRequest);
            var tempRequestExpedited = validations.Find(f => f.Field.Name == Table1FieldNames.RequestExpedited);
            if (tempAORReceiptDate.IsValid && tempPersonMadeRequest.IsValid && tempRequestExpedited.IsValid)
            {
                if (tempAORReceiptDate.Field.Value.Equals("NA") &&
                    (tempPersonMadeRequest.Field.Value.Equals("BR") ||
                     tempRequestExpedited.Field.Value.Equals("BR")
                    ))
                {
                    var v = new Validation();
                    v.Field = tempAORReceiptDate.Field;
                    v.AdditionalFields.Add(tempPersonMadeRequest.Field);
                    v.AdditionalFields.Add(tempRequestExpedited.Field);
                    v.IsValid = false;
                    v.Message = string.Format("{0} cannot be NA and needs to be a valid date",
                        tempAORReceiptDate.Field.Name);
                    validations.Add(v);
                }
            }


            return validations;
        }

       
    }
}
