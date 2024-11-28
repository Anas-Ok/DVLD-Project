namespace DTOallclasses
{
    static public class DTOclass
    {
        public class PersonDTO
        {
            public int PersonID { get; set; }
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public string ThirdName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Address { get; set; }
            public DateTime DateOfBirth { get; set; }
            public string NationalNo { get; set; }
            public int NationalityCountryID { get; set; }
            public string ImagePath { get; set; }


            public PersonDTO(int personID, string firstName, string secondName, string thirdName, string lastName,
                string email, string phone, string address, DateTime dateOfBirth, string nationalNo,
                int nationalityCountryID, string imagePath)
            {
                this.PersonID = personID;
                this.FirstName = firstName;
                this.SecondName = secondName;
                this.ThirdName = thirdName;
                this.LastName = lastName;
                this.Email = email;
                this.Phone = phone;
                this.Address = address;
                this.DateOfBirth = dateOfBirth;
                this.NationalNo = nationalNo;
                this.NationalityCountryID = nationalityCountryID;
                this.ImagePath = imagePath;
            }


        }



    }
}
