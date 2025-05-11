import React from 'react'
import { assets } from '../../assets/assets'

const AddDoctor = () => {
  return (
    <form>
      <p>Add a doctor</p>
      <div>
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
          </div>
        </div>
      </div>
    </form>
  )
}

export default AddDoctor
