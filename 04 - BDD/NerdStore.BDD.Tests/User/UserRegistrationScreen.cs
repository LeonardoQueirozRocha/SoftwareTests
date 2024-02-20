using NerdStore.BDD.Tests.Configurations;

namespace NerdStore.BDD.Tests.User;

public class UserRegistrationScreen : UserBaseScreen
{
    private const string InputEmailId = "Input_Email";
    private const string InputPasswordId = "Input_Password";
    private const string InputConfirmPasswordId = "Input_ConfirmPassword";

    public UserRegistrationScreen(SeleniumHelper helper) : base(helper) { }

    public void ClickOnRegistrationLink()
    {
        Helper.ClickLinkByText("Register");
    }

    public void FillRegisterForm(User user)
    {
        Helper.FillTextBoxById(InputEmailId, user.Email);
        Helper.FillTextBoxById(InputPasswordId, user.Password);
        Helper.FillTextBoxById(InputConfirmPasswordId, user.Password);
    }

    public bool ValidateFillRegisterForm(User user)
    {
        if (Helper.GetTextBoxValuebyId(InputEmailId) != user.Email || 
            Helper.GetTextBoxValuebyId(InputPasswordId) != user.Password || 
            Helper.GetTextBoxValuebyId(InputConfirmPasswordId) != user.Password) 
            return false;

        return true;
    }

    public void ClickOnRegisterButton()
    {
        Helper.GetElementByXPath("/html/body/div/main/div/div[1]/form/button").Click();
    }
}