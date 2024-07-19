### Food Item Validation
`db.runCommand( { collMod: "FoodItems", validator: {$jsonSchema: {bsonType: "object", required: ["Name", "Calories", "Protein", "Carbs", "Fat"], properties: {Name: {bsonType: "string"}, Calories: {bsonType: "double"}, Protein: {bsonType: "double"}, Carbs: {bsonType: "double"}, Fat: {bsonType: "double"}}}}})`

### Meal Validation
`db.runCommand( { collMod: "Meals", validator: {$jsonSchema: {bsonType: "object", required: ["Name", "FoodItems"], properties: {Name: {bsonType: "string"}, FoodItems: {bsonType: "array"}}}}})`

### Day Validation
`db.runCommand( { collMod: "Days", validator: {$jsonSchema: {bsonType: "object", required: ["Date", "FoodItems", "Meals"], properties: {Name: {bsonType: "string"}, FoodItems: {bsonType: "array"}, Meals: {bsonType: "array"}}}}})`
