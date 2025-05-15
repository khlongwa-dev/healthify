import React, { useEffect, useState } from 'react'
import { useContext } from 'react'
import { DoctorContext } from '../../context/DoctorContext'
import { AppContext } from '../../context/AppContext'


const DoctorProfile = () => {
  const {dToken, docData, loadDoctorProfileData} = useContext(DoctorContext)
  const [isEdit, setIsEdit] = useState(false)
  const [image, setImage] = useState(false)
  const {currency} = useContext(AppContext)

  const updateDoctorProfileData = async () => {
    try {
      const formData = new FormData()
      formData.append('Phone', userData.phone)
      formData.append('Name', userData.name)
      formData.append('AddressLine1', userData.addressLine1)
      formData.append('AddressLine2', userData.addressLine2)
      formData.append('Gender', userData.gender)
      formData.append('DoB', userData.doB)
      
      image && formData.append('ImageUrl', image)
      const {data} = await axios.post(backendUrl + 'api/doctor/update-profile', formData, {headers:{token}})

      if (data.success) {
        toast.success(data.message)
        await loadDoctorProfileData()
        setIsEdit(false)
        setImage(false)
      } else {
        toast.error(data.message)
      }
    } catch (error) {
      console.log(error)
      toast.error(error.message)
    }
  }

  useEffect(()=>{
    if(dToken) {
      loadDoctorProfileData()
    }
  })
  return docData && (
    <div>

      <div>
        <div>
          <img src={docData.imageUrl} alt="" />
        </div>

        {/* ------- doc info name, degree, experience -------- */}
        <div>
          <p>{loadDoctorProfileData.name}</p>
          <div>
            <p>{docData.degree} - {docData.specialty}</p>
            <button>{docData.experience}</button>
          </div>

          {/* ----- doctor about --------*/}
          <div>
            <p>About</p>
            <p>{docData.about}</p>
          </div>

          <p>Appointment fee: <span>{currency} {docData.fees}</span></p>

          <div>
            <p>Address:</p>
            <p>
              {docData.addressLine1}
              <br />
              {docData.addressLine2}
            </p>
          </div>

          <div>
            <input type="checkbox" />
            <label htmlFor="">Available</label>
          </div>

          <button>Edit</button>
        </div>
      </div>

    </div>
  )
}

export default DoctorProfile
