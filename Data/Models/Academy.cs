using System.Collections.Generic;
using Data.Models.Academies;

namespace Data.Models
{
    public class Academy
    {
        public Academy()
        {
            Performance = new AcademyPerformance();
            PupilNumbers = new PupilNumbers();
            LatestOfstedJudgement = new LatestOfstedJudgement();
            Address = new List<string>();
        }
        
        public string Ukprn { get; set; }
        public string Urn { get; set; }
        public string Name { get; set; }
        public string LocalAuthorityName { get; set; }
        public string EstablishmentType { get; set; }
        public string FaithSchool { get; set; }
        public string Pfi { get; set; }
        public AcademyPerformance Performance { get; set; }
        public PupilNumbers PupilNumbers { get; set; }

        public LatestOfstedJudgement LatestOfstedJudgement { get; set; }
        public List<string> Address { get; set; }
    }
}