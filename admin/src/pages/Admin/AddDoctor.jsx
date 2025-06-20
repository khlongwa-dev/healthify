import React, { useContext, useState } from 'react'
import { assets } from '../../assets/assets'
import { AdminContext } from '../../context/AdminContext'
import { toast } from 'react-toastify'
import axios from 'axios'

const AddDoctor = () => {
  const [doctorImage, setDoctorImage] = useState(null)
  const [fullName, setFullName] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [experienceLevel, setExperienceLevel] = useState('1 Year')
  const [consultationFees, setConsultationFees] = useState('')
  const [bio, setBio] = useState('')
  const [specialty, setSpecialty] = useState('General Physician')
  const [qualification, setQualification] = useState('')
  const [addressLine1, setAddressLine1] = useState('')
  const [addressLine2, setAddressLine2] = useState('')

  const { backendUrl, adminToken } = useContext(AdminContext)

  const handleDoctorSubmit = async (event) => {
    event.preventDefault()

    if (!doctorImage) {
      return toast.error('Please select a profile image for the doctor.')
    }

    const formData = new FormData()
    formData.append('Name', fullName)
    formData.append('Email', email)
    formData.append('Password', password)
    formData.append('Image', doctorImage)
    formData.append('Specialty', specialty)
    formData.append('Degree', qualification)
    formData.append('Experience', experienceLevel)
    formData.append('About', bio)
    formData.append('Fees', Number(consultationFees))
    formData.append('AddressLine1', addressLine1)
    formData.append('AddressLine2', addressLine2)

    try {
      const { data } = await axios.post(
        `${backendUrl}api/admin/add-doctor`,
        formData,
        { headers: { Authorization: `Bearer ${adminToken}` } }
      )

      if (data.success) {
        toast.success(data.message)
        resetForm()
      } else {
        toast.error(data.message)
      }
    } catch (error) {
      toast.error(error.message)
    }
  }

  const resetForm = () => {
    setDoctorImage(null)
    setFullName('')
    setEmail('')
    setPassword('')
    setBio('')
    setAddressLine1('')
    setAddressLine2('')
    setConsultationFees('')
    setQualification('')
  }

  return (
    <form onSubmit={handleDoctorSubmit} className='m-5 w-full'>
      <p className='mb-3 text-lg font-medium'>Add a doctor</p>

      <div className='bg-white px-8 py-8 border rounded w-full max-w-4xl max-h-[80vh] overflow-y-scroll'>
        <div className='flex items-center gap-4 mb-8 text-gray-500'>
          <label htmlFor="doc-img">
            <img className='w-16 bg-gray-100 rounded-full cursor-pointer' src={ doctorImage ? URL.createObjectURL(doctorImage) : assets.upload_area} alt="" />
          </label>
          <input onChange={(e)=> setDoctorImage(e.target.files[0])} type="file" id="doc-img" hidden/>
          <p>Upload doctor <br /> picture</p>
        </div>

        <div className='flex flex-col lg:flex-row items-start gap-10 text-gray-600'>
          <div className='w-full lg:flex-1 flex flex-col  gap-4'>
            <div className='flex-1 flex flex-col gap-1'>
              <p>Doctor name</p>
              <input onChange={(e)=> setName(e.target.value)} value={fullName} className='border rounded px-3 py-2' type="text" placeholder='Name' required />
            </div>

            <div className='flex-1 flex flex-col gap-1'>
              <p>Doctor Email</p>
              <input onChange={(e)=> setEmail(e.target.value)} value={email} className='border rounded px-3 py-2' type="email" placeholder='Email' required />
            </div>

            <div className='flex-1 flex flex-col gap-1'>
              <p>Doctor Password</p>
              <input onChange={(e)=> setPassword(e.target.value)} value={password} className='border rounded px-3 py-2' type="password" placeholder='Password' required />
            </div>

            <div className='flex-1 flex flex-col gap-1'>
              <p>Experience</p>
              <select onChange={(e)=> setExperienceLevel(e.target.value)} value={experienceLevel} className='border rounded px-3 py-2'>
                <option value="1 Year">1 Year</option>
                <option value="2 Years">2 Years</option>
                <option value="3 Years">3 Years</option>
                <option value="4 Years">4 Years</option>
                <option value="5 Years">5 Years</option>
                <option value="6 Years">6 Years</option>
                <option value="7 Years">7 Years</option>
                <option value="8 Years">8 Years</option>
                <option value="9 Years">9 Years</option>
                <option value="10 Years">10 Years</option>
              </select>
            </div>

            <div className='flex-1 flex flex-col gap-1'>
              <p>Fees</p>
              <input onChange={(e)=> setConsultationFees(e.target.value)} value={consultationFees} className='border rounded px-3 py-2' type="number" placeholder='Fees' required />
            </div>
          </div>
          
          <div className='w-full lg:flex-1 flex flex-col gap-4'>
            <div className='flex-1 flex flex-col gap-1'>
              <p>Specialty</p>
              <select onChange={(e)=> setSpecialty(e.target.value)} value={specialty} className='border rounded px-3 py-2'>
                <option value="General Physician">General Physician</option>
                <option value="Gynecologist">Gynecologist</option>
                <option value="Dermatologist">Dermatologist</option>
                <option value="Pediatrician">Pediatrician</option>
                <option value="Neurologist">Neurologist</option>
                <option value="Gastroenterologist">Gastroenterologist</option>
              </select>
            </div>

            <div className='flex-1 flex flex-col gap-1'>
              <p>Education</p>
              <input  onChange={(e)=> setQualification(e.target.value)} value={qualification} className='border rounded px-3 py-2' type="text" placeholder='Education' required />
            </div>

            <div className='flex-1 flex flex-col gap-1'>
              <p>Address</p>
              <input onChange={(e)=> setAddressLine1(e.target.value)} value={addressLine1} className='border rounded px-3 py-2' type="text" placeholder='address line 1' required/>
              <input onChange={(e)=> setAddressLine2(e.target.value)} value={addressLine2} className='border rounded px-3 py-2' type="text" placeholder='address line 2' required/>
            </div>

          </div>
        </div>

        <div>
          <p className='mt-4 mb-2'>About Doctor</p>
          <textarea onChange={(e)=> setBio(e.target.value)} value={bio} className='w-full px-4 pt-2 border rounded' placeholder='Write doctor about..' rows={5} required/>
        </div>

        <button className='bg-primary px-10 py-3 mt-4 text-white rounded-full' type='submit'>Add doctor</button>
      </div>
    </form>
  )
}

export default AddDoctor