using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpriteClasses
{
    public class StringUtilities
    {
        public static string NextImageNameNoExtension(string filename, int newNumber)
        {
            string newName = "";
            newName = filename.Substring(0, filename.Length - 2);
            if (newNumber < 10)
            {
                newName += "0";
            }
            return newName + newNumber;
        }

        public static string NextImageNameWithExtension(string filename, int newNumber)
        {
            string newName = "";
            //where is last period (before file extension)
            int posn = filename.LastIndexOf('.');
            if (posn >= 0)
            {
                if (newNumber >= 10 || newNumber == 0)
                {
                    //get the filename up to but not including the character before the last period
                    newName = filename.Substring(0, posn - 2);
                    if (newNumber == 0)
                    {
                        newName += "0";
                    }
                }
                else
                {
                    newName = filename.Substring(0, posn - 1);
                }
            }
            //concatenate number
            newName += newNumber;
            //concatenate file extension
            newName += filename.Substring(posn);
            return newName;
        }
    }
}
