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
      <h1>Related Doctors</h1>
      <p>Simply browse through our extensive list of trusted doctors.</p>
      <div>
        {relDocs.slice(0,5).map((item, index)=>(
            <div onClick={()=>{navigate(`/appointments/${item._id}`); scrollTo(0,0)}} key={index} >
                <img src={item.image} alt="" />
                <div >
                    <div>
                        <p></p><p>Available</p>
                    </div>
                    <p>{item.name}</p>
                    <p>{item.speciality}</p>
                </div>
            </div>
        ))}
      </div>
      <button onClick={()=>{ navigate('/doctors'); scrollTo(0,0)}}>more</button>
    </div>
  )
}

export default RelatedDoctors
