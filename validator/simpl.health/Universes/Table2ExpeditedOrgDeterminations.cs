using System;
using System.Collections.Generic;

namespace simpl.health.Universes
{


    public class Table2ExpeditedOrgDeterminations : Universe
    {
        private readonly List<string> subsequentRequestPossibleValues =
            new List<string>() { "B", "BR", "CP", "NCP" };


        public Table2ExpeditedOrgDeterminations() : base()
        {
            this.Name = "Expedited Org Determination";

            
            Fields.Add(UniverseFieldNames.PersonMakingRequest, true, 3, 7, typeof(String));
            Fields.Add(UniverseFieldNames.ProviderType, true, 3, 8, typeof(String));
            Fields.Add(UniverseFieldNames.RequestReceivedDate, true, 10, 9, typeof(DateTime));
            Fields.Add(UniverseFieldNames.RequestReceivedTime, true, 8, 10, typeof(DateTime));
            Fields.Add(UniverseFieldNames.Diagnosis, true, 100, 11, typeof(String));
            Fields.Add(UniverseFieldNames.TypeOfService, true, 2000, 12, typeof(string));
            Fields.Add(UniverseFieldNames.LevelOfService, true, 50, 13, typeof(String));
            Fields.Add(UniverseFieldNames.SubsequentRequest, true, 3, 14, typeof(String));
            Fields.Add(UniverseFieldNames.TimeframeExtensionTaken, true, 1, 15, typeof(String));
            Fields.Add(UniverseFieldNames.SponsorNotificationDelay, true, 2, 16, typeof(String));
            Fields.Add(UniverseFieldNames.RequestDisposition, true, 8, 17, typeof(String), true);
            Fields.Add(UniverseFieldNames.SponsorDecisionDate, true, 10, 18, typeof(DateTime), ignoreCase: false, notApplicable);
            Fields.Add(UniverseFieldNames.SponsorDecisionTime, true, 8, 19, typeof(DateTime), ignoreCase: false, notApplicable);
            Fields.Add(UniverseFieldNames.RequestDeniedNecessity, true, 2, 20, typeof(String), false, notApplicable);
            Fields.Add(UniverseFieldNames.RequestDeniedReviewd, true, 2, 21, typeof(String));
            Fields.Add(UniverseFieldNames.OralNotificationDate, true, 10, 22, typeof(DateTime), false, notApplicable);
            Fields.Add(UniverseFieldNames.OralNotificationTime, true, 8, 23, typeof(DateTime), false, notApplicable);
            Fields.Add(UniverseFieldNames.EnrolleeWrittenNotificationDate, true, 10, 24, typeof(DateTime), false, notApplicable);
            Fields.Add(UniverseFieldNames.WrittenNotificationTime, true, 8, 25, typeof(DateTime), false, notApplicable);
            Fields.Add(UniverseFieldNames.AuthorizationEnteredDate, true, 10, 26, typeof(DateTime), false, notApplicable);
            Fields.Add(UniverseFieldNames.SponsorEffectuatedTime, true, 8, 27, typeof(DateTime), false, notApplicable);
            Fields.Add(UniverseFieldNames.AORReceiptDate, true, 10, 28, typeof(DateTime), false, notApplicable);
            Fields.Add(UniverseFieldNames.FirstTierRelatedEntity, true, 70, 29, typeof(String));
        }

        /// <summary>
        // 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public override Validations ValidateFields(List<string> values)
        {
            var validations = new Validations();

            ValidateOrgDeterminationFields(validations, values);
            ValidateField(UniverseFieldNames.PersonMakingRequest, values, validations,
                ValidateFieldWithPossibleValues, personWhoMadeRequestPossibleValues);
            ValidateField(UniverseFieldNames.RequestReceivedTime, values, validations, ValidateTime);
            ValidateField(UniverseFieldNames.SubsequentRequest, values, validations,
                ValidateFieldWithPossibleValues, subsequentRequestPossibleValues);
            ValidateField(UniverseFieldNames.SponsorDecisionTime, values, validations, ValidateTime);
            ValidateField(UniverseFieldNames.OralNotificationTime, values, validations, ValidateTime);
            ValidateField(UniverseFieldNames.WrittenNotificationTime, values, validations, ValidateTime);
            ValidateField(UniverseFieldNames.SponsorEffectuatedTime, values, validations, ValidateTime);

            var errors = validations.FindAll(v => !v.IsValid);

            //Logical Errors
            //If this is BR then Column AORReceiptDate should not be NA and Subsequent Request should also be BR.
            var tmpPersonRequestValidation =
                validations.Find(UniverseFieldNames.PersonMakingRequest);
            if (tmpPersonRequestValidation.IsValid
                && tmpPersonRequestValidation.Field.Value.Equals("BR"))
            {
                var tmprequestSubsequentRequest =
                    validations.Find(UniverseFieldNames.SubsequentRequest);
                var tmprequestAORValidation =
                    validations.Find(UniverseFieldNames.AORReceiptDate);

                if (tmprequestSubsequentRequest.IsValid &&
                     !tmprequestSubsequentRequest.Field.Value.Equals("BR"))
                {
                    var v = new Validation();
                    v.IsValid = false;
                    v.Field = tmprequestSubsequentRequest.Field;
                    v.AdditionalFields.Add(tmpPersonRequestValidation.Field);
                    v.Message = string.Format("{0} needs to have a value of BR", tmprequestSubsequentRequest.Field.Name);
                    validations.Add(v);
                }

                if (tmprequestAORValidation.IsValid && tmprequestAORValidation.Field.Value.Equals("NA"))
                {
                    var v = new Validation();
                    v.IsValid = false;
                    v.Field = tmprequestAORValidation.Field;
                    v.AdditionalFields.Add(tmpPersonRequestValidation.Field);
                    v.Message = string.Format("{0} cannot have a value of NA", tmprequestSubsequentRequest.Field.Name);
                    validations.Add(v);
                }
            }

            //If this is BR then Person Making request and AOR Receipt date cannot be NA
            var tmpValidation = validations.Find(UniverseFieldNames.SubsequentRequest);
            if (tmpValidation.IsValid && tmpValidation.Field.Value.Equals("BR"))
            {
                var tmprequestAORValidation =
                    validations.Find(UniverseFieldNames.AORReceiptDate);

                var tmprequestPersonRequest =
                    validations.Find(UniverseFieldNames.PersonMakingRequest);

                if (tmprequestPersonRequest.IsValid &&
                     tmprequestPersonRequest.Field.Value.Equals("NA"))
                {
                    var v = new Validation();
                    v.IsValid = false;
                    v.Field = tmpPersonRequestValidation.Field;
                    v.AdditionalFields.Add(tmpValidation.Field);
                    v.Message = string.Format("{0} cannot have a value of NA", tmpPersonRequestValidation.Field.Name);
                    validations.Add(v);
                }

                if (tmprequestAORValidation.IsValid &&
                    tmprequestAORValidation.Field.Value.Equals("NA"))
                {
                    var v = new Validation();
                    v.IsValid = false;
                    v.Field = tmprequestAORValidation.Field;
                    v.AdditionalFields.Add(tmpValidation.Field);
                    v.Message = string.Format("{0} cannot have a value of NA", tmprequestAORValidation.Field.Name);
                    validations.Add(v);
                }
            }

            //If column O is Y then this cannot be NA.
            tmpValidation = validations.Find(UniverseFieldNames.TimeframeExtensionTaken);
            if (tmpValidation.IsValid && tmpValidation.Field.Value.Equals("Y"))
            {
                var tmprequestSponsorNotificationDelay =
                    validations.Find(UniverseFieldNames.SponsorNotificationDelay);

                if (tmprequestSponsorNotificationDelay.IsValid &&
                   tmprequestSponsorNotificationDelay.Field.Value.Equals("NA"))
                {

                    var v = new Validation();
                    v.Field = tmprequestSponsorNotificationDelay.Field;
                    v.IsValid = false;
                    v.AdditionalFields.Add(tmpValidation.Field);

                    v.Message = string.Format("{0} cannot be NA", tmprequestSponsorNotificationDelay.Field.Name);
                    validations.Add(v);
                }
            }

            //if request disposition is approved
            //request denied for lack of necessity should be NA
            //review completed for denial should be NA
            var tmpRequestDisposition = validations.Find(UniverseFieldNames.RequestDisposition);
            var tmpRequestDenied = validations.Find(UniverseFieldNames.RequestDeniedNecessity);
            var tmpRequestReviewed = validations.Find(UniverseFieldNames.RequestDeniedReviewd);

            if (tmpRequestDisposition.IsValid && tmpRequestDisposition.Field.Value.ToLower().Equals("approved"))
            {
                if (!tmpRequestDenied.Field.Value.ToUpper().Equals("NA") ||
                    !tmpRequestReviewed.Field.Value.ToUpper().Equals("NA"))
                {
                    var v = new Validation();
                    v.Field = tmpValidation.Field;
                    v.IsValid = false;
                    v.AdditionalFields.Add(tmpRequestDenied.Field);
                    v.AdditionalFields.Add(tmpRequestReviewed.Field);
                    v.Message = string.Format("{0} and {1} fields both need to be NA",
                        tmpRequestDenied.Field.Name, tmpRequestReviewed.Field.Name);
                    validations.Add(v);
                }

            }

            if (tmpRequestReviewed.IsValid &&
                (tmpRequestReviewed.Field.Value.Equals("Y") ||
                tmpRequestReviewed.Field.Value.Equals("N")) &&
                tmpRequestDenied.IsValid &&
                tmpRequestDenied.Field.Value.Equals("NA") &&
                tmpRequestDisposition.IsValid &&
                !tmpRequestDisposition.Field.Value.ToLower().Equals("approved"))
            {
                var v = new Validation();
                v.Field = tmpRequestDenied.Field;
                v.IsValid = false;
                v.AdditionalFields.Add(tmpRequestReviewed.Field);
                v.Message = string.Format("{0} cannot be NA", tmpRequestDenied.Field.Name);
                validations.Add(v);

            }




            if (tmpRequestReviewed.IsValid &&
                tmpRequestReviewed.Field.Value.Equals("NA") &&
                tmpRequestDenied.IsValid &&
                tmpRequestDenied.Field.Value.Equals("Y") &&
                tmpRequestDisposition.IsValid &&
                !tmpRequestDisposition.Field.Value.ToLower().Equals("approved"))
            {
                var v = new Validation();
                v.Field = tmpRequestReviewed.Field;
                v.IsValid = false;
                v.AdditionalFields.Add(tmpRequestDenied.Field);
                v.Message = string.Format("{0} cannot be NA", tmpRequestReviewed.Field.Name);
                validations.Add(v);
            }


            //Oral Notification should be within 24 hours of Sponsor Decision Date
            var tmpSponsorDecisionDate = validations.Find(UniverseFieldNames.SponsorDecisionDate);
            var tmpSponsorDecisionTime = validations.Find(UniverseFieldNames.SponsorDecisionTime);
            var tmpOralNotificationDate = validations.Find(UniverseFieldNames.OralNotificationDate);
            var tmpOralNotificationTime = validations.Find(UniverseFieldNames.OralNotificationTime);

            if (tmpSponsorDecisionDate.IsValid &&
                tmpSponsorDecisionDate.Field.DateValue.HasValue &&
                tmpSponsorDecisionTime.IsValid &&
                tmpSponsorDecisionTime.Field.DateValue.HasValue &&
                tmpOralNotificationDate.IsValid &&
                tmpOralNotificationDate.Field.DateValue.HasValue &&
                tmpOralNotificationTime.IsValid &&
                tmpOralNotificationTime.Field.DateValue.HasValue)
            {

                var sponsorDate = tmpSponsorDecisionDate.Field.DateValue.Value.AddTicks(tmpSponsorDecisionTime.Field.DateValue.Value.Ticks);
                var oralDate = tmpOralNotificationDate.Field.DateValue.Value.AddTicks(tmpOralNotificationTime.Field.DateValue.Value.Ticks);
                var span = oralDate.Subtract(sponsorDate);
                if (span.TotalHours > 24)
                {
                    var v = new Validation();
                    v.Field = tmpOralNotificationDate.Field;
                    v.AdditionalFields.Add(tmpOralNotificationTime.Field);
                    v.AdditionalFields.Add(tmpSponsorDecisionDate.Field);
                    v.AdditionalFields.Add(tmpSponsorDecisionTime.Field);
                    v.IsValid = false;
                    v.Message = string.Format("{0} and {1} need to be within 24 hours of {2} and {3}",
                        tmpOralNotificationDate.Field.Name, tmpOralNotificationTime.Field.Name,
                        tmpSponsorDecisionDate.Field.Name, tmpSponsorDecisionTime.Field.Name);
                    validations.Add(v);

                }
            }


            //Written Notification should be within 72 hours of Oral Notification
            //Oral Notification should be within 24 hours of Sponsor Decision Date
            var tmpWrittenDate = validations.Find(UniverseFieldNames.EnrolleeWrittenNotificationDate);
            var tmpWrittenTime = validations.Find(UniverseFieldNames.WrittenNotificationTime);
            tmpOralNotificationDate = validations.Find(UniverseFieldNames.OralNotificationDate);
            tmpOralNotificationTime = validations.Find(UniverseFieldNames.OralNotificationTime);

            if (tmpSponsorDecisionDate.IsValid &&
                tmpSponsorDecisionDate.Field.DateValue.HasValue &&
                tmpSponsorDecisionTime.IsValid &&
                tmpSponsorDecisionTime.Field.DateValue.HasValue &&
                tmpOralNotificationDate.IsValid &&
                tmpOralNotificationDate.Field.DateValue.HasValue &&
                tmpOralNotificationTime.IsValid &&
                tmpOralNotificationTime.Field.DateValue.HasValue)
            {

                var sponsorDate = tmpSponsorDecisionDate.Field.DateValue.Value.AddTicks(tmpSponsorDecisionTime.Field.DateValue.Value.Ticks);
                var oralDate = tmpOralNotificationDate.Field.DateValue.Value.AddTicks(tmpOralNotificationTime.Field.DateValue.Value.Ticks);
                var span = oralDate.Subtract(sponsorDate);
                if (span.TotalHours > 24)
                {
                    var v = new Validation();
                    v.Field = tmpOralNotificationDate.Field;
                    v.AdditionalFields.Add(tmpOralNotificationTime.Field);
                    v.AdditionalFields.Add(tmpSponsorDecisionDate.Field);
                    v.AdditionalFields.Add(tmpSponsorDecisionTime.Field);
                    v.IsValid = false;
                    v.Message = string.Format("{0} and {1} need to be within 24 hours of {2} and {3}",
                        tmpOralNotificationDate.Field.Name, tmpOralNotificationTime.Field.Name,
                        tmpSponsorDecisionDate.Field.Name, tmpSponsorDecisionTime.Field.Name);
                    validations.Add(v);

                }
            }


            return validations;
        }


    }
}
