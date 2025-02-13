# 🛒 ShoppingCartApp

ShoppingCartApp is a CRUD-based shopping cart web application that allows users to browse products, add them to a shopping cart, and save items to a wishlist. Admins can manage the product catalog by adding, editing, and deleting products, ensuring that the store remains up to date.

## 🚀 Features

### 🛍️ Main Functionalities
- **Browse Products**: Users can explore a collection of products with descriptions and pricing.
- **Shopping Cart Management**: Add, update quantities, or remove items from the cart.
- **Wishlist Feature**: Save favorite products for later viewing.
- **User Authentication & Management**: Secure login and registration for both customers and administrators.
- **Product Management (Admin Panel)**:
  - **Create**: Admins can add new products with images, descriptions, and pricing.
  - **Update**: Modify existing product details, including price, name, and description.
  - **Delete**: Remove outdated or unavailable products from the catalog.
- **Google OAuth Integration**: Allows users to log in quickly using their Google accounts.

## 🛠️ Technologies Used

### 🔹 Backend:
- ASP.NET Core MVC
- Entity Framework Core (EF Core) with SQLite
- Google OAuth Authentication

### 🔹 Frontend:
- Bootstrap 5
- HTML, CSS, JavaScript (with jQuery)

### 🔹 Tools & Services:
- Git & GitHub for version control
- Visual Studio Code / Visual Studio

## 🎥 Demo Video

👉 [Watch the ShoppingCartApp Demo](https://www.canva.com/design/DAGe9BKpv70/5uuuMtxH9pOh_SYtU3d9QQ/edit?utm_content=DAGe9BKpv70&utm_campaign=designshare&utm_medium=link2&utm_source=sharebutton)


## 📂 Installation & Setup

### 🔹 Prerequisites
Before starting, ensure you have installed:
- [.NET SDK](https://dotnet.microsoft.com/en-us/download)
- [SQLite](https://www.sqlite.org/download.html)
- [Git](https://git-scm.com/downloads)

### 🔹 Clone the Repository
```sh
git clone https://github.com/tavarezyira/ShoppingCart.git
cd ShoppingCartApp
```

### 🔹 Configure Environment Variables
Before running the application, set up Google OAuth credentials:
```sh
set GOOGLE_CLIENT_ID=your_client_id
set GOOGLE_CLIENT_SECRET=your_client_secret
```

### 🔹 Restore Dependencies
```sh
dotnet restore
```

### 🔹 Apply Migrations
```sh
dotnet ef database update
```

### 🔹 Run the Application
```sh
dotnet run
```

Then, open your browser at `http://localhost:5000` or the assigned port.

## 🛠️ Maintenance & Updates
To keep the project updated:
```sh
git pull origin master
```

If you made changes, commit and push them to GitHub:
```sh
git add .
git commit -m "Description of the change"
git push origin master
```

## ✨ Contribution
If you want to contribute to the project:
1. **Fork** the repository.
2. **Create a branch** for your new feature: `git checkout -b feature-new`.
3. **Make your changes and commit them**: `git commit -m "Description of the change"`.
4. **Push the branch to GitHub**: `git push origin feature-new`.
5. **Create a Pull Request** on GitHub.
