using System;
using Ex = System.Exception;

namespace LibNeuroOCR.Exception
{
    public class MooooException : NeuroException
    {
        public MooooException(string Message, Ex e) : base(Message, e)
        {
            //Mooo! This is an easter egg!

 //                   (__)  
 //                   (oo)  
 //            /-------\/   
 //           / |     ||   
 //          *  ||----||   
 //             ^^    ^^    
 //               MOO!       
                
        }
    }
}
