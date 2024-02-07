Feature: User - Registration
	As a store visitor
	I want to register as a user
	So I can make porchases in the store

Scenario: Successful user registration
Given that the visitor is accessing the store's website
When he clicks register
And fill in the form data
	| Datas                 |
	| E-mail                |
	| Password              |
	| Password confirmation |
And click on the register button
Then it will be redirected to the storefront
And a greeting with his email will be displayed in the top menu

Scenario: Registration with password without capital letters
Given that the visitor is accessing the store's website
When he clicks register
And fill in the form data with a password without capital letters
	| Datas                 |
	| E-mail                |
	| Password              |
	| Password confirmation |
And click on the register button
Then he will recieve an error message that the password must contain an uppercase letter

Scenario: Registration with a password without special character
Given that the visitor is accessing the store's website
When he clicks register
And fill in the form data with a password without special character
	| Datas                 |
	| E-mail                |
	| Password              |
	| Password confirmation |
And click on the register button
Then he will recieve an error message that the password must contain a special caracter