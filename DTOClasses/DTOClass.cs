using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOClasses
{
    public class DTOClass
    {

        public class PersonDTO
        {
            public int PersonID { get; set; }
            public string NationalNo { get; set; }
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public string ThirdName { set; get; }
            public string LastName { set; get; }
            public short Gender {  get; set; }
            public DateTime DateOfBirth { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public int NationalityCountryID { get; set; }
            public string ImagePath {  get; set; }


            public PersonDTO(int personID, string nationalNo, string firstName, string secondName,
                 string thirdName,string lastName, short gender, DateTime dateOfBirth, 
                 string phone,string email, int nationalityCountryID, string imagePath)
            {
                this.PersonID = personID;
                this.NationalNo = nationalNo;
                this.FirstName = firstName;
                this.SecondName = secondName;
                this.ThirdName = thirdName;
                this.LastName = lastName;
                this.Gender = gender;   
                this.DateOfBirth = dateOfBirth;
                this.Phone = phone;
                this.Email = email;
                this.NationalityCountryID = nationalityCountryID;
                this.ImagePath = imagePath;
              


        }


        }

        public class LicenseDTO
        {
            public int LicenseID { get; set; }
            public int DriverID { set; get; }
            public int LicenseClass { set; get; }
            public DateTime IssueDate { set; get; }
            public DateTime ExpirationDate { set; get; }
            public bool IsActive { set; get; }

            public LicenseDTO(int licenseId,int driverId,int licenseClass,DateTime issueDate,DateTime expirationDate,bool isActive)
            {
                this.LicenseID = licenseId;
                this.DriverID = driverId;
                this.LicenseClass = licenseClass;
                this.IssueDate = issueDate;
                this.ExpirationDate = expirationDate;
                this.IsActive= isActive;
               
            }


        }










    }
}
