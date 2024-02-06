Feature: Order - Add Item to Cart
	As a user
	I want to add an item to the cart
	So I can buy it later

Scenario: Successfully add item to a new order
Given the user is logged in
And that a product is in the window
And be available in stock
And do not have any products added to the cart
When the user adds a unit to the cart
Then the user will be redirected to the purchase summary
And the total order value will be exactly the value of the added item

Scenario: Add items above the limit
Given the user is logged in
And that a product is in the window
And be available in stock
When the user adds an item above the maximum quantity allowed
Then receive an error message stating that the limit quantity has been exceeded

Scenario: Add existing item to cart
Given the user is logged in
And that a product is in the window
And be available in stock
And the same product has already been added to the cart previously
When the user adds a unit to the cart
Then the user will be redirected to the purchase summary
And the quantity of items for that product will have been increased by one more unit
And the total order value will be the multiplication of the quantity of items by the unit value

Scenario: Add existing item where sum exceeds maximum limit
Given the user is logged in
And that a product is in the window
And be available in stock
And the same product has already been added to the cart previously
When the user adds the maximum quantity allowed to the cart
Then receive an error message stating that the limit quantity has been exceeded
