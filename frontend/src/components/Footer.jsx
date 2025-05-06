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

        {/* --- right section --- */}
        <div>
            <p>GET IN TOUCH</p>
            <ul>
                <li>+27-63-384-8266</li>
                <li>ayandahlongwa21@gmail.com</li>
            </ul>
        </div>
      </div>

      {/* --- copyright section --- */}
      <div>
        <hr />
        <p>Copyright 2025 @ KHlongwa.dev - All Right Reserved.</p>
      </div>
    </div>
  )
}

export default Footer
