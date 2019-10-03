using System;

namespace Common
{
    public class DMFObject
    {
        public DescriptionType DescriptionType { get; set; }
        public string SSN { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public VerifiedProofType VerifiedProofType { get; set; }
        public DateTime DateOfDeath { get; set; }
        public DateTime DateOfBirth { get; set; }        
    }

    public enum DescriptionType
    {
        /// <summary>
        /// Add
        /// </summary>
        A,
        /// <summary>
        /// Change
        /// </summary>
        C,
        /// <summary>
        /// Delete
        /// </summary>
        D
    }
    
    public enum VerifiedProofType
    {
        /// <summary>
        /// Verified
        /// </summary>
        V,
        /// <summary>
        /// Proof
        /// </summary>
        P
    }
}
