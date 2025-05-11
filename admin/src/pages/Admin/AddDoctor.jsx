import React from 'react'
import { assets } from '../../assets/assets'

const AddDoctor = () => {
  return (
    <form className='m-5 w-full'>
      <p className='mb-3 text-lg font-medium'>Add a doctor</p>

      <div className='bg-white px-8 py-8 border rounded w-full max-w-4xl max-h-[80vh] overflow-y-scroll'>
        <div>
          <label htmlFor="doc-img">
            <img src={assets.upload_area} alt="" />
          </label>
          <input type="file" id="doc-img" hidden/>
          <p>Upload doctor <br /> picture</p>
        </div>

        <div>
          <div>
            <div>
              <p>Doctor name</p>
              <input type="text" placeholder='Name' required />
            </div>

            <div>
              <p>Doctor Email</p>
              <input type="email" placeholder='Email' required />
            </div>

            <div>
              <p>Doctor Password</p>
              <input type="password" placeholder='Password' required />
            </div>

            <div>
              <p>Experience</p>
              <select name="" id="">
                <option value="1 Year">1 Year</option>
                <option value="2 Year">2 Year</option>
                <option value="3 Year">3 Year</option>
                <option value="4 Year">4 Year</option>
                <option value="5 Year">5 Year</option>
                <option value="6 Year">6 Year</option>
                <option value="7 Year">7 Year</option>
                <option value="8 Year">8 Year</option>
                <option value="9 Year">9 Year</option>
                <option value="10 Year">10 Year</option>
              </select>
            </div>

            <div>
              <p>Fees</p>
              <input type="number" placeholder='Fees' required />
            </div>
          </div>
          
          <div>
            <div>
              <p>Speciality</p>
              <select name="" id="">
                <option value="General Physician">General Physician</option>
                <option value="Gynecologist">Gynecologist</option>
                <option value="Dermatologist">Dermatologist</option>
                <option value="Pediatrician">Pediatrician</option>
                <option value="Neurologist">Neurologist</option>
                <option value="Gastroenterologist">Gastroenterologist</option>
              </select>
            </div>

            <div>
              <p>Education</p>
              <input type="text" placeholder='Education' required />
            </div>

            <div>
              <p>Address</p>
              <input type="text" placeholder='line 1' required/>
              <input type="text" placeholder='line 2' required/>
            </div>

          </div>
        </div>

        <div>
          <p>About Doctor</p>
          <text placeholder='Write doctor about..' rows={5} required/>
        </div>

        <button>Add doctor</button>
      </div>
    </form>
  )
}

export default AddDoctor
