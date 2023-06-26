namespace Tonvo.ViewModels
{
    public class PersonalAccountViewModel : ViewModelBase
    {
        public string userID { get; set; }
        public PersonalAccountViewModel()
        {
            userID = System.Configuration.ConfigurationManager.AppSettings["UserID"];
        }
    }
}
