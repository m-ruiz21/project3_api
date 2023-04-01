# Project 2 API Definition

[/orders](#orders)
- [Create Order](#create-order)
    - [Create Order Request](#create-order-request)
    - [Create Order Response](#create-order-response)
- [Get Order](#get-order)
    -[Get Order Request](#get-order-request)
    -[Get Order Response](#get-order-response)
- [Get All Orders](#get-all-orders)
    -[Get All Orders Request](#get-all-orders-request)
    -[Get All Orders Response](#get-all-orders-response)
- [Update Order](#update-order)
    -[Update Order Request](#update-order-request)
    -[Update Order Response](#update-order-response)
- [Delete Order](#delete-order)
    -[Delete Order Request](#delete-order-request)
    -[Update Order Response](#delete-order-response)

[/menu-item](#menu-item)
- [Create Menu Item](#menu-item)
    - [Create Menu Item Request](#create-menu-item-request)
    - [Create Menu Item Response](#create-menu-item-response)
- [Get Menu Item](#get-menu-item)
    -[Get Menu Item Request](#get-menu-item-request)
    -[Get Menu Item Response](#get-menu-item-response)
- [Get All Menu Items](#get-all-menu-items)
    -[Get All Menu Items Request](#get-all-menu-items-request)
    -[Get All Menu Items Response](#get-all-menu-items-response)
- [Update Menu Item](#update-menu-item)
    -[Update Menu Item Request](#update-menu-item-request)
    -[Update Menu Item Response](#update-menu-item-response)
- [Delete Menu Item](#delete-menu-item)
    -[Delete Menu Item Request](#delete-menu-item-request)
    -[Update Menu Item Response](#delete-menu-item-response)

# /orders
## Create Order

### Create Order Request

```js
POST /orders
```

```json
{
    "items": [
        "brown rice",
        "falafel",
        "drink",
        "tomato",
        "tahini"
    ],
    "price" : 8.34
}
```

### Create Order Response

#### Successful Creation
```js
201 Created
```

Returns: created object
Example:
```json
{
    "id": 42312478191,
    "items": [
        "brown rice",
        "falafel",
        "drink",
        "tomato",
        "tahini"
    ],
    "price" : 8.34
}
```

#### Error
Will return error code + message.

Example:
```js
400 Bad Request
```
```json
{
    "error" : "Menu Item with name 'turkey' is not a valid Menu Item"
}
```

## Get Order

### Get Order Request
```js
GET /orders/{id}
```

### Get Order Response

#### Successful Request 
```js
200 Ok 
```

Returns: requested object
Example:
```json
{
    "id": 42312478191,
    "items": [
        "brown rice",
        "falafel",
        "drink",
        "tomato",
        "tahini"
    ],
    "price" : 8.34
}
```

#### Error
Will return error code + message.

Example:
```js
404 Not Found 
```
```json
{
    "error" : "Order with id '69420' not found"
}
```

## Get All Orders

### Get All Orders Request
```js
GET /orders
```

### Get All Orders Response

#### Successful Request 
```js
200 Ok 
```

Returns: all orders as list of order objects 

#### Error
Will return error code.

Example:
```js
500 Internal Server Error 
```

## Update Order

### Update Order Request
```js
PUT /orders/{id}
```
```json
{
    "items": [
        "brown rice",
        "falafel",
        "drink",
        "tomato",
    ],
    "price" : 8.34
}
```

### Update Order Response

#### Successful Update 
```js
200 Ok 
```
Returns: updated object
Example:
```json
{
    "id": 42312478191,
    "items": [
        "brown rice",
        "falafel",
        "drink",
        "tomato",
    ],
    "price" : 8.34
}
```

#### Error
Will return error code + message.

Example:
```js
404 Not Found 
```
```json
{
    "error" : "Order with id '69420' not found"
}
```

## Delete Order

### Delete Order Request
```js
DELETE /orders/{id}
```

### Delete Order Response

#### Successful Deletion 
```js
204 No Content 
```

#### Error
Will return error code + message.

Example:
```js
404 Not Found 
```
```json
{
    "error" : "Order with id '69420' not found"
}
```

# /menu-item
## Create Menu Item 

### Create Menu Item Request

```js
POST /menu-item
```

```json
{
    "name": "tortilla",
    "category": "base",
    "price": 1.00,
    "quantity": 2000,
    "MenuItemCutlery": [
        "plate"
    ]
}
```

### Create Menu Item Response

#### Successful Creation
```js
201 Created
```

Returns: created object
Example:
```json
{
    "id": 1247401,
    "name": "tortilla",
    "category": "base",
    "price": 1.00,
    "quantity": 2000,
    "MenuItemCutlery": [
        "plate"
    ]
}
```

#### Error
Will return error code + message.

Example:
```js
400 Bad Request
```
```json
{
    "error" : "Menu Item with name 'brown rice' already exists"
}
```

## Get Menu Item By Id 

### Get Menu Item Request
```js
GET /menu-item/{id}
```

### Get Menu Item Response

#### Successful Request 
```js
200 Ok 
```

Returns: requested object
Example:
```json
{
    "id": 1247401,
    "name": "tortilla",
    "category": "base",
    "price": 1.00,
    "quantity": 2000,
    "MenuItemCutlery": [
        "plate"
    ]
}
```

#### Error
Will return error code + message.

Example:
```js
404 Not Found 
```
```json
{
    "error" : "Menu Item 'Monster Energy' not found"
}
```

## Get All Menu Items 

### Get All Menu Items Request
```js
GET /menu-item
```

### Get All Menu Items Response

#### Successful Request 
```js
200 Ok 
```

Returns: all menu items by category
Example:
```json
{
    "base": [
        {
            "id": 1247401,
            "name": "tortilla",
            "category": "base",
            "price": 1.00,
            "quantity": 2000,
            "MenuItemCutlery": [
                "plate"
            ]
        },
        {
            "id": 1247402,
            "name": "brown rice",
            "category": "base",
            "price": 0.00,
            "quantity": 1000,
            "MenuItemCutlery": [
                "plate",
                "fork"
            ]
        }
    ],
    "topping": [
        {
            "id": 1247404,
            "name": "tomato",
            "category": "topping",
            "price": 0.00,
            "quantity": 1000,
            "MenuItemCutlery": []
        }
    ]
}
```

#### Error
Will return error code.

Example:
```js
500 Internal Server Error 
```

## Update Menu Item 

### Update Menu Item Request
```js
PUT /menu-item/{name}
```
```json
{
    "id": 1247402,
    "name": "brown rice",
    "category": "base",
    "price": 0.00,
    "quantity": 1001,
    "MenuItemCutlery": [
        "plate",
        "fork"
    ]
}
```

### Update Menu Item Response

#### Successful Update 
```js
200 Ok 
```
Returns: updated object
Example:
```json
{
    "id": 1247402,
    "name": "brown rice",
    "category": "base",
    "price": 0.00,
    "quantity": 1001,
    "MenuItemCutlery": [
        "plate",
        "fork"
    ]
}
```

#### Error
Will return error code + message.

Example:
```js
404 Not Found 
```
```json
{
    "error" : "Menu Item 'burrito' not found"
}
```

## Delete Menu Item 

### Delete Menu Item Request
```js
DELETE /orders/{name}
```

### Delete Menu Item Response

#### Successful Deletion 
```js
204 No Content 
```

#### Error
Will return error code + message.

Example:
```js
404 Not Found 
```
```json
{
    "error" : "Menu Item 'cow' not found"
}
```