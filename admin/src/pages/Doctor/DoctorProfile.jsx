import React, { useEffect } from 'react'
import { useContext } from 'react'
import { DoctorContext } from '../../context/DoctorContext'


const DoctorProfile = () => {
  const {dToken, loadDoctorProfileData} = useContext(DoctorContext)
  const [isEdit, setIsEdit] = useState(false)
  const [image, setImage] = useState(false)

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
  return (
    <div>
      
    </div>
  )
}

export default DoctorProfile
