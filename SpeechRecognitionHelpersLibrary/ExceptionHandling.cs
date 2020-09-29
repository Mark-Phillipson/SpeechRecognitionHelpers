using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Text;

namespace SpeechRecognitionHelpersLibrary
{
    static public class ExceptionHandling
    {
        static public string GetShortErrorMessage(Exception exception)
        {
            var message = "The system was unable to complete the action as an error occurred\r\n";
            if (exception.InnerException != null)
            {
                message = message + exception.InnerException.ToString().Substring(0, 400) + "...";
            }
            else
            {
                message = exception.Message;
            }
            message = $"{message}{GetEntityValidationErrors((DbEntityValidationException)exception)}";
            return message;
        }
        public static string GetEntityValidationErrors(DbEntityValidationException e)
        {
            var message = "";
            foreach (var eve in e.EntityValidationErrors)
            {
                string name = eve.Entry.Entity.GetType().Name;
                string state = eve.Entry.State.ToString();
                message = $"Entity of type \"{name}\" in state \"{state}\" has the following validation errors:";
                foreach (var ve in eve.ValidationErrors)
                {
                    string propertyName = ve.PropertyName;
                    string errorMessage = ve.ErrorMessage;
                    message = message + $"- Property: \"{propertyName}\", Error: \"{errorMessage}\"";
                }
            }
            return message;
        }
    }
}
