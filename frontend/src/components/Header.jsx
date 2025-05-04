import React from 'react'
import { assets } from '../assets/assets'

const Header = () => {
  return (
    <div>
        {/* -------- Left Side -------- */}
        <div>
            <p>
                Book Your Appointment <br /> With Trusted Doctors
            </p>
            <div>
                <img src={assets.group_profiles} alt="" />
                <p>Simply browse through our extensive list of trusted doctors, <br className='hidden sm:block' /> schedule your appointment hassle-free.</p>
            </div>
            <a href="#speciality">
                Book an appointment <img src={assets.arrow_icon} alt="" />
            </a>
        </div>
    </div>
  )
}

export default Header
