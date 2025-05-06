import React from 'react'
import { assets } from '../assets/assets'
import { useNavigate } from 'react-router-dom'

const Banner = () => {
    const navigate = useNavigate()

  return (
    <div>
        {/*--- left side --- */}
      <div>
        <div>
            <p>Book Your Appointment</p>
            <p>With 100+ Trusted Doctors</p>
        </div>
        <button onClick={()=>{navigate('/login'), scrollTo(0,0)}}></button>
      </div>
    </div>
  )
}

export default Banner
