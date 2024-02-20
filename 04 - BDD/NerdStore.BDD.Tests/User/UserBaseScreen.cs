using NerdStore.BDD.Tests.Configurations;

namespace NerdStore.BDD.Tests.User;

public abstract class UserBaseScreen : PageObjectModel
{
    protected UserBaseScreen(SeleniumHelper helper) : base(helper) { }

    public void AccessStoreWebSite()
    {
        Helper.GoToUrl(Helper.Configuration.DomainUrl);
    }

    public bool ValidateLoggedUserGreeting(User user)
    {
        return Helper.GetTextElementById("user-greeting").Contains(user.Email);
    }
}
