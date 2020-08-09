using System;
using System.Collections.Generic;
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
            //message = message + GetEntityValidationErrors(exception);
            return message;
        }

    }
}
