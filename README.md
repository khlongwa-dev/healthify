# Healthify 🏥
_A Doctor Booking Web Application_
> **Note:**  
> This project is currently under active development.  
> The documentation (README) is subject to change as new features are added or the planning is modified.
> Most of the changes are expected on the backend development stage, currently I am working on frontend development

---

## Table of Contents
- [About](#about)
- [Features](#features)
- [Tech Stack](#tech-stack)
- [Installation](#installation)
- [Usage](#usage)
- [Screenshots](#screenshots)
- [API Endpoints](#api-endpoints)
- [Contributing](#contributing)
- [Contact](#contact)

---

## About
Healthify is a web application designed to simplify the process of booking doctor appointments online. Users can find doctors by navigating to specialty menu, check availability and make instant bookings. Doctors can manage their availability and view patient appointments through the platform.

---

## Features
- User Registration and Login
- Doctor Profile Management
- User Profile Management
- Find Doctors by Specialty
- Book, Reschedule, and Cancel Appointments
- Doctor Availability Calendar
- Admin Dashboard

---

## Tech Stack
**Frontend:**  
- React.js
- Tailwind CSS

**Backend:**  
- .NET

**Database:**  
- Microsoft SQL Server  

**Others:**  
- JWT Authentication / OAuth
- Cloudinary

---

## Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/khlongwa-dev/healthify.git
   cd healthify
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm run dev
   ```

---

## Usage
- Open your browser and navigate to `http://localhost:5173`
- Register as a user or doctor (i am looking into admin authentication also, the admin will handle adding doctors)
- Search for doctors and book appointments
- Manage your profile and bookings

---

## Screenshots
*(Insert screenshots here to show off your app UI)*  
Example:
> ![Landing Page](screenshots/landing-page.png)
> ![Doctor Search](screenshots/search-page.png)

---

## API Endpoints
*(These are to be edited and replaced with real endpoints as soon as backend hits development)*  
Example:

| Method | Endpoint | Description |
|:------:|:--------:|:-----------:|
| POST | `/api/auth/register` | Register a new user |
| POST | `/api/auth/login` | Login user |
| GET | `/api/doctors` | List all doctors |
| POST | `/api/appointments` | Book an appointment |

---

## Contributing
Contributions are welcome!  
Please fork the repository and create a pull request for review.  
Make sure your code follows the project's coding standards.

Steps:
1. Fork this repo
2. Create your feature branch (`git checkout -b feature/my-feature`)
3. Commit your changes (`git commit -am 'Add some feature'`)
4. Push to the branch (`git push origin feature/my-feature`)
5. Open a pull request

---

## Contact
**Project Owner:** Kusasalakhe Hllongwa  
**Email:** ayandahlongwa21@gmail.com
**GitHub:** [khlongwa-dev](https://github.com/khlongwa-dev)  
**LinkedIn:** [k-hlongwa](https://linkedin.com/in/k-hlongwa)

