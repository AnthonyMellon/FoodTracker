### Food Item Validation
`db.runCommand( { collMod: "FoodItems", validator: {$jsonSchema: {bsonType: "object", required: ["Name", "Calories", "Protein", "Carbs", "Fat"], properties: {Name: {bsonType: "string"}, Calories: {bsonType: "double"}, Protein: {bsonType: "double"}, Carbs: {bsonType: "double"}, Fat: {bsonType: "double"}}}}})`
