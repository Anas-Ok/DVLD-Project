using DVLD_Buisness;
using DVLD_DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static DVLD_Buisness.clsApplication;
using static DVLD_Buisness.clsLicense;


namespace ApiDVLDLayer.Controllers
{

    [Route("api/People")]
    [ApiController]
    public class PeopleController: ControllerBase
    {
        //=========================================================
        //Get all people
        [HttpGet("AllPeople", Name = "GetAllPeople")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<PersonDTO>> GetAllPeople()
        {
            try
            {
                var allPeople = clsPerson.GetAllPeople();

                if (allPeople.Rows.Count < 1)
                    return NotFound("Not found! No data found!");



                var PeopleList = new List<PersonDTO>();
                foreach (DataRow row in allPeople.Rows)
                {
                    PeopleList.Add(new PersonDTO(

                        Convert.ToInt32(row["PersonID"]),
                        Convert.ToString(row["FirstName"]),
                        Convert.ToString(row["SecondName"]),
                        Convert.ToString(row["ThirdName"]),
                        Convert.ToString(row["LastName"]),
                         Convert.ToString(row["NationalNo"]),
                         Convert.ToDateTime(row["DateOfBirth"]),
                        Convert.ToInt16(row["Gendor"]),
                        Convert.ToString(row["Address"]),
                        Convert.ToString(row["Phone"]),
                        Convert.ToString(row["Email"]),
                        Convert.ToInt32(row["NationalityCountryID"]),
                        Convert.ToString(row["ImagePath"])

                        ));

                }

                return Ok(PeopleList);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        //===========================================================
        //Get person by id
        [HttpGet("FindPersonById", Name = "GetPersonByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<PersonDTO>> GetPersonByID(int id)
        {
            try
            {
                if (id < 1)
                {
                    return BadRequest($"Not accepted id {id}");
                }

                clsPerson person = clsPerson.Find(id);

                if (person == null)
                {
                    return NotFound($"Not found! No student with id = {id}");
                }

                PersonDTO PDTO = new PersonDTO(person.PersonID, person.FirstName,
                    person.SecondName, person.ThirdName, person.LastName, person.NationalNo,
                    person.DateOfBirth, person.Gendor, person.Address, person.Phone, person.Email,
                    person.NationalityCountryID, person.ImagePath);

                return Ok(PDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }




        }



        //============================================================
        //Add new person
        [HttpPost("AddPerson", Name = "AddNewPerson")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonDTO> AddNewPerson(PersonDTO personDTO)
        {
            try
            {
                if (personDTO == null || string.IsNullOrEmpty(personDTO.FirstName) || personDTO.NationalNo == "")
                {
                    return BadRequest("Invalid data, bad request!");
                }



                clsPerson person = new clsPerson(new PersonDTO(personDTO.PersonID,
                                 personDTO.FirstName, personDTO.SecondName, personDTO.ThirdName,
                                 personDTO.LastName, personDTO.NationalNo, personDTO.DateOfBirth,
                                 personDTO.Gender, personDTO.Address, personDTO.Phone,
                                 personDTO.Email, personDTO.NationalityCountryID, personDTO.ImagePath));


                if (person.Save())
                {
                    personDTO.PersonID = person.PersonID;

                    return CreatedAtRoute("GetPersonByID", new { id = personDTO.PersonID }, personDTO);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ann error occurred during adding new person!");
            }

        }



        //============================================================
        //Update person
        [HttpPut("UpdatePerson", Name = "UpdatePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonDTO> UpdatePerson(int personId, PersonDTO updatedPerson)
        {
            if (personId < 1)
                return BadRequest($"Bad request with id {personId}");

            clsPerson Person = clsPerson.Find(personId);
            if (Person == null)
            {
                return NotFound($"No person with id {personId}");
            }

            Person.PersonID = updatedPerson.PersonID;
            Person.NationalNo = updatedPerson.NationalNo;
            Person.FirstName = updatedPerson.FirstName;
            Person.SecondName = updatedPerson.SecondName;
            Person.ThirdName = updatedPerson.ThirdName;
            Person.LastName = updatedPerson.LastName;
            Person.DateOfBirth = updatedPerson.DateOfBirth;
            Person.Phone = updatedPerson.Phone;
            Person.Email = updatedPerson.Email;
            Person.NationalityCountryID = updatedPerson.NationalityCountryID;
            Person.ImagePath = updatedPerson.ImagePath;

            if (Person.Save())
            {
                return Ok(updatedPerson);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }



        //============================================================
        //Delete person
        [HttpDelete("DeletePerson", Name = "DeletePerson")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeletePerson(int personId)
        {
            try
            {
                if (personId < 1)
                    return BadRequest($"Bad request with id {personId}");

                clsPerson Person = clsPerson.Find(personId);
                if (Person == null)
                    return NotFound($"No person with id {personId}");

                if (clsPerson.DeletePerson(personId))
                    return Ok($"Person with id [{personId}] has been deleted successfully");
                else
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during delete person!");


            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during delete person!");
            }
        }
    }



    [Route("api/User")]
    [ApiController]
    public class UserController:ControllerBase
    {
        //============================================================
        //Get all users
        [HttpGet("AllUsers", Name = "GetAllUsers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<UserDTO>> GetAllUsers()
        {
            try
            {
                var allUsers = clsUser.GetAllUsers();
                if (allUsers.Rows.Count < 1)
                    return NotFound("There are no data!");


                var UsersList = new List<UserDTO>();
                foreach (DataRow row in allUsers.Rows)
                {
                    UsersList.Add(new UserDTO
                        (
                          Convert.ToInt32(row["UserID"]),
                          Convert.ToInt32(row["PersonID"]),
                          Convert.ToString(row["UserName"]),
                           Convert.ToString(row["Password"]),
                          Convert.ToBoolean(row["IsActive"])
                        ));
                }

                return Ok(UsersList);


            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during retive the data.");
            }
        }



        //============================================================
        //Get user by id
        [HttpGet("{id}", Name = "GetUserByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserDTO> GetUserByID(int id)
        {

            try
            {
                if (id < 1)
                    return BadRequest($"Bad request with id {id}");


                clsUser User = clsUser.FindByUserID(id);
                if (User == null)
                    return NotFound($"No user with id {id}");


                UserDTO uerDTO = new UserDTO(User.UserID, User.PersonID, User.UserName,User.Password, User.IsActive);

                return Ok(uerDTO);



            }catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during retriving the data!");
            }



        }



        //============================================================
        //Add new user
        [HttpPost("AddUser", Name = "AddNewUser")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserDTO> AddNewUser(UserDTO userDTO)
        {
            try
            {
                if (userDTO == null || string.IsNullOrEmpty(userDTO.UserName) || userDTO.PersonID < 1)
                    return BadRequest("Bad request,wrong information!");


                clsUser User = new clsUser(new UserDTO(userDTO.UserID, userDTO.PersonID,
                                                userDTO.UserName, userDTO.Password, userDTO.IsActive));

                if (User.Save())
                {
                    userDTO.UserID = User.UserID;

                    return CreatedAtRoute("GetUserByID", new { id = userDTO.UserID }, userDTO);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during adding");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }




        //============================================================
        //Update User
        [HttpPut("UpdateUser", Name = "UpdateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<UserDTO>UpdateUser(int id, UserDTO updatedUser)
        {
            try
            {
                if (id < 1)
                    return BadRequest($"Bad request with id {id}");


                if (updatedUser == null || string.IsNullOrEmpty(updatedUser.UserName) || updatedUser.PersonID == 0)
                    return BadRequest($"Bad request with id {id}");


                clsUser User = clsUser.FindByUserID(id);
                if (User == null)
                    return NotFound($"No user with id {id}");


                User.UserID = updatedUser.UserID;
                User.PersonID = updatedUser.PersonID;
                User.UserName = updatedUser.UserName;
                User.Password = updatedUser.Password;
                User.IsActive = updatedUser.IsActive;

                if(User.Save())
                {
                    return Ok(updatedUser);
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,"An error occurred during updating data");
                }


            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }



        //============================================================
        //Delete user
        [HttpDelete("DeleteUser", Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                if (id < 1)
                    return BadRequest($"Bad request with id {id}");


                clsUser User = clsUser.FindByUserID(id);
                if (User == null)
                    return NotFound($"No user with id {id}");


                if (clsUser.DeleteUser(id))
                {
                    return Ok($"User with id = {id} has been deleted successfully");
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during the deleting");
                }

            }catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }



    }



    [Route("api/License")]
    [ApiController]
    public class LicenseController : ControllerBase
    {


        //============================================================
        //Get all licenses
        [HttpGet("AllLicenses", Name = "GetAllLicenses")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<LicenseDTO>> GetAllLicenses()
        {

            try
            {
                var allLicenses = clsLicense.GetAllLicenses();
                if (allLicenses.Rows.Count < 1)
                    return NotFound("Not found!, No data fount!");


                var LicensesList = new List<LicenseDTO>();
                foreach (DataRow row in allLicenses.Rows)
                {
                    LicensesList.Add(new LicenseDTO
                        (
                           Convert.ToInt32(row["LicenseID"]),
                           Convert.ToInt32(row["ApplicationID"]),
                           Convert.ToInt32(row["DriverID"]),
                           Convert.ToInt32(row["LicenseClass"]),
                           Convert.ToDateTime(row["IssueDate"]),
                           Convert.ToDateTime(row["ExpirationDate"]),
                           Convert.ToString(row["Notes"]),
                           Convert.ToSingle(row["PaidFees"]),
                           Convert.ToBoolean(row["IsActive"]),
                           Convert.ToByte(row["IssueReason"]),
                           Convert.ToInt32(row["CreatedByUserID"])

                        ));
                }

                return Ok(LicensesList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred ");
            }

        }



        //============================================================
        //Get licenes by id
        [HttpGet("LicenseById", Name = "GetLicenseByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<LicenseDTO> GetLicenseByID(int licenseID)
        {
            try
            {
                if (licenseID < 1)
                    return BadRequest($"Bad request with id {licenseID}");


                clsLicense License = clsLicense.Find(licenseID);
                if (License == null)
                {
                    return NotFound($"Not found! No license with id {licenseID}");
                }


                LicenseDTO LDTO = new LicenseDTO(License.LicenseID, License.ApplicationID,
                License.DriverID, License.LicenseClass, License.IssueDate, License.ExpirationDate,
                License.Notes, License.PaidFees, License.IsActive,Convert.ToByte(License.IssueReason),
                License.CreatedByUserID);

                return Ok(LDTO);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,ex.Message);
            }

        }





        //============================================================
        //Add new License
        [HttpPost("AddLicense", Name = "AddNewLicense")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<LicenseDTO>AddNewLicense(LicenseDTO newLicenseDTO)
        {
            if (newLicenseDTO == null || newLicenseDTO.LicenseID < 1 || newLicenseDTO.ApplicationID < 1 || newLicenseDTO.DriverID < 1 || newLicenseDTO.CreatedByUserID < 1 || newLicenseDTO.LicenseClass < 1 || newLicenseDTO.IssueReason < 0)
                return BadRequest("Bad request, invalid information!");


            clsLicense License = new clsLicense();

            License.LicenseID = newLicenseDTO.LicenseID;
            License.ApplicationID = newLicenseDTO.ApplicationID;
            License.DriverID = newLicenseDTO.DriverID;
            License.LicenseClass = newLicenseDTO.LicenseClass;
            License.IssueDate = newLicenseDTO.IssueDate;
            License.ExpirationDate = newLicenseDTO.ExpirationDate;
            License.Notes = newLicenseDTO.Notes;
            License.PaidFees = newLicenseDTO.PaidFees;
            License.IsActive = newLicenseDTO.IsActive;
            License.IssueReason = (enIssueReason)newLicenseDTO.IssueReason;
            License.CreatedByUserID = newLicenseDTO.CreatedByUserID;

            if(License.Save())
            {
                newLicenseDTO.LicenseID = License.LicenseID;

                return CreatedAtRoute("GetLicenseByID", new { id = newLicenseDTO.LicenseID }, newLicenseDTO);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during the adding");
            }

        }



    }

    [Route("api/Application")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {

        [HttpGet("ApplicationByID", Name = "GetApplicationByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<ApplicationDTO>> GetApplicationByID(int ApplicationID)
        {
            if (ApplicationID < 1)
               return BadRequest("Invalid Application ID");


            var application = clsApplication.FindBaseApplication(ApplicationID);

            if (application == null)
                return NotFound($"there is no Application with ID {ApplicationID}");


            ApplicationDTO ADTO = new ApplicationDTO(application.ApplicationID, application.ApplicantPersonID, application.ApplicationDate, application.ApplicationTypeID, (byte)application.ApplicationStatus,
                application.LastStatusDate, application.PaidFees, application.CreatedByUserID);

             return Ok(ADTO);
        }

        [HttpGet("GetAll", Name = "GetAllApplications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<ApplicationDTO>> GetAllApplications()
        {
           
            var applications = clsApplication.GetAllApplications();
            if (applications == null)
                return NotFound("There is no Applications");


            var ApplicationList = new List<ApplicationDTO>();
            foreach (DataRow row in applications.Rows)
            {
                ApplicationList.Add(new ApplicationDTO
                    (
                         Convert.ToInt32(row["ApplicationID"]),
                         Convert.ToInt32(row["ApplicantPersonID"]),
                         Convert.ToDateTime(row["ApplicationDate"]),
                         Convert.ToInt32(row["ApplicationTypeID"]),
                         Convert.ToByte(row["ApplicationStatus"]),
                         Convert.ToDateTime(row["LastStatusDate"]),
                         Convert.ToInt32(row["PaidFees"]),
                         Convert.ToInt32(row["CreatedByUserID"])

                    ));
            }

            return Ok(ApplicationList);
        }


        [HttpPost("AddApplication", Name = "AddNewApplication")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<ApplicationDTO>> AddNewApplication(ApplicationDTO newapplication)
        {
            if(newapplication == null || newapplication.ApplicantPersonID < 1 || newapplication.ApplicationStatus < 1 || newapplication.ApplicationStatus > 3 || newapplication.ApplicationTypeID < 1 || newapplication.ApplicationTypeID > 7 || newapplication.CreatedByUserID < 1)
            {
                return BadRequest("Invalid Numbers");
            }

            clsApplication application = new clsApplication();

            application.ApplicantPersonID = newapplication.ApplicantPersonID;
            application.ApplicationDate = newapplication.ApplicationDate;
            application.ApplicationTypeID = newapplication.ApplicationTypeID;
            application.ApplicationStatus = (enApplicationStatus)newapplication.ApplicationStatus;
            application.LastStatusDate = newapplication.LastStatusDate;
            application.PaidFees = newapplication.PaidFees;
            application.CreatedByUserID = newapplication.CreatedByUserID;

            if(application.Save())
            {
                newapplication.ApplicationID = application.ApplicationID;

                return CreatedAtRoute("GetLicenseByID", new { id = newapplication.ApplicationID }, newapplication);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during the adding");

            }
        }


        [HttpPut("UpdateApplication", Name = "UpdateApplication")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<ApplicationDTO>> UpdateApplication(int id, ApplicationDTO UpdatedDTO)
        {
            if (id < 1)
                return BadRequest("Invalid ID");

            if (UpdatedDTO == null || UpdatedDTO.ApplicantPersonID < 1 || UpdatedDTO.ApplicationStatus < 1 || UpdatedDTO.ApplicationStatus > 3 || UpdatedDTO.ApplicationTypeID < 1 || UpdatedDTO.ApplicationTypeID > 7 || UpdatedDTO.CreatedByUserID < 1)
            {
                return BadRequest("Invalid Numbers");
            }

            clsApplication application = clsApplication.FindBaseApplication(id);

            if(application == null)
            {
                return NotFound("Application not found");
            }

            application.ApplicantPersonID = UpdatedDTO.ApplicantPersonID;
            application.ApplicationDate = UpdatedDTO.ApplicationDate;
            application.ApplicationTypeID = UpdatedDTO.ApplicationTypeID;
            application.ApplicationStatus = (enApplicationStatus)UpdatedDTO.ApplicationStatus;
            application.LastStatusDate = UpdatedDTO.LastStatusDate;
            application.PaidFees = UpdatedDTO.PaidFees;
            application.CreatedByUserID = UpdatedDTO.CreatedByUserID;

            if (application.Save())
            {
                return Ok(UpdatedDTO);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during the adding");
            }
        }

        [HttpDelete("DeleteApplication", Name = "DeleteApplication")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteApplication(int id)
        {
            if (id < 1)
                return BadRequest("Invalid Application ID");

            clsApplication application = clsApplication.FindBaseApplication(id);

            if (application == null)
            {
                return NotFound();
            }

            if (application.Delete())
            {
                return Ok($"Application with ID {id} deleted Successfully");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred during the adding");
            }

        }
    }
    
}
