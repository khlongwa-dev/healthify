import React, { useEffect, useState, useRef } from 'react'
import { assets } from '../assets/assets';
import { NavLink, useNavigate } from 'react-router-dom';

const Navbar = () => {

  const navigate = useNavigate();
  const [showMenu, setShowMenu] = useState(false);
  const [dropdownOpen, setDropdownOpen] = useState(false);
  const [token, setToken] = useState(true);
  const dropdownRef = useRef(null); // Create a ref for the dropdown

  const toggleDropdown = () => {
    setDropdownOpen(prev => !prev);
  };

  useEffect(()=> {
    const handleClickOustide = (event) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target)) {
        setDropdownOpen(false);
      }
    };

    document.addEventListener('mousedown', handleClickOustide);

    return () => {
      document.removeEventListener('mousedown', handleClickOustide);
    };
  }, []);

  return (
    <div className='flex items-center justify-between text-sm py-4 mb-5 border-b border-b-gray-400'>
      <img onClick={() => navigate('/')} className='w-44 cursor-pointer' src={assets.logo} alt="" />
      <ul className='hidden md:flex items-start gap-5 font-medium'>
        <NavLink to='/'>
          <li className='py-1'>HOME</li>
          <hr className='border-none outline-none h-0.5 bg-primary w-3/5 m-auto hidden' />
        </NavLink>
        <NavLink to='/doctors'>
          <li className='py-1'>ALL DOCTORS</li>
          <hr className='border-none outline-none h-0.5 bg-primary w-3/5 m-auto hidden' />
        </NavLink>
        <NavLink to='/about'>
          <li className='py-1'>ABOUT</li>
          <hr className='border-none outline-none h-0.5 bg-primary w-3/5 m-auto hidden' />
        </NavLink>
        <NavLink to='/contacts'>
          <li className='py-1'>CONTACTS</li>
          <hr className='border-none outline-none h-0.5 bg-primary w-3/5 m-auto hidden' />
        </NavLink>
      </ul>
      
      <div className='flex items-center gap-4'>
        {
          token
          ? <div className='flex items-center gap-2 cursor-pointer relative'>
            <img className='w-8 rounded-full' src={assets.profile_pic} onClick={toggleDropdown} alt="" />
            <img className='w-2.5' src={assets.dropdown_icon} onClick={toggleDropdown} alt="" />
            {dropdownOpen && (
              <div ref={dropdownRef} className='absolute top-0 right-0 pt-14 text-base font-medium text-gray-600 z-20'>
                <div className='min-w-48 bg-stone-100 rounded flex flex-col gap-4 p-4'>
                  <p onClick={() => { navigate('/my-profile'); setDropdownOpen(false); }} className='hover:text-black cursor-pointer'>My Profile</p>
                  <p onClick={() => { navigate('/my-appointments'); setDropdownOpen(false); }} className='hover:text-black cursor-pointer'>My Appointments</p>
                  <p onClick={() => { setToken(false); setDropdownOpen(false); }}>Logout</p>
                </div>
              </div>
            )}
          </div>
          : <button onClick={() => navigate('/login')} className='bg-primary text-white px-8 py-3 rounded-full font-light hidden md:block cursor-pointer' >Create Account</button>
        }

        <img onClick={() => setShowMenu(true)} className='w-6 md:hidden' src={assets.menu_icon} alt="" />
        {/* ------ Mobile Menu ------ */}
        <div>
          <div>
            <img src={assets.logo} alt="" />
            <img src={assets.cross_icon} alt="" />
          </div>
          <ul>
            <NavLink onClick={() => setShowMenu(false)} to='/' >HOME</NavLink>
            <NavLink onClick={() => setShowMenu(false)} to='/doctors'>ALL DOCTORS</NavLink>
            <NavLink onClick={() => setShowMenu(false)} to='/about'>ABOUT</NavLink>
            <NavLink onClick={() => setShowMenu(false)} to='/contacts'>CONTACTS</NavLink>
          </ul>
        </div>
      </div>
    </div>
  )
}

export default Navbar
