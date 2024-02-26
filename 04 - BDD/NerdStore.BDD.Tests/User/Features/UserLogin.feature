Feature: User - login
	As a user
	I want to login
	So that I can access the other features

Scenario: Login successfully
Given that the visitor is accessing the store's website
When he clicks login
And fill in the login form data
	| Datas                 |
	| E-mail                |
	| Password              |
And click on the login button
Then it will be redirected to the storefront
And a greeting with his email will be displayed in the top menu