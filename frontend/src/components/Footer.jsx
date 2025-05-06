import React from 'react'
import { assets } from '../assets/assets'

const Footer = () => {
  return (
    <div>
      <div>
        {/* --- left section --- */}
        <div>
            <img src={assets.logo} alt="" />
            <p>Healthify is a convenient app for booking doctor appointments with ease. Healthify has become a go-to solution for patients seeking quick access to healthcare professionals, offering a seamless and efficient way to schedule medical visits anytime, anywhere.
            </p>
        </div>

        {/* --- mid section --- */}
        <div>
            <p>COMPANY</p>
            <ul>
                <li>Home</li>
                <li>About us</li>
                <li>Contact us</li>
                <li>Privacy policy</li>
            </ul> 
        </div>
      </div>
    </div>
  )
}

export default Footer
