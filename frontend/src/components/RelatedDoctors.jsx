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
    <div className='flex flex-col items-center gap-4 my-16 text-gray-900 md:mx-10'>
      <h1 className='text-3xl font-medium'>Related Doctors</h1>
      <p className='sm:w-1/3 text-center text-sm'>Simply browse through our extensive list of trusted doctors.</p>
      <div className='w-full grid grid-cols-auto gap-4 pt-5 gap-y-6 px-3 sm:px-0'>
        {relatedDoctors.slice(0,5).map((item, index)=>(
            <div onClick={()=>{navigate(`/appointments/${item._id}`); scrollTo(0,0)}} className='border border-blue-200 rounded-xl overflow-hidden cursor-pointer hover:translate-y-[-10px] transition-all duration-500' key={index} >
                <img className='bg-[#fdf6ee]' src={item.image} alt="" />
                <div className='p-4'>
                    <div className='flex items-center gap-2 text-sm text-center text-green-500'>
                        <p className='w-2 h-2 bg-green-500 rounded-full'></p><p>Available</p>
                    </div>
                    <p>{item.name}</p>
                    <p>{item.speciality}</p>
                </div>
            </div>
        ))}
      </div>
      <button onClick={()=>{ navigate('/doctors'); scrollTo(0,0)}} className='bg-[#fdf6ee] text-gray-600 px-12 py-3 rounded-full mt-10'>more</button>
    </div>
  )
}

export default RelatedDoctors
