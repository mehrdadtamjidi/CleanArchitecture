using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CleanArchitecture.Application.Responses
{
    public enum ApiResultStatusCode
    {
        [Display(Name = "Operation completed successfully")]
        Success = 0,

        [Display(Name = "An error occurred on the server")]
        ServerError = 1,

        [Display(Name = "Submitted parameters are invalid")]
        BadRequest = 2,

        [Display(Name = "Resource not found")]
        NotFound = 3,

        [Display(Name = "The list is empty")]
        ListEmpty = 4,

        [Display(Name = "A logical error occurred during processing")]
        LogicError = 5,

        [Display(Name = "Authentication error")]
        UnAuthorized = 6,
    }
}
