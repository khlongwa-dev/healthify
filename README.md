# 🏥 Healthify — Find & Book Doctors Online

> A full-stack healthcare appointment booking system with real-time backend functionality, role-based login, and fully connected UI. Built using **React**, **.NET Core Web API**, and **SQL Server** — ready for deployment and real-world use.

---

## 🚀 Project Overview

**Healthify** empowers users to find doctors by specialization, book appointments, and manage their profiles. Doctors and admins access a role-based dashboard to handle appointments and manage system data.

This project showcases:
- Role-based authentication (Admin, Doctor, User)
- Real-time data from backend API (no mock data)
- Full CRUD where applicable (Book, Update, Cancel)
- Scalable backend architecture using .NET Core Web API
- Responsive UI with clean UX built in React + Vite

---

## 🧑‍💻 Roles & Features

### 👥 User
- 🔍 Find doctors by specialization
- 📆 Book an appointment (date & time)
- 👤 Manage profile (edit & save)
- 📋 View booked appointments
- ❌ Cancel appointment
- 💰 UI-ready button for payment (to be integrated)

### 🧑‍⚕️ Doctor
- 📄 View appointments made to them
- ✅ Mark appointments as completed
- ❌ Cancel appointments
- 🧑‍💼 Edit and manage own profile

### 🛡️ Admin
- 📊 View dashboard stats
- 🧑‍⚕️ Add new doctors
- 🗃️ View all registered doctors
- 🔄 Toggle doctor availability
- ❌ Cancel user appointments

---

## 🧱 Tech Stack

| Layer       | Technology             |
|-------------|------------------------|
| Frontend    | React (Vite, Tailwind) |
| Backend     | .NET Core Web API      |
| Database    | SQL Server (EF Core)   |
| Auth        | Role-based login       |
| API         | RESTful architecture   |

---

## 📁 Project Structure
```bash
/healthify
├── /admin         # React app
│   └── src
│   │  ├── assets
│   │  ├── components
│   │  ├── context
│   │  ├── pages
│   │  ├── App.jsx
│   │  ├── index.css
│   │  └── main.jsx
│   ├── .env
│   ├── .gitignore
│   ├── eslint.config.js
│   ├── index.html
│   ├── package-lock.json
│   ├── package.json
│   └── vite.config.js
├── /frontend          # React app
│   └── src
│   │  ├── assets
│   │  ├── components
│   │  ├── context
│   │  ├── pages
│   │  ├── App.jsx
│   │  ├── index.css
│   │  └── main.jsx
│   ├── .env
│   ├── .gitignore
│   ├── eslint.config.js
│   ├── index.html
│   ├── package-lock.json
│   ├── package.json
│   └── vite.config.js
├── /backend          # .NET Core Web API
│   ├── Configurations
│   ├── Controllers
│   ├── Data
│   ├── Dependencies
│   ├── DTOs
│   ├── Helpers
│   ├── Models
│   ├── Services
│   └── Program.cs
└── README.md
```
## 🔐 Authentication Flow

- All roles (admin, doctor, user) log in via `POST /api/auth/login`.
- Users can register via `POST /api/auth/register`.
- Backend validates credentials and returns a token for authorization (JWT or role-based logic).
- Navigation and access are conditionally rendered in the frontend based on role.

---

## 📡 API Endpoints

### `HealthifyController`
- `GET /api/healthify/doctors-list` — fetch all doctors

### `AdminController`
- `GET /api/admin/dashboard`
- `GET /api/admin/doctors-list`
- `POST /api/admin/add-doctor` — add new doctor
- `PUT /api/admin/change-availability`
- `PUT /api/admin/cancel-appointment`

### `DoctorController`
- `GET /api/doctor/dashboard`
- `PUT /api/doctor/cancel-appointment`
- `PUT /api/doctor/complete-appointment`
- `GET /api/doctor/get-profile`
- `PUT /api/doctor/update-profile`

### `UserController`
- `POST /api/user/book-appointment`
- `GET /api/user/get-appointments`
- `PUT /api/user/cancel-appointments`
- `GET /api/user/get-profile`
- `PUT /api/user/update-profile`

### `AuthenticationController`
- `POST /api/auth/login`
- `POST /api/auth/register`

---

## 📦 Super Admin Setup

To create an initial admin:

```bash
dotnet run createsuperuser└── Program.cs
```
## 🛠 Getting Started
### 🖥️ Frontend
```bash
cd frontend /cd admin
npm install
npm run dev
```
## 🔙 Backend
```bash
cd backend
dotnet restore
dotnet run
```
> Make sure SQL Server is running and the appsettings.json connection string is configured properly.

## 📸 Screenshots

### 🏠 Home Page  
Landing page with search functionality and quick access to key features.  
![Home Page](https://res.cloudinary.com/dxs6tromb/image/upload/v1747491976/Screenshot_From_2025-05-17_16-19-22_vaukxi.png)

### 🧑‍⚕️ Doctor List & Specialization Filter  
Shows filtered doctors based on selected specialization with availability indicators.  
![Doctor List](https://res.cloudinary.com/dxs6tromb/image/upload/v1747491958/Screenshot_From_2025-05-17_16-19-49_xuavu9.png)

### 📅 Appointment Booking  
Users can book appointments by selecting time and date.  
![Appointment Booking](https://res.cloudinary.com/dxs6tromb/image/upload/v1747491964/Screenshot_From_2025-05-17_16-20-07_vbxoim.png)

### 📁 User Appointments Page  
Displays all booked appointments with cancel and pay actions.  
![User Appointments](https://res.cloudinary.com/dxs6tromb/image/upload/v1747491964/Screenshot_From_2025-05-17_16-20-33_klbump.png)

### 👤 User Profile Page  
Edit and update user details with state-based form functionality.  
![User Profile](https://res.cloudinary.com/dxs6tromb/image/upload/v1747491951/Screenshot_From_2025-05-17_16-20-51_k3weu2.png)

### 📊 Admin Dashboard  
Role-based admin panel with doctor and appointment management tools.  
![Admin Dashboard](https://res.cloudinary.com/dxs6tromb/image/upload/v1747491967/Screenshot_From_2025-05-17_16-21-58_fesvyh.png)

### 🩺 Doctor Dashboard  
Doctors can manage appointments, mark them complete or cancelled, and edit their profile.  
![Doctor Dashboard](https://res.cloudinary.com/dxs6tromb/image/upload/v1747491949/Screenshot_From_2025-05-17_16-21-08_nqyzmv.png)

### 🔐 Login / Register Page  
Simple role-based login and registration for users, doctors, and admin.  
![Login Page](https://res.cloudinary.com/dxs6tromb/image/upload/v1747491971/Screenshot_From_2025-05-17_16-21-24_jeftup.png)


## 👨‍💼 About the Developer
> I built Healthify as a production-grade portfolio project to demonstrate my full-stack development capabilities. It reflects best practices, scalable design, and an eye toward real-world applicability.

If you're hiring or collaborating on healthcare or booking platforms — let’s talk.

## 📬 Contact
- **Project Owner:** Kusasalakhe Hlongwa  
- **Email:** ayandahlongwa21@gmail.com
- **GitHub:** [khlongwa-dev](https://github.com/khlongwa-dev)  
- **LinkedIn:** [k-hlongwa](https://linkedin.com/in/k-hlongwa)


