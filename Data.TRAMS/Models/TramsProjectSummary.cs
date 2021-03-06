using System.Collections.Generic;
using Data.TRAMS.Models.AcademyTransferProject;

namespace Data.TRAMS.Models
{
    public class TramsProjectSummary
    {
        public TrustSummary OutgoingTrust { get; set; }
        public string OutgoingTrustUkprn { get; set; }
        public string ProjectNumber { get; set; }
        public string ProjectUrn { get; set; }
        public List<TransferringAcademy> TransferringAcademies { get; set; }
    }
}