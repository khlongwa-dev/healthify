import React from 'react'
import { specialityData } from '../assets/assets'
import { Link } from 'react-router-dom'

const SpecialityMenu = () => {
  return (
    <div>
      <h1>Find a Doctor By Speciality</h1>
      <p>Simply browse through our extensive list of trusted doctors, <br /> schedule your appointment hassle-free.</p>
      <div>
        {specialityData.map((item, index)=>(
            <Link onClick={()=>scrollTo(0,0)}>
                <img src={item.image} alt="" />
                <p>{item.speciality}</p>
            </Link>
        ))}
      </div>
    </div>
  )
}

export default SpecialityMenu
