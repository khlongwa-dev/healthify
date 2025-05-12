import React, { useContext, useEffect } from 'react'
import { AdminContext } from '../../context/AdminContext'

const DoctorsList = () => {
  const { doctors, aToken, getAllDoctors } = useContext(AdminContext)

  useEffect(()=>{
    if (aToken) {
      getAllDoctors()
    }
  }, [aToken])
  return (
    <div className='m-5 max-h-[90vh] overflow-y-scroll'>
      <h1 className='text-lg font-medium'>All Doctors</h1>
      <div className='w-full flex flex-wrap gap-4 pt-5 gap-y-6'>
        {
          doctors.map((item, index)=>(
            <div className='border border-indigo-200 rounded-xl max-w-56 overflow-hidden cursor-pointer group' key={index}>
              <img src={item.imageUrl} alt="" />
              <div>
                <p>{item.name}</p>
                <p>{item.specialty}</p>
              </div>
              <div>
                <input type="checkbox" checked={item.availabe}/>
                <p>Available</p>
              </div>
            </div>
          ))
        }
      </div>
    </div>
  )
}

export default DoctorsList
