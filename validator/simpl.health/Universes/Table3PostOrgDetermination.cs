using System;
using System.Collections.Generic;

namespace simpl.health.Universes
{


    public class Table3PostOrgDetermination : Universe
    {
        public Table3PostOrgDetermination() : base()
        {
            this.Name = "Post Org Determination";

            Fields.Add(UniverseFieldNames.ProviderType, true, 3, 7, typeof(String));
            Fields.Add(UniverseFieldNames.CleanClaim, true, 2, 8, typeof(String));
            Fields.Add(UniverseFieldNames.RequestReceivedDate, true, 10, 9, typeof(DateTime));
            Fields.Add(UniverseFieldNames.Diagnosis, true, 100, 10, typeof(String));
            Fields.Add(UniverseFieldNames.TypeOfService, true, 2000, 11, typeof(string));
            Fields.Add(UniverseFieldNames.LevelOfService, true, 50, 12, typeof(String));
            Fields.Add(UniverseFieldNames.RequestDisposition, true, 8, 13, typeof(String), true);
            Fields.Add(UniverseFieldNames.ClaimPaidDate, true, 10, 14, typeof(DateTime));
            Fields.Add(UniverseFieldNames.ClaimInterestPaid, true, 1, 15, typeof(string));
            Fields.Add(UniverseFieldNames.RequestDeniedNecessity, true, 2, 16, typeof(String), false, notApplicable);
            Fields.Add(UniverseFieldNames.RequestDeniedReviewd, true, 2, 17, typeof(String));
            Fields.Add(UniverseFieldNames.EnrolleeWrittenNotificationDate, true, 10, 18, typeof(DateTime), false, notApplicable);
            Fields.Add(UniverseFieldNames.ProviderWrittenNotifiationDate, true, 10, 19, typeof(DateTime), false, notApplicable);
            Fields.Add(UniverseFieldNames.FirstTierRelatedEntity, true, 70, 20, typeof(String));
        }

        /// <summary>
        // 
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public override Validations ValidateFields(List<string> values)
        {
            var v = new Validations();

            ValidateOrgDeterminationFields(v, values);
            ValidateField(UniverseFieldNames.CleanClaim, values, v, ValidateFieldWithPossibleValues, yesNoNotApplicable);
            ValidateField(UniverseFieldNames.ClaimPaidDate, values, v, ValidateDate);
            ValidateField(UniverseFieldNames.ClaimInterestPaid, values, v, ValidateFieldWithPossibleValues, yesNo);
            ValidateField(UniverseFieldNames.ProviderWrittenNotifiationDate, values, v, ValidateDate);

            Validation tmpV;

            //If this is Y, then Column I and S should be less than or equal to 30 days.
            ///If this is N, then column I and S should less than or equal to 60 days.
            var vCleanClaim = v.Find(UniverseFieldNames.CleanClaim);
            var vReceivedDate = v.Find(UniverseFieldNames.RequestReceivedDate);
            var vWrittenDate = v.Find(UniverseFieldNames.ProviderWrittenNotifiationDate);
            var vClaimInterest = v.Find(UniverseFieldNames.ClaimInterestPaid);
            if (vCleanClaim.IsValid && vReceivedDate.IsValid && vWrittenDate.IsValid && vClaimInterest.IsValid)
            {
                if (vWrittenDate.Field.DateValue.HasValue)
                {
                    if (vCleanClaim.Field.Value.Equals("Y"))
                    {
                        if (vWrittenDate.Field.DateValue.Value.Subtract(vReceivedDate.Field.DateValue.Value).TotalDays > 30)
                        {
                            tmpV = new Validation();
                            tmpV.Field = vCleanClaim.Field;
                            tmpV.IsValid = false;
                            tmpV.AdditionalFields.Add(vReceivedDate.Field);
                            tmpV.AdditionalFields.Add(vWrittenDate.Field);
                            tmpV.Message = string.Format("{0} and {1} must be within 30 days", vWrittenDate.Field.Name,
                                vReceivedDate.Field.Name);
                            v.Add(tmpV);
                        }
                        else if (vClaimInterest.Field.Value.Equals("Y"))
                        {
                            tmpV = new Validation();
                            tmpV.Field = vClaimInterest.Field;
                            tmpV.IsValid = false;
                            tmpV.AdditionalFields.Add(vReceivedDate.Field);
                            tmpV.AdditionalFields.Add(vWrittenDate.Field);
                            tmpV.Message = string.Format("No Interest should be charged, claim was paid on time");
                            v.Add(tmpV);
                        }
                    }
                    else if (vCleanClaim.Field.Value.Equals("N"))
                    {
                        if (vWrittenDate.Field.DateValue.Value.Subtract(vReceivedDate.Field.DateValue.Value).TotalDays > 60)
                        {
                            tmpV = new Validation();
                            tmpV.Field = vWrittenDate.Field;
                            tmpV.IsValid = false;
                            tmpV.AdditionalFields.Add(vReceivedDate.Field);
                            tmpV.AdditionalFields.Add(vWrittenDate.Field);
                            tmpV.Message = string.Format("{0} and {1} must be within 60 days", vWrittenDate.Field.Name,
                                vReceivedDate.Field.Name);
                            v.Add(tmpV);
                        }
                        else if (vClaimInterest.Field.Value.Equals("Y"))
                        {
                            tmpV = new Validation();
                            tmpV.Field = vClaimInterest.Field;
                            tmpV.IsValid = false;
                            tmpV.AdditionalFields.Add(vReceivedDate.Field);
                            tmpV.AdditionalFields.Add(vWrittenDate.Field);
                            tmpV.Message = string.Format("No Interest should be charged, claim was paid on time");
                            v.Add(tmpV);
                        }
                    }
                }
                else
                {
                    tmpV = new Validation();
                    tmpV.Field = vWrittenDate.Field;
                    tmpV.IsValid = false;
                    tmpV.Message = String.Format("{0} is required to be a date to compute timeliness", vWrittenDate.Field.Name);
                    v.Add(tmpV);
                }
            }


            //if request disposition is approved
            //request denied for lack of necessity should be NA
            //review completed for denial should be NA
            var tmpReqDis = v.Find(UniverseFieldNames.RequestDisposition);
            var tmpReqDenied = v.Find(UniverseFieldNames.RequestDeniedNecessity);
            var tmpReqReviewed = v.Find(UniverseFieldNames.RequestDeniedReviewd);

            if (tmpReqDis.IsValid && tmpReqDis.Field.Value.ToLower().Equals("approved"))
            {
                if (!tmpReqDenied.Field.Value.ToUpper().Equals("NA") ||
                    !tmpReqReviewed.Field.Value.ToUpper().Equals("NA"))
                {
                    tmpV = new Validation();
                    tmpV.Field = tmpReqDis.Field;
                    tmpV.IsValid = false;
                    tmpV.AdditionalFields.Add(tmpReqDenied.Field);
                    tmpV.AdditionalFields.Add(tmpReqReviewed.Field);
                    tmpV.Message = string.Format("{0} and {1} fields both need to be NA",
                        tmpReqDenied.Field.Name, tmpReqReviewed.Field.Name);
                    v.Add(tmpV);
                }

            }

            if (tmpReqReviewed.IsValid &&
                    (tmpReqReviewed.Field.Value.Equals("Y") ||
                    tmpReqReviewed.Field.Value.Equals("N")) &&
                    tmpReqDenied.IsValid &&
                    tmpReqDenied.Field.Value.Equals("NA"))
            {
                tmpV = new Validation();
                tmpV.Field = tmpReqDenied.Field;
                tmpV.IsValid = false;
                tmpV.AdditionalFields.Add(tmpReqReviewed.Field);
                tmpV.Message = string.Format("{0} cannot be NA", tmpReqDenied.Field.Name);
                v.Add(tmpV);
            }

            if (tmpReqReviewed.IsValid &&
                tmpReqReviewed.Field.Value.Equals("NA") &&
                tmpReqDenied.IsValid &&
                tmpReqDenied.Field.Value.Equals("Y"))
            {
                tmpV = new Validation();
                tmpV.Field = tmpReqReviewed.Field;
                tmpV.IsValid = false;
                tmpV.AdditionalFields.Add(tmpReqDenied.Field);
                tmpV.Message = string.Format("{0} cannot be NA", tmpReqReviewed.Field.Name);
                v.Add(tmpV);
            }


            return v;
        }


    }
}
