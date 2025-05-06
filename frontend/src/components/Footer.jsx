import React from 'react'
import { assets } from '../assets/assets'

const Footer = () => {
  return (
    <div className='md:mx-10'>
      <div className='flex flex-col sm:grid grid-cols-[3fr_1fr_1fr] gap-14 my-10 mt-40 text-sm'>
        {/* --- left section --- */}
        <div className='mb-5 w-40'>
            <img src={assets.logo} alt="" />
            <p className='w-full md:w-2/3 text-gray-600 leading-6'>Healthify is a convenient app for booking doctor appointments with ease. Healthify has become a go-to solution for patients seeking quick access to healthcare professionals, offering a seamless and efficient way to schedule medical visits anytime, anywhere.
            </p>
        </div>

        {/* --- mid section --- */}
        <div>
            <p className='text-xl font-medium mb-5'>COMPANY</p>
            <ul className='flex flex-col gap-2 text-gray-600'>
                <li>Home</li>
                <li>About us</li>
                <li>Contact us</li>
                <li>Privacy policy</li>
            </ul> 
        </div>

        {/* --- right section --- */}
        <div>
            <p className='text-xl font-medium mb-5'>GET IN TOUCH</p>
            <ul className='flex flex-col gap-2 text-gray-600'>
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
