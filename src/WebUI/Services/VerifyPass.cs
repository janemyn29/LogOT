namespace WebUI.Services;

public class VerifyPass
{
    public VerifyPass()
    {
        
    }
    public bool validatePass(string pass)
    {
        if (pass == null)
        {
            return false;
        }
        //check length 
        if (pass.Length < 6)
        {
            return false;
        }
        if (!pass.Any(char.IsUpper))
        {
            return false;

        }
        if (!pass.Any(char.IsLower))
        {
            return false;

        }
        if (pass.Contains(" "))
        {
            return false;

        }
        string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
        char[] specialChArray = specialCh.ToCharArray();
        foreach (char ch in specialChArray)
        {
            if (pass.Contains(ch))
            {
                return true;
            }
        }
        return false;

    }


    public bool validateUsername(string username)
    {
        if (username == null)
        {
            return false;
        }
        if (username.Contains(" "))
        {
            return false;

        }
        string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
        char[] specialChArray = specialCh.ToCharArray();
        foreach (char ch in specialChArray)
        {
            if (username.Contains(ch))
            {
                return false;
            }
        }
        return true;

    }
}
