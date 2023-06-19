namespace WebUI.Services.Format;

public class FormatMoney:IFormatMoney
{
    /*public string Format(double value)
    {
        var temp = "";
        var length = value.ToString().Length;
        char[] charArray = value.ToString().ToCharArray();
        int check= 1;
        for(var i = length; i > 0; i--) {
            if(check%3 == 0)
            {
                temp = temp + charArray[i]+ ".";
            }
            else
            {
                check++;
                temp = temp + charArray[i];
            }
        }
        //revert
        var 

    }*/



    public string Format(double value)
    {
        string strNumber = value.ToString();

        string reversedStrNumber = ReverseString(strNumber);
        string formattedNumber = InsertDots(reversedStrNumber, 3, '.');

        string finalNumber = ReverseString(formattedNumber);
        return finalNumber;
    }

        public string ReverseString(string str)
    {
        char[] charArray = str.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public string InsertDots(string str, int interval, char dotChar)
    {
        int length = str.Length;
        int dotCount = (int)Math.Ceiling((double)length / interval) - 1;
        char[] dottedArray = new char[length + dotCount];

        int index = 0;
        for (int i = 0; i < length; i++)
        {
            dottedArray[index++] = str[i];
            if ((i + 1) % interval == 0 && i != length - 1)
            {
                dottedArray[index++] = dotChar;
            }
        }

        return new string(dottedArray);
    }
}