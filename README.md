# DesigoClimatixApi

A C# library (.NET Standard 2.0) designed for communication with **Siemens Desigo CC** (Climatix) controllers with their JSON API interface. This library simplifies reading and writing point values using Base64 identifiers.

## 📥 Installation

Currently, you can include this library in your project by adding a reference to the `DesigoClimatixApi.dll` found in the [Releases](https://github.com/markroskaric/Desigo-Climatix-API/releases) tab.

## 📂 Desigo CC Integration

To use this library within Desigo CC scripts:

1. Copy `DesigoClimatixApi.dll` to the following folder: `\GMSMainProject\scriptingLibraries` in Desigo CC project

2. In your Desigo CC script, import the namespace and initialize the connection:

```javascript
// Import the namespace from your DLL
var climatix = importNamespace("DesigoClimatixApi");

// Initialize the connection
// Note: Ensure the DLL has a public constructor available
// Initialize the connection
var con = new climatix.Connection(
  "admin", // username
  "password123", // password
  "http://10.201.180.16", // controller IP/URL
  "7659" // PIN code
);

// Read value
// Parameter: (base64Id)
var resultRead = con.ReadValue("AiN4e05FAAE=");
console("Current Value: " + resultRead);

// Write value
// Parameters: (base64Id, value)
// "AiN4e05FAAE=" is the Point ID
// "1" is the new value you want to set
var resultWrite = con.WriteValue("AiN4e05FAAE=", "1");
console("Write Result: " + resultWrite);
```

## 🔧 Advanced Features & Debugging

The DesigoClimatixApi includes an **Advanced Developer Mode**.

### **The Developer Mode (devMode)**

By default, the library operates in **Standard Mode**, returning simple strings (the value or the error). When `devMode` is enabled, the library returns a **Full Response Object**.

### **Developer Object Schema**

When `devMode` is active, the returned object contains the following properties:

```javascript
{
  "IsSuccess": true,       // Boolean: True if HTTP status is 200
  "StatusCode": 200,       // Integer: Raw HTTP status (e.g., 401, 404)
  "Content": "{...}",      // String: The raw JSON string from the controller
  "ErrorMessage": "",      // String: Internal error details
  "PointId": "AiN4e0..."   // String: The Base64 ID targeted in the request
}
```

```javascript
// Import the namespace from your DLL
var climatix = importNamespace("DesigoClimatixApi");

// Initialize the connection
// Note: Ensure the DLL has a public constructor available
// Initialize the connection
var con = new climatix.Connection(
  "admin", // username
  "password123", // password
  "http://10.201.180.16", // controller IP/URL
  "7659", // PIN code
  true //dev mode default false
);

// Read value
// Parameter: (base64Id)
var resultRead = con.ReadValue("AiN4e05FAAE=");
console("Current Value: " + resultRead);

// Write value
// Parameters: (base64Id, value)
// "AiN4e05FAAE=" is the Point ID
// "1" is the new value you want to set
var resultWrite = con.WriteValue("AiN4e05FAAE=", "1");
console("Write Result: " + resultWrite);
```
