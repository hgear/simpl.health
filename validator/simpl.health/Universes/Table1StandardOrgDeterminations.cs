using System;
using System.Collections.Generic;

namespace simpl.health.Universes
{


    public class Table1StandardOrgDeterminations : Universe
    {

        private readonly List<string> requestExpeditedTimeFrame =
            new List<string>() {"NCP", "B", "BR", "NA"};

        public Table1StandardOrgDeterminations() :base()
        {
            this.Name = "Stanrdard Org Determination";
            
            Fields.Add(UniverseFieldNames.PersonMakingRequest, true, 3, 7, typeof(String));
            Fields.Add(UniverseFieldNames.ProviderType, true, 3, 8, typeof(String));
            Fields.Add(UniverseFieldNames.RequestReceivedDate, true, 10, 9, typeof(DateTime));
            Fields.Add(UniverseFieldNames.Diagnosis, true, 100, 10, typeof(String));
            Fields.Add(UniverseFieldNames.TypeOfService, true, 2000, 11, typeof(string));
            Fields.Add(UniverseFieldNames.LevelOfService, true, 50, 12, typeof(String));
            Fields.Add(UniverseFieldNames.ExpeditedOrStandardTimeFrame, true, 1, 13, typeof(String));
            Fields.Add(UniverseFieldNames.RequestExpedited, true, 3, 14, typeof(String));
            Fields.Add(UniverseFieldNames.TimeframeExtensionTaken, true, 1, 15, typeof(String));
            Fields.Add(UniverseFieldNames.SponsorNotificationDelay, true, 2, 16, typeof(String));
            Fields.Add(UniverseFieldNames.RequestDisposition, true, 8, 17, typeof(String), true);
            Fields.Add(UniverseFieldNames.SponsorDecisionDate, true, 10, 18, typeof(DateTime), ignoreCase: false, notApplicable);
            Fields.Add(UniverseFieldNames.RequestDeniedNecessity, true, 2, 19, typeof(String), false, notApplicable);
            Fields.Add(UniverseFieldNames.RequestDeniedReviewd, true, 2, 20, typeof(String));
            Fields.Add(UniverseFieldNames.OralNotificationDate, true, 10, 21, typeof(DateTime), false, notApplicable);
            Fields.Add(UniverseFieldNames.EnrolleeWrittenNotificationDate, true, 10, 22, typeof(DateTime), false, notApplicable);
            Fields.Add(UniverseFieldNames.AuthorizationEnteredDate, true, 10, 23, typeof(DateTime), false, notApplicable);
            Fields.Add(UniverseFieldNames.AORReceiptDate, true, 10, 24, typeof(DateTime), false, notApplicable);
            Fields.Add(UniverseFieldNames.FirstTierRelatedEntity, true, 70, 25, typeof(String));
        }

        /// <summary>
        // 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public override Validations ValidateFields(List<string> values)
        {
            var v = new Validations();
            UniverseField field = null;

            ValidateOrgDeterminationFields(v, values);

            ValidateField(UniverseFieldNames.PersonMakingRequest, values, v,
                ValidateFieldWithPossibleValues, personWhoMadeRequestPossibleValues);

            ValidateField(UniverseFieldNames.ExpeditedOrStandardTimeFrame, values, v,
                ValidateFieldWithPossibleValues, yesNo);

            ValidateField(UniverseFieldNames.RequestExpedited, values, v,
                ValidateFieldWithPossibleValues, requestExpeditedTimeFrame);
                
            //Logical Errors
            //If this is BR then column X should have a date that is not more than a year from column I
            var tmpValidation =
                v.Find(f => f.Field.Name == UniverseFieldNames.PersonMakingRequest);
            if (tmpValidation.IsValid
                && field.Value.Equals("BR"))
            {
                var tmprequestReceivedValidation =
                    v.Find(f => f.Field.Name == UniverseFieldNames.RequestReceivedDate);
                var tmprequestAORValidation =
                    v.Find(f => f.Field.Name == UniverseFieldNames.AORReceiptDate);

                if (tmprequestReceivedValidation.IsValid &&
                    tmprequestAORValidation.IsValid)
                {
                    DateTime requesteReceived =
                    Convert.ToDateTime(tmprequestReceivedValidation.Field.Value);
                    DateTime AORRequest =
                        Convert.ToDateTime(tmprequestAORValidation.Field.Value);

                    if (AORRequest.Subtract(requesteReceived).TotalDays > 365)
                    {
                        var tmpV = new Validation();
                        tmpV.Field = tmpValidation.Field;
                        tmpV.IsValid = false;
                        tmpV.AdditionalFields.Add(tmprequestReceivedValidation.Field);
                        tmpV.AdditionalFields.Add(tmprequestAORValidation.Field);
                        tmpV.Message = String.Format("Fields {0} and {1} have to be 1 year apart",
                            tmprequestAORValidation.Field.Name,
                            tmprequestReceivedValidation.Field.Name);
                        tmpV.ValidationType = ValidationType.Field;
                        v.Add(tmpV);
                    }
                }
            }

            DateTime tmpDate;
            //Request for Expedited time frame
            //If value is BR then AORDate should be a date.
            tmpValidation = v.Find(f => f.Field.Name == UniverseFieldNames.RequestExpedited);
            if (tmpValidation.IsValid && tmpValidation.Field.Value.Equals("BR"))
            {
                var tmprequestAORValidation =
                    v.Find(f => f.Field.Name == UniverseFieldNames.AORReceiptDate);
                
                if (!DateTime.TryParse(tmprequestAORValidation.Field.Value,
                    out tmpDate))
                {
                    var tmpV = new Validation();
                    tmpV.Field = tmpValidation.Field;
                    tmpV.IsValid = false;
                    tmpV.AdditionalFields.Add(tmprequestAORValidation.Field);
                    tmpV.Message = string.Format("{0} needs to be a valid date", tmprequestAORValidation.Field.Name);
                    v.Add(tmpV);
                }
            }

            //If column O is Y then this cannot be NA.
            tmpValidation = v.Find(UniverseFieldNames.TimeframeExtensionTaken);
            if (tmpValidation.IsValid && tmpValidation.Field.Value.Equals("Y"))
            {
                var tmprequestSponsorNotificationDelay = v.Find(UniverseFieldNames.SponsorNotificationDelay);
                
                if (!DateTime.TryParse(tmprequestSponsorNotificationDelay.Field.Value, out tmpDate))
                {
                    var tmpV = new Validation();
                    tmpV.Field = tmpValidation.Field;
                    tmpV.IsValid = false;
                    tmpV.AdditionalFields.Add(tmprequestSponsorNotificationDelay.Field);
                    tmpV.Message = string.Format("{0} needs to be a valid date", tmprequestSponsorNotificationDelay.Field.Name);
                    v.Add(tmpV);
                }
            }

            //if request disposition is approved
            //request denied for lack of necessity should be NA
            //review completed for denial should be NA
            tmpValidation = v.Find(UniverseFieldNames.RequestDisposition);
            var tmpRequestDenied = v.Find(UniverseFieldNames.RequestDeniedNecessity);
            var tmpRequestReviewed = v.Find(UniverseFieldNames.RequestDeniedReviewd);

            if (tmpValidation.IsValid && tmpValidation.Field.Value.ToLower().Equals("approved"))
            {
                if (!tmpRequestDenied.Field.Value.ToUpper().Equals("NA") ||
                    !tmpRequestReviewed.Field.Value.ToUpper().Equals("NA"))
                {
                    var tmpV = new Validation();
                    tmpV.Field = tmpValidation.Field;
                    tmpV.IsValid = false;
                    tmpV.AdditionalFields.Add(tmpRequestDenied.Field);
                    tmpV.AdditionalFields.Add(tmpRequestReviewed.Field);
                    tmpV.Message = string.Format("{0} and {1} fields both need to be NA",
                        tmpRequestDenied.Field.Name, tmpRequestReviewed.Field.Name);
                    v.Add(tmpV);
                }

            }

            if (tmpRequestReviewed.IsValid &&
                (tmpRequestReviewed.Field.Value.Equals("Y") ||
                tmpRequestReviewed.Field.Value.Equals("N")) &&
                tmpRequestDenied.IsValid &&
                tmpRequestDenied.Field.Value.Equals("NA"))
            {
                var tmpV = new Validation();
                tmpV.Field = tmpRequestDenied.Field;
                tmpV.IsValid = false;
                tmpV.AdditionalFields.Add(tmpRequestReviewed.Field);
                tmpV.Message = string.Format("{0} cannot be NA", tmpRequestDenied.Field.Name);
                v.Add(tmpV);
                
            }

            if (tmpRequestReviewed.IsValid &&
                tmpRequestReviewed.Field.Value.Equals("NA") &&
                tmpRequestDenied.IsValid &&
                tmpRequestDenied.Field.Value.Equals("Y"))
            {
                var tmpV = new Validation();
                tmpV.Field = tmpRequestReviewed.Field;
                tmpV.IsValid = false;
                tmpV.AdditionalFields.Add(tmpRequestDenied.Field);
                tmpV.Message = string.Format("{0} cannot be NA", tmpRequestReviewed.Field.Name);
                v.Add(tmpV);
            }

            var tmpOralNotification = v.Find(UniverseFieldNames.OralNotificationDate);
            var tmpWrittenNotfication = v.Find(UniverseFieldNames.EnrolleeWrittenNotificationDate);
            if (tmpOralNotification.IsValid && tmpWrittenNotfication.IsValid)
            {
                DateTime dateOralNotification;

                DateTime dateWrittenNotification;

                if (DateTime.TryParse(tmpOralNotification.Field.Value, out dateOralNotification))
                {
                    if (DateTime.TryParse(tmpWrittenNotfication.Field.Value, out dateWrittenNotification))
                    {
                        var timeSpan = dateWrittenNotification.Subtract(dateOralNotification);
                        if (timeSpan.TotalHours > 72)
                        {
                            var tmpV = new Validation();
                            tmpV.Field = tmpOralNotification.Field;
                            String.Format("{0} field must be within 72 hours of {1}",
                                tmpOralNotification.Field.Name, tmpWrittenNotfication.Field.Name);
                            tmpV.IsValid = false;
                            v.Add(tmpV);
                        }
                    }
                    else
                    {
                        var tmpV = new Validation();
                        tmpV.Field = tmpOralNotification.Field;
                        tmpV.AdditionalFields.Add(tmpWrittenNotfication.Field);
                        tmpV.IsValid = false;
                        tmpV.Message = string.Format("{0} field must be a valid date",
                            tmpOralNotification.Field.Value);
                        v.Add(tmpV);
                    }
                }
            }


            var tmpAORReceiptDate = v.Find(UniverseFieldNames.AORReceiptDate);
            var tmpPersonMadeRequest = v.Find(UniverseFieldNames.PersonMakingRequest);
            var tmpRequestExpedited = v.Find(UniverseFieldNames.RequestExpedited);
            if (tmpAORReceiptDate.IsValid &&
                tmpPersonMadeRequest.IsValid &&
                tmpRequestExpedited.IsValid)
            {
                if (tmpAORReceiptDate.Field.Value.Equals("NA") &&
                    (tmpPersonMadeRequest.Field.Value.Equals("BR") ||
                     tmpRequestExpedited.Field.Value.Equals("BR")
                    ))
                {
                    var tmpV = new Validation();
                    tmpV.Field = tmpAORReceiptDate.Field;
                    tmpV.AdditionalFields.Add(tmpPersonMadeRequest.Field);
                    tmpV.AdditionalFields.Add(tmpRequestExpedited.Field);
                    tmpV.IsValid = false;
                    tmpV.Message = string.Format("{0} cannot be NA and needs to be a valid date",
                        tmpAORReceiptDate.Field.Name);
                    v.Add(tmpV);
                }
            }


            return v;
        }

       
    }
}
