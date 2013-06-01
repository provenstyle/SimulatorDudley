using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueSimulator
{
   public static class Logger
   {
      public static void Error(Exception ex, string message = "")
      {
         if(!string.IsNullOrEmpty(message))
            Console.WriteLine(message);
         Console.WriteLine(ex);
      }
   }
}
