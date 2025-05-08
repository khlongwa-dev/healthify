import React, { useContext, useEffect, useState } from 'react'
import { AppContext } from '../context/AppContext'
import { useNavigate } from 'react-router-dom'

const RelatedDoctors = ({speciality, doctorId}) => {
    const {doctors} = useContext(AppContext)
    const [relatedDoctors, setRelatedDoctors] = useState([])
    const navigate = useNavigate()

    useNavigate(()=>{
        if (doctors.length > 0 && speciality) {
            const relatedDoctorsData = doctors.filter((doctor)=>doctor.speciality === doctor.speciality && doctor._id !== docId)
            setRelatedDoctors(relatedDoctorsData)
        }
    }, [doctors, speciality, doctorId])
    
  return (
    <div>
      
    </div>
  )
}

export default RelatedDoctors
