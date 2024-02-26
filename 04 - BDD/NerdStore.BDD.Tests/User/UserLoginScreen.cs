using NerdStore.BDD.Tests.Configurations;

namespace NerdStore.BDD.Tests.User;

public class UserLoginScreen : UserBaseScreen
{
    private const string InputEmailId = "Input_Email";
    private const string InputPasswordId = "Input_Password";

    public UserLoginScreen(SeleniumHelper helper) : base(helper) { }

    public void ClickOnLoginLink()
    {
        Helper.ClickLinkByText("Login");
    }

    public void FillLoginForm(User user)
    {
        Helper.FillTextBoxById(InputEmailId, user.Email);
        Helper.FillTextBoxById(InputPasswordId, user.Password);
    }

    public void ClickOnLoginButton()
    {
        var button = Helper.GetElementByXPath("//*[@id=\"login-submit\"]");
        button.Click();
    }

    public bool ValidateFilledLoginForm(User user)
    {
        if (Helper.GetTextBoxValuebyId(InputEmailId) != user.Email ||
            Helper.GetTextBoxValuebyId(InputPasswordId) != user.Password)
        {
            return false;
        }

        return true;
    }

    public bool Login(User user)
    {
        AccessStoreWebSite();
        ClickOnLoginLink();
        FillLoginForm(user);

        if (!ValidateFilledLoginForm(user)) return false;

        ClickOnLoginButton();

        if (!ValidateLoggedUserGreeting(user)) return false;

        return true;
    }
}