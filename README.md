# GoMartApplication
# 🛒 Go Mart Application

A **Windows Forms (.NET) based supermarket management system** developed using **C# and SQL Server**.
This application helps manage products, categories, sellers, and sales transactions in a simple desktop interface.

---

## 📌 Features

* 🔐 Admin and Seller Login System
* 📦 Product Management (Add / Update / Delete)
* 📂 Category Management
* 🧾 Sales / Billing System
* 📊 Sales Reports
* 👤 Seller Management
* 🔎 Product Search by Category
* 💾 SQL Server Database Integration

---

## 🖥️ Technologies Used

* C#
* .NET Windows Forms
* Microsoft SQL Server
* ADO.NET
* Visual Studio

---

## 📂 Project Structure

GoMartApplication
│
├── Forms
│ ├── FormMain.cs
│ ├── FormCategory.cs
│ ├── FormAddProduct.cs
│ ├── FormSelling.cs
│ ├── FormReport.cs
│
├── Database
│ ├── tblCategory
│ ├── tblProduct
│ ├── tblBill
| ├── tblSeller
| ├── tblAdmin
│
└── DBConnect.cs

---

## ⚙️ Installation Steps

1. Clone the repository

```
git clone https://github.com/irohitsaha/gomartapplication.git
```

2. Open the project in **Visual Studio**

3. Restore the database in **SQL Server**

4. Update the database connection string in:

```
DBConnect.cs
```

5. Run the application.

---

## 🗄️ Database

Main tables used:

* `tblCategory`
* `tblProduct`
* `tblBill`
* `tblSeller`
* `tblAdmin`

---

## 👨‍💻 Author

**ROHIT SAHA**

GitHub: https://github.com/irohitsaha

---

## 📄 License

This project is for **educational purposes**.
