import React, { useContext, useEffect, useState } from 'react'
import { AppContext } from '../context/AppContext'
import { useNavigate } from 'react-router-dom'

const RelatedDoctors = ({ specialty, docId }) => {
  const { doctors } = useContext(AppContext)
  const [relatedDoctors, setRelatedDoctors] = useState([])
  const navigate = useNavigate()

  useEffect(() => {
    if (doctors.length > 0 && specialty) {
      const relatedDoctorsData = doctors.filter((doctor) => doctor.specialty === specialty && doctor.id !== parseInt(docId))
      setRelatedDoctors(relatedDoctorsData)
    }
  }, [doctors, specialty, docId])


  return (
    <div className='flex flex-col items-center gap-4 my-16 text-gray-900 md:mx-10'>
      <h1 className='text-3xl font-medium'>Related Doctors</h1>
      <p className='sm:w-1/3 text-center text-sm'>Simply browse through our extensive list of trusted doctors.</p>
      <div className='w-full grid grid-cols-auto gap-4 pt-5 gap-y-6 px-3 sm:px-0'>
        {relatedDoctors.slice(0, 5).map((item, index) => (
          <div onClick={() => { navigate(`/appointments/${item.id}`); scrollTo(0, 0) }} className='border border-blue-200 rounded-xl overflow-hidden cursor-pointer hover:translate-y-[-10px] transition-all duration-500' key={index} >
            <img className='bg-[#fdf6ee]' src={item.imageUrl} alt="" />
            <div className='p-4'>
              <div className={`flex items-center gap-2 text-sm text-center ${item.available ? 'text-green-500' : 'text-gray-500'} `}>
                <p className={`w-2 h-2 ${item.available ? ' bg-green-500' : ' bg-gray-500'} rounded-full`}></p><p>{item.available ? 'Available' : 'Not available'}</p>
              </div>
              <p className='text-gray-900 text-lg font-medium'>{item.name}</p>
              <p className='text-gray-600 text-sm'>{item.specialty}</p>
            </div>
          </div>
        ))}
      </div>
      <button onClick={() => { navigate('/doctors'); scrollTo(0, 0) }} className='bg-[#fdf6ee] text-gray-600 px-12 py-3 rounded-full mt-10'>more</button>
    </div>
  )
}

export default RelatedDoctors
