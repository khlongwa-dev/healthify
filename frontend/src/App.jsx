import React from 'react'
import Navbar from './components/Navbar'
import { Routes, Route } from 'react-router-dom'
import UserProfile from './pages/UserProfile'
import UserAppointments from './pages/UserAppointments'
import Home from './pages/Home'
import Doctors from './pages/Doctors'
import Login from './pages/Login'
import About from './pages/About'
import Contacts from './pages/Contacts'
import Appointment from './pages/Appointment'

const App = () => {
  return (
    <div>
      <Navbar />
      <Routes>
      <Route path='/' element={<Home />} />
        <Route path='/doctors' element={<Doctors />} />
        <Route path='/doctors/:speciality' element={<Doctors />} />
        <Route path='/login' element={<Login />} />
        <Route path='/about' element={<About />} />
        <Route path='/contacts' element={<Contacts />} />
        <Route path='/user-profile' element={<UserProfile/>} />
        <Route path='/user-appointments' element={<UserAppointments/>} />
        <Route path='/appointments/:docId' element={<Appointment/>} />
      </Routes>
    </div>
  )
}

export default App
