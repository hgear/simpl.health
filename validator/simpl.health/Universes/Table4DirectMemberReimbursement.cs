using System;
using System.Collections.Generic;

namespace simpl.health.Universes
{
    

    public class Table4DirectMemberReimbursement : Universe
    {

        private static readonly string deniedIREAutoForward = "denied with IRE auto forward";

        private static readonly string IREUntimelyDecision = "IRE auto-forward due to untimely decision";

        private readonly List<string> approvedDeniedIRE =
            new List<string>() { "approved", "denied", deniedIREAutoForward,
            IREUntimelyDecision};

        private readonly List<string> pendingUntimely =
            new List<string>() { "Pending", "Untimely" };


        public Table4DirectMemberReimbursement() :base()
        {
            this.Name = "Direct Member Reimbursement";

            
            Fields.Add(UniverseFieldNames.PersonMakingRequest, true, 3, 7, typeof(String));
            Fields.Add(UniverseFieldNames.ProviderType, true, 3, 8, typeof(String));
            Fields.Add(UniverseFieldNames.RequestReceivedDate, true, 10, 9, typeof(DateTime));
            Fields.Add(UniverseFieldNames.Diagnosis, true, 100, 10, typeof(String));
            Fields.Add(UniverseFieldNames.TypeOfService, true, 2000, 11, typeof(string));
            Fields.Add(UniverseFieldNames.LevelOfService, true, 50, 12, typeof(String));
            Fields.Add(UniverseFieldNames.RequestDisposition, true, 8, 13, typeof(String), true);
            Fields.Add(UniverseFieldNames.ReimbursementPaidDate, true, 10, 14, typeof(DateTime));
            Fields.Add(UniverseFieldNames.ReimbursementInterestPaid, true, 1, 15, typeof(string));
            Fields.Add(UniverseFieldNames.EnrolleeWrittenNotificationDate, true, 10, 16, typeof(DateTime), false, pendingUntimely);
            Fields.Add(UniverseFieldNames.IREForwardedDate, true, 10, 17, typeof(DateTime), false, notApplicable);
            Fields.Add(UniverseFieldNames.EnrolleeRequestForwardedIREDate, true, 10, 18, typeof(DateTime), false, notApplicable);
            Fields.Add(UniverseFieldNames.AORReceiptDate, true, 10, 19, typeof(DateTime), false, notApplicable);
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

            v.RemoveAll(v => v.Field.Name == UniverseFieldNames.RequestDisposition);
            ValidateField(UniverseFieldNames.RequestDisposition, values, v,
                    ValidateFieldWithPossibleValues, this.approvedDeniedIRE);


            Validation tmpV;

            ///If this is BR then Column S should have a data and not be NA
            var tmpRequestor = v.Find(UniverseFieldNames.PersonMakingRequest);
            var tmpAORReceiptDate = v.Find(UniverseFieldNames.AORReceiptDate);
            if (tmpAORReceiptDate.IsValid &&
               tmpRequestor.IsValid)
            {
                if (tmpAORReceiptDate.Field.Value.Equals("NA") &&
                    tmpRequestor.Field.Value.Equals("BR"))
                {
                    tmpV = new Validation();
                    tmpV.Field = tmpAORReceiptDate.Field;
                    tmpV.AdditionalFields.Add(tmpRequestor.Field);
                    tmpV.IsValid = false;
                    tmpV.Message = string.Format("{0} cannot be NA and needs to be a valid date",
                        tmpAORReceiptDate.Field.Name);
                    v.Add(tmpV);
                }
            }

            //If this is denied with IRE auto forward, or IRE auto-forward due
            //to untimely decision then columns Q and R should not be NA
            var tmpDisposition = v.Find(UniverseFieldNames.RequestDisposition);
            var tmpIREDate = v.Find(UniverseFieldNames.IREForwardedDate);
            var tmpIRENotifiedDate = v.Find(UniverseFieldNames.EnrolleeRequestForwardedIREDate);

            if (tmpDisposition.IsValid && tmpDisposition.Field.Value.Equals(deniedIREAutoForward) ||
                tmpDisposition.Field.Value.Equals(IREUntimelyDecision))
            {
                if(tmpIREDate.Field.Equals("NA"))
                {
                    tmpV = new Validation();
                    tmpV.Field = tmpIREDate.Field;
                    tmpV.IsValid = false;
                    tmpV.AdditionalFields.Add(tmpDisposition.Field);
                    tmpV.Message = "This field cannot be NA";
                    v.Add(tmpV);

                }

                if (tmpIRENotifiedDate.Field.Equals("NA"))
                {
                    tmpV = new Validation();
                    tmpV.Field = tmpIRENotifiedDate.Field;
                    tmpV.IsValid = false;
                    tmpV.AdditionalFields.Add(tmpDisposition.Field);
                    tmpV.Message = "This field cannot be NA";
                    v.Add(tmpV);
                }
            }

            //If column P is untimely, then this should be auto - forward due to untimely decision
            tmpDisposition = v.Find(UniverseFieldNames.RequestDisposition);
            var tmpWrittenNotification = v.Find(UniverseFieldNames.IREForwardedDate);
            

            if (tmpWrittenNotification.IsValid && tmpWrittenNotification.Field.Value.Equals("Untimely"))
            {
                if (!tmpDisposition.Field.Equals(deniedIREAutoForward))
                {
                    tmpV = new Validation();
                    tmpV.Field = tmpIREDate.Field;
                    tmpV.IsValid = false;
                    tmpV.AdditionalFields.Add(tmpDisposition.Field);
                    tmpV.Message = string.Format("This field has to be {0}", deniedIREAutoForward);
                    v.Add(tmpV);
                }
            }


            //If column M is denied, this should be denied as well
            var tmpDateReimPaid = v.Find(UniverseFieldNames.ReimbursementPaidDate);
            if (tmpDateReimPaid.IsValid && tmpDisposition.IsValid &&
                !tmpDateReimPaid.Field.Value.ToLower().Equals("denied") &&
                tmpDisposition.Field.Value.Equals("denied"))
            {
                tmpV = new Validation();
                tmpV.Field = tmpDateReimPaid.Field;
                tmpV.AdditionalFields.Add(tmpDisposition.Field);
                tmpV.Message = string.Format("Field also needs to be denied");
                v.Add(tmpV);

            }

            //If column M is IRE auto - forward due to untimely decsion, then this should be denied.
            if (tmpDateReimPaid.IsValid && tmpDisposition.IsValid &&
                !tmpDateReimPaid.Field.Value.ToLower().Equals("denied") &&
                tmpDisposition.Field.Value.Equals(IREUntimelyDecision))
            {
                tmpV = new Validation();
                tmpV.Field = tmpDateReimPaid.Field;
                tmpV.AdditionalFields.Add(tmpDisposition.Field);
                tmpV.Message = string.Format("Field needs to be denied");
                v.Add(tmpV);

            }

            //This should always be N if the request was paid within 60 days or  Column I and S calculation
            var tmpInterestOnTime = v.Find(UniverseFieldNames.ReimbursementInterestPaid);
            var tmpReqRecDate = v.Find(UniverseFieldNames.RequestReceivedDate);
            tmpAORReceiptDate = v.Find(UniverseFieldNames.AORReceiptDate);
            if (tmpInterestOnTime.IsValid &&
                tmpReqRecDate.IsValid &&
                tmpReqRecDate.Field.DateValue.HasValue &&
                tmpAORReceiptDate.IsValid)
            {
                if (!tmpAORReceiptDate.Field.DateValue.HasValue)
                {
                    tmpV = new Validation();
                    tmpV.Field = tmpAORReceiptDate.Field;
                    tmpV.AdditionalFields.Add(tmpInterestOnTime.Field);
                    tmpV.AdditionalFields.Add(tmpReqRecDate.Field);
                    tmpV.IsValid = false;
                    tmpV.Message = "Field is required";
                    v.Add(tmpV);
                }
                else
                {
                    var ReqRecDate = tmpReqRecDate.Field.DateValue.Value;
                    var aorDate = tmpAORReceiptDate.Field.DateValue.Value;
                    if (aorDate.Subtract(ReqRecDate).TotalDays <= 60 && tmpInterestOnTime.Field.Value.Equals('Y'))
                    {
                        tmpV = new Validation();
                        tmpV.Field = tmpInterestOnTime.Field;
                        tmpV.AdditionalFields.Add(tmpReqRecDate.Field);
                        tmpV.AdditionalFields.Add(tmpAORReceiptDate.Field);
                        tmpV.Message = "This field should be N since request was paid within 60 days";
                        v.Add(tmpV);
                    }
                    else if (aorDate.Subtract(ReqRecDate).TotalDays > 60 && tmpInterestOnTime.Field.Value.Equals('N'))
                    {
                        tmpV = new Validation();
                        tmpV.Field = tmpInterestOnTime.Field;
                        tmpV.AdditionalFields.Add(tmpReqRecDate.Field);
                        tmpV.AdditionalFields.Add(tmpAORReceiptDate.Field);
                        tmpV.Message = "This should be Y since request was paid after 60 days";
                        v.Add(tmpV);
                    }
                }
            }

            //If this is Untimely, then column Q and R should not be NA
            //If this is untimely, then Column M should be IRE auto-forward due to untimely decision.
            var tmpEnrolleeWrittenDate = v.Find(UniverseFieldNames.EnrolleeWrittenNotificationDate);
            var tmpIREForwardDate = v.Find(UniverseFieldNames.IREForwardedDate);
            var tmpEnrolleeNotifiedIRE = v.Find(UniverseFieldNames.EnrolleeRequestForwardedIREDate);
            if (tmpEnrolleeWrittenDate.IsValid &&
                tmpIREForwardDate.IsValid &&
                tmpEnrolleeNotifiedIRE.IsValid)
            {

            }



            return v;
        }



       
    }
}
